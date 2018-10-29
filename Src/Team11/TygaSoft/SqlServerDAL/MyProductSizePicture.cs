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
    public partial class ProductSizePicture
    {
        #region IProductSizePicture Member

        public ProductSizePictureInfo GetModelByJoin(object productId, object productItemId)
        {
            ProductSizePictureInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 psp.ProductId,psp.ProductItemId,psp.Named,psp.PictureId,
                        pps.FileExtension,pps.FileDirectory,pps.RandomFolder
			            from ProductSizePicture psp
                        left join Picture_ProductSize pps on pps.Id = psp.PictureId
						where psp.ProductId = @ProductId and psp.ProductItemId = @ProductItemId ");

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
                        model = new ProductSizePictureInfo();
                        model.ProductId = reader.GetGuid(0);
                        model.ProductItemId = reader.GetGuid(1);
                        model.Named = reader.GetString(2);
                        model.PictureId = reader.GetGuid(3);
                        model.FileExtension = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        model.FileDirectory = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        model.RandomFolder = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    }
                }
            }

            return model;
        }

        public DataSet GetDsByProduct(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select psp.ProductId,psp.ProductItemId,psp.Named,psp.PictureId,
                        pi.Named ProductItemName,pps.FileExtension,pps.FileDirectory,pps.RandomFolder
                        from ProductSizePicture psp
                        left join ProductItem pi on pi.Id = psp.ProductItemId
                        left join Picture_ProductSize pps on pps.Id = psp.PictureId
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
                if (items.Length != 2) break;

                sb.Append(@"delete from ProductSizePicture where ProductId = @ProductId" + n + " and ProductItemId = @ProductItemId" + n + " ;");
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
            sb.Append(@" select 1 from [ProductSizePicture] where ProductId = @ProductId and ProductItemId = @ProductItemId ");

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
