using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities.ValueObjects
{
    public class ForgotPasswordViewModel
    {
        [DisplayName("Şifre"),
        Required(ErrorMessage = "{0} Boş geçilemez..."),
        StringLength(50, ErrorMessage = "{0} alanı {1} karakterden fazla girilemez..."),
        DataType(DataType.Password),
            ]
        public string Password { get; set; }
        [DisplayName("Şifre Tekrar"),
        Required(ErrorMessage = "{0} Boş geçilemez..."),
        StringLength(50, ErrorMessage = "{0} alanı {1} karakterden fazla girilemez..."),
        DataType(DataType.Password),
         Compare("Password", ErrorMessage = "{1} ile {0} uyuşmuyor.")

           ]
        public string RePassword { get; set; }
    }
}
