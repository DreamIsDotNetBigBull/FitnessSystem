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
    public  class CategoryBll:BaseBll<Category>,ICategoryBll
    {
        private ICategoryDal _categoryDal;
        public CategoryBll(ICategoryDal categoryDal) : base(categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public object GetCategoryListByPage(int page, int limit, out int count, string categoryName)
        {
            //获取数据库中类别全部没删除的数据（未真实查询）
            var type = _categoryDal.GetEntitiesDb().AsQueryable();

            if (!string.IsNullOrEmpty(categoryName))
            {
                type = type.Where(u => u.CategoryType.Contains(categoryName));
            }

            //查询出来数据的数量
            count = _categoryDal.GetEntitiesDb().Count();

            //分页
            type = type.OrderBy(u => u.CategoryType).Skip((page - 1) * limit).Take(limit);

            var list = type.ToList().Select(u =>
            {
                return new
                {
                    u.CategoryType,
                    u.ID,
                    u.Description
                };
            });

            return list;
        }

        public object GetCategorySelectOption()
        {
            var category = _categoryDal.GetEntitiesDb().Select(d => new
            {
                d.ID,
                d.CategoryType
            }).ToList();
            return category;
        }

        public bool IsHadCategory(string categoryType)
        {
            return _categoryDal.GetEntitiesDb().Where(c => c.CategoryType == categoryType).Count() > 0;
        }

        public bool UpdateCategory(string categoryId, string categoryType, string description)
        {
            Category category = _categoryDal.GetEntity(categoryId);
            if (category != null)
            {
                category.CategoryType = categoryType;
                category.Description = description;
                return _categoryDal.Update(category);
            }
            else
            {
                return false;
            }
        }
    }
}
