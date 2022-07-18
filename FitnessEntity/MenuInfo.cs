using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEntity
{
    /// <summary>
    /// 菜单表实例
    /// </summary>
     public class MenuInfo:BaseDeleteEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Column(TypeName = "varchar(16)")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column(TypeName = "nvarchar(32)")]
        public string Description { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 访问地址
        /// </summary>
        [Column(TypeName = "varchar(128)")]
        public string Href { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        [Column(TypeName = "varchar(36)")]
        public string ParentId { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Column(TypeName = "varchar(32)")]
        public string Icon { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        [Column(TypeName = "varchar(16)")]
        public string Target { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        ////是否删除
        //public bool IsDelete { get; set; }

        ////删除时间
        //public DateTime DeleteTime { get; set; }
    }
}
