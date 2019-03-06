using ShareBook.BusinessLayer;
using ShareBook.Entities;
using ShareBook.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShareBook.WebApp.Filters
{
    public class ExceptionFilter : FilterAttribute, IExceptionFilter
    {
    
        public void OnException(ExceptionContext filterContext)
        {
            SiteLogManager siteLogManager = new SiteLogManager();
            filterContext.ExceptionHandled = true;
            string exeption = filterContext.Exception.Message;

            SiteLog siteLog = new SiteLog();

            siteLog.ActionName=filterContext.RouteData.Values["action"].ToString();
            siteLog.ControllerName = filterContext.RouteData.Values["controller"].ToString();

            if (CurrentSession.shareBookUser != null)
            {
                siteLog.Username = CurrentSession.shareBookUser.Username;
            }
            else
            {
                siteLog.Username = "Guest User";
            }

            siteLog.LogDate = DateTime.Now;
            siteLog.Info = "Site Error :" +exeption ;
            siteLogManager.Insert(siteLog);

            
            filterContext.Result = new RedirectResult("/Home/ErrorPage");
            

            

        }
    }
}