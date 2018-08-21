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
    public partial class PushUser : IPushUser
    {
        #region IPushUser Member

        public int Insert(PushUserInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into PushUser (PushUser)
			            values
						(@PushUser)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@PushUser",SqlDbType.VarChar)
                                   };
            parms[0].Value = model.PushUser;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(PushUserInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update PushUser set PushUser = @PushUser 
			            where PushId = @PushId
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@PushId",SqlDbType.UniqueIdentifier),
new SqlParameter("@PushUser",SqlDbType.VarChar)
                                   };
            parms[0].Value = model.PushId;
            parms[1].Value = model.PushUser;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object PushId)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from PushUser where PushId = @PushId");
            SqlParameter parm = new SqlParameter("@PushId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(PushId.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);
        }

        public bool DeleteBatch(IList<object> list)
        {
            bool result = false;
            StringBuilder sb = new StringBuilder(500);
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from PushUser where PushId = @PushId" + n + " ;");
                SqlParameter parm = new SqlParameter("@PushId" + n + "", SqlDbType.UniqueIdentifier);
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

        public PushUserInfo GetModel(object PushId)
        {
            PushUserInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 PushId,PushUser 
			            from PushUser
						where PushId = @PushId ");
            SqlParameter parm = new SqlParameter("@PushId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(PushId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new PushUserInfo();
                        model.PushId = reader.GetGuid(0);
                        model.PushUser = reader.GetString(1);
                    }
                }
            }

            return model;
        }

        public IList<PushUserInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from PushUser ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<PushUserInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          PushId,PushUser
					  from PushUser ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<PushUserInfo> list = new List<PushUserInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PushUserInfo model = new PushUserInfo();
                        model.PushId = reader.GetGuid(1);
                        model.PushUser = reader.GetString(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<PushUserInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           PushId,PushUser
					   from PushUser ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<PushUserInfo> list = new List<PushUserInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PushUserInfo model = new PushUserInfo();
                        model.PushId = reader.GetGuid(1);
                        model.PushUser = reader.GetString(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<PushUserInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select PushId,PushUser
                        from PushUser ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<PushUserInfo> list = new List<PushUserInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PushUserInfo model = new PushUserInfo();
                        model.PushId = reader.GetGuid(0);
                        model.PushUser = reader.GetString(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<PushUserInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select PushId,PushUser 
			            from PushUser
					    order by LastUpdatedDate desc ");

            IList<PushUserInfo> list = new List<PushUserInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PushUserInfo model = new PushUserInfo();
                        model.PushId = reader.GetGuid(0);
                        model.PushUser = reader.GetString(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
