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
    public partial class ServiceUserVole
    {
        #region IServiceUserVole Member

        public bool IsExist(object userId, object serviceItemId)
        {
            SqlParameter[] parms = {
                                       new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@ServiceItemId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = Guid.Parse(userId.ToString());
            parms[1].Value = Guid.Parse(serviceItemId.ToString());

            string cmdText = "select 1 from [Service_UserVole] where UserId = @UserId and ServiceItemId = @ServiceItemId ";

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
