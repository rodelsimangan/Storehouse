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
    public class OtherSettingsController : BaseController
    {
        //
        // GET: /HomePageSetting/
        StorehouseDBContext db = new StorehouseDBContext();
        string tenantId = string.Empty;

        public ActionResult Index()
        {
            db = new StorehouseDBContext();

            if (TempData["TenantId"] != null)
                tenantId = TempData["TenantId"].ToString();

            var homepagesettings = (from h in db.HomePageSettings
                                    where h.TenantId == tenantId
                                    select h).FirstOrDefault();

            if (homepagesettings == null)
            {
                return HttpNotFound();
            }
            ViewBag.Title = "Other Settings";
            return View(homepagesettings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HomePageSettings homepagesettings)
        {
            if (ModelState.IsValid)
            {
                db = new StorehouseDBContext();

                if (Request.Files["headerLogo"].ContentLength > 0)
                {
                    FileInfo headerlogo = new FileInfo(Request.Files["headerLogo"].FileName);
                    string webRootPath = Server.MapPath("~");

                    string headerlogoFilename = string.Concat(ConfigurationManager.AppSettings["UploadFilePath"], "Homepage/headerlogo", headerlogo.Extension);
                    Request.Files["headerLogo"].SaveAs(Server.MapPath(headerlogoFilename));
                    homepagesettings.HeaderLogo = headerlogoFilename;
                }

                if (Request.Files["footerLogo"].ContentLength > 0)
                {
                    FileInfo footerlogo = new FileInfo(Request.Files["footerLogo"].FileName);
                    string webRootPath = Server.MapPath("~");

                    string footerlogoFilename = string.Concat(ConfigurationManager.AppSettings["UploadFilePath"], "Homepage/footerlogo", footerlogo.Extension);
                    Request.Files["footerLogo"].SaveAs(Server.MapPath(footerlogoFilename));
                    homepagesettings.FooterLogo = footerlogoFilename;
                }

                db.Entry(homepagesettings).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

    }
}