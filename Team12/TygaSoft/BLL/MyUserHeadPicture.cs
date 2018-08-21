using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;

namespace TygaSoft.BLL
{
    public partial class UserHeadPicture
    {
        #region UserHeadPicture Member

        public bool IsExist(string fileName, int fileSize)
        {
            return dal.IsExist(fileName, fileSize);
        }

        public UserHeadPictureInfo GetModel(string fileName, int fileSize)
        {
            return dal.GetModel(fileName, fileSize);
        }

        #endregion
    }
}
