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
    public partial class Category : ICategory
    {
        #region ICategory Member

        public int Insert(CategoryInfo model)
        {
            if (IsExist(model.CategoryName,model.ParentId, null)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Category (CategoryName,CategoryCode,ParentId,PictureId,Sort,Remark,LastUpdatedDate)
			            values
						(@CategoryName,@CategoryCode,@ParentId,@PictureId,@Sort,@Remark,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@CategoryName",SqlDbType.NVarChar,50),
                                        new SqlParameter("@CategoryCode",SqlDbType.VarChar,50),
                                        new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@Remark",SqlDbType.NVarChar,300),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.CategoryName;
            parms[1].Value = model.CategoryCode;
            parms[2].Value = model.ParentId;
            parms[3].Value = model.PictureId;
            parms[4].Value = model.Sort;
            parms[5].Value = model.Remark;
            parms[6].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(CategoryInfo model)
        {
            if (IsExist(model.CategoryName, model.ParentId, model.Id)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update Category set CategoryName = @CategoryName,CategoryCode = @CategoryCode,ParentId = @ParentId,PictureId = @PictureId,Sort = @Sort,Remark = @Remark,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@CategoryName",SqlDbType.NVarChar,50),
                                        new SqlParameter("@CategoryCode",SqlDbType.VarChar,50),
                                        new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@Remark",SqlDbType.NVarChar,300),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.CategoryName;
            parms[2].Value = model.CategoryCode;
            parms[3].Value = model.ParentId;
            parms[4].Value = model.PictureId;
            parms[5].Value = model.Sort;
            parms[6].Value = model.Remark;
            parms[7].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from Category where Id = @Id");
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
                sb.Append(@"delete from Category where Id = @Id" + n + " ;");
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

        public CategoryInfo GetModel(object Id)
        {
            CategoryInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,CategoryName,CategoryCode,ParentId,PictureId,Sort,Remark,LastUpdatedDate 
			            from Category
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new CategoryInfo();
                        model.Id = reader.GetGuid(0);
                        model.CategoryName = reader.GetString(1);
                        model.CategoryCode = reader.GetString(2);
                        model.ParentId = reader.GetGuid(3);
                        model.PictureId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                    }
                }
            }

            return model;
        }

        public IList<CategoryInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Category ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<CategoryInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,CategoryName,CategoryCode,ParentId,PictureId,Sort,Remark,LastUpdatedDate
					  from Category ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<CategoryInfo> list = new List<CategoryInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryInfo model = new CategoryInfo();
                        model.Id = reader.GetGuid(1);
                        model.CategoryName = reader.GetString(2);
                        model.CategoryCode = reader.GetString(3);
                        model.ParentId = reader.GetGuid(4);
                        model.PictureId = reader.GetGuid(5);
                        model.Sort = reader.GetInt32(6);
                        model.Remark = reader.GetString(7);
                        model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<CategoryInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,CategoryName,CategoryCode,ParentId,PictureId,Sort,Remark,LastUpdatedDate
					   from Category ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<CategoryInfo> list = new List<CategoryInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryInfo model = new CategoryInfo();
                        model.Id = reader.GetGuid(1);
                        model.CategoryName = reader.GetString(2);
                        model.CategoryCode = reader.GetString(3);
                        model.ParentId = reader.GetGuid(4);
                        model.PictureId = reader.GetGuid(5);
                        model.Sort = reader.GetInt32(6);
                        model.Remark = reader.GetString(7);
                        model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<CategoryInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,CategoryName,CategoryCode,ParentId,PictureId,Sort,Remark,LastUpdatedDate
                        from Category ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<CategoryInfo> list = new List<CategoryInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryInfo model = new CategoryInfo();
                        model.Id = reader.GetGuid(0);
                        model.CategoryName = reader.GetString(1);
                        model.CategoryCode = reader.GetString(2);
                        model.ParentId = reader.GetGuid(3);
                        model.PictureId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<CategoryInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,CategoryName,CategoryCode,ParentId,PictureId,Sort,Remark,LastUpdatedDate 
			            from Category
					    order by LastUpdatedDate desc ");

            IList<CategoryInfo> list = new List<CategoryInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryInfo model = new CategoryInfo();
                        model.Id = reader.GetGuid(0);
                        model.CategoryName = reader.GetString(1);
                        model.CategoryCode = reader.GetString(2);
                        model.ParentId = reader.GetGuid(3);
                        model.PictureId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
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
