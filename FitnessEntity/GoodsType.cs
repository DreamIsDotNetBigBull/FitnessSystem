using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEntity
{
    /// <summary>
/// 商品类别表实体
/// </summary>
 public   class GoodsType:BaseDeleteEntity
    {
        /// <summary>
        /// 类别名称
        /// </summary>
        [Column(TypeName = "nvarchar(32)")]
        public string GoodsTypeName { get; set; }

        public string Description { get; set; }
    }
}
