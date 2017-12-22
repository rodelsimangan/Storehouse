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
    public class SlidersController : BaseController
    {
        StorehouseDBContext db = new StorehouseDBContext();
        //
        // GET: /PortalHomePage/
        public ActionResult Index()
        {
            db = new StorehouseDBContext();
            var sliders = from s in db.Sliders
                          where s.IsDeleted == false
                          orderby s.Sequence
                          select s;

            if (sliders == null)
            {
                return HttpNotFound();
            }
            return View(sliders.AsEnumerable());

        }

        public ActionResult Add()
        {
            ViewBag.Title = "Sliders";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Sliders slider)
        {
            if (ModelState.IsValid)
            {
                db = new StorehouseDBContext();
                int seqCtr = 0;

                var sliderList = (from s in db.Sliders
                                  where s.IsDeleted == false
                                  select s.Sequence).ToList();
                if (sliderList.Count > 0)
                    seqCtr = sliderList.Max();

                slider.Id = Guid.NewGuid();
                slider.Sequence = seqCtr + 1;

                if (Request.Files["sliderImage"].ContentLength > 0)
                {
                    FileInfo sliderImageInfo = new FileInfo(Request.Files["sliderImage"].FileName);
                    string webRootPath = Server.MapPath("~");

                    string sliderImageFilename = string.Concat(ConfigurationManager.AppSettings["UploadFilePath"], "Sliders/", slider.Id.ToString(), sliderImageInfo.Extension);
                    Request.Files["sliderImage"].SaveAs(Server.MapPath(sliderImageFilename));
                    slider.SlideImage = sliderImageFilename;
                }

                if (Request.Files["sliderBackground"].ContentLength > 0)
                {
                    FileInfo sliderBackgroundInfo = new FileInfo(Request.Files["sliderBackground"].FileName);
                    string webRootPath = Server.MapPath("~");

                    string sliderBackgroundFilename = string.Concat(ConfigurationManager.AppSettings["UploadFilePath"], "Sliders/", ConfigurationManager.AppSettings["SliderBackgroundPrefix"], slider.Sequence, sliderBackgroundInfo.Extension);
                    Request.Files["sliderBackground"].SaveAs(Server.MapPath(sliderBackgroundFilename));
                    slider.SlideBackground = sliderBackgroundFilename;
                }

                db.Sliders.Add(slider);
                db.SaveChanges();

            }
            return RedirectToAction("Index", "Sliders");
        }
        public ActionResult Update(string id)
        {
            db = new StorehouseDBContext();

            var slider = (from s in db.Sliders
                           where s.Id == new Guid(id)
                           select s).First();

            ViewBag.Title = "Sliders";
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Sliders slider)
        {
            if (ModelState.IsValid)
            {
                db = new StorehouseDBContext();

                if (Request.Files["sliderImage"].ContentLength > 0)
                {
                    FileInfo sliderImageInfo = new FileInfo(Request.Files["sliderImage"].FileName);
                    string webRootPath = Server.MapPath("~");

                    string sliderImageFilename = string.Concat(ConfigurationManager.AppSettings["UploadFilePath"], "Sliders/", slider.Id.ToString(), sliderImageInfo.Extension);
                    Request.Files["sliderImage"].SaveAs(Server.MapPath(sliderImageFilename));
                    slider.SlideImage = sliderImageFilename;
                }

                if (Request.Files["sliderBackground"].ContentLength > 0)
                {
                    FileInfo sliderBackgroundInfo = new FileInfo(Request.Files["sliderBackground"].FileName);
                    string webRootPath = Server.MapPath("~");

                    string sliderBackgroundFilename = string.Concat(ConfigurationManager.AppSettings["UploadFilePath"], "Sliders/", ConfigurationManager.AppSettings["SliderBackgroundPrefix"], slider.Sequence, sliderBackgroundInfo.Extension);
                    Request.Files["sliderBackground"].SaveAs(Server.MapPath(sliderBackgroundFilename));
                    slider.SlideBackground = sliderBackgroundFilename;
                }

                db.Entry(slider).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Sliders");
        }
        public ActionResult Delete(string id)
        {
            db = new StorehouseDBContext();

            var slider = (from c in db.Sliders
                           where c.Id == new Guid(id)
                           select c).First();
            slider.IsDeleted = true;
            //slider.ModifiedById = User.Identity.GetUserId();
            //slider.DateModified = DateTime.Now;

            db.Entry(slider).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Sliders");
        }

        public ActionResult MoveUp(string id)
        {
            db = new StorehouseDBContext();

            var slider = (from c in db.Sliders
                           where c.Id == new Guid(id)
                           select c).First();
            if (slider.Sequence > 1)
            {

                var higherSlider = (from h in db.Sliders
                                     where h.Sequence < slider.Sequence && h.IsDeleted == false
                                     orderby h.Sequence descending
                                     select h).First();

                int higherSeq = higherSlider.Sequence;
                higherSlider.Sequence = slider.Sequence;
                //higherSlider.ModifiedById = User.Identity.GetUserId();
                //higherSlider.DateModified = DateTime.Now;
                db.Entry(higherSlider).State = EntityState.Modified;
                db.SaveChanges();

                slider.Sequence = higherSeq;
                //slider.ModifiedById = User.Identity.GetUserId();
                //slider.DateModified = DateTime.Now;
                db.Entry(slider).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Sliders");
        }
        public ActionResult MoveDown(string id)
        {
            db = new StorehouseDBContext();

            var slider = (from c in db.Sliders
                           where c.Id == new Guid(id)
                           select c).First();
            if (slider.Sequence < db.Sliders.Count())
            {
                var lowerSlider = (from l in db.Sliders
                                    where l.Sequence > slider.Sequence && l.IsDeleted == false
                                    orderby l.Sequence
                                    select l).First();

                int lowerSeq = lowerSlider.Sequence;
                lowerSlider.Sequence = slider.Sequence;
                //lowerSlider.ModifiedById = User.Identity.GetUserId();
                //lowerSlider.DateModified = DateTime.Now;
                db.Entry(lowerSlider).State = EntityState.Modified;
                db.SaveChanges();

                slider.Sequence = lowerSeq;
                //slider.ModifiedById = User.Identity.GetUserId();
                //slider.DateModified = DateTime.Now;
                db.Entry(slider).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Sliders");
        }
    }
}