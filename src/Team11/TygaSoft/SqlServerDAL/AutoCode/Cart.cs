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
    public partial class Cart : ICart
    {
        #region ICart Member

        public int Insert(CartInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Cart (ProductId,CategoryId,Price,Quantity,Named,IsShoppingCart,LastUpdatedDate)
			            values
						(@ProductId,@CategoryId,@Price,@Quantity,@Named,@IsShoppingCart,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
new SqlParameter("@CategoryId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Price",SqlDbType.Decimal),
new SqlParameter("@Quantity",SqlDbType.Int),
new SqlParameter("@Named",SqlDbType.NVarChar,50),
new SqlParameter("@IsShoppingCart",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.ProductId;
            parms[1].Value = model.CategoryId;
            parms[2].Value = model.Price;
            parms[3].Value = model.Quantity;
            parms[4].Value = model.Named;
            parms[5].Value = model.IsShoppingCart;
            parms[6].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(CartInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update Cart set ProductId = @ProductId,CategoryId = @CategoryId,Price = @Price,Quantity = @Quantity,Named = @Named,IsShoppingCart = @IsShoppingCart,LastUpdatedDate = @LastUpdatedDate 
			            where ProfileId = @ProfileId
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@ProfileId",SqlDbType.UniqueIdentifier),
new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
new SqlParameter("@CategoryId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Price",SqlDbType.Decimal),
new SqlParameter("@Quantity",SqlDbType.Int),
new SqlParameter("@Named",SqlDbType.NVarChar,50),
new SqlParameter("@IsShoppingCart",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.ProfileId;
            parms[1].Value = model.ProductId;
            parms[2].Value = model.CategoryId;
            parms[3].Value = model.Price;
            parms[4].Value = model.Quantity;
            parms[5].Value = model.Named;
            parms[6].Value = model.IsShoppingCart;
            parms[7].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object ProfileId)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from Cart where ProfileId = @ProfileId");
            SqlParameter parm = new SqlParameter("@ProfileId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ProfileId.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm);
        }

        public bool DeleteBatch(IList<object> list)
        {
            bool result = false;
            StringBuilder sb = new StringBuilder(500);
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from Cart where ProfileId = @ProfileId" + n + " ;");
                SqlParameter parm = new SqlParameter("@ProfileId" + n + "", SqlDbType.UniqueIdentifier);
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

        public CartInfo GetModel(object ProfileId)
        {
            CartInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 ProfileId,ProductId,CategoryId,Price,Quantity,Named,IsShoppingCart,LastUpdatedDate 
			            from Cart
						where ProfileId = @ProfileId ");
            SqlParameter parm = new SqlParameter("@ProfileId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ProfileId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new CartInfo();
                        model.ProfileId = reader.GetGuid(0);
                        model.ProductId = reader.GetGuid(1);
                        model.CategoryId = reader.GetGuid(2);
                        model.Price = reader.GetDecimal(3);
                        model.Quantity = reader.GetInt32(4);
                        model.Named = reader.GetString(5);
                        model.IsShoppingCart = reader.GetBoolean(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                    }
                }
            }

            return model;
        }

        public IList<CartInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Cart ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<CartInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          ProfileId,ProductId,CategoryId,Price,Quantity,Named,IsShoppingCart,LastUpdatedDate
					  from Cart ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<CartInfo> list = new List<CartInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CartInfo model = new CartInfo();
                        model.ProfileId = reader.GetGuid(1);
                        model.ProductId = reader.GetGuid(2);
                        model.CategoryId = reader.GetGuid(3);
                        model.Price = reader.GetDecimal(4);
                        model.Quantity = reader.GetInt32(5);
                        model.Named = reader.GetString(6);
                        model.IsShoppingCart = reader.GetBoolean(7);
                        model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<CartInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           ProfileId,ProductId,CategoryId,Price,Quantity,Named,IsShoppingCart,LastUpdatedDate
					   from Cart ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<CartInfo> list = new List<CartInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CartInfo model = new CartInfo();
                        model.ProfileId = reader.GetGuid(1);
                        model.ProductId = reader.GetGuid(2);
                        model.CategoryId = reader.GetGuid(3);
                        model.Price = reader.GetDecimal(4);
                        model.Quantity = reader.GetInt32(5);
                        model.Named = reader.GetString(6);
                        model.IsShoppingCart = reader.GetBoolean(7);
                        model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<CartInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ProfileId,ProductId,CategoryId,Price,Quantity,Named,IsShoppingCart,LastUpdatedDate
                        from Cart ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<CartInfo> list = new List<CartInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CartInfo model = new CartInfo();
                        model.ProfileId = reader.GetGuid(0);
                        model.ProductId = reader.GetGuid(1);
                        model.CategoryId = reader.GetGuid(2);
                        model.Price = reader.GetDecimal(3);
                        model.Quantity = reader.GetInt32(4);
                        model.Named = reader.GetString(5);
                        model.IsShoppingCart = reader.GetBoolean(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<CartInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ProfileId,ProductId,CategoryId,Price,Quantity,Named,IsShoppingCart,LastUpdatedDate 
			            from Cart
					    order by LastUpdatedDate desc ");

            IList<CartInfo> list = new List<CartInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CartInfo model = new CartInfo();
                        model.ProfileId = reader.GetGuid(0);
                        model.ProductId = reader.GetGuid(1);
                        model.CategoryId = reader.GetGuid(2);
                        model.Price = reader.GetDecimal(3);
                        model.Quantity = reader.GetInt32(4);
                        model.Named = reader.GetString(5);
                        model.IsShoppingCart = reader.GetBoolean(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
