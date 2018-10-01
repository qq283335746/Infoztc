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
    public partial class CategoryProduct
    {
        #region CategoryProduct Member

        public int Delete(object productId, object categoryId)
        {
            string cmdText = "delete from CategoryProduct where ProductId = @ProductId and CategoryId = @CategoryId ";

            SqlParameter[] parms = {
                                       new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@CategoryId", SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = productId;
            parms[1].Value = categoryId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
        }

        #endregion
    }
}
