using ShareBook.BusinessLayer.Abstract;
using ShareBook.BusinessLayer.Results;
using ShareBook.Entities;
using ShareBook.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.BusinessLayer
{
    public class SliderManager:ManagerBase<Slider>
    {
        public BusinessLayerResult<Slider> priorityControlInsert(Slider sld)
        {
            List<Slider> slider = ListQueryable().Where(x => x.Priority == sld.Priority&&x.Id!=sld.Id&&!x.isDeleted).ToList();
            BusinessLayerResult<Slider> res = new BusinessLayerResult<Slider>();

            if (slider.Count>0)
            {
                res.AddError(ErrorMessagesCode.PriorityValueAlreadyExist,"Öncelik değeri daha önce kullanılmış. Lütfen farklı bir öncelik değeri veriniz.");
                res.Result = sld;

                return res;
                
            }
            res.Result = sld;

            return res;

        }


        public BusinessLayerResult<Slider> priorityControlUpdate(Slider sld)
        {
            List<Slider> slider =ListQueryable().Where(x=>x.Priority==sld.Priority&&x.Id!=sld.Id&&!x.isDeleted).ToList();
            BusinessLayerResult<Slider> res = new BusinessLayerResult<Slider>();

            if (slider.Count>0)
            {
                res.AddError(ErrorMessagesCode.PriorityValueAlreadyExist, "Öncelik değeri daha önce kullanılmış. Lütfen farklı bir öncelik değeri veriniz.");
                res.Result = sld;

                return res;

            }

            res.Result = sld;

            return res;

        }
    }
}
