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
    public partial class QuestionBank : IQuestionBank
    {
        #region IQuestionBank Member

        public int Insert(QuestionBankInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into QuestionBank (Named,Remark,IsDisable,LastUpdatedDate)
			            values
						(@Named,@Remark,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Named",SqlDbType.NVarChar,100),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Named;
parms[1].Value = model.Remark;
parms[2].Value = model.IsDisable;
parms[3].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(QuestionBankInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update QuestionBank set Named = @Named,Remark = @Remark,IsDisable = @IsDisable,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@Named",SqlDbType.NVarChar,100),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
parms[1].Value = model.Named;
parms[2].Value = model.Remark;
parms[3].Value = model.IsDisable;
parms[4].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from QuestionBank where Id = @Id");
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
                sb.Append(@"delete from QuestionBank where Id = @Id" + n + " ;");
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

        public QuestionBankInfo GetModel(object Id)
        {
            QuestionBankInfo model = null;

			StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,Named,Remark,IsDisable,LastUpdatedDate 
			                   from QuestionBank
							   where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new QuestionBankInfo();
                        model.Id = reader.GetGuid(0);
model.Named = reader.GetString(1);
model.Remark = reader.GetString(2);
model.IsDisable = reader.GetBoolean(3);
model.LastUpdatedDate = reader.GetDateTime(4);
                    }
                }
            }

            return model;
        }

        public IList<QuestionBankInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from QuestionBank ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

			if (totalRecords == 0) return new List<QuestionBankInfo>();

			sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,Named,Remark,IsDisable,LastUpdatedDate
					  from QuestionBank ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
			sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<QuestionBankInfo> list = new List<QuestionBankInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        QuestionBankInfo model = new QuestionBankInfo();
                        model.Id = reader.GetGuid(1);
model.Named = reader.GetString(2);
model.Remark = reader.GetString(3);
model.IsDisable = reader.GetBoolean(4);
model.LastUpdatedDate = reader.GetDateTime(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<QuestionBankInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,Named,Remark,IsDisable,LastUpdatedDate
					   from QuestionBank ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<QuestionBankInfo> list = new List<QuestionBankInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        QuestionBankInfo model = new QuestionBankInfo();
                        model.Id = reader.GetGuid(1);
model.Named = reader.GetString(2);
model.Remark = reader.GetString(3);
model.IsDisable = reader.GetBoolean(4);
model.LastUpdatedDate = reader.GetDateTime(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<QuestionBankInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Named,Remark,IsDisable,LastUpdatedDate
                        from QuestionBank ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<QuestionBankInfo> list = new List<QuestionBankInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        QuestionBankInfo model = new QuestionBankInfo();
                        model.Id = reader.GetGuid(0);
model.Named = reader.GetString(1);
model.Remark = reader.GetString(2);
model.IsDisable = reader.GetBoolean(3);
model.LastUpdatedDate = reader.GetDateTime(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<QuestionBankInfo> GetList()
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Named,Remark,IsDisable,LastUpdatedDate 
			            from QuestionBank
					    order by LastUpdatedDate desc ");

            IList<QuestionBankInfo> list = new List<QuestionBankInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        QuestionBankInfo model = new QuestionBankInfo();
                        model.Id = reader.GetGuid(0);
model.Named = reader.GetString(1);
model.Remark = reader.GetString(2);
model.IsDisable = reader.GetBoolean(3);
model.LastUpdatedDate = reader.GetDateTime(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
