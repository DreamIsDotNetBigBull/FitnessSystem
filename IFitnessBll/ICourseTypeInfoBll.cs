using FitnessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFitnessBll
{
    public interface ICourseTypeInfoBll : IBaseBll<CourseTypeInfo>
    {
        object GetCourseTypeSelectOption();
        object GetCourseInfoListByPage(int page, int limit, out int count, string courseTypeID);
        bool IsHadCourseTypeInfo(string courseType);
        bool UpdateCourseTypeInfo(string categoryId, string categoryType, string description);
    }
}
