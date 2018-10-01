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
    public partial class ProductStock
    {
        #region IProductStock Member

        public DataSet GetDsByProduct(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select pst.Id,pst.ProductId,pst.ProductItemId,pst.ProductSize,pst.StockNum,
                        pi.Named ProductItemName
                        from ProductStock pst
                        left join ProductItem pi on pi.Id = pst.ProductItemId
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
                var items = item.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (items.Length != 3) break;

                sb.Append(@"delete from ProductStock where ProductId = @ProductId" + n + " and ProductItemId = @ProductItemId" + n + " and ProductSizeId = @ProductSizeId" + n + " ;");
                SqlParameter parm = new SqlParameter("@ProductId" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(items[0]);
                parms.Add(parm);
                parm = new SqlParameter("@ProductItemId" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(items[1]);
                parms.Add(parm);
                parm = new SqlParameter("@ProductSizeId" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(items[2]);
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

        public bool IsExist(object productId, object productItemId, string productSize, object Id)
        {
            Guid gId = Guid.Empty;
            if (Id != null)
            {
                Guid.TryParse(Id.ToString(), out gId);
            }

            SqlParameter[] parms = {
                                       new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@ProductItemId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@ProductSize",SqlDbType.VarChar,10)
                                   };
            parms[0].Value = Guid.Parse(productId.ToString());
            parms[1].Value = Guid.Parse(productItemId.ToString());
            parms[2].Value = productSize;

            StringBuilder sb = new StringBuilder(300);
            if (!gId.Equals(Guid.Empty))
            {
                sb.Append(@" select 1 from [ProductStock] where ProductId = @ProductId and ProductItemId = @ProductItemId and ProductSize = @ProductSize and Id <> @Id ");

                Array.Resize(ref parms, 4);
                parms[3] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                parms[3].Value = gId;
            }
            else
            {
                sb.Append(@" select 1 from [ProductStock] where ProductId = @ProductId and ProductItemId = @ProductItemId and ProductSize = @ProductSize ");
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
