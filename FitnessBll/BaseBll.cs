using FitnessEntity;
using IFitnessDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessBll
{
   public class BaseBll<T> where T: BaseEntity
    {
        private IBaseDal<T> _baseDal;

        public BaseBll(IBaseDal<T> baseDal)
        {
            _baseDal = baseDal;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Add(T entity)
        {
            //UserInfoDal userInfoDal = new UserInfoDal();
            return _baseDal.Add(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(string id)
        {
            return _baseDal.Delete(id);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetEntity(string id)
        {
            return _baseDal.GetEntity(id);
        }
        public bool UpdateEntity(T entity)
        {
            return _baseDal.Update(entity);
        }
    }
}
