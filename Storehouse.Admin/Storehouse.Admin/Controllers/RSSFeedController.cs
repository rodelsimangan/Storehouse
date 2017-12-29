using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using System.Data.Entity;
using StorehouseAdmin.Models;
using Storehouse.Model;

namespace StorehouseAdmin.Controllers
{
    [Authorize(Roles = "Tenant")]
    public class RSSFeedController : BaseController
    {
        StorehouseDBContext db = new StorehouseDBContext();
        string tenantId = string.Empty;

        public ActionResult Index()
        {
            db = new StorehouseDBContext();

            if (TempData["TenantId"] != null)
                tenantId = TempData["TenantId"].ToString();

            var temp = from s in db.Templates
                          where s.Name != ""
                          && s.TenantId == tenantId
                          select s;

            if (temp == null)
            {
                return HttpNotFound();
            }

            //int Size_Of_Page = 4;
            //int No_Of_Page = (Page_No ?? 1);

            return View(db.Templates.ToList());
        }

        public ActionResult Update(string id)
        {
            db = new StorehouseDBContext();

            var templates = (from s in db.Templates
                                  where s.Id == new Guid(id)
                                  select s).FirstOrDefault();

            ViewBag.Title = "RSSFeed";
            return View(templates);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Templates templates)
        {
            if (ModelState.IsValid)
            {
                db = new StorehouseDBContext();

                db.Entry(templates).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "RSSFeed");
        }
	}
}