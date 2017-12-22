using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StorehouseAdmin.Models;
using System.Configuration;
using System.IO;
using System.Data.Entity;

namespace StorehouseAdmin.Controllers
{
    [Authorize(Roles = "Tenant")]
    public class FeedbacksController : BaseController
    {
        StorehouseDBContext db = new StorehouseDBContext();
        public ActionResult Members(string Filter_Value, int? Page_No)
        {
            db = new StorehouseDBContext();
            var feedback = from s in db.Feedbacks
                          where s.IsMember == true
                          orderby s.From
                          select s;

            if (feedback == null)
            {
                return HttpNotFound();
            }

            int Size_Of_Page = 4;
            int No_Of_Page = (Page_No ?? 1);

            return View(db.Feedbacks.ToList());
        }

        public ActionResult NonMembers()
        {
            db = new StorehouseDBContext();
            var feedback = from s in db.Feedbacks
                           where s.IsMember == false
                           orderby s.From
                           select s;

            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(db.Feedbacks.ToList());
        }

        public ActionResult ViewEmailContent(string id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            else
            {
                Guid userid = new Guid(id);
                ViewBag.ID = userid;
                db = new StorehouseDBContext();
                var feedback = from s in db.Feedbacks
                               where s.Id == userid
                               select s;

                var updatedata = db.Feedbacks.Find(userid);
                updatedata.IsRead = true;
                db.Entry(updatedata).State = EntityState.Modified;
                db.SaveChanges();


                if (feedback == null)
                {
                    return HttpNotFound();
                }
                return View(feedback.AsEnumerable());
            }
           
        }
	}
}