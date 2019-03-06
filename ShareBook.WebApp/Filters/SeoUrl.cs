using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareBook.WebApp.Filters
{
    public static class SeoUrl
    {

        public static string ToUrl(this string text)
        {
            text = text.Trim();
            text = text.ToLower();
            if (text == "" || text == null) { return ""; }
            text = text.Replace("ş", "s"); text = text.Replace("Ş", "S"); text = text.Replace(".", "");
            text = text.Replace(":", ""); text = text.Replace(";", ""); text = text.Replace(",", "");
            text = text.Replace(" ", "-"); text = text.Replace("!", ""); text = text.Replace("(", "");
            text = text.Replace(")", ""); text = text.Replace("'", ""); text = text.Replace("ğ", "g");
            text = text.Replace("Ğ", "G"); text = text.Replace("ı", "i"); text = text.Replace("I", "i");
            text = text.Replace("ç", "c"); text = text.Replace("ç", "C"); text = text.Replace("ö", "o");
            text = text.Replace("Ö", "O"); text = text.Replace("ü", "u"); text = text.Replace("Ü", "U");
            text = text.Replace("`", ""); text = text.Replace("=", ""); text = text.Replace("&", "");
            text = text.Replace("%", ""); text = text.Replace("#", ""); text = text.Replace("<", "");
            text = text.Replace(">", ""); text = text.Replace("*", ""); text = text.Replace("?", "");
            text = text.Replace("+", "-"); text = text.Replace("\"", "-"); text = text.Replace("»", "-");
            text = text.Replace("|", "-"); text = text.Replace("^", "");
            return text;
        }

    }
}