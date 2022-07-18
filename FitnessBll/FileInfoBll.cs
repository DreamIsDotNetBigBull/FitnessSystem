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
  public  class FileInfoBll:BaseBll<FileInfo>,IFileInfoBll
    {
        private IFileInfoDal _fileInfoDal;
        public FileInfoBll(IFileInfoDal fileInfoDal) :base(fileInfoDal)
        {
            _fileInfoDal = fileInfoDal;
        }
    }

}
