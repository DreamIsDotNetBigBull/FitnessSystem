using FitnessEntity;
using IFitnessDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessBll
{
    public class BaseDeleteBll<T>:BaseBll<T> where T :BaseDeleteEntity
    {
        private IBaseDal<T> _baseDal;

        public BaseDeleteBll(IBaseDal<T> baseDal) : base(baseDal)
        {
            _baseDal = baseDal;
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SoftDeleteEntity(string id)
        {
            T entity = _baseDal.GetEntity(id);
            if (entity != null)
            {
                entity.IsDelete = true;
                entity.DeleteTime = DateTime.Now;
                return _baseDal.Update(entity);
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 软删除多个
        /// </summary>
        /// <param name="ids"></param>
        public void SoftDeleteEntites(List<string> ids)
        {

            //获取用户要删除的用户集合
            var deleteUserInfos = _baseDal.GetEntitiesDb().Where(u => ids.Contains(u.ID)).ToList();
            DateTime dateTime = DateTime.Now;
            foreach (var item in deleteUserInfos)
            {
                item.IsDelete = true;
                item.DeleteTime = dateTime;
            }
            _baseDal.Updates(deleteUserInfos);
        }
    }
}
