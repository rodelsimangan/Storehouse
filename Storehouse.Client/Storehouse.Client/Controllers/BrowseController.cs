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
    public class BrowseController : BaseController
    {
        RobinsonsDBContext db = new RobinsonsDBContext();
        public ActionResult Articles(string id)
        {
            db = new RobinsonsDBContext();
            dynamic articles = new ExpandoObject();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var template = (from t in db.Templates
                                    where t.Id == new Guid(id)
                                    select t).First();
                    articles.Template = template;

                    string templateId = template.Id.ToString();
                    var contents = from c in db.Contents
                                   where c.IsPublished == true && c.IsDeleted == false && c.TemplateId == templateId
                                   orderby c.PublishedDate descending
                                   select c;
                    articles.Contents = contents.ToList().AsEnumerable();
                }
            }
            catch
            {
                articles.Template = new Templates();
                articles.Contents = new Contents[] { new Contents() };
            }

            return View(articles);

        }

        public ActionResult RobinsonsStores(string id)
        {
            db = new RobinsonsDBContext();
            dynamic articles = new ExpandoObject();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var template = (from t in db.Templates
                                    where t.Id == new Guid(id)
                                    select t).First();
                    articles.Template = template;

                    string templateId = template.Id.ToString();
                    var contents = from c in db.Contents
                                   where c.IsPublished == true && c.IsDeleted == false && c.TemplateId == templateId
                                   orderby c.PublishedDate
                                   select c;
                    articles.Contents = contents;

                }
            }
            catch
            {
                articles.Template = new Templates();
                articles.Contents = new Contents[] { new Contents() };
            }
            return View(articles);

        }
        public ActionResult FullArticle(string id)
        {
            db = new RobinsonsDBContext();
            dynamic fullarticle = new ExpandoObject();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var content = (from c in db.Contents
                                   where c.Id == new Guid(id)
                                   select c).First();
                    fullarticle.Content = content;

                    var template = (from t in db.Templates
                                    where t.Id == new Guid(content.TemplateId)
                                    select t).First();
                    fullarticle.Template = template;
                }
            }
            catch
            {
                fullarticle.Template = new Templates();
                fullarticle.Content = new Contents();
            }
            return View(fullarticle);
        }
        public ActionResult Lists(string id)
        {
            db = new RobinsonsDBContext();
            dynamic lists = new ExpandoObject();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var template = (from t in db.Templates
                                    where t.Id == new Guid(id)
                                    select t).First();
                    lists.Template = template;
                    string templateId = template.Id.ToString();
                    var contents = from c in db.Contents
                                   where c.IsPublished == true && c.IsDeleted == false && c.TemplateId == templateId
                                   orderby c.PublishedDate
                                   select c;
                    lists.Contents = contents;

                }
            }
            catch
            {
                lists.Template = new Templates();
                lists.Contents = new Contents[] { new Contents() };
            }
            return View(lists);

        }
        public ActionResult Collapsables(string id)
        {
            db = new RobinsonsDBContext();
            dynamic lists = new ExpandoObject();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var template = (from t in db.Templates
                                    where t.Id == new Guid(id)
                                    select t).First();
                    lists.Template = template;
                    string templateId = template.Id.ToString();
                    var contents = from c in db.Contents
                                   where c.IsPublished == true && c.IsDeleted == false && c.TemplateId == templateId
                                   orderby c.PublishedDate
                                   select c;
                    lists.Contents = contents;

                }
            }
            catch
            {
                lists.Template = new Templates();
                lists.Contents = new Contents[] { new Contents() };
            }
            return View(lists);

        }
        public ActionResult Results(string Keywords)
        {
            dynamic lists = SearchKeyword(Keywords);
            ViewBag.Keywords = Keywords;
            ViewBag.Message = string.Format("Result(s) for {0}", Keywords);
            return View(lists);
        }
        [HttpPost]
        public ActionResult Results()
        {
            string Keywords = Request.Form["Keywords"].ToString();

            dynamic lists = SearchKeyword(Keywords);
            ViewBag.Keywords = Keywords;
            ViewBag.Message = string.Format("Result(s) for {0}", Keywords);
            return View(lists);
        }

        private dynamic SearchKeyword(string Keywords)
        {
            db = new RobinsonsDBContext();
            dynamic lists = new ExpandoObject();

            //dynamic resultcontent = new ExpandoObject();
            //List<dynamic> resultcontentsList = new List<dynamic>();
            //try
            //{
            //    if (!string.IsNullOrEmpty(Keywords))
            //    {
            //        var contents = from c in db.Contents
            //                       where c.IsPublished == true && c.IsDeleted == false
            //                       && (c.Name.Contains(Keywords) || c.Description.Contains(Keywords) || c.Markup.Contains(Keywords))
            //                       select c;
            //        foreach (Contents content in contents.ToList())
            //        {
            //            var template = (from t in db.Templates
            //                            where t.Id == new Guid(content.TemplateId)
            //                            select t).First();
            //            resultcontent.Id = content.Id;
            //            resultcontent.Name = content.Name;
            //            resultcontent.TemplateId = content.TemplateId;
            //            resultcontent.Type = template.Type;
            //            string description = string.Empty;
            //            if (content.Name.ToLower().Contains(Keywords.ToLower()))
            //                description = content.Name;
            //            else if (content.Description.ToLower().Contains(Keywords.ToLower()))
            //                description = content.Description;
            //            else if (content.Markup.ToLower().Contains(Keywords.ToLower()))
            //            {
            //                int index = content.Markup.ToLower().IndexOf(Keywords.ToLower());
            //                string substringdesc = description = content.Markup.Substring(index);
            //                if (substringdesc.Count() > 100)
            //                    description = substringdesc.Substring(0, 100);
            //                else
            //                    description = substringdesc;
            //            }
            //            resultcontent.Description = HttpUtility.HtmlDecode(description);
            //            resultcontentsList.Add(resultcontent);
            //        }
            //        lists.Contents = resultcontentsList.AsEnumerable();
            //    }

            try
            {
                if (!string.IsNullOrEmpty(Keywords))
                {
                    Keywords = Keywords.ToLower();

                    dynamic resultcontent = new ExpandoObject();
                    List<dynamic> resultcontentsList = new List<dynamic>();

                    var contents = from c in db.Contents
                                   where c.IsPublished == true && c.IsDeleted == false
                                   && (c.Name.ToLower().Contains(Keywords) || c.Description.ToLower().Contains(Keywords) || c.Markup.ToLower().Contains(Keywords))
                                   select c;

                    List<Contents> contentList = new List<Contents>();

                    foreach (Contents content in contents.ToList())
                    {

                        string substringdesc = content.Description;

                        if (substringdesc.Count() > 100)
                            substringdesc = substringdesc.Substring(0, 100);

                        content.Description = substringdesc;

                        contentList.Add(content);
                    }

                    lists.Contents = contentList.ToList();
                }
                else
                {
                    Keywords = string.Empty;

                    dynamic resultcontent = new ExpandoObject();
                    List<dynamic> resultcontentsList = new List<dynamic>();

                    var contents = from c in db.Contents
                                   where c.IsPublished == true && c.IsDeleted == false
                                   && (c.Name.Contains(Keywords) || c.Description.Contains(Keywords) || c.Markup.Contains(Keywords))
                                   select c;

                    List<Contents> contentList = new List<Contents>();

                    foreach (Contents content in contents.ToList())
                    {
                        contentList.Add(content);
                    }

                    lists.Contents = contentList.ToList();
                }

            }
            catch
            {
                lists.Contents = new Contents[] { new Contents() };
            }
            return lists;
        }

        public ActionResult MyRewards()
        {
            if (TempData["MyRewardsAction"] == null)
                TempData["MyRewardsAction"] = "Account/Login";
            return View();
        }

        public ActionResult RedirectToMyRewards(string urlaction)
        {
            if (!string.IsNullOrEmpty(urlaction))
                TempData["MyRewardsAction"] = HttpUtility.HtmlDecode(urlaction);
            else
                TempData["MyRewardsAction"] = "Account/Login";
            return RedirectToAction("MyRewards");
            //return View();
        }
    }
}