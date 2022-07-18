using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEntity
{
    /// <summary>
    /// 菜单角色实例
    /// </summary>
    public class Menu_RoleInfo
    {
        /// <summary>
        /// 菜单id
        /// </summary>
        public string MenuId { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
