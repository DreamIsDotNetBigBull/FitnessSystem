using FitnessEntity;
using IFitnessBll;
using IFitnessDal;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessBll
{
    public  class GoodsRecordBll:BaseBll<GoodsRecord>,IGoodsRecordBll
    {
        private IGoodsRecordDal _goodsRecordDal;
        private FitnessDbContext _dbContext;
        private IGoodsInfoDal _goodsInfoDal;
        private IUserInfoDal _userInfoDal;

        public GoodsRecordBll(IGoodsRecordDal goodsRecordDal, FitnessDbContext dbContext, IGoodsInfoDal goodsInfoDal, IUserInfoDal userInfoDal) :base(goodsRecordDal)
        {
            _goodsRecordDal = goodsRecordDal;
            _dbContext = dbContext;
            _goodsInfoDal = goodsInfoDal;
            _userInfoDal = userInfoDal;
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddGoodsRecord(GoodsRecord entity)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                //获取商品
                var goodsInfo = _goodsInfoDal.GetEntity(entity.GoodsId);

                if (goodsInfo != null && entity != null)
                {
                    //添加商品记录
                    bool isSuccess1 = _goodsRecordDal.Add(entity);
                    //修改商品库存数量
                    goodsInfo.Num += entity.Num;
                    bool isSuccess2 = _goodsInfoDal.Update(goodsInfo);
                    if (isSuccess1 && isSuccess2)
                    {
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }


        /// <summary>
        /// 下载
        /// </summary>
        /// <returns></returns>
        public Stream GetDownloadDatas()
        {
            var datas = (from gr in _goodsRecordDal.GetEntitiesDb()
                         join g in _goodsInfoDal.GetEntitiesDb().Where(c => !c.IsDelete)
                         on gr.GoodsId equals g.ID
                         select new
                         {
                             gr.CreateTime,
                             gr.Num,
                             gr.Type,
                             g.GoodsName
                         }).ToList();
            var count = datas.Count();

            //获取当前的目录
            var dPath = Directory.GetCurrentDirectory();
            //文件名
            string fileName = "output.xlsx";
            string filePath = Path.Combine(dPath, fileName);
            //把数据转换成文件流
            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            IWorkbook workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet("Sheet1");
            //IRow row = excelSheet.CreateRow(0);

            //row.CreateCell(0).SetCellValue("111");

            for (int i = 0; i < datas.Count; i++)
            {
                IRow row = excelSheet.CreateRow(i);

                row.CreateCell(0).SetCellValue(datas[i].GoodsName);
                row.CreateCell(1).SetCellValue(datas[i].Num);
                string typeStr = datas[i].Type == 1 ? "入库" : "出库";
                row.CreateCell(2).SetCellValue(typeStr);
                string dateTimeStr = datas[i].CreateTime.ToString("yyyy-MM-dd,HH-mm-ss");
                row.CreateCell(3).SetCellValue(dateTimeStr);
            }



            workbook.Write(fileStream);

            FileStream fileStream2 = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            return fileStream2;
        }

        public object GetGoodsRecordListByPage(int page, int limit, out int count, string goodsId)
        {
            //获取数据库中商品记录全部没删除的数据（未真实查询）
            var goodsRecords = _goodsRecordDal.GetEntitiesDb().AsQueryable();

            if (!string.IsNullOrEmpty(goodsId))
            {
                goodsRecords = goodsRecords.Where(c => c.GoodsId.Contains(goodsId));
            }

            //查询出来数据的数量
            count = goodsRecords.Count();

            //做连表查询
            var tempList = (from gr in goodsRecords
                            join g in _goodsInfoDal.GetEntitiesDb().Where(c => !c.IsDelete)
                            on gr.GoodsId equals g.ID into tempCRC
                            from gg in tempCRC.DefaultIfEmpty()

                            join u in _userInfoDal.GetEntitiesDb().Where(u => !u.IsDelete)
                            on gr.Creator equals u.ID into tempCRU
                            from uu in tempCRU.DefaultIfEmpty()
                            select new
                            {
                                gr.Num,
                                gr.ID,
                                gr.Type,
                                gr.CreateTime,
                                uu.UserName,
                                gg.GoodsName
                            });



            //分页
            tempList = tempList.OrderBy(u => u.CreateTime).Skip((page - 1) * limit).Take(limit);

            var list = tempList.ToList().Select(u =>
            {
                return new
                {
                    u.UserName,
                    u.GoodsName,
                    u.Num,
                    Type = u.Type == 1 ? "入库" : "出库",
                    u.ID,
                    CreateTime = u.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")
                };
            });

            return list;
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="iD"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool Upload(Stream stream, string userInfoId, out string msg)
        {
            msg = "";
            stream.Position = 0;
            XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);
            ISheet sheet = xssWorkbook.GetSheetAt(0);
            if (sheet != null)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    //获取全部数量
                    var LastRow = sheet.LastRowNum;

                    //批量添加集合
                    List<GoodsRecord> goodsRecords = new List<GoodsRecord>();
                    //批量更新集合
                    List<GoodsInfo> goodsInfos = new List<GoodsInfo>();

                    for (int index = 0; index <= LastRow; index++)
                    {
                        //获取行
                        IRow row = sheet.GetRow(index);
                        if (row != null)
                        {
                            //一行最后一个cell的编号 即总的列数
                            //int cellCount = firstRow.LastCellNum;

                            var name = row.Cells[0].ToString();
                            var value = row.Cells[1].ToString();

                            //类型装换 
                            //var num = int.Parse(value);
                            //num = Convert.ToInt32(content);
                            int num = 0;
                            int.TryParse(value, out num);

                            //根据名称查询耗材id
                            var goodsInfo = _goodsInfoDal.GetEntitiesDb().FirstOrDefault(c => c.GoodsName == name);
                            if (goodsInfo != null)
                            {
                                //创建耗材记录实体
                                GoodsRecord goodsRecord = new GoodsRecord()
                                {
                                    ID = Guid.NewGuid().ToString(),
                                    CreateTime = DateTime.Now,
                                    Type = 1,
                                    Creator = userInfoId,
                                    Num = num,
                                    GoodsId = goodsInfo.ID
                                };
                                //把耗材记录实体添加到数据库中
                                //bool isSuccess = _consumableRecordDal.Add(consumableRecord);

                                //把添加耗材实体记录放到集合里
                                goodsRecords.Add(goodsRecord);

                                //修改耗材表的库存数量
                                goodsInfo.Num += num;
                                //bool isSuccess1 = _consumableInfoDal.Update(consumableInfo);

                                //把修改耗材实体记录放到集合里
                                goodsInfos.Add(goodsInfo);

                            }
                            else
                            {
                                msg = string.Format("{0}商品名称有误在第{1}行", name, index + 1);
                                transaction.Rollback();
                                return false;
                            }
                        }
                    }
                    //批量添加
                    _goodsRecordDal.Adds(goodsRecords);
                    //批量更新
                    _goodsInfoDal.Updates(goodsInfos);
                    msg = "成功";
                    transaction.Commit();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
