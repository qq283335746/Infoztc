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
    public partial class ProductSize : IProductSize
    {
        #region IProductSize Member

        public int Insert(ProductSizeInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ProductSize (ProductId,ProductItemId,SizeAppend)
			            values
						(@ProductId,@ProductItemId,@SizeAppend)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@ProductItemId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@SizeAppend",SqlDbType.VarChar,300)
                                   };
            parms[0].Value = model.ProductId;
            parms[1].Value = model.ProductItemId;
            parms[2].Value = model.SizeAppend;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ProductSizeInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ProductSize set SizeAppend = @SizeAppend 
			            where ProductItemId = @ProductItemId
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@ProductItemId",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@SizeAppend",SqlDbType.VarChar,300)
                                   };
            parms[0].Value = model.ProductItemId;
            parms[1].Value = model.SizeAppend;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object ProductItemId)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ProductSize where ProductItemId = @ProductItemId");
            SqlParameter parm = new SqlParameter("@ProductItemId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ProductItemId.ToString());

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
                sb.Append(@"delete from ProductSize where ProductItemId = @ProductItemId" + n + " ;");
                SqlParameter parm = new SqlParameter("@ProductItemId" + n + "", SqlDbType.UniqueIdentifier);
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

        public ProductSizeInfo GetModel(object ProductItemId)
        {
            ProductSizeInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 ProductItemId,ProductId,SizeAppend 
			            from ProductSize
						where ProductItemId = @ProductItemId ");
            SqlParameter parm = new SqlParameter("@ProductItemId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ProductItemId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ProductSizeInfo();
                        model.ProductItemId = reader.GetGuid(0);
                        model.ProductId = reader.GetGuid(1);
                        model.SizeAppend = reader.GetString(2);
                    }
                }
            }

            return model;
        }

        public IList<ProductSizeInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ProductSize ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ProductSizeInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          ProductItemId,ProductId,SizeAppend
					  from ProductSize ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProductSizeInfo> list = new List<ProductSizeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductSizeInfo model = new ProductSizeInfo();
                        model.ProductItemId = reader.GetGuid(1);
                        model.ProductId = reader.GetGuid(2);
                        model.SizeAppend = reader.GetString(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductSizeInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           ProductItemId,ProductId,SizeAppend
					   from ProductSize ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProductSizeInfo> list = new List<ProductSizeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductSizeInfo model = new ProductSizeInfo();
                        model.ProductItemId = reader.GetGuid(1);
                        model.ProductId = reader.GetGuid(2);
                        model.SizeAppend = reader.GetString(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductSizeInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ProductItemId,ProductId,SizeAppend
                        from ProductSize ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ProductSizeInfo> list = new List<ProductSizeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductSizeInfo model = new ProductSizeInfo();
                        model.ProductItemId = reader.GetGuid(0);
                        model.ProductId = reader.GetGuid(1);
                        model.SizeAppend = reader.GetString(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductSizeInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ProductItemId,ProductId,SizeAppend 
			            from ProductSize
					    order by LastUpdatedDate desc ");

            IList<ProductSizeInfo> list = new List<ProductSizeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductSizeInfo model = new ProductSizeInfo();
                        model.ProductItemId = reader.GetGuid(0);
                        model.ProductId = reader.GetGuid(1);
                        model.SizeAppend = reader.GetString(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
