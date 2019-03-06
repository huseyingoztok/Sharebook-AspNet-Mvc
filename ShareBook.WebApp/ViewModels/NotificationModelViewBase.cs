using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareBook.WebApp.ViewModels
{
    public class NotificationModelViewBase<T>
    {
        public List<T> Items { get; set; }
        public string Header { get; set; }
        public string Tittle { get; set; }

        public bool IsRedirecting { get; set; }

        public string RedirectingUrl { get; set; }
        public int RedirectingTimeout { get; set; }

        public NotificationModelViewBase()
        {
            Header = "Yönlendiriliyorsunuz...";
            Tittle = "Geçersiz İşlem";
            IsRedirecting = true;
            RedirectingUrl = "/Home/Index";
            RedirectingTimeout = 4000;
            Items = new List<T>();
        }
    }
}