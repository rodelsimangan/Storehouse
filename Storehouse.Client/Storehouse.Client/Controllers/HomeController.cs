using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using System.Net.Mail;
using System.Text;
using System.Web.Helpers;

using Storehouse.Model;
using CMSPortal.Models;
using CMSPortal.Util;

namespace CMSPortal.Controllers
{
    public class HomeController : BaseController
    {
        RobinsonsDBContext db = new RobinsonsDBContext();
        public ActionResult Index()
        {
            db = new RobinsonsDBContext();
            dynamic homepage = new ExpandoObject();

            /*var template = (from t in db.Templates
                            where t.Name == "Events and Promos"
                            select t.Id).AsEnumerable().Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });*/

            var contents = from c in db.Contents
                           where c.IsPublished == true && c.IncludeInHomePage == true && c.IsDeleted == false
                           orderby c.Sequence
                           select c;

            homepage.Contents = contents;

            var sliders = from s in db.Sliders
                          where s.IsDeleted == false
                          orderby s.Sequence
                          select s;
            homepage.Sliders = sliders;

            return View(homepage);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult MyRewards()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(Feedbacks feedback)
        {
            if (ModelState.IsValid)
            {
                //db = new RobinsonsDBContext();
                //db.Feedbacks.Add(feedback);
                //db.SaveChanges();

                StringBuilder sb = new StringBuilder();

                string subject = String.Format("Robinsons Reward Cards Inquiry from {0}", feedback.Name);
                sb.Append("<table>");
                sb.Append(string.Format("<tr><td><b>Name:</b></td><td>{0}</td></tr>", feedback.Name));
                sb.Append(string.Format("<tr><td><b>Robinsons Rewards Card Email Address:</b></td><td>{0}</td></tr>", feedback.From));
                if (feedback.IsMember)
                {
                    sb.Append(string.Format("<tr><td><b>Robinsons Rewards Card Holder:</b></td><td>Yes</td></tr>", feedback.Name));
                    sb.Append(string.Format("<tr><td><b>Robinsons Rewards Card Number:</b></td><td>{0}</td></tr>", feedback.MemberId));
                }
                else
                {
                    sb.Append(string.Format("<tr><td><b>Robinsons Rewards Card Holder:</b></td><td>No</td></tr>", feedback.Name));
                }
                sb.Append("</table><br/><br/><br/><br/>");
                sb.Append("<table>");
                sb.Append(string.Format("<tr><td colspan=\"2\">{0}</td></tr>", feedback.Message));
                sb.Append("</table><br/>");

                EmailSender(feedback.From, "Robinsons Rewards - Online", sb.ToString());

            }

            @ViewBag.Message = "Your feedback has been sent. We will consider this to serve you better. Thank you so much.";
            //@(Url.Action("Contents", "Template", new { id = item.Id }))"
            return View(feedback);
            //return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Search()
        {
            string txtSearch = Request.Form["txtSearch"].ToString();
            return RedirectToAction("Results", "Browse", new { Keywords = txtSearch });
        }

        public void EmailSender(string sender, string subject, string body)
        {
            db = new RobinsonsDBContext();
            var homepagesettings = (from h in db.HomePageSettings
                                    select h).FirstOrDefault();

            if (homepagesettings != null)
            {
                Logger.LogInfo(this.GetType(), string.Concat("Host:", homepagesettings.ContactUsEmailHost));
                Logger.LogInfo(this.GetType(), string.Concat("Port:", homepagesettings.ContactUsEmailPort));
                Logger.LogInfo(this.GetType(), string.Concat("Recipient:", homepagesettings.ContactUsEmailRecipient));
                Logger.LogInfo(this.GetType(), string.Concat("UserName:", System.Configuration.ConfigurationManager.AppSettings["smtpUsername"]));
                Logger.LogInfo(this.GetType(), string.Concat("UserName:", System.Configuration.ConfigurationManager.AppSettings["smtpPassword"]));

                string host = homepagesettings.ContactUsEmailHost;
                int port = int.Parse(homepagesettings.ContactUsEmailPort);
                string recipient = homepagesettings.ContactUsEmailRecipient;
                string UserName = System.Configuration.ConfigurationManager.AppSettings["smtpUsername"];
                string Password = System.Configuration.ConfigurationManager.AppSettings["smtpPassword"];

                #region New

                try
                {
                    WebMail.SmtpServer = host;
                    WebMail.From = sender;

                    WebMail.Send(
                     to: recipient,
                     subject: subject,
                     body: body,
                     isBodyHtml: true
                    );
                }
                catch (Exception ex)
                {
                    Logger.LogError(this.GetType(), ex);
                    throw ex;
                }
            }

                #endregion

            #region old
            /*db = new RobinsonsDBContext();
            var homepagesettings = (from h in db.HomePageSettings
                                    select h).FirstOrDefault();

            if (homepagesettings != null)
            {
                string host = homepagesettings.ContactUsEmailHost;
                int port = int.Parse(homepagesettings.ContactUsEmailPort);
                string recipient = homepagesettings.ContactUsEmailRecipient;
                try
                {
                    //HomePageSettings settings = new HomePageSettings();
                    SmtpClient client = new SmtpClient(host, port);
                    System.Net.NetworkCredential credential = new System.Net.NetworkCredential();

                    string UserName = System.Configuration.ConfigurationManager.AppSettings["smtpUsername"];
                    string Password = System.Configuration.ConfigurationManager.AppSettings["smtpPassword"];

                    client.Credentials = new System.Net.NetworkCredential("csr.robinsons@gmail.com", "csr123$$");

                    //credential.UserName = UserName;//settings.Email_Credential_User;
                    //credential.Password = Password;//settings.Email_Credential_Password;                    
                    
                    client.UseDefaultCredentials = false;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.DeliveryFormat = SmtpDeliveryFormat.International;
                    client.EnableSsl = true;//Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["enableSsl"]);
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress("csr.robinsons@gmail.com");
                    message.To.Add("customercare@robinsonsrewards.com.ph");
                    message.Subject = "Inquiry";
                    message.Body = "test";
                    message.IsBodyHtml = true;
                    client.Send(message);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }*/
            #endregion

        }

        public ActionResult TermsofUse()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            ViewBag.Message = "Privacy Policy";

            return View();
        }

    }
}