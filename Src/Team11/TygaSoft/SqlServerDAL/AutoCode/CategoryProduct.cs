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
    public partial class CategoryProduct : ICategoryProduct
    {
        #region ICategoryProduct Member

        public int Insert(CategoryProductInfo model)
        {
            string cmdText = @"insert into CategoryProduct (CategoryId,ProductId)
			                 values
							 (@CategoryId,@ProductId)
			                 ";

            SqlParameter[] parms = {
                                       new SqlParameter("@CategoryId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.CategoryId;
            parms[1].Value = model.ProductId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
        }

        public int Update(CategoryProductInfo model)
        {
            string cmdText = @"update CategoryProduct set CategoryId = @CategoryId 
			                 where ProductId = @ProductId";

            SqlParameter[] parms = {
                                     new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@CategoryId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.ProductId;
            parms[1].Value = model.CategoryId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
        }

        public int Delete(object ProductId)
        {
            string cmdText = "delete from CategoryProduct where ProductId = @ProductId";
            SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ProductId.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parm);
        }

        public bool DeleteBatch(IList<object> list)
        {
            if (list == null || list.Count == 0) return false;

            bool result = false;
            StringBuilder sb = new StringBuilder();
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from CategoryProduct where ProductId = @ProductId" + n + " ;");
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

        public CategoryProductInfo GetModel(object ProductId)
        {
            CategoryProductInfo model = null;

            string cmdText = @"select top 1 ProductId,CategoryId 
			                   from CategoryProduct
							   where ProductId = @ProductId ";
            SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ProductId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new CategoryProductInfo();
                        model.ProductId = reader.GetGuid(0);
                        model.CategoryId = reader.GetGuid(1);
                    }
                }
            }

            return model;
        }

        public List<CategoryProductInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = @"select count(*) from CategoryProduct ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms);

            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            cmdText = @"select * from(select row_number() over(order by ProductId) as RowNumber,
			          ProductId,CategoryId
					  from CategoryProduct ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            List<CategoryProductInfo> list = new List<CategoryProductInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryProductInfo model = new CategoryProductInfo();
                        model.ProductId = reader.GetGuid(1);
                        model.CategoryId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<CategoryProductInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            string cmdText = @"select * from(select row_number() over(order by ProductId) as RowNumber,
			                 ProductId,CategoryId
							 from CategoryProduct";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            List<CategoryProductInfo> list = new List<CategoryProductInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryProductInfo model = new CategoryProductInfo();
                        model.ProductId = reader.GetGuid(1);
                        model.CategoryId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<CategoryProductInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = @"select ProductId,CategoryId
                              from CategoryProduct";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;

            List<CategoryProductInfo> list = new List<CategoryProductInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryProductInfo model = new CategoryProductInfo();
                        model.ProductId = reader.GetGuid(0);
                        model.CategoryId = reader.GetGuid(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<CategoryProductInfo> GetList()
        {
            string cmdText = @"select ProductId,CategoryId 
			                from CategoryProduct
							order by ProductId ";

            List<CategoryProductInfo> list = new List<CategoryProductInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryProductInfo model = new CategoryProductInfo();
                        model.ProductId = reader.GetGuid(0);
                        model.CategoryId = reader.GetGuid(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
