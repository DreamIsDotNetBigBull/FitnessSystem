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
    public class R_UserInfo_RoleInfoBll: BaseBll<R_UserInfo_RoleInfo>,IR_UserInfo_RoleInfoBll
    {
        IR_UserInfo_RoleInfoDal _R_UserInfo_RoleInfoDal;

        public R_UserInfo_RoleInfoBll(IR_UserInfo_RoleInfoDal R_UserInfo_RoleInfoDal) :base(R_UserInfo_RoleInfoDal)
        {
            _R_UserInfo_RoleInfoDal = R_UserInfo_RoleInfoDal;
        }
    }
}
