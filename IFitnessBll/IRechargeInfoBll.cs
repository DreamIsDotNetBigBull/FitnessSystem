using FitnessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFitnessBll
{
    public interface IRechargeInfoBll : IBaseBll<RechargeInfo>
    {
        //获取充值数据集合
        object GetRechargeInfoListByPage(int page, int limit, out int count, UserInfo userInfo, InsertTypeEnum insertType);
        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        bool AddRecharge(UserInfo userInfo, int money, string description,int type);
        object GetStatusSelectOption();
    }
}
