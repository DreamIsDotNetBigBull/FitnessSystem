using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEntity.ResultModel.MenuInfoResultModel
{
    /// <summary>
    /// 菜单信息Model
    /// </summary>
 public   class MenuInfoResultModel
    {

        public string ID { get; set; }

        public string title { get; set; }

        public string icon { get; set; }

        public string href { get; set; }

        public string target { get; set; }

        public List<MenuInfoResultModel> child { get; set; }
    }
}
