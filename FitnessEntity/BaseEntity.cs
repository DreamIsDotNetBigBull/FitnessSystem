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
    /// 父类主键ID
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        /// 
        [Key]
        [Column(TypeName ="varchar(36)")]
        public string ID { get; set; }
    }
}
