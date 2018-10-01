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
    public partial class ContentType : IContentType
    {
        #region IContentType Member

        public int Insert(ContentTypeInfo model)
        {
            if (IsExist(model.TypeCode, model.ParentId, null)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ContentType (TypeName,TypeCode,TypeValue,ParentId,Sort,PictureId,HasChild,IsSys,LastUpdatedDate)
			            values
						(@TypeName,@TypeCode,@TypeValue,@ParentId,@Sort,@PictureId,@HasChild,@IsSys,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@TypeName",SqlDbType.NVarChar,50),
                                        new SqlParameter("@TypeCode",SqlDbType.VarChar,50),
                                        new SqlParameter("@TypeValue",SqlDbType.NVarChar,256),
                                        new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@HasChild",SqlDbType.Bit),
                                        new SqlParameter("@IsSys",SqlDbType.Bit),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.TypeName;
            parms[1].Value = model.TypeCode;
            parms[2].Value = model.TypeValue;
            parms[3].Value = model.ParentId;
            parms[4].Value = model.Sort;
            parms[5].Value = model.PictureId;
            parms[6].Value = model.HasChild;
            parms[7].Value = model.IsSys;
            parms[8].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ContentTypeInfo model)
        {
            if (IsExist(model.TypeCode, model.ParentId, model.Id)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ContentType set TypeName = @TypeName,TypeCode = @TypeCode,TypeValue = @TypeValue,ParentId = @ParentId,Sort = @Sort,PictureId = @PictureId,HasChild = @HasChild,IsSys = @IsSys,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@TypeName",SqlDbType.NVarChar,50),
                                        new SqlParameter("@TypeCode",SqlDbType.VarChar,50),
                                        new SqlParameter("@TypeValue",SqlDbType.NVarChar,256),
                                        new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@HasChild",SqlDbType.Bit),
                                        new SqlParameter("@IsSys",SqlDbType.Bit),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.TypeName;
            parms[2].Value = model.TypeCode;
            parms[3].Value = model.TypeValue;
            parms[4].Value = model.ParentId;
            parms[5].Value = model.Sort;
            parms[6].Value = model.PictureId;
            parms[7].Value = model.HasChild;
            parms[8].Value = model.IsSys;
            parms[9].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            string cmdText = "delete from ContentType where Id = @Id";
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm);
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
                sb.Append(@"delete from ContentType where Id = @Id" + n + " ;");
                SqlParameter parm = new SqlParameter("@Id" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(item);
                parms.Add(parm);
            }
            using (SqlConnection conn = new SqlConnection(SqlHelper.SqlProviderConnString))
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

        public ContentTypeInfo GetModel(object Id)
        {
            ContentTypeInfo model = null;

            string cmdText = @"select top 1 Id,TypeName,TypeCode,TypeValue,ParentId,Sort,PictureId,HasChild,IsSys,LastUpdatedDate 
			                   from ContentType
							   where Id = @Id ";
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ContentTypeInfo();
                        model.Id = reader.GetGuid(0);
                        model.TypeName = reader.GetString(1);
                        model.TypeCode = reader.GetString(2);
                        model.TypeValue = reader.GetString(3);
                        model.ParentId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.PictureId = reader.GetGuid(6);
                        model.HasChild = reader.GetBoolean(7);
                        model.IsSys = reader.GetBoolean(8);
                        model.LastUpdatedDate = reader.GetDateTime(9);
                    }
                }
            }

            return model;
        }

        public IList<ContentTypeInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ContentType ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ContentTypeInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,TypeName,TypeCode,TypeValue,ParentId,Sort,PictureId,HasChild,IsSys,LastUpdatedDate
					  from ContentType ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ContentTypeInfo> list = new List<ContentTypeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContentTypeInfo model = new ContentTypeInfo();
                        model.Id = reader.GetGuid(1);
                        model.TypeName = reader.GetString(2);
                        model.TypeCode = reader.GetString(3);
                        model.TypeValue = reader.GetString(4);
                        model.ParentId = reader.GetGuid(5);
                        model.Sort = reader.GetInt32(6);
                        model.PictureId = reader.GetGuid(7);
                        model.HasChild = reader.GetBoolean(8);
                        model.IsSys = reader.GetBoolean(9);
                        model.LastUpdatedDate = reader.GetDateTime(10);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ContentTypeInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,TypeName,TypeCode,TypeValue,ParentId,Sort,PictureId,HasChild,IsSys,LastUpdatedDate
					   from ContentType ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ContentTypeInfo> list = new List<ContentTypeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContentTypeInfo model = new ContentTypeInfo();
                        model.Id = reader.GetGuid(1);
                        model.TypeName = reader.GetString(2);
                        model.TypeCode = reader.GetString(3);
                        model.TypeValue = reader.GetString(4);
                        model.ParentId = reader.GetGuid(5);
                        model.Sort = reader.GetInt32(6);
                        model.PictureId = reader.GetGuid(7);
                        model.HasChild = reader.GetBoolean(8);
                        model.IsSys = reader.GetBoolean(9);
                        model.LastUpdatedDate = reader.GetDateTime(10);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ContentTypeInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,TypeName,TypeCode,TypeValue,ParentId,Sort,PictureId,HasChild,IsSys,LastUpdatedDate
                        from ContentType ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ContentTypeInfo> list = new List<ContentTypeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContentTypeInfo model = new ContentTypeInfo();
                        model.Id = reader.GetGuid(0);
                        model.TypeName = reader.GetString(1);
                        model.TypeCode = reader.GetString(2);
                        model.TypeValue = reader.GetString(3);
                        model.ParentId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.PictureId = reader.GetGuid(6);
                        model.HasChild = reader.GetBoolean(7);
                        model.IsSys = reader.GetBoolean(8);
                        model.LastUpdatedDate = reader.GetDateTime(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ContentTypeInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,TypeName,TypeCode,TypeValue,ParentId,Sort,PictureId,HasChild,IsSys,LastUpdatedDate 
			            from ContentType
					    order by LastUpdatedDate desc ");

            IList<ContentTypeInfo> list = new List<ContentTypeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContentTypeInfo model = new ContentTypeInfo();
                        model.Id = reader.GetGuid(0);
                        model.TypeName = reader.GetString(1);
                        model.TypeCode = reader.GetString(2);
                        model.TypeValue = reader.GetString(3);
                        model.ParentId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.PictureId = reader.GetGuid(6);
                        model.HasChild = reader.GetBoolean(7);
                        model.IsSys = reader.GetBoolean(8);
                        model.LastUpdatedDate = reader.GetDateTime(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
