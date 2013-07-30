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
    [Authorize]
    [InitializeSimpleMembership]
    public class DetailController : Controller
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
        
        //
        // GET: /Detail/

        public ActionResult Index()
        {

            var userInfo = Context.UserInfo.Where(x => x.UserId == WebSecurity.CurrentUserId).FirstOrDefault();

            return View(userInfo);
        }

    }
}
