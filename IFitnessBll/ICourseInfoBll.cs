using FitnessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFitnessBll
{
    public interface ICourseInfoBll : IBaseDeleteBll<CourseInfo>
    {
        /// <summary>
        /// 判断是否有重复的课程名称
        /// </summary>
        /// <param name="name"></param>
        bool IsHadCourseInfo(string name);

        bool UpdateCourseInfo(string id, string courseName, int courseMoney, string courseAddress, DateTime startCourseTime, string thecoachID, string courseTypeID);
        
        object GetCourseInfoListByPage(int page, int limit, out int count, string courseName, string courseId, string thecoachId);

        /// <summary>
        /// 课程报名
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="courseInfoId"></param>
        /// <returns></returns>
        bool ApplyCourseInfo(UserInfo userInfo, string courseInfoId, out string msg);
        object GetMyCourseInfoListByPage(int page, int limit, out int count, string courseName, string courseId, UserInfo userInfo);
        bool IsApply(string courseInfoId);
    }
}
