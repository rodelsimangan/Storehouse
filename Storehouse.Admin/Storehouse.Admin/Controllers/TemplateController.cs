using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using StorehouseAdmin.Models;
using Storehouse.Model;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.ServiceModel.Syndication;

namespace StorehouseAdmin.Controllers
{

    [Authorize(Roles = "Tenant")]
    public class TemplateController : BaseController
    {
        //
        // GET: /Template/
        StorehouseDBContext db = new StorehouseDBContext();
        string path = System.Configuration.ConfigurationManager.AppSettings["XMLPath"].ToString();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contents(string id)
        {
            db = new StorehouseDBContext();
            var contents = from c in db.Contents
                           where c.TemplateId == id && c.IsDeleted == false && c.ParentId == null
                           orderby c.PublishedDate descending
                           select c;

            List<Contents> contentList = new List<Contents>();
            foreach (Contents content in contents.ToList())
            {
                contentList.Add(content);
                string parentId = content.Id.ToString();
                var childcontents = from cc in db.Contents
                                    where cc.ParentId == parentId && cc.IsDeleted == false
                                    orderby cc.Id, cc.Sequence
                                    select cc;
                foreach (Contents childcontent in childcontents.ToList())
                {
                    contentList.Add(childcontent);
                }
            }

            var template = (from t in db.Templates
                            where t.Id == new Guid(id)
                            select t.Name).AsEnumerable().Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });
            ViewBag.Title = template.FirstOrDefault().Text;
            ViewBag.ID = id;

