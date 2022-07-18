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
   public class AttendanceInfoBll:BaseBll<AttendanceInfo>, IAttendanceInfoBll
    {
        IAttendanceInfoDal _attendanceInfoDal;
        public AttendanceInfoBll(IAttendanceInfoDal attendanceInfoDal):base(attendanceInfoDal)
        {
            _attendanceInfoDal = attendanceInfoDal;
        }
    }
}
