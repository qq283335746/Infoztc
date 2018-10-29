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
    public partial class ProductDetail : IProductDetail
    {
        #region IProductDetail Member

        public int Insert(ProductDetailInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ProductDetail (ProductId,ProductItemId,OriginalPrice,ProductPrice,Discount,DiscountDescr,ContentText,PayOption,ViewCount)
			            values
						(@ProductId,@ProductItemId,@OriginalPrice,@ProductPrice,@Discount,@DiscountDescr,@ContentText,@PayOption,@ViewCount)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@ProductItemId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@OriginalPrice",SqlDbType.Decimal),
                                        new SqlParameter("@ProductPrice",SqlDbType.Decimal),
                                        new SqlParameter("@Discount",SqlDbType.Float),
                                        new SqlParameter("@DiscountDescr",SqlDbType.NVarChar,20),
                                        new SqlParameter("@ContentText",SqlDbType.NText,1073741823),
                                        new SqlParameter("@PayOption",SqlDbType.NVarChar,100),
                                        new SqlParameter("@ViewCount",SqlDbType.Int)
                                   };
            parms[0].Value = model.ProductId;
            parms[1].Value = model.ProductItemId;
            parms[2].Value = model.OriginalPrice;
            parms[3].Value = model.ProductPrice;
            parms[4].Value = model.Discount;
            parms[5].Value = model.DiscountDescr;
            parms[6].Value = model.ContentText;
            parms[7].Value = model.PayOption;
            parms[8].Value = model.ViewCount;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ProductDetailInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ProductDetail set OriginalPrice = @OriginalPrice,ProductPrice = @ProductPrice,Discount = @Discount,DiscountDescr = @DiscountDescr,ContentText = @ContentText,PayOption = @PayOption,ViewCount = @ViewCount 
			            where ProductId = @ProductId and ProductItemId = @ProductItemId
					    ");

            SqlParameter[] parms = {
                                        new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@ProductItemId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@OriginalPrice",SqlDbType.Decimal),
                                        new SqlParameter("@ProductPrice",SqlDbType.Decimal),
                                        new SqlParameter("@Discount",SqlDbType.Float),
                                        new SqlParameter("@DiscountDescr",SqlDbType.NVarChar,20),
                                        new SqlParameter("@ContentText",SqlDbType.NText,1073741823),
                                        new SqlParameter("@PayOption",SqlDbType.NVarChar,100),
                                        new SqlParameter("@ViewCount",SqlDbType.Int)
                                   };
            parms[0].Value = model.ProductId;
            parms[1].Value = model.ProductItemId;
            parms[2].Value = model.OriginalPrice;
            parms[3].Value = model.ProductPrice;
            parms[4].Value = model.Discount;
            parms[5].Value = model.DiscountDescr;
            parms[6].Value = model.ContentText;
            parms[7].Value = model.PayOption;
            parms[8].Value = model.ViewCount;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object ProductId)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ProductDetail where ProductId = @ProductId");
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
                sb.Append(@"delete from ProductDetail where ProductId = @ProductId" + n + " ;");
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

        public ProductDetailInfo GetModel(object ProductId)
        {
            ProductDetailInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 ProductId,ProductItemId,OriginalPrice,ProductPrice,Discount,DiscountDescr,ContentText,PayOption,ViewCount 
			            from ProductDetail
						where ProductId = @ProductId ");
            SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ProductId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm))
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

        public IList<ProductDetailInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ProductDetail ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ProductDetailInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          ProductId,ProductItemId,OriginalPrice,ProductPrice,Discount,DiscountDescr,ContentText,PayOption,ViewCount
					  from ProductDetail ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProductDetailInfo> list = new List<ProductDetailInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductDetailInfo model = new ProductDetailInfo();
                        model.ProductId = reader.GetGuid(1);
                        model.ProductItemId = reader.GetGuid(2);
                        model.OriginalPrice = reader.GetDecimal(3);
                        model.ProductPrice = reader.GetDecimal(4);
                        model.Discount = reader.GetDouble(5);
                        model.DiscountDescr = reader.GetString(6);
                        model.ContentText = reader.GetString(7);
                        model.PayOption = reader.GetString(8);
                        model.ViewCount = reader.GetInt32(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductDetailInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           ProductId,ProductItemId,OriginalPrice,ProductPrice,Discount,DiscountDescr,ContentText,PayOption,ViewCount
					   from ProductDetail ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProductDetailInfo> list = new List<ProductDetailInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductDetailInfo model = new ProductDetailInfo();
                        model.ProductId = reader.GetGuid(1);
                        model.ProductItemId = reader.GetGuid(2);
                        model.OriginalPrice = reader.GetDecimal(3);
                        model.ProductPrice = reader.GetDecimal(4);
                        model.Discount = reader.GetDouble(5);
                        model.DiscountDescr = reader.GetString(6);
                        model.ContentText = reader.GetString(7);
                        model.PayOption = reader.GetString(8);
                        model.ViewCount = reader.GetInt32(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductDetailInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ProductId,ProductItemId,OriginalPrice,ProductPrice,Discount,DiscountDescr,ContentText,PayOption,ViewCount
                        from ProductDetail ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ProductDetailInfo> list = new List<ProductDetailInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductDetailInfo model = new ProductDetailInfo();
                        model.ProductId = reader.GetGuid(0);
                        model.ProductItemId = reader.GetGuid(1);
                        model.OriginalPrice = reader.GetDecimal(2);
                        model.ProductPrice = reader.GetDecimal(3);
                        model.Discount = reader.GetDouble(4);
                        model.DiscountDescr = reader.GetString(5);
                        model.ContentText = reader.GetString(6);
                        model.PayOption = reader.GetString(7);
                        model.ViewCount = reader.GetInt32(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductDetailInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ProductId,ProductItemId,OriginalPrice,ProductPrice,Discount,DiscountDescr,ContentText,PayOption,ViewCount 
			            from ProductDetail
					    order by LastUpdatedDate desc ");

            IList<ProductDetailInfo> list = new List<ProductDetailInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductDetailInfo model = new ProductDetailInfo();
                        model.ProductId = reader.GetGuid(0);
                        model.ProductItemId = reader.GetGuid(1);
                        model.OriginalPrice = reader.GetDecimal(2);
                        model.ProductPrice = reader.GetDecimal(3);
                        model.Discount = reader.GetDouble(4);
                        model.DiscountDescr = reader.GetString(5);
                        model.ContentText = reader.GetString(6);
                        model.PayOption = reader.GetString(7);
                        model.ViewCount = reader.GetInt32(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
