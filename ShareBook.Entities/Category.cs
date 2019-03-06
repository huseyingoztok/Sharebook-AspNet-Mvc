using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities
{
    [Table("Categories")]
    public class Category:MyEntitiesBase
    {
        [DisplayName("Kategori Adı"),StringLength(100), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string Title { get; set; }
        [DisplayName("Açıklama"), StringLength(150), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string Description { get; set; }
        [DisplayName("İkon"), StringLength(150)]
        public string Icon { get; set; }
        [DisplayName("Aktif")]
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        public virtual List<Sharing> Shares{ get; set; }

        public Category()
        {
            Shares = new List<Sharing>();
        }

    }
}
