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
    public partial class ActivityPlayerPhotoPicture
    {
        #region ActivityPlayerPhotoPicture Member

        public bool IsExist(string fileName, int fileSize)
        {
            return dal.IsExist(fileName, fileSize);
        }

        public int InsertByOutput(ActivityPlayerPhotoPictureInfo model)
        {
            return dal.InsertByOutput(model);
        }

        #endregion
    }
}
