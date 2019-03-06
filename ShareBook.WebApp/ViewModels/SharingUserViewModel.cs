using ShareBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareBook.WebApp.ViewModels
{
    public class SharingUserViewModel<Sbu,S>
    {
        public Sbu sbu { get; set; }
        public S s { get; set; }
    }
}