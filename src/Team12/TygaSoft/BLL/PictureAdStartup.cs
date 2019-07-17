using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;

namespace TygaSoft.BLL
{
    public partial class PictureAdStartup
    {
        #region PictureAdStartup Member

        public bool IsExist(string fileName, int fileSize)
        {
            return dal.IsExist(fileName, fileSize);
        }

        #endregion
    }
}
