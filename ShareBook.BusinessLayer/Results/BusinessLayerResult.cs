using ShareBook.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.BusinessLayer.Results
{
    public class BusinessLayerResult<T>:IDisposable
    {
        public List<ErrorMessageObj> Errors { get; set; }
        public T Result { get; set; }

        public BusinessLayerResult()
        {
            Errors = new List<ErrorMessageObj>();
        }

        public void AddError(ErrorMessagesCode code,string message)
        {
            Errors.Add(new ErrorMessageObj()
            {
                Code = code,
                Message = message,
            });
             
        }

        public void Dispose()
        {
            
        }
    }
}
