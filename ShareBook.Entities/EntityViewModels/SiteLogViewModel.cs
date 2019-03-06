using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities.EntityViewModels
{
    public class SiteLogViewModel
    {
       
        public int Id { get; set; }

        public string Username { get; set; }

        public DateTime LogDate { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }

        public string Info { get; set; }
    }
}
