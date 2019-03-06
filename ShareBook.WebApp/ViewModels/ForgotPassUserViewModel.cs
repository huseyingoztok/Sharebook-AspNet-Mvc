using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareBook.WebApp.ViewModels
{
    public class ForgotPassUserViewModel<U,P>
    {
        public U User { get; set; }
        public P NewPass { get; set; }
    }
}