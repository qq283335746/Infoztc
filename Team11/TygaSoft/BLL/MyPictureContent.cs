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
    public partial class PictureContent
    {
        #region PictureContent Member

        public bool IsExist(object userId, string fileName, int fileSize)
        {
            return dal.IsExist(userId, fileName, fileSize);
        }

        #endregion
    }
}
