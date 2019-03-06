using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities
{
    [Table("ShareBookUsers")]
    public class ShareBookUser:MyEntitiesBase
    {
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Surname { get; set; }
        [StringLength(50), Required(ErrorMessage = "{0} Boş geçilemez...")]
        public string Username { get; set; }
        [StringLength(150), Required(ErrorMessage = "{0} Boş geçilemez...")]
        public string Email { get; set; }
        [StringLength(50), Required(ErrorMessage = "{0} Boş geçilemez...")]
        public string Password { get; set; }
        [StringLength(800)]
        public string ProfilPhoto { get; set; }
        [Required]
        public Guid activatedGuid { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        public bool isAdmin { get; set; }

        


        public virtual List<Sharing> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}
