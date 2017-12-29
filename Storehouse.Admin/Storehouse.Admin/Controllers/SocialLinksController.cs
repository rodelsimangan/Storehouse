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
    public class SocialLinksController : BaseController
    {
        StorehouseDBContext db = new StorehouseDBContext();
        string tenantId = string.Empty;

        // GET: /SocialLinks/
        public ActionResult Index()
        {
            db = new StorehouseDBContext();
            if (TempData["TenantId"] != null)
                tenantId = TempData["TenantId"].ToString();

            var socialmedia = from s in db.SocialMediaLinks                              
                          where s.IsDeleted == false
                          && s.TenantId == tenantId
                          select s;

            if (socialmedia == null)
            {
                return HttpNotFound();
            }
            return View(socialmedia.AsEnumerable());
        }
        
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Title = "Social Media Links";
            PopulateSocialMedia();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SocialMediaLinks socialmedia)
        {
            if (ModelState.IsValid)
            {
                db = new StorehouseDBContext();
                socialmedia.Id = Guid.NewGuid();                
                db.SocialMediaLinks.Add(socialmedia);
                db.SaveChanges();

            }
            return RedirectToAction("Index", "SocialLinks");
        }

        public ActionResult Update(string id)
        {
            db = new StorehouseDBContext();

            var socialmedia = (from s in db.SocialMediaLinks
                          where s.Id == new Guid(id)
                          select s).FirstOrDefault();

            ViewBag.Title = "Social Media Links";
            PopulateSocialMedia(socialmedia.Name);
            return View(socialmedia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(SocialMediaLinks socialmedia)
        {
            if (ModelState.IsValid)
            {
                db = new StorehouseDBContext();

                db.Entry(socialmedia).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Sliders");
        }

        public ActionResult Delete(string id)
        {
            db = new StorehouseDBContext();

            var socialmedia = (from c in db.SocialMediaLinks
                          where c.Id == new Guid(id)
                          select c).FirstOrDefault();
            socialmedia.IsDeleted = true;
            //slider.ModifiedById = User.Identity.GetUserId();
            //slider.DateModified = DateTime.Now;

            db.Entry(socialmedia).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "SocialLinks");
        }

        #region Private Methods

        private void PopulateSocialMedia(object selectedSocialMedia = null)
        {
            if (TempData["TenantId"] != null)
                tenantId = TempData["TenantId"].ToString();

            var socialMediaList = from s in db.Lookups
                                  where s.Category == "SocialMedia" && s.IsDeleted == false
                                  && s.TenantId == tenantId
                                  select s;
            ViewBag.SocialMedia = new SelectList(socialMediaList, "Name", "Name", selectedSocialMedia);
        }

        #endregion
    }
}