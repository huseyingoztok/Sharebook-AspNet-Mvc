using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareBook.WebApp.ViewModels
{
    public class WarningViewModel:NotificationModelViewBase<string>
    {
        public WarningViewModel()
        {
            Tittle = "Uyarı";
        }
    }
}