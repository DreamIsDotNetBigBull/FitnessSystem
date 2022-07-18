using FitnessEntity;
using IFitnessBll;
using IFitnessDal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessBll
{
    public class RechargeInfoBll:BaseBll<RechargeInfo>,IRechargeInfoBll
    {
        private IRechargeInfoDal _rechargeInfoDal;
        private IUserInfoDal _userInfoDal;
        private FitnessDbContext _dbContext;

        public RechargeInfoBll(IRechargeInfoDal rechargeInfoDal, IUserInfoDal userInfoDal, FitnessDbContext dbContext) :base(rechargeInfoDal)
        {
            _rechargeInfoDal = rechargeInfoDal;
            _userInfoDal = userInfoDal;
            _dbContext = dbContext;
        }

        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        public bool AddRecharge(UserInfo userInfo, int money, string description, int type)
        {
            var userInfos = _userInfoDal.GetEntitiesDb().FirstOrDefault(u => u.ID == userInfo.ID && !u.IsDelete);

            if (userInfo==null)
            {
                return false;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                //修改该用户的余额
                userInfos.Balance += money;
                bool isSuccess = _userInfoDal.Update(userInfos);

                RechargeInfo rechargeInfo = new RechargeInfo()
                {
                    CreateTime = DateTime.Now,
                    Creator = userInfo.ID,
                    ID = Guid.NewGuid().ToString(),
                    InsertType = InsertTypeEnum.充值,
                    CurrentBalance = money,
                    Description = description,
                    Type = type
                };

                bool isSuccess1=_rechargeInfoDal.Add(rechargeInfo);

                if (isSuccess && isSuccess1)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取充值数据集合并进行分页
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public object GetRechargeInfoListByPage(int page, int limit, out int count,UserInfo userInfo,InsertTypeEnum insertType)
        {
            

            //获取数据库中的数据
            var rechargeInfos = _rechargeInfoDal.GetEntitiesDb().AsQueryable();

            //判断是否管理员 如果是可以查看全部实例 如果不是则只能看到自己的
            if (!userInfo.IsAdmin)
            {
                rechargeInfos = rechargeInfos.Where(w => w.Creator == userInfo.ID);
            }

            if (insertType != 0)
            {
                rechargeInfos = rechargeInfos.Where(u => u.InsertType == insertType);
            }

            //查询出来的数据的数量
            count = rechargeInfos.Count();

            //获取用户数据集合
            DbSet<UserInfo> userInfos = _userInfoDal.GetEntitiesDb();

            var temp = from r in rechargeInfos
                       join u in userInfos
                       on r.Creator equals u.ID into ruTemp
                       from ru in ruTemp.DefaultIfEmpty()
                       select new
                       {
                           r.ID,
                           r.CurrentBalance,
                           r.InsertType,
                           r.CreateTime,
                           Creator=ru.UserName,
                           r.Description,
                           r.Type
                       };
            //分页
            temp = temp.OrderBy(r => r.InsertType).Skip((page - 1) * limit).Take(limit);

            var list = temp.ToList().Select(r =>
            {
                return new
                {
                    r.ID,
                    r.CurrentBalance,
                    InsertType=r.InsertType==InsertTypeEnum.充值 ? "充值" : "消费",
                    r.Creator,
                    CreateTime = r.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    r.Description,
                    Type = r.Type == 1 ? "微信" : r.Type == 2 ? "支付宝" : r.Type == 3 ? "现金" : ""
                };
            });
            return list;
        }

        public object GetStatusSelectOption()
        {
            List<object> options = new List<object>();

            //获取枚举中所有的名字
            var names = Enum.GetNames(typeof(InsertTypeEnum));

            foreach (var namesItem in names)
            {
                var value = (int)Enum.Parse(typeof(InsertTypeEnum), namesItem);
                options.Add(new
                {
                    Key = namesItem,
                    Value = value
                });
            }
            return options;
        }
    }
}
