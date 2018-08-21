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
    public partial class Ernie : IErnie
    {
        #region IErnie Member

        public int Insert(ErnieInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Ernie (Id,StartTime,EndTime,UserBetMaxCount,IsOver,IsDisable,LastUpdatedDate)
			            values
						(@Id,@StartTime,@EndTime,@UserBetMaxCount,@IsOver,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@StartTime",SqlDbType.DateTime),
                                        new SqlParameter("@EndTime",SqlDbType.DateTime),
                                        new SqlParameter("@UserBetMaxCount",SqlDbType.Int),
                                        new SqlParameter("@IsOver",SqlDbType.Bit),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.StartTime;
            parms[2].Value = model.EndTime;
            parms[3].Value = model.UserBetMaxCount;
            parms[4].Value = model.IsOver;
            parms[5].Value = model.IsDisable;
            parms[6].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ErnieInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update Ernie set StartTime = @StartTime,EndTime = @EndTime,UserBetMaxCount = @UserBetMaxCount,IsOver = @IsOver,IsDisable = @IsDisable,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                        new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@StartTime",SqlDbType.DateTime),
                                        new SqlParameter("@EndTime",SqlDbType.DateTime),
                                        new SqlParameter("@UserBetMaxCount",SqlDbType.Int),
                                        new SqlParameter("@IsOver",SqlDbType.Bit),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.StartTime;
            parms[2].Value = model.EndTime;
            parms[3].Value = model.UserBetMaxCount;
            parms[4].Value = model.IsOver;
            parms[5].Value = model.IsDisable;
            parms[6].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from Ernie where Id = @Id");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);
        }

        public bool DeleteBatch(IList<object> list)
        {
            if (list == null || list.Count == 0) return false;

            bool result = false;
            StringBuilder sb = new StringBuilder(1000);
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from Ernie where Id = @Id" + n + " ;");
                sb.Append(@"delete from ErnieItem where ErnieId = @Id" + n + " ;");
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

        public ErnieInfo GetModel(object Id)
        {
            ErnieInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,StartTime,EndTime,UserBetMaxCount,IsOver,IsDisable,LastUpdatedDate 
			            from Ernie
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ErnieInfo();
                        model.Id = reader.GetGuid(0);
                        model.StartTime = reader.GetDateTime(1);
                        model.EndTime = reader.GetDateTime(2);
                        model.UserBetMaxCount = reader.GetInt32(3);
                        model.IsOver = reader.GetBoolean(4);
                        model.IsDisable = reader.GetBoolean(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);
                    }
                }
            }

            return model;
        }

        public IList<ErnieInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Ernie ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ErnieInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,StartTime,EndTime,UserBetMaxCount,IsOver,IsDisable,LastUpdatedDate
					  from Ernie ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ErnieInfo> list = new List<ErnieInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ErnieInfo model = new ErnieInfo();
                        model.Id = reader.GetGuid(1);
                        model.StartTime = reader.GetDateTime(2);
                        model.EndTime = reader.GetDateTime(3);
                        model.UserBetMaxCount = reader.GetInt32(4);
                        model.IsOver = reader.GetBoolean(5);
                        model.IsDisable = reader.GetBoolean(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ErnieInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,StartTime,EndTime,UserBetMaxCount,IsOver,IsDisable,LastUpdatedDate
					   from Ernie ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ErnieInfo> list = new List<ErnieInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ErnieInfo model = new ErnieInfo();
                        model.Id = reader.GetGuid(1);
                        model.StartTime = reader.GetDateTime(2);
                        model.EndTime = reader.GetDateTime(3);
                        model.UserBetMaxCount = reader.GetInt32(4);
                        model.IsOver = reader.GetBoolean(5);
                        model.IsDisable = reader.GetBoolean(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ErnieInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,StartTime,EndTime,UserBetMaxCount,IsOver,IsDisable,LastUpdatedDate
                        from Ernie ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ErnieInfo> list = new List<ErnieInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ErnieInfo model = new ErnieInfo();
                        model.Id = reader.GetGuid(0);
                        model.StartTime = reader.GetDateTime(1);
                        model.EndTime = reader.GetDateTime(2);
                        model.UserBetMaxCount = reader.GetInt32(3);
                        model.IsOver = reader.GetBoolean(4);
                        model.IsDisable = reader.GetBoolean(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ErnieInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,StartTime,EndTime,UserBetMaxCount,IsOver,IsDisable,LastUpdatedDate 
			            from Ernie
					    order by LastUpdatedDate desc ");

            IList<ErnieInfo> list = new List<ErnieInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ErnieInfo model = new ErnieInfo();
                        model.Id = reader.GetGuid(0);
                        model.StartTime = reader.GetDateTime(1);
                        model.EndTime = reader.GetDateTime(2);
                        model.UserBetMaxCount = reader.GetInt32(3);
                        model.IsOver = reader.GetBoolean(4);
                        model.IsDisable = reader.GetBoolean(5);
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
