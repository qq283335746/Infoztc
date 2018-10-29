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
    public partial class ProductSizePicture : IProductSizePicture
    {
        #region IProductSizePicture Member

        public int Insert(ProductSizePictureInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ProductSizePicture (ProductId,ProductItemId,Named,PictureId)
			            values
						(@ProductId,@ProductItemId,@Named,@PictureId)
			            ");

            SqlParameter[] parms = {
                                        new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@ProductItemId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Named",SqlDbType.NVarChar,20),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = Guid.Parse(model.ProductId.ToString());
            parms[1].Value = Guid.Parse(model.ProductItemId.ToString());
            parms[2].Value = model.Named;
            parms[3].Value = Guid.Parse(model.PictureId.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ProductSizePictureInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ProductSizePicture set Named = @Named,PictureId = @PictureId 
			            where ProductId = @ProductId and ProductItemId = @ProductItemId
					    ");

            SqlParameter[] parms = {
                                        new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@ProductItemId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Named",SqlDbType.NVarChar,20),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = Guid.Parse(model.ProductId.ToString());
            parms[1].Value = Guid.Parse(model.ProductItemId.ToString());
            parms[2].Value = model.Named;
            parms[3].Value = Guid.Parse(model.PictureId.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object ProductId)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ProductSizePicture where ProductId = @ProductId");
            SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ProductId.ToString());

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
                sb.Append(@"delete from ProductSizePicture where ProductId = @ProductId" + n + " ;");
                SqlParameter parm = new SqlParameter("@ProductId" + n + "", SqlDbType.UniqueIdentifier);
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

        public ProductSizePictureInfo GetModel(object ProductId)
        {
            ProductSizePictureInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 ProductId,ProductItemId,Named,PictureId 
			                   from ProductSizePicture
							   where ProductId = @ProductId ");
            SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ProductId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm))
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
                    }
                }
            }

            return model;
        }

        public IList<ProductSizePictureInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ProductSizePicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ProductSizePictureInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          ProductId,ProductItemId,Named,PictureId
					  from ProductSizePicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProductSizePictureInfo> list = new List<ProductSizePictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductSizePictureInfo model = new ProductSizePictureInfo();
                        model.ProductId = reader.GetGuid(1);
                        model.ProductItemId = reader.GetGuid(2);
                        model.Named = reader.GetString(3);
                        model.PictureId = reader.GetGuid(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductSizePictureInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           ProductId,ProductItemId,Named,PictureId
					   from ProductSizePicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProductSizePictureInfo> list = new List<ProductSizePictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductSizePictureInfo model = new ProductSizePictureInfo();
                        model.ProductId = reader.GetGuid(1);
                        model.ProductItemId = reader.GetGuid(2);
                        model.Named = reader.GetString(3);
                        model.PictureId = reader.GetGuid(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductSizePictureInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ProductId,ProductItemId,Named,PictureId
                        from ProductSizePicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ProductSizePictureInfo> list = new List<ProductSizePictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductSizePictureInfo model = new ProductSizePictureInfo();
                        model.ProductId = reader.GetGuid(0);
                        model.ProductItemId = reader.GetGuid(1);
                        model.Named = reader.GetString(2);
                        model.PictureId = reader.GetGuid(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductSizePictureInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ProductId,ProductItemId,Named,PictureId 
			            from ProductSizePicture
					    order by LastUpdatedDate desc ");

            IList<ProductSizePictureInfo> list = new List<ProductSizePictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductSizePictureInfo model = new ProductSizePictureInfo();
                        model.ProductId = reader.GetGuid(0);
                        model.ProductItemId = reader.GetGuid(1);
                        model.Named = reader.GetString(2);
                        model.PictureId = reader.GetGuid(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
