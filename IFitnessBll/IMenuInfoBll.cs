using FitnessEntity;
using FitnessEntity.ResultModel.MenuInfoResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFitnessBll
{
    public interface IMenuInfoBll:IBaseDeleteBll<MenuInfo>
    {
        /// <summary>
        /// 获取菜单集合信息
        /// </summary>
        /// <returns></returns>
        List<MenuInfoResultModel> GetInitMenus(UserInfo userInfoId);

        ///// <summary>
        ///// 更新菜单信息
        ///// </summary>
        ///// <param name="menuInfo"></param>
        ///// <returns></returns>
        //bool UpdateMenuInfo(MenuInfo menuInfo);


        object GetMenuInfoListByPage(int page, int limit, out int count, string title);

        /// <summary>
        /// 判断当前菜单是否存在
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        bool IsHadMenuInfo(string title);


        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="iD"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="level"></param>
        /// <param name="sort"></param>
        /// <param name="href"></param>
        /// <param name="parentId"></param>
        /// <param name="icon"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        bool UpdateMenuInfo(string iD, string title, string description, int level, int sort, string href, string parentId, string icon, string target);


        /// <summary>
        /// 获取菜单下拉选数据集
        /// </summary>
        /// <returns></returns>
        object GetSelectOption();

        /// <summary>
        /// 获取菜单下拉选数据集（不要当前自己的选项）
        /// </summary>
        /// <returns></returns>
        object GetSelectOption(string menuInfoId);
    }
}
