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
    public partial class UserHeadPicture
    {
        #region IUserHeadPicture Member

        public bool IsExist(string fileName, int fileSize)
        {
            SqlParameter[] parms = {
                                       new SqlParameter("@FileName",SqlDbType.NVarChar, 100),
                                       new SqlParameter("@FileSize",SqlDbType.Int),
                                   };
            parms[0].Value = fileName;
            parms[1].Value = fileSize;

            string cmdText = "select 1 from [UserHeadPicture] where lower(FileName) = @FileName and FileSize = @FileSize ";

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcSystemDbConnString, CommandType.Text, cmdText, parms);
            if (obj != null) return true;

            return false;
        }

        public UserHeadPictureInfo GetModel(string fileName, int fileSize)
        {
            UserHeadPictureInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,FileName,FileSize,FileExtension,FileDirectory,RandomFolder,LastUpdatedDate 
			                   from UserHeadPicture
							   where FileName = @FileName and FileSize = @FileSize ");
            SqlParameter[] parms = {
                                     new SqlParameter("@FileName",SqlDbType.NVarChar,100),
                                     new SqlParameter("@FileSize",SqlDbType.Int)
                                 };
            parms[0].Value = fileName;
            parms[1].Value = fileSize;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new UserHeadPictureInfo();
                        model.Id = reader.GetGuid(0);
                        model.FileName = reader.GetString(1);
                        model.FileSize = reader.GetInt32(2);
                        model.FileExtension = reader.GetString(3);
                        model.FileDirectory = reader.GetString(4);
                        model.RandomFolder = reader.GetString(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);
                    }
                }
            }

            return model;
        }

        #endregion
    }
}
