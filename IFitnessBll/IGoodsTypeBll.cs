using FitnessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFitnessBll
{
    public interface IGoodsTypeBll : IBaseDeleteBll<GoodsType>
    {
        /// <summary>
        /// 获取类别集合
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <param name="goodsTypeName"></param>
        /// <returns></returns>
        object GetGoodsTypeListByPage(int page, int limit, out int count, string goodsTypeName);

        /// <summary>
        /// 判断是否有同样类别名
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        bool IsHadGoodsType(string typeName);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="goodsTypeId"></param>
        /// <param name="goodsTypeName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        bool UpdateGoodsType(string goodsTypeId, string goodsTypeName, string description);
    }
}
