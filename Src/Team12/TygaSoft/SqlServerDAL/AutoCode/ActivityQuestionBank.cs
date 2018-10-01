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
    public partial class ActivityQuestionBank : IActivityQuestionBank
    {
        #region IActivityQuestionBank Member

        public int Insert(ActivityQuestionBankInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ActivityQuestionBank (QuestionBankId,QuestionCount)
			            values
						(@QuestionBankId,@QuestionCount)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@QuestionBankId",SqlDbType.UniqueIdentifier),
new SqlParameter("@QuestionCount",SqlDbType.Int)
                                   };
            parms[0].Value = model.QuestionBankId;
parms[1].Value = model.QuestionCount;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ActivityQuestionBankInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ActivityQuestionBank set QuestionBankId = @QuestionBankId,QuestionCount = @QuestionCount 
			            where ActivityReleaseId = @ActivityReleaseId
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@ActivityReleaseId",SqlDbType.UniqueIdentifier),
new SqlParameter("@QuestionBankId",SqlDbType.UniqueIdentifier),
new SqlParameter("@QuestionCount",SqlDbType.Int)
                                   };
            parms[0].Value = model.ActivityReleaseId;
parms[1].Value = model.QuestionBankId;
parms[2].Value = model.QuestionCount;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object ActivityReleaseId)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ActivityQuestionBank where ActivityReleaseId = @ActivityReleaseId");
            SqlParameter parm = new SqlParameter("@ActivityReleaseId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ActivityReleaseId.ToString());

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
                sb.Append(@"delete from ActivityQuestionBank where ActivityReleaseId = @ActivityReleaseId" + n + " ;");
                SqlParameter parm = new SqlParameter("@ActivityReleaseId" + n + "", SqlDbType.UniqueIdentifier);
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

        public ActivityQuestionBankInfo GetModel(object ActivityReleaseId)
        {
            ActivityQuestionBankInfo model = null;

			StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 ActivityReleaseId,QuestionBankId,QuestionCount 
			                   from ActivityQuestionBank
							   where ActivityReleaseId = @ActivityReleaseId ");
            SqlParameter parm = new SqlParameter("@ActivityReleaseId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ActivityReleaseId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ActivityQuestionBankInfo();
                        model.ActivityReleaseId = reader.GetGuid(0);
model.QuestionBankId = reader.GetGuid(1);
model.QuestionCount = reader.GetInt32(2);
                    }
                }
            }

            return model;
        }

        public IList<ActivityQuestionBankInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ActivityQuestionBank ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

			if (totalRecords == 0) return new List<ActivityQuestionBankInfo>();

			sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          ActivityReleaseId,QuestionBankId,QuestionCount
					  from ActivityQuestionBank ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
			sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivityQuestionBankInfo> list = new List<ActivityQuestionBankInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityQuestionBankInfo model = new ActivityQuestionBankInfo();
                        model.ActivityReleaseId = reader.GetGuid(1);
model.QuestionBankId = reader.GetGuid(2);
model.QuestionCount = reader.GetInt32(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityQuestionBankInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           ActivityReleaseId,QuestionBankId,QuestionCount
					   from ActivityQuestionBank ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivityQuestionBankInfo> list = new List<ActivityQuestionBankInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityQuestionBankInfo model = new ActivityQuestionBankInfo();
                        model.ActivityReleaseId = reader.GetGuid(1);
model.QuestionBankId = reader.GetGuid(2);
model.QuestionCount = reader.GetInt32(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityQuestionBankInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ActivityReleaseId,QuestionBankId,QuestionCount
                        from ActivityQuestionBank ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ActivityQuestionBankInfo> list = new List<ActivityQuestionBankInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityQuestionBankInfo model = new ActivityQuestionBankInfo();
                        model.ActivityReleaseId = reader.GetGuid(0);
model.QuestionBankId = reader.GetGuid(1);
model.QuestionCount = reader.GetInt32(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityQuestionBankInfo> GetList()
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ActivityReleaseId,QuestionBankId,QuestionCount 
			            from ActivityQuestionBank
					    order by LastUpdatedDate desc ");

            IList<ActivityQuestionBankInfo> list = new List<ActivityQuestionBankInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityQuestionBankInfo model = new ActivityQuestionBankInfo();
                        model.ActivityReleaseId = reader.GetGuid(0);
model.QuestionBankId = reader.GetGuid(1);
model.QuestionCount = reader.GetInt32(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
