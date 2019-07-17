using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IActivityPlayerPhotoPicture
    {
        #region IActivityPlayerPhotoPicture Member

        bool IsExist(string fileName, int fileSize);

        int InsertByOutput(ActivityPlayerPhotoPictureInfo model);

        #endregion
    }
}
