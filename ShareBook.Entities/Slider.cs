using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities
{
    [Table("Sliders")]
    public class Slider
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(100),Required(ErrorMessage ="{0} alanı boş geçilemez")]
        public string Name { get; set; }
        [StringLength(250), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string Url { get; set; }
        public int Priority { get; set; }
        [StringLength(1000)]
        public string imageUrl { get; set; }
        public bool isDeleted { get; set; }
        public bool isActive { get; set; }
    }
}
