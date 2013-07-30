using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lugo.EntityFramework;
using WebMatrix.WebData;
using System.Collections;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;


namespace ALLunguletu.Controllers
{
    [Authorize(Roles = "admin")]
    public class IndexAdminController : Controller
    {
        private Entities _context = new Entities();

        //
        // GET: /IndexAdmin/

        [HttpGet]
        public ActionResult Index()
        {
            var index = _context.Index.Include(i => i.UserProfile);
            return View();
        }


        public JsonResult GetIndex(string sidx, string sord, int page, int rows
             , bool _search, string filters, string searchField, string searchOper, string searchString)
        {
            IEnumerable<Index> indexList = _context.Index.ToList();

            //filter
            indexList = FilterIndex(filters, indexList);

            //sort
            indexList = OrderIndex(sidx, sord, indexList);

            int count = 0;
            int totalPages = (indexList.Count() % rows) != 0 ? (indexList.Count() / rows) + 1 : indexList.Count() / rows;
            
            //paging
            indexList = indexList
                .Skip((page - 1) * rows)
                .Take(rows);

            var jsonData = new
            {
                total = totalPages, //(int)Math.Ceiling((double)count / grid.PageSize)
                page = page,
                records = indexList.Count(),
                rows = indexList.Select(x => new
                {
                    id = count++,
                    cell = new[]
                    {
                        x.UserProfile.UserName,
                        x.IndexYear.ToString(),
                        x.IndexMonth.ToString(),
                        x.IndexOld.ToString(),
                        x.IndexNew.ToString(),
                        x.IndexDiff.ToString(),
                        x.SysWaterType.WaterName,
                        x.Data.ToShortDateString() + ' ' + x.Data.ToShortTimeString(),
                        x.Description.ToString()
                    }
                }).ToList()

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private static IEnumerable<Lugo.EntityFramework.Index> FilterIndex(string filters, IEnumerable<Index> indexList)
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
                                indexList = indexList.Where(x => x.UserProfile.UserName.ToLower().Contains(item.data.ToLower() ));
                            break;
                        case "indexyear":
                            if (item.op.ToLower().Equals("eq"))
                                indexList = indexList.Where(x => x.IndexYear == int.Parse(item.data));
                            break;
                        case "indexmonth":
                            if (item.op.ToLower().Equals("eq"))
                                indexList = indexList.Where(x => x.IndexMonth == int.Parse(item.data));
                            break;
                        case "watername":
                            if (item.op.ToLower().Equals("cn"))
                                indexList = indexList.Where(x => x.SysWaterType.WaterName.ToLower().Contains(item.data.ToLower()));
                            break;
                    }
                }

            }
            return indexList;
        }



        private static IEnumerable<Lugo.EntityFramework.Index> OrderIndex(string sidx, string sord, IEnumerable<Index> indexList)
        {
            if (!string.IsNullOrEmpty(sidx))
                switch (sidx.ToLower())
                {
                    case "username":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.UserProfile.UserName) : indexList.OrderBy(x => x.UserProfile.UserName);
                        break;
                    case "indexyear":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexYear) : indexList.OrderBy(x => x.IndexYear);
                        break;
                    case "indexmonth":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexMonth) : indexList.OrderBy(x => x.IndexMonth);
                        break;
                    case "indexold":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexOld) : indexList.OrderBy(x => x.IndexOld);
                        break;
                    case "indexnew":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexNew) : indexList.OrderBy(x => x.IndexNew);
                        break;
                    case "indexdiff":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexDiff) : indexList.OrderBy(x => x.IndexDiff);
                        break;
                    case "watername":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.SysWaterType.WaterName) : indexList.OrderBy(x => x.SysWaterType.WaterName);
                        break;
                    case "data":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.Data) : indexList.OrderBy(x => x.Data);
                        break;
                    case "description":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.Description) : indexList.OrderBy(x => x.Description);
                        break;
                }
            return indexList;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
   
}