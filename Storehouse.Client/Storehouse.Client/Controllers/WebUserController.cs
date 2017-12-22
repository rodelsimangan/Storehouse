using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using System.Data.Entity;
using CMSPortal.Models;
using Storehouse.Model;

namespace CMSPortal.Controllers
{
    public class WebUserController : Controller
    {
        RobinsonsDBContext db = new RobinsonsDBContext();
        public ActionResult Index()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult Add(WebUser WebUser)
        {
            if (ModelState.IsValid)
            {
                db = new RobinsonsDBContext();
                WebUser.Id = Guid.NewGuid();

                db.WebUser.Add(WebUser);
                db.SaveChanges();
            }
            else
            {
                TempData["ErrMessage"] = "Registration Unsuccessful! Check fields with (*) ";
                TempData["ShowSignUp"] = "Grid";
            }

            return RedirectToAction("MyRewards", "Home");
        }
	}
}