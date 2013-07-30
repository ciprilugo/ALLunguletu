
using Lugo.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ALLunguletu.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        #region Properties
       
        private Entities _context;
        public Entities Context
        {
            get
            {
                if (_context == null)
                    return new Entities();

                return _context;
            }
        } 
        #endregion

        
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult About()
        {
            return View();
        }
       
    }
}
