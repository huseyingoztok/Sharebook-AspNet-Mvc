using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities.EntityViewModels
{
    public class CommentViewModel
    {
        
        public string commentText { get; set; }
        public bool isDelete { get; set; }

        public virtual Sharing Sharing { get; set; }
        public virtual ShareBookUser Owner { get; set; }

   
        public int Id { get; set; }

    
        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
  
        public string ModifiedBy { get; set; }
    }
}
