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
    public partial class UserLevelView : IUserLevelView
    {
        #region IUserLevelView Member

        public int Insert(UserLevelViewInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into UserLevelView (UserId,FunCode,EnumSource,TotalGold,TotalSilver,TotalIntegral,LastUpdatedDate)
			            values
						(@UserId,@FunCode,@EnumSource,@TotalGold,@TotalSilver,@TotalIntegral,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@FunCode",SqlDbType.Int),
                                        new SqlParameter("@EnumSource",SqlDbType.Int),
                                        new SqlParameter("@TotalGold",SqlDbType.Int),
                                        new SqlParameter("@TotalSilver",SqlDbType.Int),
                                        new SqlParameter("@TotalIntegral",SqlDbType.Int),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.UserId;
            parms[1].Value = model.FunCode;
            parms[2].Value = model.EnumSource;
            parms[3].Value = model.TotalGold;
            parms[4].Value = model.TotalSilver;
            parms[5].Value = model.TotalIntegral;
            parms[6].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(UserLevelViewInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update UserLevelView set TotalGold = @TotalGold,TotalSilver = @TotalSilver,TotalIntegral = @TotalIntegral,LastUpdatedDate = @LastUpdatedDate 
			            where UserId = @UserId and FunCode = @FunCode and EnumSource = @EnumSource
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@FunCode",SqlDbType.Int),
new SqlParameter("@EnumSource",SqlDbType.Int),
new SqlParameter("@TotalGold",SqlDbType.Int),
new SqlParameter("@TotalSilver",SqlDbType.Int),
new SqlParameter("@TotalIntegral",SqlDbType.Int),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.UserId;
            parms[1].Value = model.FunCode;
            parms[2].Value = model.EnumSource;
            parms[3].Value = model.TotalGold;
            parms[4].Value = model.TotalSilver;
            parms[5].Value = model.TotalIntegral;
            parms[6].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object UserId)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from UserLevelView where UserId = @UserId");
            SqlParameter parm = new SqlParameter("@UserId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(UserId.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm);
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
                sb.Append(@"delete from UserLevelView where UserId = @UserId" + n + " ;");
                SqlParameter parm = new SqlParameter("@UserId" + n + "", SqlDbType.UniqueIdentifier);
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

        public UserLevelViewInfo GetModel(object UserId)
        {
            UserLevelViewInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 UserId,FunCode,EnumSource,TotalGold,TotalSilver,TotalIntegral,LastUpdatedDate 
			            from UserLevelView
						where UserId = @UserId ");
            SqlParameter parm = new SqlParameter("@UserId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(UserId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new UserLevelViewInfo();
                        model.UserId = reader.GetGuid(0);
                        model.FunCode = reader.GetInt32(1);
                        model.EnumSource = reader.GetInt32(2);
                        model.TotalGold = reader.GetInt32(3);
                        model.TotalSilver = reader.GetInt32(4);
                        model.TotalIntegral = reader.GetInt32(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);
                    }
                }
            }

            return model;
        }

        public IList<UserLevelViewInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from UserLevelView ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<UserLevelViewInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          UserId,FunCode,EnumSource,TotalGold,TotalSilver,TotalIntegral,LastUpdatedDate
					  from UserLevelView ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<UserLevelViewInfo> list = new List<UserLevelViewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserLevelViewInfo model = new UserLevelViewInfo();
                        model.UserId = reader.GetGuid(1);
                        model.FunCode = reader.GetInt32(2);
                        model.EnumSource = reader.GetInt32(3);
                        model.TotalGold = reader.GetInt32(4);
                        model.TotalSilver = reader.GetInt32(5);
                        model.TotalIntegral = reader.GetInt32(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<UserLevelViewInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           UserId,FunCode,EnumSource,TotalGold,TotalSilver,TotalIntegral,LastUpdatedDate
					   from UserLevelView ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<UserLevelViewInfo> list = new List<UserLevelViewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserLevelViewInfo model = new UserLevelViewInfo();
                        model.UserId = reader.GetGuid(1);
                        model.FunCode = reader.GetInt32(2);
                        model.EnumSource = reader.GetInt32(3);
                        model.TotalGold = reader.GetInt32(4);
                        model.TotalSilver = reader.GetInt32(5);
                        model.TotalIntegral = reader.GetInt32(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<UserLevelViewInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select UserId,FunCode,EnumSource,TotalGold,TotalSilver,TotalIntegral,LastUpdatedDate
                        from UserLevelView ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<UserLevelViewInfo> list = new List<UserLevelViewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserLevelViewInfo model = new UserLevelViewInfo();
                        model.UserId = reader.GetGuid(0);
                        model.FunCode = reader.GetInt32(1);
                        model.EnumSource = reader.GetInt32(2);
                        model.TotalGold = reader.GetInt32(3);
                        model.TotalSilver = reader.GetInt32(4);
                        model.TotalIntegral = reader.GetInt32(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<UserLevelViewInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select UserId,FunCode,EnumSource,TotalGold,TotalSilver,TotalIntegral,LastUpdatedDate 
			            from UserLevelView
					    order by LastUpdatedDate desc ");

            IList<UserLevelViewInfo> list = new List<UserLevelViewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserLevelViewInfo model = new UserLevelViewInfo();
                        model.UserId = reader.GetGuid(0);
                        model.FunCode = reader.GetInt32(1);
                        model.EnumSource = reader.GetInt32(2);
                        model.TotalGold = reader.GetInt32(3);
                        model.TotalSilver = reader.GetInt32(4);
                        model.TotalIntegral = reader.GetInt32(5);
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
