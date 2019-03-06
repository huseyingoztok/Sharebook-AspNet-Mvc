using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareBook.WebApp.ViewModels
{
    public class InfoViewModel:NotificationModelViewBase<string>
    {
        public InfoViewModel()
        {
            Tittle = "Bilgilendirme";
        }
    }
}