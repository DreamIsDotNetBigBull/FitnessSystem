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
    public class GoodsTypeBll : BaseDeleteBll<GoodsType>, IGoodsTypeBll
    {
        private IGoodsTypeDal _goodsTypeDal;


        public GoodsTypeBll(IGoodsTypeDal goodsTypeDal) : base(goodsTypeDal)
        {
            _goodsTypeDal = goodsTypeDal;

        }

        /// <summary>
        /// 获取类别集合
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <param name="goodsTypeName"></param>
        /// <returns></returns>
        public object GetGoodsTypeListByPage(int page, int limit, out int count, string goodsTypeName)
        {
            //获取数据库中类别全部没删除的数据（未真实查询）
            var type = _goodsTypeDal.GetEntitiesDb().Where(g=>!g.IsDelete);

            if (!string.IsNullOrEmpty(goodsTypeName))
            {
                type = type.Where(u => u.GoodsTypeName.Contains(goodsTypeName));
            }

            //查询出来数据的数量
            count = _goodsTypeDal.GetEntitiesDb().Count();

            //分页
            type = type.OrderBy(u => u.GoodsTypeName).Skip((page - 1) * limit).Take(limit);

            var list = type.ToList().Select(u =>
            {
                return new
                {
                    u.GoodsTypeName,
                    u.ID,
                    u.Description
                };
            });

            return list;
        }

        /// <summary>
        /// 判断是否有同样类别名
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public bool IsHadGoodsType(string typeName)
        {
            int count = _goodsTypeDal.GetEntitiesDb().Where(u => u.GoodsTypeName == typeName).Count();
            return count > 0;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="goodsTypeId"></param>
        /// <param name="goodsTypeName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool UpdateGoodsType(string goodsTypeId, string goodsTypeName, string description)
        {
            GoodsType goodsType = _goodsTypeDal.GetEntity(goodsTypeId);
            if (goodsType != null)
            {
                goodsType.GoodsTypeName = goodsTypeName;
                goodsType.Description = description;
                return _goodsTypeDal.Update(goodsType);
            }
            else
            {
                return false;
            }
        }
    }
}
