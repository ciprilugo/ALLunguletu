using ALLunguletu.Filters;
using Lugo.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALLunguletu.Controllers
{
    [Authorize(Roles = "admin")]
    [InitializeSimpleMembership]
    public class ComplainAdminController : Controller
    {
        private Entities _context = new Entities();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetComplain(string sidx, string sord, int page, int rows
             , bool _search, string filters, string searchField, string searchOper, string searchString)
        {
            IEnumerable<Complain> complainList = _context.Complain.ToList();

            //filter
            complainList = FilterIndex(filters, complainList);

            //sort
            complainList = OrderIndex(sidx, sord, complainList);

            int count = 0;
            int totalPages = (complainList.Count() % rows) != 0 ? (complainList.Count() / rows) + 1 : complainList.Count() / rows;

            //paging
            complainList = complainList
                .Skip((page - 1) * rows)
                .Take(rows);

            var jsonData = new
            {
                total = totalPages, //(int)Math.Ceiling((double)count / grid.PageSize)
                page = page,
                records = complainList.Count(),
                rows = complainList.Select(x => new
                {
                    id = count++,
                    cell = new[]
                    {
                        x.UserProfile.UserName,
                        x.Data.ToShortDateString() + ' ' + x.Data.ToShortTimeString(),
                        x.Subject.ToString(),
                        x.Description.ToString()
                    }
                }).ToList()

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private static IEnumerable<Lugo.EntityFramework.Complain> FilterIndex(string filters, IEnumerable<Complain> complainList)
        {
            ALLunguletu.Helpers.Filter filterList = null;
            if (!string.IsNullOrEmpty(filters))
            {
                filterList = ALLunguletu.Helpers.Filter.Create(filters);
                foreach (var item in filterList.rules)
                {
                    switch (item.field.ToLower())
                    {
                        case "username":
                            if (item.op.ToLower().Equals("cn"))
                                complainList = complainList.Where(x => x.UserProfile.UserName.ToLower().Contains(item.data.ToLower()));
                            break;
                        case "data":
                            if (item.op.ToLower().Equals("cn"))
                                complainList = complainList.Where(x => x.Data.ToString().ToLower().Contains(item.data.ToLower()));
                            break;
                        case "subject":
                            if (item.op.ToLower().Equals("cn"))
                                complainList = complainList.Where(x => x.Subject.ToLower().Contains(item.data.ToLower()));
                            break;
                        case "description":
                            if (item.op.ToLower().Equals("cn"))
                                complainList = complainList.Where(x => x.Description.ToLower().Contains(item.data.ToLower()));
                            break;
                    }
                }

            }
            return complainList;
        }



        private static IEnumerable<Lugo.EntityFramework.Complain> OrderIndex(string sidx, string sord, IEnumerable<Complain> indexList)
        {
            if (!string.IsNullOrEmpty(sidx))
            {
                switch (sidx.ToLower())
                {
                    case "username":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.UserProfile.UserName) : indexList.OrderBy(x => x.UserProfile.UserName);
                        break;
                    case "data":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.Data) : indexList.OrderBy(x => x.Data);
                        break;
                    case "subject":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.Subject) : indexList.OrderBy(x => x.Subject);
                        break;
                    case "description":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.Description) : indexList.OrderBy(x => x.Description);
                        break;
                }
            }
            else
                indexList = indexList.OrderByDescending(x => x.Data) ;

            return indexList;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
