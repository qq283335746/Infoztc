using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DBUtility;

namespace TygaSoft.SqlServerDAL
{
    public partial class PictureAdStartup
    {
        #region IPictureAdStartup Member

        public bool IsExist(string fileName, int fileSize)
        {
            SqlParameter[] parms = {
                                       new SqlParameter("@FileName",SqlDbType.NVarChar, 100),
                                       new SqlParameter("@FileSize",SqlDbType.Int)
                                   };
            parms[0].Value = fileName;
            parms[1].Value = fileSize;

            string cmdText = "select 1 from [Picture_AdStartup] where lower(FileName) = @FileName and FileSize = @FileSize ";

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, cmdText, parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
