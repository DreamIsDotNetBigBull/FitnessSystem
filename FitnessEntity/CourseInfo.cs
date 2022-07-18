using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEntity
{
    /// <summary>
    /// 课程信息表
    /// </summary>
    public class CourseInfo:BaseDeleteEntity
    {
        /// <summary>
        /// 项目名
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 课程类型ID
        /// </summary>
        [Column(TypeName = "varchar(36)")]
        public string CourseTypeID { get; set; }

        /// <summary>
        /// 课程价格
        /// </summary>
        public int CourseMoney { get; set; }

        /// <summary>
        /// 课程地点
        /// </summary>
        [Column(TypeName = "varchar(36)")]
        public string CourseAddress { get; set; }

        /// <summary>
        /// 开课时间
        /// </summary>
        public DateTime StartCourseTime { get; set; }

        /// <summary>
        /// 课程教练ID
        /// </summary>
        [Column(TypeName = "varchar(36)")]
        public string ThecoachID { get; set; }

        /// <summary>
        /// 限制人数
        /// </summary>
        public int MaxPeople { get; set; }

        /// <summary>
        /// 当前人数
        /// </summary>
        public int People { get; set; }

    }
}
