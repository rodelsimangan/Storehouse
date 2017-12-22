using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSPortal.Models;
using Storehouse.Model;
using System.Web.Configuration;

namespace CMSPortal.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            RobinsonsDBContext db = new RobinsonsDBContext();
            var templates = from t in db.Templates
                            where t.IncludeInMenu == true
                            orderby t.Sequence
                            select t;
            ViewData["TemplateList"] = templates.ToList().AsEnumerable();

            var footer = from t in db.HomePageSettings
                         select t;

            ViewData["FooterList"] = footer.ToList().AsEnumerable();

            var contentlst = from z in db.Contents
                             where z.TempName.Contains("FAQ")
                             orderby z.Sequence
                             select z;

            ViewData["ContentList"] = contentlst.ToList().AsEnumerable();

            var contentlst2 = from z in db.Contents
                             where z.TempName.Contains("Events and Promos")
                             orderby z.Sequence
                             select z;

            ViewData["ContentList2"] = contentlst2.ToList().AsEnumerable();

            var termsofuselist = from z in db.TermsofUse
                             where z.Id != null
                             select z;

            ViewData["TermsofUseList"] = termsofuselist.ToList().AsEnumerable();

            //var WhatsNewP = from x in db.Contents
            //               where x.IsPublished == true && x.IncludeInHomePage == true && x.TempName.Contains("Partners")
            //               orderby x.Sequence descending
            //               select x;

            //ViewData["WhatsNewP"] = WhatsNewP.ToList().AsEnumerable();

            //var WhatsNewEP = from x in db.Contents
            //                 where x.IsPublished == true && x.IncludeInHomePage == true && x.TempName.Contains("Events and Promos")
            //               orderby x.Sequence descending
            //               select x;

            //ViewData["WhatsNewEP"] = WhatsNewEP.ToList().AsEnumerable();

            //var WhatsNewFAQ = from x in db.Contents
            //                 where x.IsPublished == true && x.IncludeInHomePage == true && x.TempName.Contains("FAQ")
            //                 orderby x.Sequence descending
            //                 select x;

            //ViewData["WhatsNewFAQ"] = WhatsNewFAQ.ToList().AsEnumerable();

            //var WhatsNewStores = from x in db.Contents
            //                  where x.IsPublished == true && x.IncludeInHomePage == true && x.TempName.Contains("Stores")
            //                  orderby x.Sequence descending
            //                  select x;

            //ViewData["WhatsNewStores"] = WhatsNewStores.ToList().AsEnumerable();

            var contentsliders = from x in db.ContentSliders
                                 where x.IsDeleted == false
                                 orderby x.Sequence
                                 select x;

            ViewBag.ContentSliders = contentsliders.ToList().AsEnumerable();

            var sociallinks = from s in db.SocialMediaLinks
                              where s.IsDeleted == false
                              select s;
            ViewData["SocialLinks"] = sociallinks.ToList().AsEnumerable(); 

            var facebooklink =(from s in db.SocialMediaLinks
                              where s.IsDeleted == false && s.Name == "facebook"
                              select s).FirstOrDefault();
            
            ViewData["FacebookLink"] = facebooklink.Value;

            var homepagesettings = from h in db.HomePageSettings
                                   select h;
            int i = sociallinks.Count();
            ViewData["HeaderLogo"] = homepagesettings.First().HeaderLogo;
            ViewData["FooterLogo"] = homepagesettings.First().FooterLogo;
            ViewData["CompanyName"] = homepagesettings.First().CompanyName;
            ViewData["CompanyAddress"] = homepagesettings.First().CompanyAddress;
            ViewData["CompanyPhone"] = homepagesettings.First().CompanyPhone;
            ViewData["CompanyEmail"] = homepagesettings.First().CompanyEmail;
            //ViewData["HeaderLogo"] = homepagesettings.First().;            

        }
    }
}