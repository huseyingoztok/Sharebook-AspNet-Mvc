using ShareBook.Common;
using ShareBook.Entities;
using ShareBook.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareBook.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string getCurrentUsername()
        {
            if (CurrentSession.shareBookUser!=null)
            {
                return CurrentSession.shareBookUser.Username;
            }
            else if (CurrentSession.shareBookUserAdmin != null)
            {
                return CurrentSession.shareBookUserAdmin.Username;
            }
            else
            {
                return "system";
            }
        }
    }
}