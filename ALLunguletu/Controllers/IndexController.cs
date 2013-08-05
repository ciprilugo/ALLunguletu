using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lugo.EntityFramework;
using WebMatrix.WebData;
using ALLunguletu.Filters;
using ALLunguletu.Models;
using System.Collections;
using ALLunguletu.Enums;

namespace ALLunguletu.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class IndexController : Controller
    {
        #region Properties
        Entities _context = new Entities();
        #endregion

        //
        // GET: /Index/

        public ActionResult Index()
        {
            var index = _context.Index
                .Where(x => x.UserId == WebSecurity.CurrentUserId)
                .OrderByDescending(m => new { m.IndexYear, m.IndexMonth });

            return View(index);
        }

        //
        // GET: /Index/Details/5
        public ActionResult Details(int id = 0)
        {
            Index index = _context.Index.Find(id);
            if (index == null)
            {
                return HttpNotFound();
            }
            return View(index);
        }

        //
        // GET: /Index/Create

        public ActionResult Create()
        {
            //SetWaterList();
            SetMonthList();
            SetYearList();

            ////ViewBag.Month = new SelectList(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }
            ////    .Select(x => new { value = x, text = x }),
            ////    "text", "value");
            return View();
        }

        //private void SetWaterList()
        //{
        //    var waterList = new KeyValuePair<int, string>[2];
        //    waterList[0] = new KeyValuePair<int, string>((int)SysWater.Rece, Enum.GetName(typeof(SysWater), SysWater.Rece));
        //    waterList[1] = new KeyValuePair<int, string>((int)SysWater.Calda, Enum.GetName(typeof(SysWater), SysWater.Calda));
        //    ViewBag.WaterList = waterList
        //        .Select(x => new
        //        {
        //            WaterId = x.Key,
        //            WaterName = x.Value
        //        });
        //}

        private void SetYearList()
        {
            var yearList = new KeyValuePair<int, int>[5];
            for (int i = 2013; i < 2018; i++)
                yearList[i - 2013] = new KeyValuePair<int, int>(i, i);

            ViewBag.YearList = yearList
                .Select(x => new
                {
                    YearId = x.Key,
                    YearName = x.Value
                });
        }

        private void SetMonthList()
        {
            var monthList = new KeyValuePair<int, int>[12];
            for (int i = 0; i < 12; i++)
                monthList[i] = new KeyValuePair<int, int>(i + 1, i + 1);

            ViewBag.MonthList = monthList
                .Select(x => new
                {
                    MonthId = x.Key,
                    MonthName = x.Value
                });
        }

        //
        // POST: /Index/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Index index)
        {
            try
            {
                var ret = true;
                if (ModelState.IsValid)
                {
                    if (index.IndexOldBaieRece > index.IndexNewBaieRece)
                    {
                        ModelState.AddModelError("", " (Baie - Rece) 'Indexul vechi' trebuie sa aiba valoare mai mica decat 'Indexul nou'.");
                        ret = false;
                    }
                    if (index.IndexOldBaieCalda > index.IndexNewBaieCalda)
                    {
                        ModelState.AddModelError("", " (Baie - Calda)'Indexul vechi' trebuie sa aiba valoare mai mica decat 'Indexul nou'.");
                        ret = false;
                    }
                    if (index.IndexOldBucatarieRece > index.IndexNewBucatarieRece)
                    {
                        ModelState.AddModelError("", " (Bucatarie - Rece)'Indexul vechi' trebuie sa aiba valoare mai mica decat 'Indexul nou'.");
                        ret = false;
                    }
                    if (index.IndexOldBucatarieCalda > index.IndexNewBucatarieCalda)
                    {
                        ModelState.AddModelError("", " (Bucatarie - Calda)'Indexul vechi' trebuie sa aiba valoare mai mica decat 'Indexul nou'.");
                        ret = false;
                    }

                    if (!ret)
                    {
                        SetYearList();
                        SetMonthList();
                        return View(index as IndexModel);
                    }

                    //new index allready exists for Year and Month
                    if (_context.Index
                        .Where(x => x.UserId == WebSecurity.CurrentUserId
                                    && x.IndexYear == index.IndexYear
                                    && x.IndexMonth == index.IndexMonth)
                        .Count() > 0)
                    {
                        ModelState.AddModelError("", string.Format("Pentru perioada '{0}/{1}' , ati mai introdus valori Index. ", index.IndexYear, index.IndexMonth));
                        SetYearList();
                        SetMonthList();
                        return View(index as IndexModel);
                    }

                    index.TimeStamp = DateTime.Now;
                    index.Data = DateTime.Now;
                    index.UserId = WebSecurity.CurrentUserId;
                    index.IndexDiffBaieRece = index.IndexNewBaieRece - index.IndexOldBaieRece;
                    index.IndexDiffBaieCalda = index.IndexNewBaieCalda - index.IndexOldBaieCalda;
                    index.IndexDiffBucatarieRece = index.IndexNewBucatarieRece - index.IndexOldBucatarieRece;
                    index.IndexDiffBucatarieCalda = index.IndexNewBucatarieCalda - index.IndexOldBucatarieCalda;

                    _context.Index.Add(index);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Eroare neprevazuta. Contactati administratorul.");
            }

            ////ViewBag.UserId = new SelectList(_context.UserProfile, "UserId", "UserName", index.UserId);
            SetYearList();
            SetMonthList();
            return View(index as IndexModel);
        }



        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}