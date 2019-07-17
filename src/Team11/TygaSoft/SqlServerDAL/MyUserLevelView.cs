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
    public partial class UserLevelView : IUserLevelView
    {
        #region IUserLevelView Member

        public bool IsExist(object userId, int funCode, int enumSource)
        {
            var cmdText = @"select 1 from [UserLevelView] where UserId = @UserId and FunCode = @FunCode and EnumSource = @EnumSource ";
            SqlParameter[] parms = {
                                       new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@FunCode",SqlDbType.Int),
                                        new SqlParameter("@EnumSource",SqlDbType.Int)
                                   };
            parms[0].Value = Guid.Parse(userId.ToString());
            parms[1].Value = funCode;
            parms[2].Value = enumSource;

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
            if (obj != null) return true;

            return false;
        }

        public UserLevelViewInfo GetModel(object userId, int funCode, int enumSource)
        {
            UserLevelViewInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 UserId,FunCode,EnumSource,TotalGold,TotalSilver,TotalIntegral,LastUpdatedDate 
			            from UserLevelView
						where UserId = @UserId and FunCode = @FunCode and EnumSource = @EnumSource ");

            SqlParameter[] parms = {
                                       new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@FunCode",SqlDbType.Int),
                                        new SqlParameter("@EnumSource",SqlDbType.Int)
                                   };
            parms[0].Value = Guid.Parse(userId.ToString());
            parms[1].Value = funCode;
            parms[2].Value = enumSource;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new UserLevelViewInfo();
                        model.UserId = reader.GetGuid(0);
                        model.FunCode = reader.GetInt32(1);
                        model.EnumSource = reader.GetInt32(2);
                        model.TotalGold = reader.GetInt32(3);
                        model.TotalSilver = reader.GetInt32(4);
                        model.TotalIntegral = reader.GetInt32(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);
                    }
                }
            }

            return model;
        }

        #endregion
    }
}
