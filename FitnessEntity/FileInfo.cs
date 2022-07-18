using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEntity
{
  public  class FileInfo:BaseEntity
    {/// <summary>
     /// 关系Id(连接其他表的主键字段)
     /// </summary>
        [Column(TypeName = "varchar(36)")]
        public string RelationId { get; set; }
        /// <summary>
        /// 原文件名
        /// </summary>
        [Column(TypeName = "nvarchar(36)")]
        public string RawFileName { get; set; }
        /// <summary>
        /// 新文件名
        /// </summary>
        [Column(TypeName = "nvarchar(36)")]
        public string NewFileName { get; set; }
        /// <summary>
        /// 拓展名
        /// </summary>
        [Column(TypeName = "varchar(12)")]
        public string Extension { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 添加人Id
        /// </summary>
        [Column(TypeName = "varchar(36)")]
        public string Creator { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public FileCategoryEnum Category { get; set; }
    }
    public enum FileCategoryEnum
    {
        个人头像 = 1,
        毕业照 = 2,
        艺术照 = 3
    }
}
