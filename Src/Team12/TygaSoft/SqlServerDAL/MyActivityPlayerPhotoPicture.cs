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
    public partial class ActivityPlayerPhotoPicture
    {
        #region IActivityPlayerPhotoPicture Member

        public bool IsExist(string fileName, int fileSize)
        {
            SqlParameter[] parms = {
                                       new SqlParameter("@FileName",SqlDbType.NVarChar, 100),
                                       new SqlParameter("@FileSize",SqlDbType.Int)
                                   };
            parms[0].Value = fileName;
            parms[1].Value = fileSize;

            string cmdText = "select 1 from [ActivityPlayerPhotoPicture] where lower(FileName) = @FileName and FileSize = @FileSize ";

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, cmdText, parms);
            if (obj != null) return true;

            return false;
        }

        public int InsertByOutput(ActivityPlayerPhotoPictureInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ActivityPlayerPhotoPicture (Id, FileName,FileSize,FileExtension,FileDirectory,RandomFolder,LastUpdatedDate)
			            values
						(@Id, @FileName,@FileSize,@FileExtension,@FileDirectory,@RandomFolder,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@FileName",SqlDbType.NVarChar,100),
                                        new SqlParameter("@FileSize",SqlDbType.Int),
                                        new SqlParameter("@FileExtension",SqlDbType.VarChar,10),
                                        new SqlParameter("@FileDirectory",SqlDbType.NVarChar,100),
                                        new SqlParameter("@RandomFolder",SqlDbType.VarChar,20),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.FileName;
            parms[2].Value = model.FileSize;
            parms[3].Value = model.FileExtension;
            parms[4].Value = model.FileDirectory;
            parms[5].Value = model.RandomFolder;
            parms[6].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        #endregion
    }
}
