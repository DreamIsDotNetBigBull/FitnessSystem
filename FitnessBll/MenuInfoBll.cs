using FitnessEntity;
using FitnessEntity.DTO;
using FitnessEntity.ResultModel.MenuInfoResultModel;
using IFitnessBll;
using IFitnessDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessBll
{
    public class MenuInfoBll:BaseDeleteBll<MenuInfo>,IMenuInfoBll
    {
        private IMenuInfoDal _menuInfoDal;
        private IR_RoleInfo_MenuInfoDal _r_RoleInfo_MenuInfo;
        private IR_UserInfo_RoleInfoDal _r_UserInfo_RoleInfoDal;
        private IRoleInfoDal _roleInfoDal;

        public MenuInfoBll(IMenuInfoDal menuInfoDal, IR_RoleInfo_MenuInfoDal r_RoleInfo_MenuInfo, IR_UserInfo_RoleInfoDal r_UserInfo_RoleInfoDal, IRoleInfoDal roleInfoDal) : base(menuInfoDal)
        {
            _menuInfoDal = menuInfoDal;
            _r_RoleInfo_MenuInfo = r_RoleInfo_MenuInfo;
            _r_UserInfo_RoleInfoDal = r_UserInfo_RoleInfoDal;
            _roleInfoDal = roleInfoDal;
        }

        /// <summary>
        /// /获取菜单集合信息
        /// </summary>
        /// <returns></returns>
        public List<MenuInfoResultModel> GetInitMenus(UserInfo userInfo)
        {
            //先判断用户信息里有没有isAdmin等于true，并且他有“系统管理员”角色才给他看到所有的菜单
            int count = (from ur in _r_UserInfo_RoleInfoDal.GetEntitiesDb().Where(r => r.UserID == userInfo.ID)
                         join r in _roleInfoDal.GetEntitiesDb().Where(r => !r.IsDelete && r.RoleName == "系统管理员")
                         on ur.RoleID equals r.ID
                         select ur.ID).Count();

            //三目表达式
            bool isHas = count > 0 ? true : false;

            List<MenuInfoDTO> menus = new List<MenuInfoDTO>();

            //超级管理员
            if (userInfo.IsAdmin == true && isHas == true)
            {
                //获取所有菜单集合信息
                menus = _menuInfoDal.GetEntitiesDb().Where(m => !m.IsDelete).OrderBy(m => m.Sort).Select(m => new MenuInfoDTO
                {
                    ID = m.ID,
                    Title = m.Title,
                    Icon = m.Icon,
                    Href = m.Href,
                    Target = m.Target,
                    Level = m.Level,
                    ParentId = m.ParentId
                }).ToList();
            }
            else //普通用户
            {
                //根据用户id获取用户的角色
                var roleInfoIds = _r_UserInfo_RoleInfoDal.GetEntitiesDb().Where(u => u.UserID == userInfo.ID).Select(u => u.RoleID).ToList();

                //获取菜单的id集合
                var menuIds = _r_RoleInfo_MenuInfo.GetEntitiesDb().Where(r => roleInfoIds.Contains(r.RoleID)).Select(r => r.MenuID).ToList();

                //去重后的菜单id
                var dMenuIds = menuIds.Distinct().ToList();

                menus = (from m in _menuInfoDal.GetEntitiesDb().Where(m => menuIds.Contains(m.ID)).ToList()
                         join menuId in dMenuIds
                         on m.ID equals menuId
                         select new
                         {
                             ID = m.ID,
                             m.Sort,
                             Title = m.Title,
                             Icon = m.Icon,
                             Href = m.Href,
                             Target = m.Target,
                             Level = m.Level,
                             ParentId = m.ParentId
                         }).OrderBy(m => m.Sort).Select(m => new MenuInfoDTO
                         {
                             ID = m.ID,
                             Title = m.Title,
                             Icon = m.Icon,
                             Href = m.Href,
                             Target = m.Target,
                             Level = m.Level,
                             ParentId = m.ParentId
                         }).ToList();


                //根据角色来获取菜单
                //menus = _menuInfoDal.GetEntitiesDb().Where(m => menuIds.Contains(m.ID)).Select(m => new
                //{
                //    ID = m.ID,
                //    m.Sort,
                //    Title = m.Title,
                //    Icon = m.Icon,
                //    Href = m.Href,
                //    Target = m.Target,
                //    Level = m.Level,
                //    ParentId = m.ParentId
                //}).OrderBy(m => m.Sort).Select(m => new MenuInfoDTO
                //{
                //    ID = m.ID,
                //    Title = m.Title,
                //    Icon = m.Icon,
                //    Href = m.Href,
                //    Target = m.Target,
                //    Level = m.Level,
                //    ParentId = m.ParentId
                //}).ToList();

                //menus = (from rm in _r_RoleInfo_MenuInfo.GetEntitiesDb().Where(r => roleInfoIds.Contains(r.RoleID))
                //             join m in _menuInfoDal.GetEntitiesDb().Where(m => !m.IsDelete)
                //             on rm.MenuID equals m.ID
                //             select new
                //             {
                //                 ID = m.ID,
                //                 m.Sort,
                //                 Title = m.Title,
                //                 Icon = m.Icon,
                //                 Href = m.Href,
                //                 Target = m.Target,
                //                 Level = m.Level,
                //                 ParentId = m.ParentId
                //             }).OrderBy(m => m.Sort).Select(m => new MenuInfoDTO
                //             {
                //                 ID = m.ID,
                //                 Title = m.Title,
                //                 Icon = m.Icon,
                //                 Href = m.Href,
                //                 Target = m.Target,
                //                 Level = m.Level,
                //                 ParentId = m.ParentId
                //             }).ToList();

            }

            //获取父级菜单集合信息
            List<MenuInfoResultModel> parentMenus = menus.Where(m => m.Level == 0 && m.ParentId == null).Select(m => new MenuInfoResultModel
            {
                ID = m.ID,
                href = m.Href,
                icon = m.Icon,
                target = m.Target,
                title = m.Title
            }).ToList();

            foreach (var parentMenu in parentMenus)
            {
                //查询父级菜单自己的自己菜单
                var childMenus = menus.Where(m => m.ParentId == parentMenu.ID).Select(m => new MenuInfoResultModel
                {
                    ID = m.ID,
                    href = m.Href,
                    icon = m.Icon,
                    target = m.Target,
                    title = m.Title
                }).ToList();
                //为父级菜单添加子集菜单
                parentMenu.child = childMenus;
                //递归赋值子菜单
                RecursionMenu(parentMenu.child, menus);
            }

            return parentMenus;
        }

        /// <summary>
        /// 递归赋值菜单集合
        /// </summary>
        /// <param name="pMenus"></param>
        /// <param name="menus"></param>
        public void RecursionMenu(List<MenuInfoResultModel> pMenus, List<MenuInfoDTO> menus)
        {
            foreach (var item in pMenus)
            {
                var childMenus2 = menus.Where(m => m.ParentId == item.ID).Select(m => new MenuInfoResultModel
                {
                    ID = m.ID,
                    href = m.Href,
                    icon = m.Icon,
                    target = m.Target,
                    title = m.Title
                }).ToList();
                //为父级菜单添加子集菜单
                item.child = childMenus2;

                RecursionMenu(item.child, menus);
            }
        }



        ///// <summary>
        ///// 根据id获取菜单
        ///// </summary>
        ///// <param name="menuInfoId"></param>
        ///// <returns></returns>
        //public MenuInfo GetMenuInfo(string menuInfoId)
        //{
        //    return _menuInfoDal.GetEntity(menuInfoId);
        //}

        public object GetMenuInfoListByPage(int page, int limit, out int count, string title)
        {
            //获取数据库中用户全部没删除的数据（未真实查询）
            var menuInfos = _menuInfoDal.GetEntitiesDb().Where(u => u.IsDelete == false);

            //搜索
            if (!string.IsNullOrEmpty(title))
            {
                menuInfos = menuInfos.Where(u => u.Title.Contains(title));
            }

            //查询出来数据的数量
            count = menuInfos.Count();

            ////获取用户数据集
            //var userInfos = _userInfoDal.GetEntitiesDb();

            ////查询部门所有未删除的信息
            //var allDepartmentInfos = _menuInfoDal.GetEntitiesDb().Where(u => u.IsDelete == false);

            //var tempList = from d in menuInfos
            //               join u in userInfos
            //               on d.LeaderID equals u.ID into duTemp
            //               from du in duTemp.DefaultIfEmpty()

            //               join pd in allDepartmentInfos
            //               on d.ParentID equals pd.ID into dpdTemp
            //               from dpd in dpdTemp.DefaultIfEmpty()
            //               select new
            //               {
            //                   d.ID,
            //                   d.DepartmentName,
            //                   d.Description,
            //                   d.CreateTime,
            //                   du.UserName,
            //                   ParentDeparmentName = dpd.DepartmentName
            //               };


            //分页
            var res = (from m in menuInfos
                       join m2 in _menuInfoDal.GetEntitiesDb()
                       on m.ParentId equals m2.ID into mmTemp
                       from m3 in mmTemp.DefaultIfEmpty()
                       select new
                       {
                           m.ID,
                           m.Title,
                           m.Description,
                           m.Level,
                           m.Sort,
                           m.Href,
                           m.Icon,
                           m.Target,
                           m.CreateTime,
                           ParentTitle = m3.Title
                       }).OrderBy(d => d.Sort).Skip((page - 1) * limit).Take(limit).ToList().Select(u =>
                       {
                           return new
                           {
                               u.ID,
                               u.Title,
                               u.Description,
                               u.Level,
                               u.Sort,
                               u.Href,
                               u.Icon,
                               u.Target,
                               CreateTime = u.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                               u.ParentTitle
                           };
                       });

            return res;
        }

        /// <summary>
        /// 获取菜单下拉选数据集
        /// </summary>
        /// <returns></returns>
        public object GetSelectOption()
        {
            var menuInfos = _menuInfoDal.GetEntitiesDb().OrderBy(d => d.Sort).Where(d => !d.IsDelete).Select(d => new
            {
                d.ID,
                d.Title
            }).ToList();
            return menuInfos;
        }

        public object GetSelectOption(string menuInfoId)
        {
            var menuInfos = _menuInfoDal.GetEntitiesDb().Where(d => d.ID != menuInfoId && !d.IsDelete).Select(d => new
            {
                d.ID,
                d.Title
            }).ToList();
            return menuInfos;
        }

        /// <summary>
        /// 判断当前菜单是否存在
        /// </summary>
        /// <param name="menuName"></param>
        /// <returns></returns>
        public bool IsHadMenuInfo(string title)
        {
            int count = _menuInfoDal.GetEntitiesDb().Where(r => r.Title == title && !r.IsDelete).Count();
            return count > 0;
        }

        ///// <summary>
        ///// 软删除
        ///// </summary>
        ///// <param name="menuInfoId"></param>
        ///// <returns></returns>
        //public bool SoftDeleteMenuInfo(string menuInfoId)
        //{
        //    MenuInfo menuInfo = _menuInfoDal.GetEntity(menuInfoId);
        //    if (menuInfo != null)
        //    {
        //        menuInfo.IsDelete = true;
        //        menuInfo.DeleteTime = DateTime.Now;
        //        return _menuInfoDal.Update(menuInfo);
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}


        ///// <summary>
        ///// 软删除多个用户
        ///// </summary>
        ///// <param name="userInfoIds"></param>
        ///// <returns></returns>
        //public void SoftDeleteUserInfos(List<string> menuInfoIds)
        //{
        //    //获取用户要删除的菜单集合
        //    var deleteMenuInfos = _menuInfoDal.GetEntitiesDb().Where(u => menuInfoIds.Contains(u.ID)).ToList();
        //    DateTime dateTime = DateTime.Now;
        //    foreach (var item in deleteMenuInfos)
        //    {
        //        item.IsDelete = true;
        //        item.DeleteTime = dateTime;
        //    }
        //    _menuInfoDal.Updates(deleteMenuInfos);
        //}

        ///// <summary>
        ///// 更新菜单
        ///// </summary>
        ///// <param name="menuInfo"></param>
        ///// <returns></returns>
        //public bool UpdateMenuInfo(MenuInfo menuInfo)
        //{
        //    return _menuInfoDal.Update(menuInfo);
        //}

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="level"></param>
        /// <param name="sort"></param>
        /// <param name="href"></param>
        /// <param name="parentId"></param>
        /// <param name="icon"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool UpdateMenuInfo(string id, string title, string description, int level, int sort, string href, string parentId, string icon, string target)
        {
            MenuInfo menuInfo = _menuInfoDal.GetEntity(id);
            if (menuInfo != null)
            {
                menuInfo.Target = title;
                menuInfo.Description = description;
                menuInfo.Level = level;
                menuInfo.Sort = sort;
                menuInfo.Href = href;
                menuInfo.ParentId = parentId;
                menuInfo.Icon = icon;
                menuInfo.Target = target;
                return _menuInfoDal.Update(menuInfo);
            }
            else
            {
                return false;
            }
        }
    }
}
