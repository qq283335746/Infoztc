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
    public partial class UserLevelProduce
    {
        #region IUserLevelProduce Member

        public bool IsExist(object userId,int funCode, int enumSource)
        {
            var cmdText = @"select 1 from [UserLevelProduce] where datediff(day,[LastUpdatedDate],getdate())=0 and UserId = @UserId and FunCode = @FunCode and EnumSource = @EnumSource ";
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

        #endregion
    }
}
