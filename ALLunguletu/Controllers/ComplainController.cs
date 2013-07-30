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

namespace ALLunguletu.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class ComplainController : Controller
    {
        private Entities _context = new Entities();

        //
        // GET: /Complain/

        public ActionResult Index()
        {
            var complain = _context.Complain
                .Where(x => x.UserId == WebSecurity.CurrentUserId)
                .OrderByDescending(m => new { m.Data});

            return View(complain);
        }

        //
        // GET: /Complain/Details/5

        public ActionResult Details(int id = 0)
        {
            Complain complain = _context.Complain.Find(id);
            if (complain == null)
            {
                return HttpNotFound();
            }
            return View(complain);
        }

        //
        // GET: /Complain/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Complain/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ALLunguletu.Models.ComplainModel complain)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    complain.Data = DateTime.Now;
                    complain.UserId = WebSecurity.CurrentUserId;

                    _context.Complain.Add(new Complain() { 
                        Data = complain.Data, 
                        UserId = complain.UserId,
                        Subject = complain.Subject,
                        Description = complain.Description
                    });
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Eroare neprevazuta. Contactati administratorul.");
            }

            return View(complain as ComplainModel);
        }

        //
        // GET: /Complain/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Complain complain = _context.Complain.Find(id);
            if (complain == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(_context.UserProfile, "UserId", "UserName", complain.UserId);
            return View(complain);
        }

        //
        // POST: /Complain/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Complain complain)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(complain).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(_context.UserProfile, "UserId", "UserName", complain.UserId);
            return View(complain);
        }

        //
        // GET: /Complain/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Complain complain = _context.Complain.Find(id);
            if (complain == null)
            {
                return HttpNotFound();
            }
            return View(complain);
        }

        //
        // POST: /Complain/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Complain complain = _context.Complain.Find(id);
            _context.Complain.Remove(complain);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}