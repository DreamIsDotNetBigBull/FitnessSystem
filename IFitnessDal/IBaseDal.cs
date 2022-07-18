using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFitnessDal
{
 public   interface IBaseDal<T> where T:class
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Add(T entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(string id);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <returns></returns>
        DbSet<T> GetEntitiesDb();

        /// <summary>
        /// 根据id获取实体信息
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        T GetEntity(string entityId);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="deleteEntityInfos"></param>
        int Adds(List<T> addEntityInfos);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="deleteEntityInfos"></param>
        void Deletes(List<T> deleteEntityInfos);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="deleteEntityInfos"></param>
        int Updates(List<T> updateEntityInfos);
    }
}
