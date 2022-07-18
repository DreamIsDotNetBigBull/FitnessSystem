using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEntity
{
    /// <summary>
    /// 商品信息表实体
    /// </summary>
 public  class GoodsInfo:BaseDeleteEntity
    {
        /// <summary>
        /// 商品名称
        /// </summary>
       [Column(TypeName = "varchar(36)")]
        public string GoodsName { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public int Money  { get; set; }
        /// <summary>
        /// 商品单位
        /// </summary>
         [Column(TypeName = "nvarchar(8)")]
        public string Unit { get; set; }
        /// <summary>
        /// 商品类型
        /// </summary>
        [Column(TypeName = "nvarchar(36)")]
        public string GoodsTypeId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Column(TypeName = "varchar(36)")]
        public string Description { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
