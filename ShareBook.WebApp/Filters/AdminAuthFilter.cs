using ShareBook.Entities;
using ShareBook.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShareBook.WebApp.Filters
{
    public class AdminAuthFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentSession.shareBookUserAdmin == null)
            {
                filterContext.Result = new RedirectResult("/Admin/Login/LoginPage");
            }
        }
    }
}