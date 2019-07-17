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
    public partial class ActivityRelease : IActivityRelease
    {
        #region IActivityRelease Member

        public int Insert(ActivityReleaseInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ActivityRelease (Title,StartDate,EndDate,QuestionCount,Remark,IsDisable,LastUpdatedDate)
			            values
						(@Title,@StartDate,@EndDate,@QuestionCount,@Remark,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@StartDate",SqlDbType.DateTime),
new SqlParameter("@EndDate",SqlDbType.DateTime),
new SqlParameter("@QuestionCount",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Title;
parms[1].Value = model.StartDate;
parms[2].Value = model.EndDate;
parms[3].Value = model.QuestionCount;
parms[4].Value = model.Remark;
parms[5].Value = model.IsDisable;
parms[6].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ActivityReleaseInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ActivityRelease set Title = @Title,StartDate = @StartDate,EndDate = @EndDate,QuestionCount = @QuestionCount,Remark = @Remark,IsDisable = @IsDisable,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@StartDate",SqlDbType.DateTime),
new SqlParameter("@EndDate",SqlDbType.DateTime),
new SqlParameter("@QuestionCount",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
parms[1].Value = model.Title;
parms[2].Value = model.StartDate;
parms[3].Value = model.EndDate;
parms[4].Value = model.QuestionCount;
parms[5].Value = model.Remark;
parms[6].Value = model.IsDisable;
parms[7].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ActivityRelease where Id = @Id");
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
                sb.Append(@"delete from ActivityRelease where Id = @Id" + n + " ;");
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

        public ActivityReleaseInfo GetModel(object Id)
        {
            ActivityReleaseInfo model = null;

			StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,Title,StartDate,EndDate,QuestionCount,Remark,IsDisable,LastUpdatedDate 
			                   from ActivityRelease
							   where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ActivityReleaseInfo();
                        model.Id = reader.GetGuid(0);
model.Title = reader.GetString(1);
model.StartDate = reader.GetDateTime(2);
model.EndDate = reader.GetDateTime(3);
model.QuestionCount = reader.GetInt32(4);
model.Remark = reader.GetString(5);
model.IsDisable = reader.GetBoolean(6);
model.LastUpdatedDate = reader.GetDateTime(7);
                    }
                }
            }

            return model;
        }

        public IList<ActivityReleaseInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ActivityRelease ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

			if (totalRecords == 0) return new List<ActivityReleaseInfo>();

			sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,Title,StartDate,EndDate,QuestionCount,Remark,IsDisable,LastUpdatedDate
					  from ActivityRelease ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
			sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivityReleaseInfo> list = new List<ActivityReleaseInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityReleaseInfo model = new ActivityReleaseInfo();
                        model.Id = reader.GetGuid(1);
model.Title = reader.GetString(2);
model.StartDate = reader.GetDateTime(3);
model.EndDate = reader.GetDateTime(4);
model.QuestionCount = reader.GetInt32(5);
model.Remark = reader.GetString(6);
model.IsDisable = reader.GetBoolean(7);
model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityReleaseInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,Title,StartDate,EndDate,QuestionCount,Remark,IsDisable,LastUpdatedDate
					   from ActivityRelease ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivityReleaseInfo> list = new List<ActivityReleaseInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityReleaseInfo model = new ActivityReleaseInfo();
                        model.Id = reader.GetGuid(1);
model.Title = reader.GetString(2);
model.StartDate = reader.GetDateTime(3);
model.EndDate = reader.GetDateTime(4);
model.QuestionCount = reader.GetInt32(5);
model.Remark = reader.GetString(6);
model.IsDisable = reader.GetBoolean(7);
model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityReleaseInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,StartDate,EndDate,QuestionCount,Remark,IsDisable,LastUpdatedDate
                        from ActivityRelease ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ActivityReleaseInfo> list = new List<ActivityReleaseInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityReleaseInfo model = new ActivityReleaseInfo();
                        model.Id = reader.GetGuid(0);
model.Title = reader.GetString(1);
model.StartDate = reader.GetDateTime(2);
model.EndDate = reader.GetDateTime(3);
model.QuestionCount = reader.GetInt32(4);
model.Remark = reader.GetString(5);
model.IsDisable = reader.GetBoolean(6);
model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityReleaseInfo> GetList()
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,StartDate,EndDate,QuestionCount,Remark,IsDisable,LastUpdatedDate 
			            from ActivityRelease
					    order by LastUpdatedDate desc ");

            IList<ActivityReleaseInfo> list = new List<ActivityReleaseInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityReleaseInfo model = new ActivityReleaseInfo();
                        model.Id = reader.GetGuid(0);
model.Title = reader.GetString(1);
model.StartDate = reader.GetDateTime(2);
model.EndDate = reader.GetDateTime(3);
model.QuestionCount = reader.GetInt32(4);
model.Remark = reader.GetString(5);
model.IsDisable = reader.GetBoolean(6);
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