            if (contentList == null)
            {
                return HttpNotFound();
            }
            return View(contentList.AsEnumerable());

        }

        [HttpPost]
        public ActionResult Contents()
        {
            string[] getid = Request.PhysicalPath.Split('\\');
            string id = getid.Last();
            string Keywords = Request.Form["Keywords"].ToString();
            ViewBag.Keywords = Keywords;

            db = new StorehouseDBContext();
            var contents = from c in db.Contents
                           where c.TemplateId == id && c.IsDeleted == false && c.ParentId == null && (c.Name.Contains(Keywords) || c.Description.Contains(Keywords) || c.Markup.Contains(Keywords))
                           orderby c.Id, c.Sequence descending
                           select c;

            List<Contents> contentList = new List<Contents>();
            foreach (Contents content in contents.ToList())
            {
                contentList.Add(content);
                string parentId = content.Id.ToString();
                var childcontents = from cc in db.Contents
                                    where cc.ParentId == parentId && cc.IsDeleted == false && (cc.Name.Contains(Keywords) || cc.Description.Contains(Keywords) || cc.Markup.Contains(Keywords))
                                    orderby cc.Id, cc.Sequence descending
                                    select cc;
                foreach (Contents childcontent in childcontents.ToList())
                {
                    contentList.Add(childcontent);
                }
            }

            var template = (from t in db.Templates
                            where t.Id == new Guid(id)
                            select t.Name).AsEnumerable().Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });
            ViewBag.Title = template.FirstOrDefault().Text;
            ViewBag.ID = id;

            if (contentList == null)
            {
                return HttpNotFound();
            }
            return View(contentList.AsEnumerable());
        }
        public ActionResult NewContent(string id, bool isChild)
        {
            db = new StorehouseDBContext();
            if (!isChild)
            {
                var template = (from t in db.Templates
                                where t.Id == new Guid(id)
                                select t.Name).AsEnumerable().Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });
                ViewBag.Title = template.FirstOrDefault().Text;
                ViewBag.ID = id;
                ViewBag.isChild = isChild;
            }
            else
            {
                var content = (from c in db.Contents
                               where c.Id == new Guid(id)
                               select c).First();

                var template = (from t in db.Templates
                                where t.Id == new Guid(content.TemplateId)
                                select t.Name).AsEnumerable().Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });

                ViewBag.Title = string.Concat(template.FirstOrDefault().Text, " >> ", content.Name);
                ViewBag.ID = id;
                ViewBag.isChild = isChild;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewContent(Contents content, string id, bool isChild, string title, string feedType, string feedname, string feedid)
        {
            if (ModelState.IsValid)
            {
                db = new StorehouseDBContext();
                int seqCtr = 0;
                if (!content.IsParent)
                {
                    var contentList = (from c in db.Contents
                                       where c.IsDeleted == false
                                       select c.Sequence).ToList();
                    if (contentList.Count > 0)
                        seqCtr = contentList.Max();
                }

                if (isChild)
                {
                    content.ParentId = id;

                    var parentContent = (from p in db.Contents
                                         where p.Id == new Guid(id)
                                         select p).First();

                    var gettemplate = (from g in db.Templates
                                       where g.Id == new Guid(parentContent.TemplateId)
                                       select g).First();
                    content.TemplateId = gettemplate.Id.ToString();
                }
                else
                    content.TemplateId = id;

                content.Id = Guid.NewGuid();
                content.CreatedById = User.Identity.GetUserId();
                content.DateCreated = DateTime.Now;
                content.ModifiedById = User.Identity.GetUserId();
                content.DateModified = DateTime.Now;

                var gettmpname = (from t in db.Templates
                                  where t.Id == new Guid(id)
                                  select t.Name).ToList();

                var template = (from t in db.Templates
                                where t.Id == new Guid(content.TemplateId)
                                select t.Name).AsEnumerable().Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });

                ViewBag.Title = template.FirstOrDefault().Text;
                title = ViewBag.Title;

                //content.TempName = content.Name + '-' + title + " Feed.xml";
                content.TempName = title + " Feed.xml";

                if (Request.Files["iconFile"] != null && Request.Files["iconFile"].ContentLength > 0)
                {
                    FileInfo info = new FileInfo(Request.Files["iconFile"].FileName);
                    string webRootPath = Server.MapPath("~");

                    string newfilename = string.Concat(ConfigurationManager.AppSettings["UploadFilePath"], "Icons/", content.Id.ToString(), info.Extension);
                    Request.Files["iconFile"].SaveAs(Server.MapPath(newfilename));
                    content.Icon = newfilename;
                }

                if (Request.Files["imageFile"] != null && Request.Files["imageFile"].ContentLength > 0)
                {
                    FileInfo info = new FileInfo(Request.Files["imageFile"].FileName);
                    string webRootPath = Server.MapPath("~");

                    string newfilename = string.Concat(ConfigurationManager.AppSettings["UploadFilePath"], "Contents/", content.Id.ToString(), info.Extension);
                    Request.Files["imageFile"].SaveAs(Server.MapPath(newfilename));
                    content.Image = newfilename;
                }



                content.Sequence = seqCtr + 1;
                db.Contents.Add(content);
                db.SaveChanges();

                string file = Server.MapPath("") + title + " Feed.xml";

                UpdateFeed(file, feedType, path, title, feedname, feedid);

            }
            //@Html.ActionLink("Contents", "Template", new { id = content.TemplateId };
            //return View(content);
            return RedirectToAction("Contents", "Template", new { id = content.TemplateId });
        }

        public ActionResult UpdateContent(string id)
        {
            db = new StorehouseDBContext();

            var content = (from c in db.Contents
                           where c.Id == new Guid(id)
                           select c).First();

            content.Markup = HttpUtility.HtmlDecode(content.Markup);
            var template = (from t in db.Templates
                            where t.Id == new Guid(content.TemplateId)
                            select t.Name).AsEnumerable().Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });

            if (content.ParentId != null)
            {
                var parentcontent = (from p in db.Contents
                                     where p.Id == new Guid(content.ParentId)
                                     select p).First();
                ViewBag.Title = string.Concat(template.FirstOrDefault().Text, " >> ", parentcontent.Name);
            }
            else
                ViewBag.Title = template.FirstOrDefault().Text;
            ViewBag.ID = content.TemplateId;
            return View(content);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateContent(Contents content, string id, string file, string feedType, string feedname, string feedid)
        {
            if (ModelState.IsValid)
            {
                db = new StorehouseDBContext();

                content.ModifiedById = User.Identity.GetUserId();
                content.DateModified = DateTime.Now;

                var gettmpname = (from t in db.Templates
                                  where t.Id == new Guid(id)
                                  select t.Name).ToList();


                var template = (from t in db.Templates
                                where t.Id == new Guid(content.TemplateId)
                                select t.Name).AsEnumerable().Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });

                ViewBag.Title = template.FirstOrDefault().Text;
                string title = ViewBag.Title;

                content.TempName = title + " Feed.xml";

                if (Request.Files["iconFile"] != null && Request.Files["iconFile"].ContentLength > 0)
                {
                    FileInfo info = new FileInfo(Request.Files["iconFile"].FileName);
                    string webRootPath = Server.MapPath("~");

                    string newfilename = string.Concat(ConfigurationManager.AppSettings["UploadFilePath"], "Icons/", content.Id.ToString(), info.Extension);
                    Request.Files["iconFile"].SaveAs(Server.MapPath(newfilename));
                    content.Icon = newfilename;
                }

                if (Request.Files["imageFile"].ContentLength > 0)
                {
                    FileInfo info = new FileInfo(Request.Files["imageFile"].FileName);
                    string webRootPath = Server.MapPath("~");

                    string newfilename = string.Concat(ConfigurationManager.AppSettings["UploadFilePath"], "Contents/", content.Id.ToString(), info.Extension);
                    Request.Files["imageFile"].SaveAs(Server.MapPath(newfilename));

                    content.Image = newfilename;
                }

                db.Entry(content).State = EntityState.Modified;
                db.SaveChanges();

                var getid = from z in db.Templates
                            where z.Name == title
                            select z;

                foreach (var get in getid)
                {
                    feedname = get.RSSFeedFileName;
                }

                file = Server.MapPath("") + feedname + ".xml";
                UpdateFeed(file, feedType, path, title, feedname, feedid);
            }
            //@(Url.Action("Contents", "Template", new { id = item.Id }))"
            //return View(content);
            return RedirectToAction("Contents", "Template", new { id = content.TemplateId });
        }

        public ActionResult AddSocialLinks(string id, string ans)
        {
            db = new StorehouseDBContext();

            var content = (from c in db.Contents
                           where c.Id == new Guid(id)
                           select c).First();
            if (ans == "yes")
                content.AddSocialLinks = true;
            else
                content.AddSocialLinks = false;

            content.ModifiedById = User.Identity.GetUserId();
            content.DateModified = DateTime.Now;
            db.Entry(content).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Contents", "Template", new { id = content.TemplateId });
        }

        public ActionResult PushToRssFeed(string id, string ans)
        {
            db = new StorehouseDBContext();

            var content = (from c in db.Contents
                           where c.Id == new Guid(id)
                           select c).First();
            if (ans == "yes")
                content.PushToNewsFeed = true;
            else
                content.PushToNewsFeed = false;

            content.ModifiedById = User.Identity.GetUserId();
            content.DateModified = DateTime.Now;
            db.Entry(content).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Contents", "Template", new { id = content.TemplateId });
        }

        public ActionResult PublishContent(string id)
        {
            db = new StorehouseDBContext();

            var content = (from c in db.Contents
                           where c.Id == new Guid(id)
                           select c).First();
            content.IsPublished = true;
            content.PublishedDate = DateTime.Now;
            content.ModifiedById = User.Identity.GetUserId();
            content.DateModified = DateTime.Now;

            db.Entry(content).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Contents", "Template", new { id = content.TemplateId });
        }

        public ActionResult IncludeInHomepage(string id, string ans)
        {
            db = new StorehouseDBContext();

            var content = (from c in db.Contents
                           where c.Id == new Guid(id)
                           select c).First();
            if (ans == "yes")
                content.IncludeInHomePage = true;
            else
                content.IncludeInHomePage = false;

            content.ModifiedById = User.Identity.GetUserId();
            content.DateModified = DateTime.Now;
            db.Entry(content).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Contents", "Template", new { id = content.TemplateId });
        }

        public ActionResult DeleteContent(string id)
        {
            db = new StorehouseDBContext();

            var content = (from c in db.Contents
                           where c.Id == new Guid(id)
                           select c).First();
            content.IsDeleted = true;
            content.ModifiedById = User.Identity.GetUserId();
            content.DateModified = DateTime.Now;

            db.Entry(content).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Contents", "Template", new { id = content.TemplateId });
        }

        public ActionResult MoveUp(string id)
        {
            db = new StorehouseDBContext();

            var content = (from c in db.Contents
                           where c.Id == new Guid(id)
                           select c).First();
            if (content.Sequence > 1)
            {

                var higherContent = (from h in db.Contents
                                     where h.Sequence < content.Sequence && h.IsDeleted == false
                                     orderby h.Sequence descending
                                     select h).First();

                int higherSeq = higherContent.Sequence;
                higherContent.Sequence = content.Sequence;
                higherContent.ModifiedById = User.Identity.GetUserId();
                higherContent.DateModified = DateTime.Now;
                db.Entry(higherContent).State = EntityState.Modified;
                db.SaveChanges();

                content.Sequence = higherSeq;
                content.ModifiedById = User.Identity.GetUserId();
                content.DateModified = DateTime.Now;
                db.Entry(content).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Contents", "Template", new { id = content.TemplateId });
        }
        public ActionResult MoveDown(string id)
        {
            db = new StorehouseDBContext();

            var content = (from c in db.Contents
                           where c.Id == new Guid(id)
                           select c).First();
            if (content.Sequence < db.Contents.Count())
            {
                var lowerContent = (from l in db.Contents
                                    where l.Sequence > content.Sequence && l.IsDeleted == false
                                    orderby l.Sequence
                                    select l).First();

                int lowerSeq = lowerContent.Sequence;
                lowerContent.Sequence = content.Sequence;
                lowerContent.ModifiedById = User.Identity.GetUserId();
                lowerContent.DateModified = DateTime.Now;
                db.Entry(lowerContent).State = EntityState.Modified;
                db.SaveChanges();

                content.Sequence = lowerSeq;
                content.ModifiedById = User.Identity.GetUserId();
                content.DateModified = DateTime.Now;
                db.Entry(content).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Contents", "Template", new { id = content.TemplateId });
        }

        // RSS Bulilder starts here        
        public static void UpdateFeed(string file, string feedType, string path, string title, string feedname, string feedid)
        {

            StorehouseDBContext db = new StorehouseDBContext();

            var getid = from z in db.Templates
                        where z.Name == title
                        select z;

            foreach (var get in getid)
            {
                feedname = get.RSSFeedFileName;
                feedid = get.Id.ToString();
            }

            // path = path + title + " Feed.xml";
            //System.IO.File.WriteAllText(@"c:\temp", file);
            var feed = new SyndicationFeed();
            feed.Title = new TextSyndicationContent(title);

            feed.Links.Add(new SyndicationLink(new Uri(string.Concat(System.Configuration.ConfigurationManager.AppSettings["XMLPath2"], feedname, ".xml"))));
            feed.LastUpdatedTime = new DateTimeOffset(DateTime.Now);

            var items = new List<SyndicationItem>();

            foreach (var feedItem in db.Contents)
            {
                if (feedItem.TemplateId == feedid)
                {
                    var item = new SyndicationItem();

                    item.Title = new TextSyndicationContent(feedItem.Name);
                    item.Summary = new TextSyndicationContent(feedItem.Description);

                    //var contents = new TextSyndicationContent(feedItem.Markup, TextSyndicationContentKind.Html);
                    var contents = new TextSyndicationContent(System.Web.HttpUtility.HtmlDecode(feedItem.Markup), TextSyndicationContentKind.Html);
                    item.Content = contents;

                    items.Add(item);
                }
            }

            feed.Items = items;

            //var template = (from t in db.Templates
            //                where t.Id == new Guid(content.TemplateId)
            //                select t.Name).AsEnumerable().Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });

            var directory = Path.GetDirectoryName(file);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            feedType = "rss|atom";

            var types = feedType.Split('|');

            foreach (var item in types)
            {
                if (item.ToLower().Equals("rss"))
                {
                    WriteToRss(file, feed);
                }

                if (item.ToLower().Equals("atom"))
                {
                    var extension = Path.GetExtension(file).ToLower();
                    file = file.Replace(extension, ".atom");

                    WriteToAtom(file, feed);
                }
            }
        }

        private static void WriteToRss(string file, SyndicationFeed feed)
        {
            using (var fs = new FileStream(file, FileMode.Create))
            {
                using (var w = new StreamWriter(fs, Encoding.UTF8))
                {
                    var xs = new XmlWriterSettings();
                    xs.Indent = true;
                    xs.ConformanceLevel = ConformanceLevel.Auto;

                    using (var xw = XmlWriter.Create(w, xs))
                    {
                        xw.WriteStartDocument();
                        var formatter = new Rss20FeedFormatter(feed);
                        formatter.WriteTo(xw);
                        xw.Close();
                    }
                }
            }
        }
        private static void WriteToAtom(string file, SyndicationFeed feed)
        {
            using (var fs = new FileStream(file, FileMode.Create))
            {
                using (var w = new StreamWriter(fs, Encoding.UTF8))
                {
                    var xs = new XmlWriterSettings();
                    xs.Indent = true;
                    xs.ConformanceLevel = ConformanceLevel.Auto;

                    using (XmlWriter xw = XmlWriter.Create(w, xs))
                    {
                        xw.WriteStartDocument();
                        var formatter = new Atom10FeedFormatter(feed);
                        formatter.WriteTo(xw);
                        xw.Close();
                    }
                }
            }
        }
    }
}