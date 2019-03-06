using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.Entities.Messages
{
    public class ErrorMessageObj
    {
        public ErrorMessagesCode Code { get; set; }
        public string Message { get; set; }

    }
}
