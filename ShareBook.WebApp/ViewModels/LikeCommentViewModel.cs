using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareBook.WebApp.ViewModels
{
    public class LikeCommentViewModel<L,C>
    {

        public L Likes { get; set; }
        public C Comments { get; set; }
    }
}