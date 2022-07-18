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
    public class R_RoleInfo_MenuInfoBll:BaseBll<R_RoleInfo_MenuInfo>, IR_RoleInfo_MenuInfoBll
    {
        private IR_RoleInfo_MenuInfoDal _r_RoleInfo_MenuInfoDal;
        public R_RoleInfo_MenuInfoBll(IR_RoleInfo_MenuInfoDal r_RoleInfo_MenuInfoDal):base(r_RoleInfo_MenuInfoDal)
        {
            _r_RoleInfo_MenuInfoDal = r_RoleInfo_MenuInfoDal;
        }
    }
}
