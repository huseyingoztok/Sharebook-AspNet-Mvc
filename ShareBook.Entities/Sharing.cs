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
    [Table("Shares")]
    public class Sharing:MyEntitiesBase
    {
        [DisplayName("Başlık"),StringLength(150), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string Title { get; set; }
        [DisplayName("İçerik"),StringLength(2000), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string ShareContent { get; set; }
        [DisplayName("Beğeni")]
        public int LikeCount { get; set; }
        [DisplayName("Resim")]
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        [DisplayName("Taslak")]
        public bool isDraft { get; set; }
        public bool isDelete { get; set; }

        public virtual ShareBookUser Owner { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Comment> Comments { get; set; }

        public virtual List<Liked> Likes { get; set; }

        public Sharing()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }

    }
}
