using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFitnessBll
{
 public   interface IBaseDeleteBll<T>:IBaseBll<T>
    {
        bool SoftDeleteEntity(string id);

        void SoftDeleteEntites(List<string> ids);
    }
}
