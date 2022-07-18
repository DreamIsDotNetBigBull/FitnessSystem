using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEntity
{
   /// <summary>
   /// 商品记录表实体
   /// </summary>
  public  class GoodsRecord:BaseEntity
    {
        /// <summary>
        /// 商品ID
        /// </summary>
       [Column(TypeName = "nvarchar(36)")]
        public string GoodsId    { get; set; }
        /// <summary>
        /// 出入库数量
        /// </summary>
        public int Num{ get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 添加人账号
        /// </summary>
       [Column(TypeName = "nvarchar(36)")]
        public string Creator { get; set; }
    }
}
