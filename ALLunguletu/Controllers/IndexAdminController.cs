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
                        x.Data.ToShortDateString() + ' ' + x.Data.ToShortTimeString(),

                        x.IndexOldBaieRece.ToString(),
                        x.IndexNewBaieRece.ToString(),
                        x.IndexDiffBaieRece.ToString(),

                        x.IndexOldBaieCalda.ToString(),
                        x.IndexNewBaieCalda.ToString(),
                        x.IndexDiffBaieCalda.ToString(),
  
                        x.IndexOldBucatarieRece.ToString(),
                        x.IndexNewBucatarieRece.ToString(),
                        x.IndexDiffBucatarieRece.ToString(),
  
                        x.IndexOldBucatarieCalda.ToString(),
                        x.IndexNewBucatarieCalda.ToString(),
                        x.IndexDiffBucatarieCalda.ToString(),
                        x.Description != null?x.Description.ToString():string.Empty
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

                    case "indexoldbare":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexOldBaieRece) : indexList.OrderBy(x => x.IndexOldBaieRece);
                        break;
                    case "indexnewbare":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexNewBaieRece) : indexList.OrderBy(x => x.IndexNewBaieRece);
                        break;
                    case "indexdiffbare":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexDiffBaieRece) : indexList.OrderBy(x => x.IndexDiffBaieRece);
                        break;



                    case "indexoldbaca":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexOldBaieCalda) : indexList.OrderBy(x => x.IndexOldBaieCalda);
                        break;
                    case "indexnewbaca":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexNewBaieCalda) : indexList.OrderBy(x => x.IndexNewBaieCalda);
                        break;
                    case "indexdiffbaca":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexDiffBaieCalda) : indexList.OrderBy(x => x.IndexDiffBaieCalda);
                        break;



                    case "indexoldbure":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexOldBucatarieRece) : indexList.OrderBy(x => x.IndexOldBucatarieRece);
                        break;
                    case "indexnewbure":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexNewBucatarieRece) : indexList.OrderBy(x => x.IndexNewBucatarieRece);
                        break;
                    case "indexdiffbure":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexDiffBucatarieRece) : indexList.OrderBy(x => x.IndexDiffBucatarieRece);
                        break;



                    case "indexoldbuca":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexOldBucatarieCalda) : indexList.OrderBy(x => x.IndexOldBucatarieCalda);
                        break;
                    case "indexnewbuca":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexNewBucatarieCalda) : indexList.OrderBy(x => x.IndexNewBucatarieCalda);
                        break;
                    case "indexdiffbuca":
                        indexList = sord.Equals("desc") ?
                            indexList.OrderByDescending(x => x.IndexDiffBucatarieCalda) : indexList.OrderBy(x => x.IndexDiffBucatarieCalda);
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