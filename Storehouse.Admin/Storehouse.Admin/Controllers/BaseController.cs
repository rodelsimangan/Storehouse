using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StorehouseAdmin.Models;

namespace StorehouseAdmin.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
          /* if(TempData["TemplateList"]!=null)
            {
                TempData.Keep();
            } */
        }
    }
}