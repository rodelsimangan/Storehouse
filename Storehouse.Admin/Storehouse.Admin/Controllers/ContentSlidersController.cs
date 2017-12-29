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
    public class ContentSlidersController : BaseController
    {

        StorehouseDBContext db = new StorehouseDBContext();
        string tenantId = string.Empty;

        //
        // GET: /ContentSliders/
        public ActionResult Index()
        {
            if (TempData["TenantId"] != null)
                tenantId = TempData["TenantId"].ToString();
            //TempData.Keep();

            db = new StorehouseDBContext();
            var contentsliders = from s in db.ContentSliders
                                 where s.IsDeleted == false
                                 && s.TenantId == tenantId
                                 orderby s.Sequence
                                 select s;

            if (contentsliders == null)
            {
                return HttpNotFound();
            }
            return View(contentsliders.AsEnumerable());

        }

        public ActionResult Add()
        {
            ViewBag.Title = "ContentSliders";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ContentSliders contentsliders)
        {
            if (ModelState.IsValid)
            {
                db = new StorehouseDBContext();
                int seqCtr = 0;

                if (TempData["TenantId"] != null)
                    tenantId = TempData["TenantId"].ToString();

                var contentslidersList = (from s in db.ContentSliders
                                          where s.IsDeleted == false
                                          && s.TenantId == tenantId
                                          select s.Sequence).ToList();
                if (contentslidersList.Count > 0)
                    seqCtr = contentslidersList.Max();

                contentsliders.Id = Guid.NewGuid();
                contentsliders.Sequence = seqCtr + 1;


                if (Request.Files["contentslidersImage"].ContentLength > 0)
                {
                    FileInfo contentslidersImageInfo = new FileInfo(Request.Files["contentslidersImage"].FileName);
                    string webRootPath = Server.MapPath("~");

                    string contentslidersImageFilename = string.Concat(ConfigurationManager.AppSettings["UploadFilePath"], "ContentSliders/", contentsliders.Id.ToString(), contentslidersImageInfo.Extension);
                    Request.Files["contentslidersImage"].SaveAs(Server.MapPath(contentslidersImageFilename));
                    contentsliders.SlideImage = contentslidersImageFilename;
                }

                db.ContentSliders.Add(contentsliders);
                db.SaveChanges();

            }
            return RedirectToAction("Index", "ContentSliders");
        }
        public ActionResult Update(string id)
        {
            db = new StorehouseDBContext();

            var contentsliders = (from s in db.ContentSliders
                                  where s.Id == new Guid(id)
                                  select s).FirstOrDefault();

            ViewBag.Title = "ContentSliders";
            return View(contentsliders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ContentSliders contentsliders)
        {
            if (ModelState.IsValid)
            {
                db = new StorehouseDBContext();

                if (Request.Files["contentslidersImage"].ContentLength > 0)
                {
                    FileInfo contentslidersImageInfo = new FileInfo(Request.Files["contentslidersImage"].FileName);
                    string webRootPath = Server.MapPath("~");

                    string contentslidersImageFilename = string.Concat(ConfigurationManager.AppSettings["UploadFilePath"], "ContentSliders/", contentsliders.Id.ToString(), contentslidersImageInfo.Extension);
                    Request.Files["contentslidersImage"].SaveAs(Server.MapPath(contentslidersImageFilename));
                    contentsliders.SlideImage = contentslidersImageFilename;
                }

                db.Entry(contentsliders).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "ContentSliders");
        }
        public ActionResult Delete(string id)
        {
            db = new StorehouseDBContext();

            var contentsliders = (from c in db.ContentSliders
                                  where c.Id == new Guid(id)
                                  select c).FirstOrDefault();
            contentsliders.IsDeleted = true;
            //slider.ModifiedById = User.Identity.GetUserId();
            //slider.DateModified = DateTime.Now;

            db.Entry(contentsliders).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "ContentSliders");
        }

        public ActionResult MoveUp(string id)
        {
            db = new StorehouseDBContext();

            if (TempData["TenantId"] != null)
                tenantId = TempData["TenantId"].ToString();

            var contentsliders = (from c in db.ContentSliders
                                  where c.Id == new Guid(id)
                                  select c).FirstOrDefault();
            if (contentsliders.Sequence > 1)
            {

                var higherSlider = (from h in db.ContentSliders
                                    where h.Sequence < contentsliders.Sequence && h.IsDeleted == false
                                    && h.TenantId == tenantId
                                    orderby h.Sequence descending
                                    select h).First();

                int higherSeq = higherSlider.Sequence;
                higherSlider.Sequence = contentsliders.Sequence;
                //higherSlider.ModifiedById = User.Identity.GetUserId();
                //higherSlider.DateModified = DateTime.Now;
                db.Entry(higherSlider).State = EntityState.Modified;
                db.SaveChanges();

                contentsliders.Sequence = higherSeq;
                //slider.ModifiedById = User.Identity.GetUserId();
                //slider.DateModified = DateTime.Now;
                db.Entry(contentsliders).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "ContentSliders");
        }
        public ActionResult MoveDown(string id)
        {
            db = new StorehouseDBContext();

            if (TempData["TenantId"] != null)
                tenantId = TempData["TenantId"].ToString();

            var slidcontentsliderser = (from c in db.ContentSliders
                                        where c.Id == new Guid(id)
                                        select c).First();
            if (slidcontentsliderser.Sequence < db.ContentSliders.Count())
            {
                var lowerSlider = (from l in db.Sliders
                                   where l.Sequence > slidcontentsliderser.Sequence && l.IsDeleted == false
                                   && l.TenantId == tenantId
                                   orderby l.Sequence
                                   select l).First();

                int lowerSeq = lowerSlider.Sequence;
                lowerSlider.Sequence = slidcontentsliderser.Sequence;
                //lowerSlider.ModifiedById = User.Identity.GetUserId();
                //lowerSlider.DateModified = DateTime.Now;
                db.Entry(lowerSlider).State = EntityState.Modified;
                db.SaveChanges();

                slidcontentsliderser.Sequence = lowerSeq;
                //slider.ModifiedById = User.Identity.GetUserId();
                //slider.DateModified = DateTime.Now;
                db.Entry(slidcontentsliderser).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "ContentSliders");
        }
    }
}