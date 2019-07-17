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
    public partial class UserSignIn
    {
        #region IUserSignIn Member

        public UserSignInInfo GetModelByUser(object userId)
        {
            UserSignInInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,UserId,SignInXml,LastUpdatedDate 
			            from UserSignIn
						where UserId = @UserId 
                        order by LastUpdatedDate desc
                       ");
            SqlParameter parm = new SqlParameter("@UserId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(userId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new UserSignInInfo();
                        model.Id = reader.GetGuid(0);
                        model.UserId = reader.GetGuid(1);
                        model.SignInXml = reader.GetString(2);
                        model.LastUpdatedDate = reader.GetDateTime(3);
                    }
                }
            }

            return model;
        }

        #endregion
    }
}
