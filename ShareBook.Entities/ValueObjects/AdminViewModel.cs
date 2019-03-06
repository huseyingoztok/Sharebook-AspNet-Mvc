using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities.ValueObjects
{
    public class AdminViewModel
    {
        [DisplayName("Kullanıcı Adı"),
         Required(ErrorMessage = "{0} Boş geçilemez..."),
         StringLength(100, ErrorMessage = "{0} alanı {1} karakterden fazla girilemez..."),EmailAddress
            ]
        public string Email { get; set; }
        [DisplayName("Şifre"),
        Required(ErrorMessage = "{0} Boş geçilemez..."),
        StringLength(50, ErrorMessage = "{0} alanı {1} karakterden fazla girilemez..."),
        DataType(DataType.Password),

            ]
        public string Password { get; set; }
    }
}
