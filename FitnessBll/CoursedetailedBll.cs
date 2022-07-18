using FitnessEntity;
using IFitnessBll;
using IFitnessDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessBll
{
    public class CoursedetailedBll: BaseBll<Coursedetailed>, ICoursedetaileBll
    {
        private ICourseDetailedDal _coursedetaileDal;

        public CoursedetailedBll(ICourseDetailedDal coursedetaileDal):base(coursedetaileDal)
        {
            _coursedetaileDal = coursedetaileDal;
        }
    }
}
