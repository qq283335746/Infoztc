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
    public partial class ActivityPlayer : IActivityPlayer
    {
        #region IActivityPlayer Member

        public int Insert(ActivityPlayerInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ActivityPlayer (ActivityId,Named,HeadPicture,DetailInformation,ActualVoteCount,UpdateVoteCount,Remark,IsDisable,LastUpdatedDate)
			            values
						(@ActivityId,@Named,@HeadPicture,@DetailInformation,@ActualVoteCount,@UpdateVoteCount,@Remark,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ActivityId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Named",SqlDbType.NVarChar,50),
new SqlParameter("@HeadPicture",SqlDbType.VarChar,100),
new SqlParameter("@DetailInformation",SqlDbType.NVarChar,4000),
new SqlParameter("@ActualVoteCount",SqlDbType.Int),
new SqlParameter("@UpdateVoteCount",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.ActivityId;
parms[1].Value = model.Named;
parms[2].Value = model.HeadPicture;
parms[3].Value = model.DetailInformation;
parms[4].Value = model.ActualVoteCount;
parms[5].Value = model.UpdateVoteCount;
parms[6].Value = model.Remark;
parms[7].Value = model.IsDisable;
parms[8].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ActivityPlayerInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ActivityPlayer set ActivityId = @ActivityId,Named = @Named,HeadPicture = @HeadPicture,DetailInformation = @DetailInformation,ActualVoteCount = @ActualVoteCount,UpdateVoteCount = @UpdateVoteCount,Remark = @Remark,IsDisable = @IsDisable,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@ActivityId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Named",SqlDbType.NVarChar,50),
new SqlParameter("@HeadPicture",SqlDbType.VarChar,100),
new SqlParameter("@DetailInformation",SqlDbType.NVarChar,4000),
new SqlParameter("@ActualVoteCount",SqlDbType.Int),
new SqlParameter("@UpdateVoteCount",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
parms[1].Value = model.ActivityId;
parms[2].Value = model.Named;
parms[3].Value = model.HeadPicture;
parms[4].Value = model.DetailInformation;
parms[5].Value = model.ActualVoteCount;
parms[6].Value = model.UpdateVoteCount;
parms[7].Value = model.Remark;
parms[8].Value = model.IsDisable;
parms[9].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ActivityPlayer where Id = @Id");
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
                sb.Append(@"delete from ActivityPlayer where Id = @Id" + n + " ;");
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

        public ActivityPlayerInfo GetModel(object Id)
        {
            ActivityPlayerInfo model = null;

			StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,ActivityId,Named,HeadPicture,DetailInformation,ActualVoteCount,UpdateVoteCount,Remark,IsDisable,LastUpdatedDate 
			            from ActivityPlayer
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ActivityPlayerInfo();
                        model.Id = reader.GetGuid(0);
model.ActivityId = reader.GetGuid(1);
model.Named = reader.GetString(2);
model.HeadPicture = reader.GetString(3);
model.DetailInformation = reader.GetString(4);
model.ActualVoteCount = reader.GetInt32(5);
model.UpdateVoteCount = reader.GetInt32(6);
model.Remark = reader.GetString(7);
model.IsDisable = reader.GetBoolean(8);
model.LastUpdatedDate = reader.GetDateTime(9);
                    }
                }
            }

            return model;
        }

        public IList<ActivityPlayerInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ActivityPlayer ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

			if (totalRecords == 0) return new List<ActivityPlayerInfo>();

			sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,ActivityId,Named,HeadPicture,DetailInformation,ActualVoteCount,UpdateVoteCount,Remark,IsDisable,LastUpdatedDate
					  from ActivityPlayer ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
			sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivityPlayerInfo> list = new List<ActivityPlayerInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPlayerInfo model = new ActivityPlayerInfo();
                        model.Id = reader.GetGuid(1);
model.ActivityId = reader.GetGuid(2);
model.Named = reader.GetString(3);
model.HeadPicture = reader.GetString(4);
model.DetailInformation = reader.GetString(5);
model.ActualVoteCount = reader.GetInt32(6);
model.UpdateVoteCount = reader.GetInt32(7);
model.Remark = reader.GetString(8);
model.IsDisable = reader.GetBoolean(9);
model.LastUpdatedDate = reader.GetDateTime(10);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityPlayerInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,ActivityId,Named,HeadPicture,DetailInformation,ActualVoteCount,UpdateVoteCount,Remark,IsDisable,LastUpdatedDate
					   from ActivityPlayer ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivityPlayerInfo> list = new List<ActivityPlayerInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPlayerInfo model = new ActivityPlayerInfo();
                        model.Id = reader.GetGuid(1);
model.ActivityId = reader.GetGuid(2);
model.Named = reader.GetString(3);
model.HeadPicture = reader.GetString(4);
model.DetailInformation = reader.GetString(5);
model.ActualVoteCount = reader.GetInt32(6);
model.UpdateVoteCount = reader.GetInt32(7);
model.Remark = reader.GetString(8);
model.IsDisable = reader.GetBoolean(9);
model.LastUpdatedDate = reader.GetDateTime(10);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityPlayerInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ActivityId,Named,HeadPicture,DetailInformation,ActualVoteCount,UpdateVoteCount,Remark,IsDisable,LastUpdatedDate
                        from ActivityPlayer ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ActivityPlayerInfo> list = new List<ActivityPlayerInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPlayerInfo model = new ActivityPlayerInfo();
                        model.Id = reader.GetGuid(0);
model.ActivityId = reader.GetGuid(1);
model.Named = reader.GetString(2);
model.HeadPicture = reader.GetString(3);
model.DetailInformation = reader.GetString(4);
model.ActualVoteCount = reader.GetInt32(5);
model.UpdateVoteCount = reader.GetInt32(6);
model.Remark = reader.GetString(7);
model.IsDisable = reader.GetBoolean(8);
model.LastUpdatedDate = reader.GetDateTime(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityPlayerInfo> GetList()
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ActivityId,Named,HeadPicture,DetailInformation,ActualVoteCount,UpdateVoteCount,Remark,IsDisable,LastUpdatedDate 
			            from ActivityPlayer
					    order by LastUpdatedDate desc ");

            IList<ActivityPlayerInfo> list = new List<ActivityPlayerInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPlayerInfo model = new ActivityPlayerInfo();
                        model.Id = reader.GetGuid(0);
model.ActivityId = reader.GetGuid(1);
model.Named = reader.GetString(2);
model.HeadPicture = reader.GetString(3);
model.DetailInformation = reader.GetString(4);
model.ActualVoteCount = reader.GetInt32(5);
model.UpdateVoteCount = reader.GetInt32(6);
model.Remark = reader.GetString(7);
model.IsDisable = reader.GetBoolean(8);
model.LastUpdatedDate = reader.GetDateTime(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
