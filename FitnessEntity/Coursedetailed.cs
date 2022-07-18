using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEntity
{
    /// <summary>
    /// 课程详细
    /// </summary>
    public class Coursedetailed:BaseEntity
    {
        /// <summary>
        /// 会员id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 课程id
        /// </summary>
        public string CourseInfoId { get; set; }
        /// <summary>
        /// 报名时间
        /// </summary>
        public DateTime ThisTime { get; set; }
    }
}
