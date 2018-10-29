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
    public partial class Notice : INotice
    {
        #region INotice Member

        public int Insert(NoticeInfo model)
        {
            if (IsExist(model.Title, null)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Notice (ContentTypeId,Title,Descr,ContentText,LastUpdatedDate)
			            values
						(@ContentTypeId,@Title,@Descr,@ContentText,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ContentTypeId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Title",SqlDbType.NVarChar,100),
                                        new SqlParameter("@Descr",SqlDbType.NVarChar,300),
                                        new SqlParameter("@ContentText",SqlDbType.NText,1073741823),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.ContentTypeId;
            parms[1].Value = model.Title;
            parms[2].Value = model.Descr;
            parms[3].Value = model.ContentText;
            parms[4].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(NoticeInfo model)
        {
            if (IsExist(model.Title, model.Id)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update Notice set ContentTypeId = @ContentTypeId,Title = @Title,Descr = @Descr,ContentText = @ContentText,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@ContentTypeId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Title",SqlDbType.NVarChar,100),
                                        new SqlParameter("@Descr",SqlDbType.NVarChar,300),
                                        new SqlParameter("@ContentText",SqlDbType.NText,1073741823),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.ContentTypeId;
            parms[2].Value = model.Title;
            parms[3].Value = model.Descr;
            parms[4].Value = model.ContentText;
            parms[5].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from Notice where Id = @Id");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm);
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
                sb.Append(@"delete from Notice where Id = @Id" + n + " ;");
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

        public NoticeInfo GetModel(object Id)
        {
            NoticeInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,ContentTypeId,Title,Descr,ContentText,LastUpdatedDate 
			            from Notice
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new NoticeInfo();
                        model.Id = reader.GetGuid(0);
                        model.ContentTypeId = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.Descr = reader.GetString(3);
                        model.ContentText = reader.GetString(4);
                        model.LastUpdatedDate = reader.GetDateTime(5);
                    }
                }
            }

            return model;
        }

        public IList<NoticeInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Notice ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<NoticeInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,ContentTypeId,Title,Descr,ContentText,LastUpdatedDate
					  from Notice ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<NoticeInfo> list = new List<NoticeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NoticeInfo model = new NoticeInfo();
                        model.Id = reader.GetGuid(1);
                        model.ContentTypeId = reader.GetGuid(2);
                        model.Title = reader.GetString(3);
                        model.Descr = reader.GetString(4);
                        model.ContentText = reader.GetString(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<NoticeInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,ContentTypeId,Title,Descr,ContentText,LastUpdatedDate
					   from Notice ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<NoticeInfo> list = new List<NoticeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NoticeInfo model = new NoticeInfo();
                        model.Id = reader.GetGuid(1);
                        model.ContentTypeId = reader.GetGuid(2);
                        model.Title = reader.GetString(3);
                        model.Descr = reader.GetString(4);
                        model.ContentText = reader.GetString(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<NoticeInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ContentTypeId,Title,Descr,ContentText,LastUpdatedDate
                        from Notice ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<NoticeInfo> list = new List<NoticeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NoticeInfo model = new NoticeInfo();
                        model.Id = reader.GetGuid(0);
                        model.ContentTypeId = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.Descr = reader.GetString(3);
                        model.ContentText = reader.GetString(4);
                        model.LastUpdatedDate = reader.GetDateTime(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<NoticeInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ContentTypeId,Title,Descr,ContentText,LastUpdatedDate 
			            from Notice
					    order by LastUpdatedDate desc ");

            IList<NoticeInfo> list = new List<NoticeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NoticeInfo model = new NoticeInfo();
                        model.Id = reader.GetGuid(0);
                        model.ContentTypeId = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.Descr = reader.GetString(3);
                        model.ContentText = reader.GetString(4);
                        model.LastUpdatedDate = reader.GetDateTime(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
