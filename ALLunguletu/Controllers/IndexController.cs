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
            SetWaterList();
            ////ViewBag.Month = new SelectList(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }
            ////    .Select(x => new { value = x, text = x }),
            ////    "text", "value");
            return View();
        }

        private void SetWaterList()
        {
            var waterList = new KeyValuePair<int, string>[2];
            waterList[0] = new KeyValuePair<int, string>((int)SysWater.Rece, Enum.GetName(typeof(SysWater), SysWater.Rece));
            waterList[1] = new KeyValuePair<int, string>((int)SysWater.Calda, Enum.GetName(typeof(SysWater), SysWater.Calda));
            ViewBag.WaterList = waterList
                .Select(x => new
                {
                    WaterId = x.Key,
                    WaterName = x.Value
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
                if (ModelState.IsValid)
                {
                    if (index.IndexOld > index.IndexNew)
                    {
                        ModelState.AddModelError("", "'Indexul vechi' trebuie sa aiba valoare mai mica decat 'Indexul nou'.");
                        SetWaterList();
                        return View(index as IndexModel);
                    }

                    //new index allready exists for Year and Month
                    if (_context.Index
                        .Where(x => x.UserId == WebSecurity.CurrentUserId
                                    && x.IndexYear == index.IndexYear
                                    && x.IndexMonth == index.IndexMonth
                                    && x.WaterId == index.WaterId)
                        .Count() > 0)
                    {
                        ModelState.AddModelError("", string.Format("Pentru perioada '{0}/{1}' si tipul de apa '{2}', ati mai introdus valori Index. ", index.IndexYear, index.IndexMonth, (SysWater)index.WaterId));
                        SetWaterList();
                        return View(index as IndexModel);
                    }

                    index.TimeStamp = DateTime.Now;
                    index.Data = DateTime.Now;
                    index.UserId = WebSecurity.CurrentUserId;
                    index.IndexDiff = index.IndexNew - index.IndexOld;

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
            SetWaterList();
            return View(index as IndexModel);
        }



        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}