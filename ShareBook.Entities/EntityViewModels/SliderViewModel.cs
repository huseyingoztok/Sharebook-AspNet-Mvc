using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities.EntityViewModels
{
    public class SliderViewModel
    {

        public int Id { get; set; }

     
        public string Name { get; set; }

        public string Url { get; set; }
        public int Priority { get; set; }

        public string imageUrl { get; set; }
        public bool isDeleted { get; set; }
        public bool isActive { get; set; }
    }
}
