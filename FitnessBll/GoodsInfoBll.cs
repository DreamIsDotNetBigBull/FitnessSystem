using FitnessDal;
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
    public class GoodsInfoBll : BaseDeleteBll<GoodsInfo>, IGoodsInfoBll
    {
        private IGoodsInfoDal _goodsInfoDal;
        private IGoodsTypeDal _goodsTypeDal;
        private FitnessDbContext _dbContext;
        private IUserInfoDal _userInfoDal;
        private IRechargeInfoDal _rechargeInfoDal;
        private IGoodsRecordDal _goodsRecordDal;

        public GoodsInfoBll(IGoodsInfoDal goodsInfoDal, IGoodsTypeDal goodsTypeDal, 
            FitnessDbContext dbContext, IUserInfoDal userInfoDal, IRechargeInfoDal rechargeInfoDal,
            IGoodsRecordDal goodsRecordDal) : base(goodsInfoDal)
        {
            _goodsInfoDal = goodsInfoDal;
            _goodsTypeDal = goodsTypeDal;
            _dbContext = dbContext;
            _userInfoDal = userInfoDal;
            _rechargeInfoDal = rechargeInfoDal;
            _goodsRecordDal = goodsRecordDal;
        }
        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="consumableRecord"></param>
        /// <returns></returns>
        public bool AddGoodsInfo(GoodsInfo goodsInfo)
        {
            return _goodsInfoDal.Add(goodsInfo);
        }

        /// <summary>
        /// 购买商品
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="goodsId"></param>
        /// <param name="num"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool BuyGoods(UserInfo userInfo,string goodsId,int num, out string msg)
        {
            msg = "";
            var goodsInfo = _goodsInfoDal.GetEntity(goodsId);
            var u= _userInfoDal.GetEntity(userInfo.ID);
            if (goodsInfo == null)
            {
                msg = "找不到该商品";
                return false;
            }
            if (goodsInfo.Num==0)
            {
                msg = "该商品已售罄";
                return false;
            }
            if (goodsInfo.Num < num)
            {
                msg = "该商品库存不足";
                return false;
            }
            int money=Convert.ToInt32(goodsInfo.Money)* num;
            if (u.Balance<money)
            {
                msg = "您账户余额不足";
                return false;
            }
            
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                u.Balance -= money;
                //修改账户余额
                bool isSuccess1 = _userInfoDal.Update(u);

                RechargeInfo rechargeInfo = new RechargeInfo()
                {
                    CreateTime = DateTime.Now,
                    Creator = userInfo.ID,
                    CurrentBalance = money,
                    ID = Guid.NewGuid().ToString(),
                    Description="购买了"+goodsInfo.GoodsName,
                    InsertType = InsertTypeEnum.消费 ,
                };
                //添加消费记录
                bool isSuccess2=_rechargeInfoDal.Add(rechargeInfo);
                //修改商品库存数量
                goodsInfo.Num -= num;
                bool isSuccess3 = _goodsInfoDal.Update(goodsInfo);
                GoodsRecord goodsRecord = new GoodsRecord()
                {
                    CreateTime = DateTime.Now,
                    Creator = userInfo.ID,
                    GoodsId = goodsId,
                    ID = Guid.NewGuid().ToString(),
                    Num = num,
                    Type = 1
                };
                //添加出库记录
                bool isSuccess4 = _goodsRecordDal.Add(goodsRecord);
                if (isSuccess1 && isSuccess2 && isSuccess3 && isSuccess4)
                {
                    transaction.Commit();
                    msg = "成功";
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    msg = "失败";
                    return true;
                }
                //return false;

            }
        }

        /// <summary>
        /// 获取商品列表集合
        /// </summary>
        public object GetGoodsInfoListByPage(int page, int limit, out int count, string goodsName, string goodsTypeId)
        {
            //AsQueryable() 延迟查询
            var goodsInfo = _goodsInfoDal.GetEntitiesDb().Where(g=>!g.IsDelete);
            var goodsType = _goodsTypeDal.GetEntitiesDb().Where(g=>!g.IsDelete);
            if (!string.IsNullOrEmpty(goodsName))
            {
                goodsInfo = goodsInfo.Where(u => u.GoodsName.Contains(goodsName));
            }
            if (!string.IsNullOrEmpty(goodsTypeId))
            {
                goodsInfo = goodsInfo.Where(u => u.GoodsTypeId.Contains(goodsTypeId));
            }
            count = goodsInfo.Count();

            var temp = from g in goodsInfo
                       join t in goodsType
                       on g.GoodsTypeId equals t.ID into gtTemp
                       from gt in gtTemp.DefaultIfEmpty()
                       select new
                       {
                           g.ID,
                           g.GoodsName,
                           g.Num,
                           g.Money,
                           g.Unit,
                           gt.GoodsTypeName,
                           g.Description
                       };
            temp = temp.Skip((page - 1) * limit).Take(limit);
            var list = temp.ToList().Select(g =>
            {

                return new
                {
                    g.ID,
                    g.GoodsName,
                    g.Num,
                    g.Money,
                    g.Unit,
                    g.GoodsTypeName,
                    g.Description
                };
            });
            return list;
        }
        /// <summary>
        /// 获取商品类别下拉选
        /// </summary>
        public object GetGoodsSelectOption()
        {
            var goodsInfos = _goodsTypeDal.GetEntitiesDb().Select(d => new
            {
                d.ID,
                d.GoodsTypeName
            }).ToList();
            return goodsInfos;
        }

        /// <summary>
        /// 获取商品下拉选
        /// </summary>
        /// <returns></returns>
        public object GetGoodsSelectOptionF()
        {
            var goodsInfos = _goodsInfoDal.GetEntitiesDb().Select(d => new
            {
                d.ID,
                d.GoodsName
            }).ToList();
            return goodsInfos;
        }

        /// <summary>
        /// 判断是否有重复的商品名称
        /// </summary>
        /// <returns></returns>
        public bool IsHadGoodsInfo(string goodsName)
        {
            return _goodsInfoDal.GetEntitiesDb().Where(c => c.GoodsName == goodsName).Count() > 0; ;
        }

        public bool UpdateGoodsInfo(string goodsid, string goodsName, string description, int money, string unit, string goodsTypeId)
        {
            GoodsInfo goodsInfo = _goodsInfoDal.GetEntity(goodsid);
            if (goodsInfo != null)
            {
                goodsInfo.GoodsName = goodsName;
                goodsInfo.Description = description;
                goodsInfo.Money = money;
                goodsInfo.Unit = unit;
                goodsInfo.GoodsTypeId = goodsTypeId;


                return _goodsInfoDal.Update(goodsInfo);
            }
            else
            {
                return false;
            }
        }


    }
}
