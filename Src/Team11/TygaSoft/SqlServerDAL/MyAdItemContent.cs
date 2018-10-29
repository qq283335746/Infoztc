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
    public partial class AdItemContent
    {
        #region IAdItemContent Member

        public bool IsExist(object adItemId)
        {
            SqlParameter parm = new SqlParameter("@AdItemId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(adItemId.ToString());

            StringBuilder sb = new StringBuilder(100);
            sb.Append(@" select 1 from [AdItemContent] where AdItemId = @AdItemId ");

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
