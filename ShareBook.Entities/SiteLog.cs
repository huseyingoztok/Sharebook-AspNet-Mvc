using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities
{
    [Table("SiteLogs")]
    public class SiteLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(100)]
        public string Username { get; set; }

        public DateTime LogDate { get; set; }
        [StringLength(100)]
        public string ActionName { get; set; }
        [StringLength(100)]
        public string ControllerName { get; set; }
        [StringLength(2000)]
        public string Info { get; set; }
    }
}
