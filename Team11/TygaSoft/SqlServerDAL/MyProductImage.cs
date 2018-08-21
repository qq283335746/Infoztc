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
    public partial class ProductImage
    {
        #region IProductImage Member

        public DataSet GetDsByProduct(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select pim.ProductId,pim.ProductItemId,pim.PictureAppend,
                        pi.Named ProductItemName
                        from ProductImage pim
                        left join ProductItem pi on pi.Id = pim.ProductItemId
                        ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            return SqlHelper.ExecuteDataset(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);
        }

        public bool IsExist(object productId, object productItemId)
        {
            SqlParameter[] parms = {
                                       new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@ProductItemId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = Guid.Parse(productId.ToString());
            parms[1].Value = Guid.Parse(productItemId.ToString());

            string cmdText = @"select 1 from ProductImage where ProductId = @ProductId and ProductItemId = @ProductItemId ";

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
