using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities.EntityViewModels
{
    public class CategoryViewModel
    {

        public string Title { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public bool isActive { get; set; }
        public bool isDeleted { get; set; }

        public int Id { get; set; }

  
        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }
  
        public string ModifiedBy { get; set; }
    }
}
