using ShareBook.WebApp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShareBook.WebApp.Areas.Admin.Controllers
{
    [AdminAuthFilter]
    public class AdminHomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
   
    }
}