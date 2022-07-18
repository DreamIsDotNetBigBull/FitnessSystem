using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEntity
{
    /// <summary>
    /// 课程类型
    /// </summary>
    public class CourseTypeInfo:BaseEntity
    {
        /// <summary>
        /// 课程类型
        /// </summary>
        [Column(TypeName = "varchar(36)")]
        public string CourseType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }
    }
}
