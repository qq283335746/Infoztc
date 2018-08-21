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
    public partial class TopicSubject : ITopicSubject
    {
        #region ITopicSubject Member

        public int Insert(TopicSubjectInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into TopicSubject (UserId,Title,ContentText,IsTop,Sort,Remark,IsDisable,LastUpdatedDate)
			            values
						(@UserId,@Title,@ContentText,@IsTop,@Sort,@Remark,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@ContentText",SqlDbType.NVarChar,2000),
new SqlParameter("@IsTop",SqlDbType.Bit),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.UserId;
parms[1].Value = model.Title;
parms[2].Value = model.ContentText;
parms[3].Value = model.IsTop;
parms[4].Value = model.Sort;
parms[5].Value = model.Remark;
parms[6].Value = model.IsDisable;
parms[7].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(TopicSubjectInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update TopicSubject set UserId = @UserId,Title = @Title,ContentText = @ContentText,IsTop = @IsTop,Sort = @Sort,Remark = @Remark,IsDisable = @IsDisable,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@ContentText",SqlDbType.NVarChar,2000),
new SqlParameter("@IsTop",SqlDbType.Bit),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
parms[1].Value = model.UserId;
parms[2].Value = model.Title;
parms[3].Value = model.ContentText;
parms[4].Value = model.IsTop;
parms[5].Value = model.Sort;
parms[6].Value = model.Remark;
parms[7].Value = model.IsDisable;
parms[8].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from TopicSubject where Id = @Id");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);
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
                sb.Append(@"delete from TopicSubject where Id = @Id" + n + " ;");
                SqlParameter parm = new SqlParameter("@Id" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(item);
                parms.Add(parm);
            }
            using (SqlConnection conn = new SqlConnection(SqlHelper.HnztcTeamDbConnString))
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

        public TopicSubjectInfo GetModel(object Id)
        {
            TopicSubjectInfo model = null;

			StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,UserId,Title,ContentText,IsTop,Sort,Remark,IsDisable,LastUpdatedDate 
			                   from TopicSubject
							   where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new TopicSubjectInfo();
                        model.Id = reader.GetGuid(0);
model.UserId = reader.GetGuid(1);
model.Title = reader.GetString(2);
model.ContentText = reader.GetString(3);
model.IsTop = reader.GetBoolean(4);
model.Sort = reader.GetInt32(5);
model.Remark = reader.GetString(6);
model.IsDisable = reader.GetBoolean(7);
model.LastUpdatedDate = reader.GetDateTime(8);
                    }
                }
            }

            return model;
        }

        public IList<TopicSubjectInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from TopicSubject ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

			if (totalRecords == 0) return new List<TopicSubjectInfo>();

			sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,UserId,Title,ContentText,IsTop,Sort,Remark,IsDisable,LastUpdatedDate
					  from TopicSubject ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
			sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<TopicSubjectInfo> list = new List<TopicSubjectInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TopicSubjectInfo model = new TopicSubjectInfo();
                        model.Id = reader.GetGuid(1);
model.UserId = reader.GetGuid(2);
model.Title = reader.GetString(3);
model.ContentText = reader.GetString(4);
model.IsTop = reader.GetBoolean(5);
model.Sort = reader.GetInt32(6);
model.Remark = reader.GetString(7);
model.IsDisable = reader.GetBoolean(8);
model.LastUpdatedDate = reader.GetDateTime(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<TopicSubjectInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,UserId,Title,ContentText,IsTop,Sort,Remark,IsDisable,LastUpdatedDate
					   from TopicSubject ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<TopicSubjectInfo> list = new List<TopicSubjectInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TopicSubjectInfo model = new TopicSubjectInfo();
                        model.Id = reader.GetGuid(1);
model.UserId = reader.GetGuid(2);
model.Title = reader.GetString(3);
model.ContentText = reader.GetString(4);
model.IsTop = reader.GetBoolean(5);
model.Sort = reader.GetInt32(6);
model.Remark = reader.GetString(7);
model.IsDisable = reader.GetBoolean(8);
model.LastUpdatedDate = reader.GetDateTime(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<TopicSubjectInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,UserId,Title,ContentText,IsTop,Sort,Remark,IsDisable,LastUpdatedDate
                        from TopicSubject ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<TopicSubjectInfo> list = new List<TopicSubjectInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TopicSubjectInfo model = new TopicSubjectInfo();
                        model.Id = reader.GetGuid(0);
model.UserId = reader.GetGuid(1);
model.Title = reader.GetString(2);
model.ContentText = reader.GetString(3);
model.IsTop = reader.GetBoolean(4);
model.Sort = reader.GetInt32(5);
model.Remark = reader.GetString(6);
model.IsDisable = reader.GetBoolean(7);
model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<TopicSubjectInfo> GetList()
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,UserId,Title,ContentText,IsTop,Sort,Remark,IsDisable,LastUpdatedDate 
			            from TopicSubject
					    order by LastUpdatedDate desc ");

            IList<TopicSubjectInfo> list = new List<TopicSubjectInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TopicSubjectInfo model = new TopicSubjectInfo();
                        model.Id = reader.GetGuid(0);
model.UserId = reader.GetGuid(1);
model.Title = reader.GetString(2);
model.ContentText = reader.GetString(3);
model.IsTop = reader.GetBoolean(4);
model.Sort = reader.GetInt32(5);
model.Remark = reader.GetString(6);
model.IsDisable = reader.GetBoolean(7);
model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
