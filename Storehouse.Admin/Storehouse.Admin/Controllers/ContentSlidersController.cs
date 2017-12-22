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
    [Authorize(Roles ="Tenant")]
    public class ContentSlidersController : BaseController
    {

        StorehouseDBContext db = new StorehouseDBContext();
        //
        // GET: /ContentSliders/
        public ActionResult Index()
        {
            db = new StorehouseDBContext();
            var contentsliders = from s in db.ContentSliders
                          where s.IsDeleted == false
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

                var contentslidersList = (from s in db.ContentSliders
                                  where s.IsDeleted == false
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
                          select s).First();

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
                          select c).First();
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

            var contentsliders = (from c in db.ContentSliders
                          where c.Id == new Guid(id)
                          select c).First();
            if (contentsliders.Sequence > 1)
            {

                var higherSlider = (from h in db.ContentSliders
                                    where h.Sequence < contentsliders.Sequence && h.IsDeleted == false
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

            var slidcontentsliderser = (from c in db.ContentSliders
                          where c.Id == new Guid(id)
                          select c).First();
            if (slidcontentsliderser.Sequence < db.ContentSliders.Count())
            {
                var lowerSlider = (from l in db.Sliders
                                   where l.Sequence > slidcontentsliderser.Sequence && l.IsDeleted == false
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