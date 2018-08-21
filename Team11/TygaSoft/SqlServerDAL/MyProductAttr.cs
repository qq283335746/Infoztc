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
    public partial class ProductAttr
    {
        #region IProductAttr Member

        public DataSet GetDsByProduct(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select pa.ProductId,pa.ProductItemId,pa.AttrValue,
                        pi.Named ProductItemName
                        from ProductAttr pa
                        left join ProductItem pi on pi.Id = pa.ProductItemId
                        ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            return SqlHelper.ExecuteDataset(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);
        }

        #endregion
    }
}
