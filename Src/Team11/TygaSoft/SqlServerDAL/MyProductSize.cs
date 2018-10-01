using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DBUtility;

namespace TygaSoft.SqlServerDAL
{
    public partial class ProductSize
    {
        #region IProductSize Member

        public Dictionary<string, string> GetKeyValue(object productId, object productItemId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@" select SizeAppend from ProductSize where ProductId = @ProductId and ProductItemId = @ProductItemId ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@ProductItemId", SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = productId;
            parms[1].Value = productItemId;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms))
            {
                if (reader != null && reader.HasRows)
                {
                    if (reader.Read())
                    {
                        XElement xel = XElement.Parse(reader.GetString(0));
                        var q = from x in xel.Descendants("Data")
                                select new { name = x.Element("Name").Value };
                        foreach (var item in q)
                        {
                            dic.Add(item.name, item.name);
                        }
                    }
                }
            }

            return dic;
        }

        public DataSet GetDsByProduct(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ps.ProductItemId,ps.ProductId,ps.SizeAppend,
                        pi.Named ProductItemName
                        from ProductSize ps
                        left join ProductItem pi on pi.Id = ps.ProductItemId
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

            string cmdText = @"select 1 from ProductSize where ProductId = @ProductId and ProductItemId = @ProductItemId ";

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
