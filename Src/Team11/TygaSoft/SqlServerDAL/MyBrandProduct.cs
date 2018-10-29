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
    public partial class BrandProduct
    {
        public int Delete(object productId, object brandId)
        {
            string cmdText = "delete from BrandProduct where ProductId = @ProductId and BrandId = @BrandId ";

            SqlParameter[] parms = {
                                       new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@BrandId", SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = productId;
            parms[1].Value = brandId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
        }
    }
}
