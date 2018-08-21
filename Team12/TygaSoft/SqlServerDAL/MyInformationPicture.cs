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
    public partial class InformationPicture
    {
        #region IPictureInformation Member

        public int InsertModel(InformationPictureInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into InformationPicture (InformationId, PictureId)
			            values
						(@InformationId,@PictureId)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@InformationId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.InformationId;
            parms[1].Value = model.PictureId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }


        public DataSet GetListOW(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select InformationId,PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
			            from InformationPicture IP, Picture_Information PI where IP.PictureId=PI.Id and IP.InformationId=@InformationId
					    ");

            SqlParameter parm = new SqlParameter("@InformationId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);

            return ds;
        }

        #endregion
    }
}
