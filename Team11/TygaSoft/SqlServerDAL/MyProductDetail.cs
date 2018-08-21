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
    public partial class ProductDetail
    {
        #region IProductDetail Member

        public ProductDetailInfo GetModel(object productId, object productItemId)
        {
            ProductDetailInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 pd.ProductId,pd.ProductItemId,pd.OriginalPrice,pd.ProductPrice,pd.Discount,pd.DiscountDescr,pd.ContentText,pd.PayOption,pd.ViewCount
			            from ProductDetail pd
						where ProductId = @ProductId and ProductItemId = @ProductItemId ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@ProductItemId", SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = Guid.Parse(productId.ToString());
            parms[1].Value = Guid.Parse(productItemId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ProductDetailInfo();
                        model.ProductId = reader.GetGuid(0);
                        model.ProductItemId = reader.GetGuid(1);
                        model.OriginalPrice = reader.GetDecimal(2);
                        model.ProductPrice = reader.GetDecimal(3);
                        model.Discount = reader.GetDouble(4);
                        model.DiscountDescr = reader.GetString(5);
                        model.ContentText = reader.GetString(6);
                        model.PayOption = reader.GetString(7);
                        model.ViewCount = reader.GetInt32(8);
                    }
                }
            }

            return model;
        }

        public DataSet GetDsByProduct(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select pd.ProductId,pd.ProductItemId,pd.OriginalPrice,pd.ProductPrice,pd.Discount,pd.DiscountDescr,pd.ContentText,pd.PayOption,pd.ViewCount,
                        pi.Named ProductItemName
                        from ProductDetail pd
                        left join ProductItem pi on pi.Id = pd.ProductItemId
                        ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            return SqlHelper.ExecuteDataset(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);
        }

        public bool DeleteBatchByProduct(IList<object> list)
        {
            if (list == null || list.Count == 0) return false;

            bool result = false;
            StringBuilder sb = new StringBuilder(500);
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                var items = item.Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries);
                if (items.Length != 2) break;

                sb.Append(@"delete from ProductDetail where ProductId = @ProductId" + n + " and ProductItemId = @ProductItemId" + n + " ;");
                SqlParameter parm = new SqlParameter("@ProductId" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(items[0]);
                parms.Add(parm);
                parm = new SqlParameter("@ProductItemId" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(items[1]);
                parms.Add(parm);
            }
            using (SqlConnection conn = new SqlConnection(SqlHelper.HnztcShopDbConnString))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        int effect = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, sb.ToString(), parms != null ? parms.ToArray() : null);
                        tran.Commit();
                        if (effect > 0) result = true;
                    }
                    catch
                    {
                        tran.Rollback();
                    }
                }
            }
            return result;
        }

        public bool IsExist(object productId, object productItemId)
        {
            SqlParameter[] parms = {
                                       new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@ProductItemId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = Guid.Parse(productId.ToString());
            parms[1].Value = Guid.Parse(productItemId.ToString());

            StringBuilder sb = new StringBuilder(100);
            sb.Append(@" select 1 from [ProductDetail] where ProductId = @ProductId and ProductItemId = @ProductItemId ");

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
