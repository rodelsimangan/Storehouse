using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StorehouseAdmin.Models;

namespace StorehouseAdmin.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            
            StorehouseDBContext db = new StorehouseDBContext();
            var templates = from t in db.Templates
                            orderby t.Sequence
                            select t;
            ViewData["TemplateList"] = templates.ToList().AsEnumerable();            

            var feedback1 = from m in db.Feedbacks
                                where m.IsRead == false
                                select m;

            ViewBag.CountNew1 = feedback1.Count();
        }
	}
}