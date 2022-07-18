using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFitnessBll
{
    public  interface IBaseBll<T>
    {
        bool Add(T entity);

        bool Delete(string Id);

        T GetEntity(string id);

        //bool SoftDeleteEntity(string id);

        //void SoftDeleteEntities(List<string> ids);

        bool UpdateEntity(T entity);
    }
}
