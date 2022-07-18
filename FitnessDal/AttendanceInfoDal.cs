using FitnessEntity;
using IFitnessDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessDal
{
   public class AttendanceInfoDal : BaseDal<AttendanceInfo>, IAttendanceInfoDal
    {
        FitnessDbContext _DbContext;
        public AttendanceInfoDal(FitnessDbContext DbContext) : base(DbContext)
        {
            _DbContext = DbContext;
        }
    
    }
}
