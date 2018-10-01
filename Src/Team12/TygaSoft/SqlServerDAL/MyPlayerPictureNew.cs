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
    public partial class PlayerPictureNew : IPlayerPictureNew
    {
        #region IPlayerPictureNew Member

        public DataSet GetListOW(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select PlayerId,PP.PictureId,Sort,IsHeadImg,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
			            from PlayerPictureNew PP, ActivityPlayerPhotoPicture AP where PP.PictureId=AP.Id and PlayerId=@PlayerId
					    order by Sort asc ");

            SqlParameter parm = new SqlParameter("@PlayerId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);

            return ds;
        }

        #endregion
    }
}
