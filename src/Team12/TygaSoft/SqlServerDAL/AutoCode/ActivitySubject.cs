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
    public partial class ActivitySubject : IActivitySubject
    {
        #region IActivitySubject Member

        public int Insert(ActivitySubjectInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ActivitySubject (Title,ContentText,StartDate,EndDate,ActivityType,MaxVoteCount,VoteMultiple,MaxSignUpCount,ActualSignUpCount,UpdateSignUpCount,Sort,Remark,IsDisable,InsertDate,LastUpdatedDate)
			            values
						(@Title,@ContentText,@StartDate,@EndDate,@ActivityType,@MaxVoteCount,@VoteMultiple,@MaxSignUpCount,@ActualSignUpCount,@UpdateSignUpCount,@Sort,@Remark,@IsDisable,@InsertDate,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@ContentText",SqlDbType.NVarChar,2000),
new SqlParameter("@StartDate",SqlDbType.DateTime),
new SqlParameter("@EndDate",SqlDbType.DateTime),
new SqlParameter("@ActivityType",SqlDbType.TinyInt),
new SqlParameter("@MaxVoteCount",SqlDbType.Int),
new SqlParameter("@VoteMultiple",SqlDbType.Int),
new SqlParameter("@MaxSignUpCount",SqlDbType.Int),
new SqlParameter("@ActualSignUpCount",SqlDbType.Int),
new SqlParameter("@UpdateSignUpCount",SqlDbType.Int),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@InsertDate",SqlDbType.DateTime),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Title;
            parms[1].Value = model.ContentText;
            parms[2].Value = model.StartDate;
            parms[3].Value = model.EndDate;
            parms[4].Value = model.ActivityType;
            parms[5].Value = model.MaxVoteCount;
            parms[6].Value = model.VoteMultiple;
            parms[7].Value = model.MaxSignUpCount;
            parms[8].Value = model.ActualSignUpCount;
            parms[9].Value = model.UpdateSignUpCount;
            parms[10].Value = model.Sort;
            parms[11].Value = model.Remark;
            parms[12].Value = model.IsDisable;
            parms[13].Value = model.InsertDate;
            parms[14].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ActivitySubjectInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ActivitySubject set Title = @Title,ContentText = @ContentText,StartDate = @StartDate,EndDate = @EndDate,ActivityType = @ActivityType,MaxVoteCount = @MaxVoteCount,VoteMultiple = @VoteMultiple,MaxSignUpCount = @MaxSignUpCount,ActualSignUpCount = @ActualSignUpCount,UpdateSignUpCount = @UpdateSignUpCount,Sort = @Sort,Remark = @Remark,IsDisable = @IsDisable,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@ContentText",SqlDbType.NVarChar,2000),
new SqlParameter("@StartDate",SqlDbType.DateTime),
new SqlParameter("@EndDate",SqlDbType.DateTime),
new SqlParameter("@ActivityType",SqlDbType.TinyInt),
new SqlParameter("@MaxVoteCount",SqlDbType.Int),
new SqlParameter("@VoteMultiple",SqlDbType.Int),
new SqlParameter("@MaxSignUpCount",SqlDbType.Int),
new SqlParameter("@ActualSignUpCount",SqlDbType.Int),
new SqlParameter("@UpdateSignUpCount",SqlDbType.Int),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.Title;
            parms[2].Value = model.ContentText;
            parms[3].Value = model.StartDate;
            parms[4].Value = model.EndDate;
            parms[5].Value = model.ActivityType;
            parms[6].Value = model.MaxVoteCount;
            parms[7].Value = model.VoteMultiple;
            parms[8].Value = model.MaxSignUpCount;
            parms[9].Value = model.ActualSignUpCount;
            parms[10].Value = model.UpdateSignUpCount;
            parms[11].Value = model.Sort;
            parms[12].Value = model.Remark;
            parms[13].Value = model.IsDisable;
            parms[14].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ActivitySubject where Id = @Id");
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
                sb.Append(@"delete from ActivitySubject where Id = @Id" + n + " ;");
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

        public ActivitySubjectInfo GetModel(object Id)
        {
            ActivitySubjectInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,Title,ContentText,StartDate,EndDate,ActivityType,MaxVoteCount,VoteMultiple,MaxSignUpCount,ActualSignUpCount,UpdateSignUpCount,Sort,Remark,IsDisable,InsertDate,LastUpdatedDate 
			            from ActivitySubject
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ActivitySubjectInfo();
                        model.Id = reader.GetGuid(0);
                        model.Title = reader.GetString(1);
                        model.ContentText = reader.GetString(2);
                        model.StartDate = reader.GetDateTime(3);
                        model.EndDate = reader.GetDateTime(4);
                        model.ActivityType = reader.GetByte(5);
                        model.MaxVoteCount = reader.GetInt32(6);
                        model.VoteMultiple = reader.GetInt32(7);
                        model.MaxSignUpCount = reader.GetInt32(8);
                        model.ActualSignUpCount = reader.GetInt32(9);
                        model.UpdateSignUpCount = reader.GetInt32(10);
                        model.Sort = reader.GetInt32(11);
                        model.Remark = reader.GetString(12);
                        model.IsDisable = reader.GetBoolean(13);
                        model.InsertDate = reader.GetDateTime(14);
                        model.LastUpdatedDate = reader.GetDateTime(15);
                    }
                }
            }

            return model;
        }

        public IList<ActivitySubjectInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ActivitySubject ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ActivitySubjectInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,Title,ContentText,StartDate,EndDate,ActivityType,MaxVoteCount,VoteMultiple,MaxSignUpCount,ActualSignUpCount,UpdateSignUpCount,Sort,Remark,IsDisable,InsertDate,LastUpdatedDate
					  from ActivitySubject ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivitySubjectInfo> list = new List<ActivitySubjectInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivitySubjectInfo model = new ActivitySubjectInfo();
                        model.Id = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.ContentText = reader.GetString(3);
                        model.StartDate = reader.GetDateTime(4);
                        model.EndDate = reader.GetDateTime(5);
                        model.ActivityType = reader.GetByte(6);
                        model.MaxVoteCount = reader.GetInt32(7);
                        model.VoteMultiple = reader.GetInt32(8);
                        model.MaxSignUpCount = reader.GetInt32(9);
                        model.ActualSignUpCount = reader.GetInt32(10);
                        model.UpdateSignUpCount = reader.GetInt32(11);
                        model.Sort = reader.GetInt32(12);
                        model.Remark = reader.GetString(13);
                        model.IsDisable = reader.GetBoolean(14);
                        model.InsertDate = reader.GetDateTime(15);
                        model.LastUpdatedDate = reader.GetDateTime(16);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivitySubjectInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,Title,ContentText,StartDate,EndDate,ActivityType,MaxVoteCount,VoteMultiple,MaxSignUpCount,ActualSignUpCount,UpdateSignUpCount,Sort,Remark,IsDisable,InsertDate,LastUpdatedDate
					   from ActivitySubject ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivitySubjectInfo> list = new List<ActivitySubjectInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivitySubjectInfo model = new ActivitySubjectInfo();
                        model.Id = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.ContentText = reader.GetString(3);
                        model.StartDate = reader.GetDateTime(4);
                        model.EndDate = reader.GetDateTime(5);
                        model.ActivityType = reader.GetByte(6);
                        model.MaxVoteCount = reader.GetInt32(7);
                        model.VoteMultiple = reader.GetInt32(8);
                        model.MaxSignUpCount = reader.GetInt32(9);
                        model.ActualSignUpCount = reader.GetInt32(10);
                        model.UpdateSignUpCount = reader.GetInt32(11);
                        model.Sort = reader.GetInt32(12);
                        model.Remark = reader.GetString(13);
                        model.IsDisable = reader.GetBoolean(14);
                        model.InsertDate = reader.GetDateTime(15);
                        model.LastUpdatedDate = reader.GetDateTime(16);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivitySubjectInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,ContentText,StartDate,EndDate,ActivityType,MaxVoteCount,VoteMultiple,MaxSignUpCount,ActualSignUpCount,UpdateSignUpCount,Sort,Remark,IsDisable,InsertDate,LastUpdatedDate
                        from ActivitySubject ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ActivitySubjectInfo> list = new List<ActivitySubjectInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivitySubjectInfo model = new ActivitySubjectInfo();
                        model.Id = reader.GetGuid(0);
                        model.Title = reader.GetString(1);
                        model.ContentText = reader.GetString(2);
                        model.StartDate = reader.GetDateTime(3);
                        model.EndDate = reader.GetDateTime(4);
                        model.ActivityType = reader.GetByte(5);
                        model.MaxVoteCount = reader.GetInt32(6);
                        model.VoteMultiple = reader.GetInt32(7);
                        model.MaxSignUpCount = reader.GetInt32(8);
                        model.ActualSignUpCount = reader.GetInt32(9);
                        model.UpdateSignUpCount = reader.GetInt32(10);
                        model.Sort = reader.GetInt32(11);
                        model.Remark = reader.GetString(12);
                        model.IsDisable = reader.GetBoolean(13);
                        model.InsertDate = reader.GetDateTime(14);
                        model.LastUpdatedDate = reader.GetDateTime(15);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivitySubjectInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,ContentText,StartDate,EndDate,ActivityType,MaxVoteCount,VoteMultiple,MaxSignUpCount,ActualSignUpCount,UpdateSignUpCount,Sort,Remark,IsDisable,InsertDate,LastUpdatedDate 
			            from ActivitySubject
					    order by LastUpdatedDate desc ");

            IList<ActivitySubjectInfo> list = new List<ActivitySubjectInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivitySubjectInfo model = new ActivitySubjectInfo();
                        model.Id = reader.GetGuid(0);
                        model.Title = reader.GetString(1);
                        model.ContentText = reader.GetString(2);
                        model.StartDate = reader.GetDateTime(3);
                        model.EndDate = reader.GetDateTime(4);
                        model.ActivityType = reader.GetByte(5);
                        model.MaxVoteCount = reader.GetInt32(6);
                        model.VoteMultiple = reader.GetInt32(7);
                        model.MaxSignUpCount = reader.GetInt32(8);
                        model.ActualSignUpCount = reader.GetInt32(9);
                        model.UpdateSignUpCount = reader.GetInt32(10);
                        model.Sort = reader.GetInt32(11);
                        model.Remark = reader.GetString(12);
                        model.IsDisable = reader.GetBoolean(13);
                        model.InsertDate = reader.GetDateTime(14);
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
