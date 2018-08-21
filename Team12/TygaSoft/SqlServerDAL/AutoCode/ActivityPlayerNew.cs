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
    public partial class ActivityPlayerNew : IActivityPlayerNew
    {
        #region IActivityPlayerNew Member

        public int Insert(ActivityPlayerNewInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ActivityPlayerNew (ActivityId,UserId,No,Named,Age,Occupation,Phone,Location,Professional,Descr,VoteCount,VirtualVoteCount,Remark,IsDisable,LastUpdatedDate)
			            values
						(@ActivityId,@UserId,@No,@Named,@Age,@Occupation,@Phone,@Location,@Professional,@Descr,@VoteCount,@VirtualVoteCount,@Remark,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ActivityId",SqlDbType.UniqueIdentifier),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@No",SqlDbType.Int),
new SqlParameter("@Named",SqlDbType.NVarChar,30),
new SqlParameter("@Age",SqlDbType.Int),
new SqlParameter("@Occupation",SqlDbType.NVarChar,50),
new SqlParameter("@Phone",SqlDbType.NVarChar,20),
new SqlParameter("@Location",SqlDbType.NVarChar,80),
new SqlParameter("@Professional",SqlDbType.NVarChar,50),
new SqlParameter("@Descr",SqlDbType.NVarChar,1000),
new SqlParameter("@VoteCount",SqlDbType.Int),
new SqlParameter("@VirtualVoteCount",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.ActivityId;
            parms[1].Value = model.UserId;
            parms[2].Value = model.No;
            parms[3].Value = model.Named;
            parms[4].Value = model.Age;
            parms[5].Value = model.Occupation;
            parms[6].Value = model.Phone;
            parms[7].Value = model.Location;
            parms[8].Value = model.Professional;
            parms[9].Value = model.Descr;
            parms[10].Value = model.VoteCount;
            parms[11].Value = model.VirtualVoteCount;
            parms[12].Value = model.Remark;
            parms[13].Value = model.IsDisable;
            parms[14].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ActivityPlayerNewInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ActivityPlayerNew set ActivityId = @ActivityId,UserId = @UserId,Named = @Named,Age = @Age,Occupation = @Occupation,Phone = @Phone,Location = @Location,Professional = @Professional,Descr = @Descr,VoteCount = @VoteCount,VirtualVoteCount = @VirtualVoteCount,Remark = @Remark,IsDisable = @IsDisable,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@ActivityId",SqlDbType.UniqueIdentifier),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Named",SqlDbType.NVarChar,30),
new SqlParameter("@Age",SqlDbType.Int),
new SqlParameter("@Occupation",SqlDbType.NVarChar,50),
new SqlParameter("@Phone",SqlDbType.NVarChar,20),
new SqlParameter("@Location",SqlDbType.NVarChar,80),
new SqlParameter("@Professional",SqlDbType.NVarChar,50),
new SqlParameter("@Descr",SqlDbType.NVarChar,1000),
new SqlParameter("@VoteCount",SqlDbType.Int),
new SqlParameter("@VirtualVoteCount",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.ActivityId;
            parms[2].Value = model.UserId;
            parms[3].Value = model.Named;
            parms[4].Value = model.Age;
            parms[5].Value = model.Occupation;
            parms[6].Value = model.Phone;
            parms[7].Value = model.Location;
            parms[8].Value = model.Professional;
            parms[9].Value = model.Descr;
            parms[10].Value = model.VoteCount;
            parms[11].Value = model.VirtualVoteCount;
            parms[12].Value = model.Remark;
            parms[13].Value = model.IsDisable;
            parms[14].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ActivityPlayerNew where Id = @Id");
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
                sb.Append(@"delete from ActivityPlayerNew where Id = @Id" + n + " ;");
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

        public ActivityPlayerNewInfo GetModel(object Id)
        {
            ActivityPlayerNewInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,ActivityId,UserId,No,Named,Age,Occupation,Phone,Location,Professional,Descr,VoteCount,VirtualVoteCount,Remark,IsDisable,LastUpdatedDate 
			            from ActivityPlayerNew
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ActivityPlayerNewInfo();
                        model.Id = reader.GetGuid(0);
                        model.ActivityId = reader.GetGuid(1);
                        model.UserId = reader.GetGuid(2);
                        model.No = reader.GetInt32(3);
                        model.Named = reader.GetString(4);
                        model.Age = reader.GetInt32(5);
                        model.Occupation = reader.GetString(6);
                        model.Phone = reader.GetString(7);
                        model.Location = reader.GetString(8);
                        model.Professional = reader.GetString(9);
                        model.Descr = reader.GetString(10);
                        model.VoteCount = reader.GetInt32(11);
                        model.VirtualVoteCount = reader.GetInt32(12);
                        model.Remark = reader.GetString(13);
                        model.IsDisable = reader.GetBoolean(14);
                        model.LastUpdatedDate = reader.GetDateTime(15);
                    }
                }
            }

            return model;
        }

        public IList<ActivityPlayerNewInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ActivityPlayerNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ActivityPlayerNewInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,ActivityId,UserId,No,Named,Age,Occupation,Phone,Location,Professional,Descr,VoteCount,VirtualVoteCount,Remark,IsDisable,LastUpdatedDate
					  from ActivityPlayerNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivityPlayerNewInfo> list = new List<ActivityPlayerNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPlayerNewInfo model = new ActivityPlayerNewInfo();
                        model.Id = reader.GetGuid(1);
                        model.ActivityId = reader.GetGuid(2);
                        model.UserId = reader.GetGuid(3);
                        model.Age = reader.GetInt32(4);
                        model.Named = reader.GetString(5);
                        model.Age = reader.GetInt32(6);
                        model.Occupation = reader.GetString(7);
                        model.Phone = reader.GetString(8);
                        model.Location = reader.GetString(9);
                        model.Professional = reader.GetString(10);
                        model.Descr = reader.GetString(11);
                        model.VoteCount = reader.GetInt32(12);
                        model.VirtualVoteCount = reader.GetInt32(13);
                        model.Remark = reader.GetString(14);
                        model.IsDisable = reader.GetBoolean(15);
                        model.LastUpdatedDate = reader.GetDateTime(16);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityPlayerNewInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,ActivityId,UserId,No,Named,Age,Occupation,Phone,Location,Professional,Descr,VoteCount,VirtualVoteCount,Remark,IsDisable,LastUpdatedDate
					   from ActivityPlayerNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivityPlayerNewInfo> list = new List<ActivityPlayerNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPlayerNewInfo model = new ActivityPlayerNewInfo();
                        model.Id = reader.GetGuid(1);
                        model.ActivityId = reader.GetGuid(2);
                        model.UserId = reader.GetGuid(3);
                        model.Age = reader.GetInt32(4);
                        model.Named = reader.GetString(5);
                        model.Age = reader.GetInt32(6);
                        model.Occupation = reader.GetString(7);
                        model.Phone = reader.GetString(8);
                        model.Location = reader.GetString(9);
                        model.Professional = reader.GetString(10);
                        model.Descr = reader.GetString(11);
                        model.VoteCount = reader.GetInt32(12);
                        model.VirtualVoteCount = reader.GetInt32(13);
                        model.Remark = reader.GetString(14);
                        model.IsDisable = reader.GetBoolean(15);
                        model.LastUpdatedDate = reader.GetDateTime(16);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityPlayerNewInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ActivityId,UserId,No,Named,Age,Occupation,Phone,Location,Professional,Descr,VoteCount,VirtualVoteCount,Remark,IsDisable,LastUpdatedDate
                        from ActivityPlayerNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ActivityPlayerNewInfo> list = new List<ActivityPlayerNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPlayerNewInfo model = new ActivityPlayerNewInfo();
                        model.Id = reader.GetGuid(0);
                        model.ActivityId = reader.GetGuid(1);
                        model.UserId = reader.GetGuid(2);
                        model.No = reader.GetInt32(3);
                        model.Named = reader.GetString(4);
                        model.Age = reader.GetInt32(5);
                        model.Occupation = reader.GetString(6);
                        model.Phone = reader.GetString(7);
                        model.Location = reader.GetString(8);
                        model.Professional = reader.GetString(9);
                        model.Descr = reader.GetString(10);
                        model.VoteCount = reader.GetInt32(11);
                        model.VirtualVoteCount = reader.GetInt32(12);
                        model.Remark = reader.GetString(13);
                        model.IsDisable = reader.GetBoolean(14);
                        model.LastUpdatedDate = reader.GetDateTime(15);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityPlayerNewInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ActivityId,UserId,No,Named,Age,Occupation,Phone,Location,Professional,Descr,VoteCount,VirtualVoteCount,Remark,IsDisable,LastUpdatedDate 
			            from ActivityPlayerNew
					    order by LastUpdatedDate desc ");

            IList<ActivityPlayerNewInfo> list = new List<ActivityPlayerNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPlayerNewInfo model = new ActivityPlayerNewInfo();
                        model.Id = reader.GetGuid(0);
                        model.ActivityId = reader.GetGuid(1);
                        model.UserId = reader.GetGuid(2);
                        model.No = reader.GetInt32(3);
                        model.Named = reader.GetString(4);
                        model.Age = reader.GetInt32(5);
                        model.Occupation = reader.GetString(6);
                        model.Phone = reader.GetString(7);
                        model.Location = reader.GetString(8);
                        model.Professional = reader.GetString(9);
                        model.Descr = reader.GetString(10);
                        model.VoteCount = reader.GetInt32(11);
                        model.VirtualVoteCount = reader.GetInt32(12);
                        model.Remark = reader.GetString(13);
                        model.IsDisable = reader.GetBoolean(14);
                        model.LastUpdatedDate = reader.GetDateTime(15);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
