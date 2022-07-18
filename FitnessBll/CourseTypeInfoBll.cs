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
  public  class CourseTypeInfoBll:BaseBll<CourseTypeInfo>, ICourseTypeInfoBll
    {
        private ICourseTypeInfoDal _courseTypeInfoDal;
        public CourseTypeInfoBll(ICourseTypeInfoDal courseTypeInfoDal):base(courseTypeInfoDal)
        {
            _courseTypeInfoDal = courseTypeInfoDal;
        }

        public object GetCourseInfoListByPage(int page, int limit, out int count, string courseTypeID)
        {
          var courseTypeInfos= _courseTypeInfoDal.GetEntitiesDb().AsQueryable();
           if(!string.IsNullOrEmpty(courseTypeID))
            {
                courseTypeInfos = courseTypeInfos.Where(c => c.ID.Contains(courseTypeID));
            }

            //查询出来的数据的数量
            count = courseTypeInfos.Count();

            var list = (from c in courseTypeInfos
                        select new
                        {
                            c.ID,
                            c.CourseType,
                            c.Description
                        }).OrderBy(c => c.ID).Skip((page - 1) * limit).Take(limit).ToList().Select(c =>
                        {
                            return new
                            {
                                c.ID,
                                CourseTypeName=  c.CourseType,
                                c.Description
                            };
            });
            return list;

            
        }


        /// <summary>
        /// 获取课程类型下拉选数据集
        /// </summary>
        public object GetCourseTypeSelectOption()
        {
        var courseTypeInfos=   _courseTypeInfoDal.GetEntitiesDb().Select(c => new
            {
                c.ID,
               CourseTypeName= c.CourseType
            }).ToList();
            return courseTypeInfos;
        }

        public bool IsHadCourseTypeInfo(string courseType)
        {
         return   _courseTypeInfoDal.GetEntitiesDb().Where(c => c.CourseType==courseType).Count() > 0;
        }

        public bool UpdateCourseTypeInfo(string courseTypeInfoId, string courseType, string description)
        {
            CourseTypeInfo courseTypeInfo = _courseTypeInfoDal.GetEntity(courseTypeInfoId);
            if (courseTypeInfo != null)
            {
                courseTypeInfo.CourseType = courseType;
                courseTypeInfo.Description = description;
                return _courseTypeInfoDal.Update(courseTypeInfo);
            }
            else
            {
                return false;
            }
        }
    }
}
