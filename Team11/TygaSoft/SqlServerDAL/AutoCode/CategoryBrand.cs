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
    public partial class CategoryBrand : ICategoryBrand
    {
        #region ICategoryBrand Member

        public int Insert(CategoryBrandInfo model)
        {
            string cmdText = @"insert into CategoryBrand (CategoryId,BrandId)
			                 values
							 (@CategoryId,@BrandId)
			                 ";

            SqlParameter[] parms = {
                                       new SqlParameter("@CategoryId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@BrandId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.CategoryId;
            parms[1].Value = model.BrandId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
        }

        public int Update(CategoryBrandInfo model)
        {
            string cmdText = @"update CategoryBrand set CategoryId = @CategoryId 
			                 where BrandId = @BrandId";

            SqlParameter[] parms = {
                                     new SqlParameter("@BrandId",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@CategoryId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.BrandId;
            parms[1].Value = model.CategoryId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
        }

        public int Delete(object BrandId)
        {
            string cmdText = "delete from CategoryBrand where BrandId = @BrandId";
            SqlParameter parm = new SqlParameter("@BrandId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(BrandId.ToString());

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
                sb.Append(@"delete from CategoryBrand where BrandId = @BrandId" + n + " ;");
                SqlParameter parm = new SqlParameter("@BrandId" + n + "", SqlDbType.UniqueIdentifier);
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

        public CategoryBrandInfo GetModel(object BrandId)
        {
            CategoryBrandInfo model = null;

            string cmdText = @"select top 1 BrandId,CategoryId 
			                   from CategoryBrand
							   where BrandId = @BrandId ";
            SqlParameter parm = new SqlParameter("@BrandId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(BrandId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new CategoryBrandInfo();
                        model.BrandId = reader.GetGuid(0);
                        model.CategoryId = reader.GetGuid(1);
                    }
                }
            }

            return model;
        }

        public List<CategoryBrandInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = @"select count(*) from CategoryBrand ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms);

            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            cmdText = @"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          BrandId,CategoryId
					  from CategoryBrand ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            List<CategoryBrandInfo> list = new List<CategoryBrandInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryBrandInfo model = new CategoryBrandInfo();
                        model.BrandId = reader.GetGuid(1);
                        model.CategoryId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<CategoryBrandInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            string cmdText = @"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			                 BrandId,CategoryId
							 from CategoryBrand";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            List<CategoryBrandInfo> list = new List<CategoryBrandInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryBrandInfo model = new CategoryBrandInfo();
                        model.BrandId = reader.GetGuid(1);
                        model.CategoryId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<CategoryBrandInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = @"select BrandId,CategoryId
                              from CategoryBrand";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;

            List<CategoryBrandInfo> list = new List<CategoryBrandInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryBrandInfo model = new CategoryBrandInfo();
                        model.BrandId = reader.GetGuid(0);
                        model.CategoryId = reader.GetGuid(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<CategoryBrandInfo> GetList()
        {
            string cmdText = @"select BrandId,CategoryId 
			                from CategoryBrand
							order by LastUpdatedDate desc ";

            List<CategoryBrandInfo> list = new List<CategoryBrandInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryBrandInfo model = new CategoryBrandInfo();
                        model.BrandId = reader.GetGuid(0);
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
