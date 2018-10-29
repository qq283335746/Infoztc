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
    public partial class ProductStock : IProductStock
    {
        #region IProductStock Member

        public int Insert(ProductStockInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ProductStock (ProductId,ProductItemId,ProductSize,StockNum)
			            values
						(@ProductId,@ProductItemId,@ProductSize,@StockNum)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@ProductItemId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@ProductSize",SqlDbType.VarChar,10),
                                        new SqlParameter("@StockNum",SqlDbType.Int)
                                   };
            parms[0].Value = model.ProductId;
            parms[1].Value = model.ProductItemId;
            parms[2].Value = model.ProductSize;
            parms[3].Value = model.StockNum;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ProductStockInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ProductStock set ProductId = @ProductId,ProductItemId = @ProductItemId,ProductSize = @ProductSize,StockNum = @StockNum 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@ProductItemId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@ProductSize",SqlDbType.VarChar,10),
                                        new SqlParameter("@StockNum",SqlDbType.Int)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.ProductId;
            parms[2].Value = model.ProductItemId;
            parms[3].Value = model.ProductSize;
            parms[4].Value = model.StockNum;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ProductStock where Id = @Id");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm);
        }

        public bool DeleteBatch(IList<object> list)
        {
            if (list == null || list.Count == 0) return false;

            bool result = false;
            StringBuilder sb = new StringBuilder(500);
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from ProductStock where Id = @Id" + n + " ;");
                SqlParameter parm = new SqlParameter("@Id" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(item);
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

        public ProductStockInfo GetModel(object Id)
        {
            ProductStockInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,ProductId,ProductItemId,ProductSize,StockNum 
			            from ProductStock
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ProductStockInfo();
                        model.Id = reader.GetGuid(0);
                        model.ProductId = reader.GetGuid(1);
                        model.ProductItemId = reader.GetGuid(2);
                        model.ProductSize = reader.GetString(3);
                        model.StockNum = reader.GetInt32(4);
                    }
                }
            }

            return model;
        }

        public IList<ProductStockInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ProductStock ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ProductStockInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,ProductId,ProductItemId,ProductSize,StockNum
					  from ProductStock ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProductStockInfo> list = new List<ProductStockInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductStockInfo model = new ProductStockInfo();
                        model.Id = reader.GetGuid(1);
                        model.ProductId = reader.GetGuid(2);
                        model.ProductItemId = reader.GetGuid(3);
                        model.ProductSize = reader.GetString(4);
                        model.StockNum = reader.GetInt32(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductStockInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,ProductId,ProductItemId,ProductSize,StockNum
					   from ProductStock ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProductStockInfo> list = new List<ProductStockInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductStockInfo model = new ProductStockInfo();
                        model.Id = reader.GetGuid(1);
                        model.ProductId = reader.GetGuid(2);
                        model.ProductItemId = reader.GetGuid(3);
                        model.ProductSize = reader.GetString(4);
                        model.StockNum = reader.GetInt32(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductStockInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ProductId,ProductItemId,ProductSize,StockNum
                        from ProductStock ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ProductStockInfo> list = new List<ProductStockInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductStockInfo model = new ProductStockInfo();
                        model.Id = reader.GetGuid(0);
                        model.ProductId = reader.GetGuid(1);
                        model.ProductItemId = reader.GetGuid(2);
                        model.ProductSize = reader.GetString(3);
                        model.StockNum = reader.GetInt32(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductStockInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ProductId,ProductItemId,ProductSize,StockNum 
			            from ProductStock
					    order by LastUpdatedDate desc ");

            IList<ProductStockInfo> list = new List<ProductStockInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductStockInfo model = new ProductStockInfo();
                        model.Id = reader.GetGuid(0);
                        model.ProductId = reader.GetGuid(1);
                        model.ProductItemId = reader.GetGuid(2);
                        model.ProductSize = reader.GetString(3);
                        model.StockNum = reader.GetInt32(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
