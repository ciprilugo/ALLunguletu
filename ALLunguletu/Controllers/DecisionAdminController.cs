using ALLunguletu.Filters;
using Lugo.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ALLunguletu.Controllers
{
    [Authorize(Roles = "Admin")]
    [InitializeSimpleMembership]
    public class DecisionAdminController : Controller
    {
        private Entities _context = new Entities();

        [HttpPost]
        public ActionResult Delete(string id, FormCollection deleteItems)
        {
            int decisionId = 0;
            int.TryParse(deleteItems["id"], out decisionId);
            _context.Entry(new Decision() { DecisionId = decisionId }).State = System.Data.EntityState.Deleted;
            _context.SaveChanges();

            return Content("Success");
        }


        [HttpPost]
        public ActionResult Add(FormCollection addItem)
        {
            _context.Entry(new Decision() 
            { 
                Data  = DateTime.Now,
                Subject  = addItem["subject"],
                Description = addItem["description"],
                UserId = WebSecurity.CurrentUserId
            }).State = System.Data.EntityState.Added;
            _context.SaveChanges();

            return Content("Success");
        }


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetDecision(string sidx, string sord, int page, int rows
             , bool _search, string filters, string searchField, string searchOper, string searchString)
        {
            IEnumerable<Decision> decisionList = _context.Decision.ToList();

            //filter
            decisionList = FilterIndex(filters, decisionList);

            //sort
            decisionList = OrderIndex(sidx, sord, decisionList);

            int count = 0;
            int totalPages = (decisionList.Count() % rows) != 0 ? (decisionList.Count() / rows) + 1 : decisionList.Count() / rows;

            //paging
            decisionList = decisionList
                .Skip((page - 1) * rows)
                .Take(rows);

            var jsonData = new
            {
                total = totalPages, //(int)Math.Ceiling((double)count / grid.PageSize)
                page = page,
                records = decisionList.Count(),
                rows = decisionList.Select(x => new
                {
                    id = count++,
                    cell = new[]
                    {
                        x.DecisionId.ToString(),
                        x.UserProfile.UserName,
                        x.Data.ToShortDateString() + ' ' + x.Data.ToShortTimeString(),
                        x.Subject.ToString(),
                        x.Description.ToString()
                    }
                }).ToList()

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private static IEnumerable<Lugo.EntityFramework.Decision> FilterIndex(string filters, IEnumerable<Decision> complainList)
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



        private static IEnumerable<Lugo.EntityFramework.Decision> OrderIndex(string sidx, string sord, IEnumerable<Decision> indexList)
        {
            if (!string.IsNullOrEmpty(sidx))
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
            else
                indexList = indexList.OrderByDescending(x => x.Data);

            return indexList;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
