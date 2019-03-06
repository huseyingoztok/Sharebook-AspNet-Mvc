using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShareBook.Entities.ValueObjects
{
    public class RegisterViewModel
    {
        [DisplayName("Ad"),
         Required(ErrorMessage = "{0} Boş geçilemez..."),
         StringLength(100, ErrorMessage = "{0} alanı {1} karakterden fazla girilemez...")
            ]
        public string Name { get; set; }

        [DisplayName("Soyad"),
         Required(ErrorMessage = "{0} Boş geçilemez..."),
         StringLength(50, ErrorMessage = "{0} alanı {1} karakterden fazla girilemez...")
            ]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adı"),
         Required(ErrorMessage = "{0} Boş geçilemez..."),
         StringLength(100, ErrorMessage = "{0} alanı {1} karakterden fazla girilemez..."),
            ]
        public string Username { get; set; }
        [DisplayName("E-posta"),
         Required(ErrorMessage = "{0} Boş geçilemez..."),
         StringLength(50, ErrorMessage = "{0} alanı {1} karakterden fazla girilemez..."),
            EmailAddress(ErrorMessage = "{0} alanı için geçerli bir e-posta giriniz...")
            ]
        public string Email { get; set; }

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