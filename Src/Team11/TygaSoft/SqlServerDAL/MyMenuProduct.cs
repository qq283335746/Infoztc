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
    public partial class MenuProduct
    {
        #region IMenuProduct Member

        public int Delete(object productId, object menuId)
        {
            string cmdText = "delete from MenuProduct where ProductId = @ProductId and MenuId = @MenuId ";

            SqlParameter[] parms = {
                                       new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@MenuId", SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = productId;
            parms[1].Value = menuId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
        }

        #endregion
    }
}
