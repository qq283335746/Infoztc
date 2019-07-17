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
    public partial class UserErnie
    {
        #region IUserErnie Member

        public int GetTotalBetCount(object userId,object ernieId)
        {
            string cmdText = "select count(1) from UserErnie where UserId = @UserId and ErnieId = @ErnieId ";
            SqlParameter[] parms = {
                                       new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@ErnieId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = Guid.Parse(userId.ToString());
            parms[1].Value = Guid.Parse(ernieId.ToString());

            return (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, cmdText, parms);
        }

        #endregion
    }
}
