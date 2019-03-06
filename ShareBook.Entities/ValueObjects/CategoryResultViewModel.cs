using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities.ValueObjects
{
    public class CategoryResultViewModel<C>
    {
        public C EditCategory { get; set; }
        public int Res;

    }
}
