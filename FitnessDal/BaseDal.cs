using FitnessEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FitnessDal
{
 public   class BaseDal<T> where T:BaseEntity
    {
        //数据库上文
        FitnessDbContext _DbContext;

        public BaseDal(FitnessDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Add(T entity)
        {
            _DbContext.Set<T>().Add(entity);
            int index = _DbContext.SaveChanges();
            if (index > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(string id)
        {
            T entity = _DbContext.Set<T>().FirstOrDefault(u => u.ID == id);
            if (entity == null)
            {
                return false;
            }
            else
            {
                _DbContext.Set<T>().Remove(entity);
                int index = _DbContext.SaveChanges();
                if (index > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            //数据库查出来的实体
            T dbEntity = _DbContext.Set<T>().FirstOrDefault(u => u.ID == entity.ID);

            //反射
            Type type = entity.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();

            foreach (PropertyInfo p in propertyInfos)
            {
                p.SetValue(dbEntity, p.GetValue(entity));
            }

            int index = _DbContext.SaveChanges();
            if (index > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public DbSet<T> GetEntitiesDb()
        {
            return _DbContext.Set<T>();
        }

        /// <summary>
        /// 根据id获取实体信息
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public T GetEntity(string entityId)
        {
            T entity = _DbContext.Set<T>().FirstOrDefault(u => u.ID == entityId);
            return entity;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="deleteEntityInfos"></param>
        public int Adds(List<T> addEntityInfos)
        {
            _DbContext.Set<T>().AddRange(addEntityInfos);
            return _DbContext.SaveChanges();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="deleteEntityInfos"></param>
        public void Deletes(List<T> deleteEntityInfos)
        {
            _DbContext.Set<T>().RemoveRange(deleteEntityInfos);
            _DbContext.SaveChanges();
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="deleteEntityInfos"></param>
        public int Updates(List<T> updateEntityInfos)
        {
            _DbContext.Set<T>().UpdateRange(updateEntityInfos);
            return _DbContext.SaveChanges();
        }

    }
}
