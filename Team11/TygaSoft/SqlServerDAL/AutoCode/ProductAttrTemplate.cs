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
    public partial class ProductAttrTemplate : IProductAttrTemplate
    {
        #region IProductAttrTemplate Member

        public int Insert(ProductAttrTemplateInfo model)
        {
            if (IsExist(model.TName, null)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ProductAttrTemplate (TName,TValue,LastUpdatedDate)
			            values
						(@TName,@TValue,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@TName",SqlDbType.NVarChar,100),
                                        new SqlParameter("@TValue",SqlDbType.NVarChar,1000),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.TName;
            parms[1].Value = model.TValue;
            parms[2].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ProductAttrTemplateInfo model)
        {
            if (IsExist(model.TName, model.Id)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ProductAttrTemplate set TName = @TName,TValue = @TValue,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                    new SqlParameter("@TName",SqlDbType.NVarChar,100),
                                    new SqlParameter("@TValue",SqlDbType.NVarChar,1000),
                                    new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.TName;
            parms[2].Value = model.TValue;
            parms[3].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ProductAttrTemplate where Id = @Id");
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
                sb.Append(@"delete from ProductAttrTemplate where Id = @Id" + n + " ;");
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

        public ProductAttrTemplateInfo GetModel(object Id)
        {
            ProductAttrTemplateInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,TName,TValue,LastUpdatedDate 
			            from ProductAttrTemplate
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ProductAttrTemplateInfo();
                        model.Id = reader.GetGuid(0);
                        model.TName = reader.GetString(1);
                        model.TValue = reader.GetString(2);
                        model.LastUpdatedDate = reader.GetDateTime(3);
                    }
                }
            }

            return model;
        }

        public IList<ProductAttrTemplateInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ProductAttrTemplate ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ProductAttrTemplateInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,TName,TValue,LastUpdatedDate
					  from ProductAttrTemplate ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProductAttrTemplateInfo> list = new List<ProductAttrTemplateInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductAttrTemplateInfo model = new ProductAttrTemplateInfo();
                        model.Id = reader.GetGuid(1);
                        model.TName = reader.GetString(2);
                        model.TValue = reader.GetString(3);
                        model.LastUpdatedDate = reader.GetDateTime(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductAttrTemplateInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,TName,TValue,LastUpdatedDate
					   from ProductAttrTemplate ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProductAttrTemplateInfo> list = new List<ProductAttrTemplateInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductAttrTemplateInfo model = new ProductAttrTemplateInfo();
                        model.Id = reader.GetGuid(1);
                        model.TName = reader.GetString(2);
                        model.TValue = reader.GetString(3);
                        model.LastUpdatedDate = reader.GetDateTime(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductAttrTemplateInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,TName,TValue,LastUpdatedDate
                        from ProductAttrTemplate ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ProductAttrTemplateInfo> list = new List<ProductAttrTemplateInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductAttrTemplateInfo model = new ProductAttrTemplateInfo();
                        model.Id = reader.GetGuid(0);
                        model.TName = reader.GetString(1);
                        model.TValue = reader.GetString(2);
                        model.LastUpdatedDate = reader.GetDateTime(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductAttrTemplateInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,TName,TValue,LastUpdatedDate 
			            from ProductAttrTemplate
					    order by LastUpdatedDate desc ");

            IList<ProductAttrTemplateInfo> list = new List<ProductAttrTemplateInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductAttrTemplateInfo model = new ProductAttrTemplateInfo();
                        model.Id = reader.GetGuid(0);
                        model.TName = reader.GetString(1);
                        model.TValue = reader.GetString(2);
                        model.LastUpdatedDate = reader.GetDateTime(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
