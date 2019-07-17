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
    public partial class CategoryBrand
    {
        public int Delete(object brandId, object categoryId)
        {
            string cmdText = "delete from CategoryBrand where BrandId = @BrandId and CategoryId @CategoryId ";

            SqlParameter[] parms = {
                                       new SqlParameter("@BrandId", SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@CategoryId", SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = brandId;
            parms[1].Value = categoryId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
        }
    }
}
