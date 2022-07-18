using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEntity
{
    public class Category:BaseEntity
    {
        /// <summary>
        /// 教练职称类型
        /// </summary>
        [Column(TypeName = "varchar(36)")]
        public string CategoryType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }
    }
}
