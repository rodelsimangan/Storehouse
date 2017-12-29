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
    public class TermsofUseController : BaseController
    {
        StorehouseDBContext db = new StorehouseDBContext();
        string tenantId = string.Empty;

        public ActionResult Index()
        {
            db = new StorehouseDBContext();
            if (TempData["TenantId"] != null)
                tenantId = TempData["TenantId"].ToString();

            var termsofuse = (from t in db.TermsofUse
                              where t.Id != null
                              && t.TenantId == tenantId
                              select t).First();

            termsofuse.Markup = HttpUtility.HtmlDecode(termsofuse.Markup);

            ViewBag.Title = "TermsofUse";
            return View(termsofuse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(TermsofUse termsofuse)
        {
            if(ModelState.IsValid)
            {
                db = new StorehouseDBContext();               
                
                db.Entry(termsofuse).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "TermsofUse");
        }
    }
}
