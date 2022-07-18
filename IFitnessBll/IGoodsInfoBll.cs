using FitnessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFitnessBll
{
    public  interface IGoodsInfoBll:IBaseDeleteBll<GoodsInfo>
    {
        /// <summary>
        /// 获取商品列表集合
        /// </summary>
        object GetGoodsInfoListByPage(int page, int limit, out int count, string goodsName, string goodsTypeId);
        /// <summary>
        /// 获取商品类别下拉选
        /// </summary>
        object GetGoodsSelectOption();
        /// <summary>
        /// 判断是否有这个商品
        /// </summary>
        /// <param name="goodsName"></param>
        /// <returns></returns>
        bool IsHadGoodsInfo(string goodsName);
        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool AddGoodsInfo(GoodsInfo goodsInfo);
        bool UpdateGoodsInfo(string goodsid, string goodsName, string description, int money, string unit, string goodsTypeId);

        /// <summary>
        /// 获取商品下拉选
        /// </summary>
        /// <returns></returns>
        object GetGoodsSelectOptionF();

        /// <summary>
        /// 购买商品
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        bool BuyGoods(UserInfo userInfo, string goodsId, int num, out string msg);
    }
}
