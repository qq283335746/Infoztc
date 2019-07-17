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
    public partial class Profiles : IProfiles
    {
        #region IProfiles Member

        public int Insert(ProfilesInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Profiles (Username,AppName,IsAnonymous,LastActivityDate,LastUpdatedDate)
			            values
						(@Username,@AppName,@IsAnonymous,@LastActivityDate,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Username",SqlDbType.NVarChar,50),
new SqlParameter("@AppName",SqlDbType.NVarChar,50),
new SqlParameter("@IsAnonymous",SqlDbType.Bit),
new SqlParameter("@LastActivityDate",SqlDbType.DateTime),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Username;
            parms[1].Value = model.AppName;
            parms[2].Value = model.IsAnonymous;
            parms[3].Value = model.LastActivityDate;
            parms[4].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ProfilesInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update Profiles set Username = @Username,AppName = @AppName,IsAnonymous = @IsAnonymous,LastActivityDate = @LastActivityDate,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@Username",SqlDbType.NVarChar,50),
new SqlParameter("@AppName",SqlDbType.NVarChar,50),
new SqlParameter("@IsAnonymous",SqlDbType.Bit),
new SqlParameter("@LastActivityDate",SqlDbType.DateTime),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.Username;
            parms[2].Value = model.AppName;
            parms[3].Value = model.IsAnonymous;
            parms[4].Value = model.LastActivityDate;
            parms[5].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from Profiles where Id = @Id");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm);
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
                sb.Append(@"delete from Profiles where Id = @Id" + n + " ;");
                SqlParameter parm = new SqlParameter("@Id" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(item);
                parms.Add(parm);
            }
            using (SqlConnection conn = new SqlConnection(SqlHelper.HnztcShopDbConnString))
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

        public ProfilesInfo GetModel(object Id)
        {
            ProfilesInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,Username,AppName,IsAnonymous,LastActivityDate,LastUpdatedDate 
			            from Profiles
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new ProfilesInfo();
                        model.Id = reader.GetGuid(0);
                        model.Username = reader.GetString(1);
                        model.AppName = reader.GetString(2);
                        model.IsAnonymous = reader.GetBoolean(3);
                        model.LastActivityDate = reader.GetDateTime(4);
                        model.LastUpdatedDate = reader.GetDateTime(5);
                    }
                }
            }

            return model;
        }

        public IList<ProfilesInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Profiles ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ProfilesInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,Username,AppName,IsAnonymous,LastActivityDate,LastUpdatedDate
					  from Profiles ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProfilesInfo> list = new List<ProfilesInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProfilesInfo model = new ProfilesInfo();
                        model.Id = reader.GetGuid(1);
                        model.Username = reader.GetString(2);
                        model.AppName = reader.GetString(3);
                        model.IsAnonymous = reader.GetBoolean(4);
                        model.LastActivityDate = reader.GetDateTime(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProfilesInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,Username,AppName,IsAnonymous,LastActivityDate,LastUpdatedDate
					   from Profiles ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProfilesInfo> list = new List<ProfilesInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProfilesInfo model = new ProfilesInfo();
                        model.Id = reader.GetGuid(1);
                        model.Username = reader.GetString(2);
                        model.AppName = reader.GetString(3);
                        model.IsAnonymous = reader.GetBoolean(4);
                        model.LastActivityDate = reader.GetDateTime(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProfilesInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Username,AppName,IsAnonymous,LastActivityDate,LastUpdatedDate
                        from Profiles ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ProfilesInfo> list = new List<ProfilesInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProfilesInfo model = new ProfilesInfo();
                        model.Id = reader.GetGuid(0);
                        model.Username = reader.GetString(1);
                        model.AppName = reader.GetString(2);
                        model.IsAnonymous = reader.GetBoolean(3);
                        model.LastActivityDate = reader.GetDateTime(4);
                        model.LastUpdatedDate = reader.GetDateTime(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProfilesInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Username,AppName,IsAnonymous,LastActivityDate,LastUpdatedDate 
			            from Profiles
					    order by LastUpdatedDate desc ");

            IList<ProfilesInfo> list = new List<ProfilesInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProfilesInfo model = new ProfilesInfo();
                        model.Id = reader.GetGuid(0);
                        model.Username = reader.GetString(1);
                        model.AppName = reader.GetString(2);
                        model.IsAnonymous = reader.GetBoolean(3);
                        model.LastActivityDate = reader.GetDateTime(4);
                        model.LastUpdatedDate = reader.GetDateTime(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
