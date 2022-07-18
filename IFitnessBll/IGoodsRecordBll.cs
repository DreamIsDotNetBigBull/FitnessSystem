using FitnessEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFitnessBll
{
    public interface IGoodsRecordBll : IBaseBll<GoodsRecord>
    {
        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool AddGoodsRecord(GoodsRecord entity);

        /// <summary>
        /// 获取商品入库记录集合
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        object GetGoodsRecordListByPage(int page, int limit, out int count, string goodsId);

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="iD"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool Upload(Stream stream, string userInfoId, out string msg);

        /// <summary>
        /// 下载
        /// </summary>
        /// <returns></returns>
        Stream GetDownloadDatas();
    }
}
