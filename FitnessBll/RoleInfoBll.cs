using FitnessEntity;
using IFitnessBll;
using IFitnessDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessBll
{
    public class RoleInfoBll: BaseDeleteBll<RoleInfo>,IRoleInfoBll
    {
        private IRoleInfoDal _roleInfoDal;
        private IR_RoleInfo_MenuInfoDal _r_RoleInfo_MenuInfo;
        public RoleInfoBll(IRoleInfoDal roleInfoDal, IR_RoleInfo_MenuInfoDal r_RoleInfo_MenuInfo) : base(roleInfoDal)
        {
            _roleInfoDal = roleInfoDal;

            _r_RoleInfo_MenuInfo = r_RoleInfo_MenuInfo;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleInfo"></param>
        /// <returns></returns>
        public bool AddRoleInfo(RoleInfo roleInfo)
        {
            //RoleInfoDal roleInfoDal = new RoleInfoDal();
            return _roleInfoDal.Add(roleInfo);
        }

        public void BindMenuInfo(string roleInfoId, List<string> menuInfoIds)
        {
            DateTime now = DateTime.Now;

            //当前角色已绑定的用户信息
            var roleInfo_MenuInfos = _r_RoleInfo_MenuInfo.GetEntitiesDb().Where(r => r.RoleID == roleInfoId).ToList();

            //先删除已绑定的
            foreach (var item in roleInfo_MenuInfos)
            {
                if (!menuInfoIds.Contains(item.MenuID))
                {
                    //userInfoIds不存在的用户id就删除
                    _r_RoleInfo_MenuInfo.Delete(item.ID);
                }
            }

            //添加
            foreach (var item in menuInfoIds)
            {
                //如果已经存在的用户就不添加，不存在的才添加
                if (!roleInfo_MenuInfos.Any(a => a.MenuID == item))
                {
                    R_RoleInfo_MenuInfo r_RoleInfo_MenuInfo = new R_RoleInfo_MenuInfo()
                    {
                        ID = Guid.NewGuid().ToString(),
                        CreateTime = now,
                        RoleID = roleInfoId,
                        MenuID = item
                    };
                    _r_RoleInfo_MenuInfo.Add(r_RoleInfo_MenuInfo);
                }
            }
        }

        /// <summary>
        /// 获取当前角色已绑定的菜单id集合
        /// </summary>
        /// <param name="roleInfoId"></param>
        /// <returns></returns>
        public List<string> GetBindMenuIds(string roleInfoId)
        {
            //查询当前角色已绑定的用户id
            return _r_RoleInfo_MenuInfo.GetEntitiesDb().Where(r => r.RoleID == roleInfoId).Select(r => r.MenuID).ToList();
        }

        /// <summary>
        /// 根据角色名称查找角色Id
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public RoleInfo GetroleId(string roleName)
        {
           return _roleInfoDal.GetEntitiesDb().FirstOrDefault(r => r.RoleName == roleName && !r.IsDelete);
        }

        /// <summary>
        /// 获取角色集合
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public object GetRoleInfoListByPage(int page, int limit, out int count, string roleName)
        {
            //获取数据库中用户全部没删除的数据（未真实查询）
            var roleInfos = _roleInfoDal.GetEntitiesDb().Where(u => u.IsDelete == false);

            if (!string.IsNullOrEmpty(roleName))
            {
                roleInfos = roleInfos.Where(u => u.RoleName.Contains(roleName));
            }

            //查询出来数据的数量
            count = roleInfos.Count();

            //分页
            roleInfos = roleInfos.OrderBy(r => r.CreateTime).Skip((page - 1) * limit).Take(limit);

            var list = roleInfos.ToList().Select(u =>
            {
                return new
                {
                    u.ID,
                    u.RoleName,
                    u.Description,
                    CreateTime = u.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")
                };
            });

            return list;
        }

        /// <summary>
        /// 获取角色下拉选
        /// </summary>
        /// <returns></returns>
        public object GetRoleSelectOption()
        {
           return _roleInfoDal.GetEntitiesDb().Where(r => !r.IsDelete).Select(r=>new 
           { 
                r.ID,
                r.RoleName
           });
        }

        /// <summary>
        /// //判断数据库有没有相同角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool IsHadRoleInfo(string roleName)
        {
            int count = _roleInfoDal.GetEntitiesDb().Where(r => r.RoleName == roleName && !r.IsDelete).Count();
            return count > 0;
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="roleName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool UpdateRoleInfo(string roleId, string roleName, string description)
        {
            RoleInfo roleInfo = _roleInfoDal.GetEntity(roleId);
            if (roleInfo != null)
            {
                roleInfo.RoleName = roleName;
                roleInfo.Description = description;

                return _roleInfoDal.Update(roleInfo);
            }
            else
            {
                return false;
            }
        }
    }
}
