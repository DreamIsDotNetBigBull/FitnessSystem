using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEntity.ResultModel.MenuInfoResultModel
{
    /// <summary>
    /// 获取菜单信息返回值model
    /// </summary>
  public  class GetInitMenusResultModel
    {

        public object homeInfo { get; set; } = new
        {
            title = "首页",
            href = ""
        };

        public object logoInfo { get; set; } =
        new
        {
            title = "健身管理系统",
            image = "  ../../layuimini/images/logo.png",
            href = "",
        };


        public List<MenuInfoResultModel> menuInfo { get; set; } = new List<MenuInfoResultModel>
        {
             new MenuInfoResultModel()
             {
                  title = "常规管理",
                  icon = "fa fa-address-book",
                  href = "",
                  target = "_self"
             }
        };

        public void SetMenu(List<MenuInfoResultModel> child)
        {
            var menu = menuInfo.FirstOrDefault();
            if (menu != null)
            {
                menu.child = child;
            }
        }
    }
}
