using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using CMSPortal.Models;
using Storehouse.Model;

namespace CMSPortal.Controllers
{
    public class LearnMoreController : BaseController
    {
        RobinsonsDBContext db = new RobinsonsDBContext();
        //
        // GET: /LearnMore/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Partners(string id)
        {
            db = new RobinsonsDBContext();
            var contents = from c in db.Contents
                           where c.TemplateId == id && c.IsDeleted == false
                           orderby c.PublishedDate descending
                           select c;

            List<dynamic> Partners = new List<dynamic>();
            foreach (Contents content in contents)
            {
                dynamic Partner = new ExpandoObject();
                Partner.Name = content.Name;
                Partner.Image = content.Image;
                Partner.Markup = HttpUtility.HtmlDecode(content.Markup);
                Partners.Add(Partner);
            }
            return View(Partners.AsEnumerable());

        }

        public ActionResult EventsAndPromos(string id)
        {
            db = new RobinsonsDBContext();
            var contents = from c in db.Contents
                           where c.TemplateId == id && c.IsDeleted == false
                           orderby c.PublishedDate descending
                           select c;

            List<dynamic> Events = new List<dynamic>();
            foreach (Contents content in contents)
            {
                dynamic Event = new ExpandoObject();
                Event.Id = content.Id;
                Event.Name = content.Name;
                Event.Description = content.Description;
                Event.Image = content.Image;
                Event.Markup = HttpUtility.HtmlDecode(content.Markup);
                Events.Add(Event);                           
            }
            return View(Events.AsEnumerable());

        }

        public ActionResult FAQAndTC(string id)
        {
            db = new RobinsonsDBContext();
            var contents = from c in db.Contents
                           where c.TemplateId == id && c.IsDeleted == false
                           orderby c.Sequence
                           select c;

            List<dynamic> Faqs = new List<dynamic>();
            foreach (Contents content in contents)
            {
                dynamic Faq = new ExpandoObject();
                Faq.Id = content.Id;
                Faq.Name = content.Name;
                Faq.Markup = HttpUtility.HtmlDecode(content.Markup);
                Faqs.Add(Faq);
            }
            return View(Faqs.AsEnumerable());

        }

	}
}