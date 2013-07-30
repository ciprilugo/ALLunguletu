using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lugo.EntityFramework;
using ALLunguletu.Filters;

namespace ALLunguletu.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class DecisionController : Controller
    {
        private Entities _context = new Entities();

        //
        // GET: /Decision/

        public ActionResult Index()
        {
            var decisionList = _context.Decision
                .Include(d => d.UserProfile)
                .OrderByDescending(x=>x.Data);

            return View(decisionList.ToList());
        }

       

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}