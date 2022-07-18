using FitnessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFitnessBll
{
    public interface ICategoryBll : IBaseBll<Category>
    {
        object GetCategoryListByPage(int page, int limit, out int count, string categoryName);
        bool IsHadCategory(string categoryType);
        bool UpdateCategory(string courseTypeInfoId, string courseType, string description);
        object GetCategorySelectOption();
    }
}
