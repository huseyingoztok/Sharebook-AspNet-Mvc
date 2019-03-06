using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities.EntityViewModels
{
    public class ShareBookUserViewModel
    {

  
        public int Id { get; set; }


        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
  
        public string ModifiedBy { get; set; }


       
        public string Name { get; set; }
 
        public string Surname { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
  
        public string ProfilPhoto { get; set; }

        public Guid activatedGuid { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        public bool isAdmin { get; set; }




        public virtual List<Sharing> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}
