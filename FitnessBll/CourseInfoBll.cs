using FitnessEntity;
using IFitnessBll;
using IFitnessDal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessBll
{
    public class CourseInfoBll : BaseDeleteBll<CourseInfo>, ICourseInfoBll
    {
        private ICourseInfoDal _courseInfoDal;
        private ICourseTypeInfoDal _courseTypeInfoDal;
        private IUserInfoDal _userInfoDal;
        private ICourseDetailedDal _courseDetailedDal;
        private FitnessDbContext _dbContext;
        private IRoleInfoDal _roleInfoDal;
        private IR_UserInfo_RoleInfoDal _R_UserInfo_RoleInfoDal;
        private IRechargeInfoDal _rechargeInfoDal;
        public CourseInfoBll(ICourseInfoDal courseInfoDal, ICourseTypeInfoDal courseTypeInfoDal, IUserInfoDal userInfoDal, ICourseDetailedDal courseDetailedDal, FitnessDbContext dbContext, IRoleInfoDal roleInfoDal, IR_UserInfo_RoleInfoDal R_UserInfo_RoleInfoDal
            , IRechargeInfoDal rechargeInfoDal) : base(courseInfoDal)
        {
            _courseInfoDal = courseInfoDal;
            _courseTypeInfoDal = courseTypeInfoDal;
            _userInfoDal = userInfoDal;
            _courseDetailedDal = courseDetailedDal;
            _dbContext = dbContext;
            _roleInfoDal = roleInfoDal;
            _R_UserInfo_RoleInfoDal = R_UserInfo_RoleInfoDal;
            _rechargeInfoDal = rechargeInfoDal;
        }

        /// <summary>
        /// 课程报名
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="courseInfoId"></param>
        /// <returns></returns>
        public bool ApplyCourseInfo(UserInfo userInfo, string courseInfoId,out string msg)
        {
            var courseInfos = _courseInfoDal.GetEntitiesDb().FirstOrDefault(c => !c.IsDelete && c.ID == courseInfoId);
            var coursedetailed1=_courseDetailedDal.GetEntitiesDb().FirstOrDefault(c => c.UserId == userInfo.ID && c.CourseInfoId == courseInfoId);
            if (coursedetailed1!=null)
            {
                msg = "不能重复报名";
                return false;
            }

            var userInfo1 = _userInfoDal.GetEntitiesDb().FirstOrDefault(u => !u.IsDelete && u.ID == userInfo.ID);
            if (courseInfos == null)
            {
                msg = "未获取到当前报名的课程";
                return false;
            }
            if (userInfo1.Balance < courseInfos.CourseMoney)
            {
                msg = "当前余额不足，请充值";
                return false;
            }
            if (courseInfos.MaxPeople == courseInfos.People)
            {
                msg = "该课程已满人";
                return false;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                Coursedetailed coursedetailed = new Coursedetailed()
                {
                    UserId = userInfo.ID,
                    CourseInfoId = courseInfoId,
                    ID = Guid.NewGuid().ToString(),
                    ThisTime = DateTime.Now
                };
                bool isSuccess1 = _courseDetailedDal.Add(coursedetailed);
                courseInfos.People++;
                bool isSuccess2 = _courseInfoDal.Update(courseInfos);
                userInfo1.Balance -= courseInfos.CourseMoney;
                bool isSuccess3 = _userInfoDal.Update(userInfo1);
                RechargeInfo rechargeInfo = new RechargeInfo()
                {
                    CreateTime = DateTime.Now,
                    Creator = userInfo.ID,
                    CurrentBalance = courseInfos.CourseMoney,
                    ID = Guid.NewGuid().ToString(),
                    Description = "报名了" + courseInfos.CourseName,
                    InsertType = InsertTypeEnum.消费,
                };
                //添加消费记录
                bool isSuccess4 = _rechargeInfoDal.Add(rechargeInfo);
                if (isSuccess1 && isSuccess2 && isSuccess3 && isSuccess4)
                {
                    transaction.Commit();
                    msg = "报名成功";
                    return true;
                }
            }
            msg = "报名失败";
            return false;
        }

        /// <summary>
        /// 获取课程集合（分页
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <param name="courseName"></param>
        /// <param name="courseId"></param>
        /// <param name="thecoachId"></param>
        /// <returns></returns>
        public object GetCourseInfoListByPage(int page, int limit, out int count, string courseName, string courseId, string thecoachId)
        {
            var courseInfos = _courseInfoDal.GetEntitiesDb().Where(c => c.IsDelete == false);

            if (!string.IsNullOrEmpty(courseName))
            {
                courseInfos = courseInfos.Where(c => c.CourseName.Contains(courseName));
            }
            if (!string.IsNullOrEmpty(courseId))
            {
                courseInfos = courseInfos.Where(c => c.CourseTypeID.Contains(courseId));
            }
            if (!string.IsNullOrEmpty(thecoachId))
            {
                courseInfos = courseInfos.Where(c => c.ThecoachID.Contains(thecoachId));
            }

            //查询出来的数据的数量
            count = courseInfos.Count();

            //获取课程类型表数据集
            DbSet<CourseTypeInfo> courseTypeInfos = _courseTypeInfoDal.GetEntitiesDb();

            //连表查询
            var temp = (from c in courseInfos
                        join ct in courseTypeInfos
                        on c.CourseTypeID equals ct.ID into cctTemp
                        from cct in cctTemp.DefaultIfEmpty()

                        join u in _userInfoDal.GetEntitiesDb().Where(u => !u.IsDelete)
                        on c.ThecoachID equals u.ID into tcTemp
                        from tc in tcTemp.DefaultIfEmpty()

                        select new
                        {
                            c.ID,
                            c.CourseName,
                            c.CourseMoney,
                            c.CourseAddress,
                            c.StartCourseTime,
                            tc.UserName,
                            cct.CourseType,
                            c.People,
                            c.MaxPeople
                        });

            //分页
            temp = temp.OrderBy(c => c.StartCourseTime).Skip((page - 1) * limit).Take(limit);

            var list = temp.ToList().Select(c =>
            {
                return new
                {
                    c.ID,
                    CourseName = c.CourseName,
                    c.CourseMoney,
                    c.CourseAddress,
                    StartCourseTime = c.StartCourseTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    ThecoachName = c.UserName,
                    CourseTypeName = c.CourseType,
                    c.MaxPeople,
                    c.People
                };
            });
            return list;

        }

        /// <summary>
        /// 获取我的课程集合
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <param name="courseName"></param>
        /// <param name="courseId"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public object GetMyCourseInfoListByPage(int page, int limit, out int count, string courseName, string courseId, UserInfo userInfo)
        {

            var courseInfos = _courseInfoDal.GetEntitiesDb().Where(c => c.IsDelete == false);

            //如果不是系统管理员，则只能看到自己申请的流程
            if (!userInfo.IsAdmin)
            {
                var _UserInfo_RoleInfo=_R_UserInfo_RoleInfoDal.GetEntitiesDb().FirstOrDefault(ur => ur.UserID == userInfo.ID);
                var roleInfo=_roleInfoDal.GetEntitiesDb().FirstOrDefault(r=>!r.IsDelete && r.ID==_UserInfo_RoleInfo.RoleID);
                if (roleInfo.RoleName=="会员")
                {
                    var coursedetaileds=_courseDetailedDal.GetEntitiesDb().Where(c => c.UserId == userInfo.ID);
                    List<string> s = new List<string>();
                    foreach (var item in coursedetaileds)
                    {
                        s.Add(item.CourseInfoId);
                    }
                    courseInfos = courseInfos.Where(w => s.Contains(w.ID));
                }
                else
                {
                    courseInfos = courseInfos.Where(w => w.ThecoachID == userInfo.ID);
                }
                
            }
            if (!string.IsNullOrEmpty(courseName))
            {
                courseInfos = courseInfos.Where(c => c.CourseName.Contains(courseName));
            }
            if (!string.IsNullOrEmpty(courseId))
            {
                courseInfos = courseInfos.Where(c => c.CourseTypeID.Contains(courseId));
            }

            //查询出来的数据的数量
            count = courseInfos.Count();

            //获取课程类型表数据集
            DbSet<CourseTypeInfo> courseTypeInfos = _courseTypeInfoDal.GetEntitiesDb();

            //连表查询
            var temp = (from c in courseInfos
                        join ct in courseTypeInfos
                        on c.CourseTypeID equals ct.ID into cctTemp
                        from cct in cctTemp.DefaultIfEmpty()

                        join u in _userInfoDal.GetEntitiesDb().Where(u => !u.IsDelete)
                        on c.ThecoachID equals u.ID into tcTemp
                        from tc in tcTemp.DefaultIfEmpty()

                        select new
                        {
                            c.ID,
                            c.CourseName,
                            c.CourseMoney,
                            c.CourseAddress,
                            c.StartCourseTime,
                            tc.UserName,
                            cct.CourseType,
                            c.People,
                            c.MaxPeople
                        });

            //分页
            temp = temp.OrderBy(c => c.StartCourseTime).Skip((page - 1) * limit).Take(limit);

            var list = temp.ToList().Select(c =>
            {
                return new
                {
                    c.ID,
                    CourseName = c.CourseName,
                    c.CourseMoney,
                    c.CourseAddress,
                    StartCourseTime = c.StartCourseTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    ThecoachName = c.UserName,
                    CourseTypeName = c.CourseType,
                    c.MaxPeople,
                    c.People
                };
            });
            return list;
        }

        public bool IsApply(string courseInfoId)
        {
            var c=_courseDetailedDal.GetEntitiesDb().Where(c => c.CourseInfoId == courseInfoId);
            if (c!=null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否有重复的课程名称
        /// </summary>
        /// <param name="name"></param>
        public bool IsHadCourseInfo(string name)
        {
            return _courseInfoDal.GetEntitiesDb().Where(c => c.CourseName == name).Count() > 0;
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="courseInfoId"></param>
        /// <returns></returns>
        public bool SoftDeleteCourseInfo(string courseInfoId)
        {
            CourseInfo courseInfo = _courseInfoDal.GetEntity(courseInfoId);
            if (courseInfo != null)
            {
                courseInfo.IsDelete = true;
                courseInfo.DeleteTime = DateTime.Now;
                return _courseInfoDal.Update(courseInfo);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新课程
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="courseMoney"></param>
        /// <param name="courseAddress"></param>
        /// <param name="startCourseTime"></param>
        /// <param name="thecoachID"></param>
        /// <returns></returns>
        public bool UpdateCourseInfo(string id, string courseName, int courseMoney, string courseAddress, DateTime startCourseTime, string thecoachID, string courseTypeID)
        {
            CourseInfo courseInfo = _courseInfoDal.GetEntity(id);
            if (courseInfo != null)
            {
                courseInfo.CourseName = courseName;
                courseInfo.CourseMoney = courseMoney;
                courseInfo.CourseAddress = courseAddress;
                courseInfo.StartCourseTime = startCourseTime;
                courseInfo.ThecoachID = thecoachID;
                courseInfo.CourseTypeID = courseTypeID;
                return _courseInfoDal.Update(courseInfo);
            }
            else
            {
                return false;
            }
        }
    }
}
