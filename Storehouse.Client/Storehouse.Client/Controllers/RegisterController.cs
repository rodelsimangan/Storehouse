using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Storehouse.Model;
using System.Web.Mvc;

namespace CMSPortal.Controllers
{
    public class RegisterController : Controller
    {

        public ActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Register(RegisterController Register)
        {
            return View();
        }
    }
}