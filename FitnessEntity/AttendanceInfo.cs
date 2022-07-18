using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEntity
{
    /// <summary>
    /// 考勤打卡表实体
    /// </summary>
 public   class AttendanceInfo:BaseEntity
    {
        /// <summary>
        /// 打卡人ID
        /// </summary>
        [Column(TypeName = "nvarchar(36)")]
        public string ThecoachId  { get; set; }
        /// <summary>
        /// 打卡时间
        /// </summary>
        public DateTime DanceTime { get; set; }

        /// <summary>
        /// 打卡类型
        /// </summary>
        public  string DanceType { get; set; }
    }
}
