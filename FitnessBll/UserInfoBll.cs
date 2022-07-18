using FitnessEntity;
using IFitnessBll;
using IFitnessDal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessBll
{
    /// <summary>
    /// 用户业务访问层
    /// </summary>
    public class UserInfoBll : BaseDeleteBll<UserInfo>, IUserInfoBll
    {
        private IUserInfoDal _userInfoDal;
        private IR_UserInfo_RoleInfoDal _r_UserInfo_RoleInfoDal;
        private IRoleInfoDal _roleInfoDal;
        private IFileInfoDal _fileInfoDal;
        private ICategoryDal _categoryDal;

        public UserInfoBll(IUserInfoDal userInfoDal, IR_UserInfo_RoleInfoDal r_UserInfo_RoleInfoDal, IRoleInfoDal roleInfoDal, IFileInfoDal fileInfoDal, ICategoryDal categoryDal) : base(userInfoDal)
        {
            _userInfoDal = userInfoDal;
            _r_UserInfo_RoleInfoDal = r_UserInfo_RoleInfoDal;
            _roleInfoDal = roleInfoDal;
            _fileInfoDal = fileInfoDal;
            _categoryDal = categoryDal;
        }

        public bool EditPassword(string oldPwdMD5, string newPwdMD5, UserInfo userInfo, out string msg)
        {
            var entity = _userInfoDal.GetEntitiesDb().FirstOrDefault(u => u.ID == userInfo.ID && !u.IsDelete);
            if (entity == null)
            {
                msg = "未获取到当前用户信息";
                return false;
            }
            if (oldPwdMD5 != entity.PassWord)
            {
                msg = "旧密码输入不正确";
                return false;
            }

            entity.PassWord = newPwdMD5;
            bool isSuccess = _userInfoDal.Update(entity);
            if (isSuccess)
            {
                msg = "修改成功";
                return true;
            }
            else
            {
                msg = "修改失败";
                return false;
            }
        }

        /// <summary>
        /// 获取教练下拉选
        /// </summary>
        /// <returns></returns>
        public object GetThecoachInfoSelectOption(string roleInfoId)
        {
            var r_UserInfo_RoleInfos = _r_UserInfo_RoleInfoDal.GetEntitiesDb().Where(r => r.RoleID == roleInfoId);
            if (r_UserInfo_RoleInfos == null)
            {
                return null;
            }
            List<string> userInfoIds = new List<string>();

            foreach (var item in r_UserInfo_RoleInfos)
            {
                userInfoIds.Add(item.UserID);
            }


            return _userInfoDal.GetEntitiesDb().Where(u => userInfoIds.Contains(u.ID)).Select(u => new
            {
                u.ID,
                u.UserName
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfoId"></param>
        /// <returns></returns>
        public string GetUserImg(string userInfoId)
        {
            var entity = _fileInfoDal.GetEntitiesDb().Where(e => e.Creator == userInfoId).OrderByDescending(e => e.CreateTime).FirstOrDefault();
            if (entity == null)
            {
                return "";
            }
            else
            {
                string path = "/img/" + entity.NewFileName;
                return path;
            }
        }

        ///// <summary>
        ///// 添加用户
        ///// </summary>
        ///// <param name="userInfo"></param>
        ///// <returns></returns>
        //public bool AddUserInfo(UserInfo userInfo)
        //{
        //    //UserInfoDal userInfoDal = new UserInfoDal();
        //    return _userInfoDal.Add(userInfo);
        //}

        ///// <summary>
        ///// 删除用户
        ///// </summary>
        ///// <param name="userInfoId"></param>
        ///// <returns></returns>
        //public bool DeleteUserInfo(string userInfoId)
        //{
        //    return _userInfoDal.Delete(userInfoId);
        //}



        ///// <summary>
        ///// 根据id获取用户
        ///// </summary>
        ///// <param name="userInfoId"></param>
        ///// <returns></returns>
        //public UserInfo GetUserInfo(string userInfoId)
        //{
        //    return _userInfoDal.GetEntity(userInfoId);
        //}

        /// <summary>
        /// 通过账号密码获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByLogin(string account, string password, string roleId)
        {
            var _UserInfo_RoleInfo = _r_UserInfo_RoleInfoDal.GetEntitiesDb().Where(r => r.RoleID == roleId).ToList();

            if (_UserInfo_RoleInfo == null)
            {
                return null;
            }
            UserInfo userInfo = new UserInfo();
            foreach (var item in _UserInfo_RoleInfo)
            {
                userInfo = _userInfoDal.GetEntitiesDb().Where(u => u.Account == account && u.PassWord == password && !u.IsDelete && u.ID == item.UserID).FirstOrDefault();
                if (userInfo != null)
                {
                    return userInfo;
                }
            }
            return userInfo;
        }

        /// <summary>
        /// 获取用户集合
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> GetUserInfoList()
        {
            return _userInfoDal.GetEntitiesDb().Where(u => u.IsDelete == false).ToList();
        }

        /// <summary>
        /// 获取用户集合（分页）
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public object GetUserInfoListByPage(int page, int limit, out int count, string userName, string phoneNum, string roleId, string roleName)
        {
            //获取数据库中用户全部没删除的数据（未真实查询）
            var userInfos = _userInfoDal.GetEntitiesDb().Where(u => u.IsDelete == false);

            if (!string.IsNullOrEmpty(userName))
            {
                userInfos = userInfos.Where(u => u.UserName.Contains(userName));
            }
            if (!string.IsNullOrEmpty(phoneNum))
            {
                userInfos = userInfos.Where(u => u.PhoneNum.Contains(phoneNum));
            }

            //查询出来数据的数量
            count = userInfos.Count();

            var r_UserInfo_RoleInfos = _r_UserInfo_RoleInfoDal.GetEntitiesDb().Where(r => r.RoleID == roleId);
            if (roleName == "教练")
            {
                var temp = (from u in userInfos
                            join ur in r_UserInfo_RoleInfos
                            on u.ID equals ur.UserID

                            join c in _categoryDal.GetEntitiesDb().AsQueryable()
                            on u.NumberId equals c.ID into cuTemp
                            from cu in cuTemp.DefaultIfEmpty()

                            select new
                            {
                                u.UserName,
                                u.ID,
                                u.Sex,
                                u.Account,
                                u.PhoneNum,
                                u.Email,
                                u.CreateTime,
                                cu.CategoryType
                            });

                //分页
                temp = temp.OrderBy(u => u.CreateTime).Skip((page - 1) * limit).Take(limit);

                var list = temp.ToList().Select(u =>
                {
                    string sexStr;
                    if (u.Sex == 1)
                    {
                        sexStr = "男";
                    }
                    else
                    {
                        sexStr = "女";
                    }

                    return new
                    {
                        u.UserName,
                        u.CategoryType,
                        u.ID,
                        u.Account,
                        u.PhoneNum,
                        u.Email,
                        CreateTime = u.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        Sex = sexStr
                    };
                });

                return list;
            }
            else
            {
                var temp = (from u in userInfos
                            join ur in r_UserInfo_RoleInfos
                            on u.ID equals ur.UserID

                            select new
                            {
                                u.UserName,
                                u.ID,
                                u.Sex,
                                u.Account,
                                u.PhoneNum,
                                u.Email,
                                u.CreateTime
                            });

                //分页
                temp = temp.OrderBy(u => u.CreateTime).Skip((page - 1) * limit).Take(limit);

                var list = temp.ToList().Select(u =>
                {
                    string sexStr;
                    if (u.Sex == 1)
                    {
                        sexStr = "男";
                    }
                    else
                    {
                        sexStr = "女";
                    }

                    return new
                    {
                        u.UserName,
                        u.ID,
                        u.Account,
                        u.PhoneNum,
                        u.Email,
                        CreateTime = u.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        Sex = sexStr
                    };
                });

                return list;
            }
        }

        /// <summary>
        /// 获取用户下拉选数据集
        /// </summary>
        /// <returns></returns>
        public object GetUserInfoSelectOption()
        {
            return _userInfoDal.GetEntitiesDb().Where(u => !u.IsDelete).Select(u => new
            {
                u.ID,
                u.UserName
            }).ToList();
        }

        /// <summary>
        /// 通过账号判断是否存在相同的用户
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool IsHadUserInfo(string account)
        {
            int count = _userInfoDal.GetEntitiesDb().Where(u => u.Account == account && u.IsDelete == false).Count();
            return count > 0;
        }

        public bool UpdateThecoach(string iD, string name,int sex, string numberId, string phoneNum)
        {
            UserInfo userInfo = _userInfoDal.GetEntity(iD);
            if (userInfo != null)
            {
                userInfo.UserName = name;
                userInfo.NumberId = numberId;
                userInfo.Sex = sex;
                userInfo.PhoneNum = phoneNum;
                return _userInfoDal.Update(userInfo);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新用户的用户名、姓名
        /// </summary>
        /// <param name="userInfoId"></param>
        /// <param name="userName"></param>
        /// <param name="sex"></param>
        /// <returns></returns>
        public bool UpdateUserInfo(string userInfoId, string userName, int sex, string phoneNum, string numberId)
        {
            UserInfo userInfo = _userInfoDal.GetEntity(userInfoId);
            if (userInfo != null)
            {
                userInfo.UserName = userName;
                userInfo.Sex = sex;
                userInfo.PhoneNum = phoneNum;
                userInfo.NumberId = numberId;
                return _userInfoDal.Update(userInfo);
            }
            else
            {
                return false;
            }
        }

        public bool Upload(Stream stream, string extensionName, long length, string id, string rawFileName)
        {
            var root = Directory.GetCurrentDirectory();

            DateTime dateTime = new DateTime(1970, 1, 1, 8, 0, 0);

            long timeStamp = Convert.ToInt32(DateTime.Now.Subtract(dateTime).TotalSeconds);

            string fileName = timeStamp + "." + extensionName;

            string filePath = Path.Combine(root, "wwwroot", "img", fileName);

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fs);
            }
            return _fileInfoDal.Add(new FitnessEntity.FileInfo()
            {
                ID = Guid.NewGuid().ToString(),
                Category = FileCategoryEnum.个人头像,
                CreateTime = DateTime.Now,
                Creator = id,
                Extension = extensionName,
                Length = length,
                NewFileName = fileName,
                RawFileName = rawFileName,
                RelationId = id
            });
        }
    }
}
