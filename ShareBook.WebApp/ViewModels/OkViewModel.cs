using ShareBook.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareBook.WebApp.ViewModels
{
    public class OkViewModel:NotificationModelViewBase<string>
    {
        public OkViewModel()
        {
            Tittle = "İşlem başarılı";
        }
    }
}