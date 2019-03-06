using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities.EntityViewModels
{
    public class SharingViewModel
    {

        
        public int Id { get; set; }


        public DateTime CreatedOn { get; set; }
   
        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }



        public string Title { get; set; }
  
        public string ShareContent { get; set; }

        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public bool isDraft { get; set; }
        public bool isDelete { get; set; }

        public virtual ShareBookUser Owner { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Comment> Comments { get; set; }

        public virtual List<Liked> Likes { get; set; }

      
    }
}
