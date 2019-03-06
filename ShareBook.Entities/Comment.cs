using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities
{
    [Table("Comments")]
    public class Comment:MyEntitiesBase
    {
        [StringLength(500), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string commentText { get; set; }
        public bool isDelete { get; set; }

        public virtual Sharing Sharing { get; set; }
        public virtual ShareBookUser Owner { get; set; }

        

    }
}
