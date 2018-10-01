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
    public partial class AnswerStatistics : IAnswerStatistics
    {
        #region IAnswerStatistics Member

        public int Insert(AnswerStatisticsInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into AnswerStatistics (UserId,QuestionSubjectId,PaperId,IsTrue,Integral,LastUpdatedDate)
			            values
						(@UserId,@QuestionSubjectId,@PaperId,@IsTrue,@Integral,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@QuestionSubjectId",SqlDbType.UniqueIdentifier),
new SqlParameter("@PaperId",SqlDbType.UniqueIdentifier),
new SqlParameter("@IsTrue",SqlDbType.Bit),
new SqlParameter("@Integral",SqlDbType.Int),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.UserId;
parms[1].Value = model.QuestionSubjectId;
parms[2].Value = model.PaperId;
parms[3].Value = model.IsTrue;
parms[4].Value = model.Integral;
parms[5].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(AnswerStatisticsInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update AnswerStatistics set UserId = @UserId,QuestionSubjectId = @QuestionSubjectId,PaperId = @PaperId,IsTrue = @IsTrue,Integral = @Integral,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@QuestionSubjectId",SqlDbType.UniqueIdentifier),
new SqlParameter("@PaperId",SqlDbType.UniqueIdentifier),
new SqlParameter("@IsTrue",SqlDbType.Bit),
new SqlParameter("@Integral",SqlDbType.Int),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
parms[1].Value = model.UserId;
parms[2].Value = model.QuestionSubjectId;
parms[3].Value = model.PaperId;
parms[4].Value = model.IsTrue;
parms[5].Value = model.Integral;
parms[6].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from AnswerStatistics where Id = @Id");
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
                sb.Append(@"delete from AnswerStatistics where Id = @Id" + n + " ;");
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

        public AnswerStatisticsInfo GetModel(object Id)
        {
            AnswerStatisticsInfo model = null;

			StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,UserId,QuestionSubjectId,PaperId,IsTrue,Integral,LastUpdatedDate 
			                   from AnswerStatistics
							   where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new AnswerStatisticsInfo();
                        model.Id = reader.GetGuid(0);
model.UserId = reader.GetGuid(1);
model.QuestionSubjectId = reader.GetGuid(2);
model.PaperId = reader.GetGuid(3);
model.IsTrue = reader.GetBoolean(4);
model.Integral = reader.GetInt32(5);
model.LastUpdatedDate = reader.GetDateTime(6);
                    }
                }
            }

            return model;
        }

        public IList<AnswerStatisticsInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from AnswerStatistics ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

			if (totalRecords == 0) return new List<AnswerStatisticsInfo>();

			sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,UserId,QuestionSubjectId,PaperId,IsTrue,Integral,LastUpdatedDate
					  from AnswerStatistics ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
			sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<AnswerStatisticsInfo> list = new List<AnswerStatisticsInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AnswerStatisticsInfo model = new AnswerStatisticsInfo();
                        model.Id = reader.GetGuid(1);
model.UserId = reader.GetGuid(2);
model.QuestionSubjectId = reader.GetGuid(3);
model.PaperId = reader.GetGuid(4);
model.IsTrue = reader.GetBoolean(5);
model.Integral = reader.GetInt32(6);
model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AnswerStatisticsInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,UserId,QuestionSubjectId,PaperId,IsTrue,Integral,LastUpdatedDate
					   from AnswerStatistics ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<AnswerStatisticsInfo> list = new List<AnswerStatisticsInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AnswerStatisticsInfo model = new AnswerStatisticsInfo();
                        model.Id = reader.GetGuid(1);
model.UserId = reader.GetGuid(2);
model.QuestionSubjectId = reader.GetGuid(3);
model.PaperId = reader.GetGuid(4);
model.IsTrue = reader.GetBoolean(5);
model.Integral = reader.GetInt32(6);
model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AnswerStatisticsInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,UserId,QuestionSubjectId,PaperId,IsTrue,Integral,LastUpdatedDate
                        from AnswerStatistics ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<AnswerStatisticsInfo> list = new List<AnswerStatisticsInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AnswerStatisticsInfo model = new AnswerStatisticsInfo();
                        model.Id = reader.GetGuid(0);
model.UserId = reader.GetGuid(1);
model.QuestionSubjectId = reader.GetGuid(2);
model.PaperId = reader.GetGuid(3);
model.IsTrue = reader.GetBoolean(4);
model.Integral = reader.GetInt32(5);
model.LastUpdatedDate = reader.GetDateTime(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AnswerStatisticsInfo> GetList()
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,UserId,QuestionSubjectId,PaperId,IsTrue,Integral,LastUpdatedDate 
			            from AnswerStatistics
					    order by LastUpdatedDate desc ");

            IList<AnswerStatisticsInfo> list = new List<AnswerStatisticsInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AnswerStatisticsInfo model = new AnswerStatisticsInfo();
                        model.Id = reader.GetGuid(0);
model.UserId = reader.GetGuid(1);
model.QuestionSubjectId = reader.GetGuid(2);
model.PaperId = reader.GetGuid(3);
model.IsTrue = reader.GetBoolean(4);
model.Integral = reader.GetInt32(5);
model.LastUpdatedDate = reader.GetDateTime(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
