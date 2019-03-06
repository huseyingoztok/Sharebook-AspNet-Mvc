using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities.EntityViewModels
{
    public class LikedViewModel
    {
     
        public int Id { get; set; }
        public virtual Sharing Shareing { get; set; }
        public virtual ShareBookUser likedUser { get; set; }
    }
}
