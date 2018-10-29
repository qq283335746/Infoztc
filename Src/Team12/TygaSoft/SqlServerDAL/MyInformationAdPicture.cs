using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TygaSoft.DBUtility;
using TygaSoft.Model;

namespace TygaSoft.SqlServerDAL
{
    public partial class InformationAdPicture
    {
        #region InformationAdPicture Member

        public int InsertModel(InformationAdPictureInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into InformationAdPicture (InformationAdId, PictureId)
			            values
						(@InformationAdId,@PictureId)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@InformationAdId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.InformationAdId;
            parms[1].Value = model.PictureId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }


        public DataSet GetListOW(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select InformationAdId,PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
			            from InformationAdPicture IP, Picture_Information PI where IP.PictureId=PI.Id and IP.InformationAdId=@InformationAdId
					    ");

            SqlParameter parm = new SqlParameter("@InformationAdId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);

            return ds;
        }

        #endregion
    }
}
