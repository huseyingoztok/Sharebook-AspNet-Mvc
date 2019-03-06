using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShareBook.Entities.ValueObjects
{
    public class LoginViewModel
    {
        [DisplayName("Kullanıcı Adı"),
         Required(ErrorMessage = "{0} Boş geçilemez..."),
         StringLength(50, ErrorMessage = "{0} alanı {1} karakterden fazla girilemez...")
            ]
        public string Username { get; set; }
        [DisplayName("Şifre"),
        Required(ErrorMessage = "{0} Boş geçilemez..."),
        StringLength(50, ErrorMessage = "{0} alanı {1} karakterden fazla girilemez..."),
        DataType(DataType.Password),

            ]
        public string Password { get; set; }
    }
}