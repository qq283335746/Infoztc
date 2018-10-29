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
    public partial class VoteToPlayerNew : IVoteToPlayerNew
    {
        #region IVoteToPlayerNew Member

        public int Insert(VoteToPlayerNewInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into VoteToPlayerNew (PlayerId,UserId,UserFlag,TotalVoteCount,Remark,LastUpdatedDate)
			            values
						(@PlayerId,@UserId,@UserFlag,@TotalVoteCount,@Remark,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@PlayerId",SqlDbType.UniqueIdentifier),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@UserFlag",SqlDbType.VarChar,50),
new SqlParameter("@TotalVoteCount",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.PlayerId;
parms[1].Value = model.UserId;
parms[2].Value = model.UserFlag;
parms[3].Value = model.TotalVoteCount;
parms[4].Value = model.Remark;
parms[5].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(VoteToPlayerNewInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update VoteToPlayerNew set PlayerId = @PlayerId,UserId = @UserId,UserFlag = @UserFlag,TotalVoteCount = @TotalVoteCount,Remark = @Remark,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@PlayerId",SqlDbType.UniqueIdentifier),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@UserFlag",SqlDbType.VarChar,50),
new SqlParameter("@TotalVoteCount",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
parms[1].Value = model.PlayerId;
parms[2].Value = model.UserId;
parms[3].Value = model.UserFlag;
parms[4].Value = model.TotalVoteCount;
parms[5].Value = model.Remark;
parms[6].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from VoteToPlayerNew where Id = @Id");
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
                sb.Append(@"delete from VoteToPlayerNew where Id = @Id" + n + " ;");
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

        public VoteToPlayerNewInfo GetModel(object Id)
        {
            VoteToPlayerNewInfo model = null;

			StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,PlayerId,UserId,UserFlag,TotalVoteCount,Remark,LastUpdatedDate 
			            from VoteToPlayerNew
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new VoteToPlayerNewInfo();
                        model.Id = reader.GetGuid(0);
model.PlayerId = reader.GetGuid(1);
model.UserId = reader.GetGuid(2);
model.UserFlag = reader.GetString(3);
model.TotalVoteCount = reader.GetInt32(4);
model.Remark = reader.GetString(5);
model.LastUpdatedDate = reader.GetDateTime(6);
                    }
                }
            }

            return model;
        }

        public IList<VoteToPlayerNewInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from VoteToPlayerNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

			if (totalRecords == 0) return new List<VoteToPlayerNewInfo>();

			sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,PlayerId,UserId,UserFlag,TotalVoteCount,Remark,LastUpdatedDate
					  from VoteToPlayerNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
			sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<VoteToPlayerNewInfo> list = new List<VoteToPlayerNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        VoteToPlayerNewInfo model = new VoteToPlayerNewInfo();
                        model.Id = reader.GetGuid(1);
model.PlayerId = reader.GetGuid(2);
model.UserId = reader.GetGuid(3);
model.UserFlag = reader.GetString(4);
model.TotalVoteCount = reader.GetInt32(5);
model.Remark = reader.GetString(6);
model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<VoteToPlayerNewInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,PlayerId,UserId,UserFlag,TotalVoteCount,Remark,LastUpdatedDate
					   from VoteToPlayerNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<VoteToPlayerNewInfo> list = new List<VoteToPlayerNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        VoteToPlayerNewInfo model = new VoteToPlayerNewInfo();
                        model.Id = reader.GetGuid(1);
model.PlayerId = reader.GetGuid(2);
model.UserId = reader.GetGuid(3);
model.UserFlag = reader.GetString(4);
model.TotalVoteCount = reader.GetInt32(5);
model.Remark = reader.GetString(6);
model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<VoteToPlayerNewInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,PlayerId,UserId,UserFlag,TotalVoteCount,Remark,LastUpdatedDate
                        from VoteToPlayerNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<VoteToPlayerNewInfo> list = new List<VoteToPlayerNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        VoteToPlayerNewInfo model = new VoteToPlayerNewInfo();
                        model.Id = reader.GetGuid(0);
model.PlayerId = reader.GetGuid(1);
model.UserId = reader.GetGuid(2);
model.UserFlag = reader.GetString(3);
model.TotalVoteCount = reader.GetInt32(4);
model.Remark = reader.GetString(5);
model.LastUpdatedDate = reader.GetDateTime(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<VoteToPlayerNewInfo> GetList()
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,PlayerId,UserId,UserFlag,TotalVoteCount,Remark,LastUpdatedDate 
			            from VoteToPlayerNew
					    order by LastUpdatedDate desc ");

            IList<VoteToPlayerNewInfo> list = new List<VoteToPlayerNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        VoteToPlayerNewInfo model = new VoteToPlayerNewInfo();
                        model.Id = reader.GetGuid(0);
model.PlayerId = reader.GetGuid(1);
model.UserId = reader.GetGuid(2);
model.UserFlag = reader.GetString(3);
model.TotalVoteCount = reader.GetInt32(4);
model.Remark = reader.GetString(5);
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
