using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TygaSoft.DBUtility;
using TygaSoft.IDAL;
using TygaSoft.Model;

namespace TygaSoft.SqlServerDAL
{
    public partial class PushUser
    {
        #region PushMsg Member

        public int InsertOW(PushUserInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into PushUser (PushId, PushUser)
			            values
						(@PushId, @PushUser)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@PushId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@PushUser",SqlDbType.VarChar)
                                   };
            parms[0].Value = model.PushId;
            parms[1].Value = model.PushUser;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }
        
        #endregion
    }
}
