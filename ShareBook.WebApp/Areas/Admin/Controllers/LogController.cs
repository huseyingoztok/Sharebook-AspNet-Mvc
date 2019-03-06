using ShareBook.BusinessLayer;
using ShareBook.WebApp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShareBook.WebApp.Areas.Admin.Controllers
{
    [AdminAuthFilter]
    public class LogController : Controller
    {
        // GET: Admin/Log
        private LogManager logManager = new LogManager();
        public ActionResult Index()
        {
            var model = logManager.List();
            return View(model);
        }
    }
}