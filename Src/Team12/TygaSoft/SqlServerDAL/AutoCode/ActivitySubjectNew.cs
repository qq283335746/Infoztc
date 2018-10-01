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
    public partial class ActivitySubjectNew : IActivitySubjectNew
    {
        #region IActivitySubjectNew Member

        public int Insert(ActivitySubjectNewInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ActivitySubjectNew (Title,ContentText,StartDate,EndDate,SignUpRule,MaxVoteCount,VoteMultiple,MaxSignUpCount,SignUpCount,ViewCount,VirtualSignUpCount,Sort,HiddenAttribute,IsPrize,PrizeRule,PrizeProbability,Remark,IsDisable,LastUpdatedDate)
			            values
						(@Title,@ContentText,@StartDate,@EndDate,@SignUpRule,@MaxVoteCount,@VoteMultiple,@MaxSignUpCount,@SignUpCount,@ViewCount,@VirtualSignUpCount,@Sort,@HiddenAttribute,@IsPrize,@PrizeRule,@PrizeProbability,@Remark,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@ContentText",SqlDbType.NVarChar,4000),
new SqlParameter("@StartDate",SqlDbType.DateTime),
new SqlParameter("@EndDate",SqlDbType.DateTime),
new SqlParameter("@SignUpRule",SqlDbType.NVarChar,2000),
new SqlParameter("@MaxVoteCount",SqlDbType.Int),
new SqlParameter("@VoteMultiple",SqlDbType.Int),
new SqlParameter("@MaxSignUpCount",SqlDbType.Int),
new SqlParameter("@SignUpCount",SqlDbType.Int),
new SqlParameter("@ViewCount",SqlDbType.BigInt),
new SqlParameter("@VirtualSignUpCount",SqlDbType.Int),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@HiddenAttribute",SqlDbType.NVarChar,300),
new SqlParameter("@IsPrize",SqlDbType.Bit),
new SqlParameter("@PrizeRule",SqlDbType.NVarChar,2000),
new SqlParameter("@PrizeProbability",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@InsertDate",SqlDbType.DateTime),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Title;
parms[1].Value = model.ContentText;
parms[2].Value = model.StartDate;
parms[3].Value = model.EndDate;
parms[4].Value = model.SignUpRule;
parms[5].Value = model.MaxVoteCount;
parms[6].Value = model.VoteMultiple;
parms[7].Value = model.MaxSignUpCount;
parms[8].Value = model.SignUpCount;
parms[9].Value = model.ViewCount;
parms[10].Value = model.VirtualSignUpCount;
parms[11].Value = model.Sort;
parms[12].Value = model.HiddenAttribute;
parms[13].Value = model.IsPrize;
parms[14].Value = model.PrizeRule;
parms[15].Value = model.PrizeProbability;
parms[16].Value = model.Remark;
parms[17].Value = model.IsDisable;
parms[18].Value = model.InsertDate;
parms[19].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ActivitySubjectNewInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ActivitySubjectNew set Title = @Title,ContentText = @ContentText,StartDate = @StartDate,EndDate = @EndDate,SignUpRule = @SignUpRule,MaxVoteCount = @MaxVoteCount,VoteMultiple = @VoteMultiple,MaxSignUpCount = @MaxSignUpCount,SignUpCount = @SignUpCount,ViewCount = @ViewCount,VirtualSignUpCount = @VirtualSignUpCount,Sort = @Sort,HiddenAttribute = @HiddenAttribute,IsPrize=@IsPrize,PrizeRule=@PrizeRule,PrizeProbability=@PrizeProbability,Remark = @Remark,IsDisable = @IsDisable,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@ContentText",SqlDbType.NVarChar,4000),
new SqlParameter("@StartDate",SqlDbType.DateTime),
new SqlParameter("@EndDate",SqlDbType.DateTime),
new SqlParameter("@SignUpRule",SqlDbType.NVarChar,2000),
new SqlParameter("@MaxVoteCount",SqlDbType.Int),
new SqlParameter("@VoteMultiple",SqlDbType.Int),
new SqlParameter("@MaxSignUpCount",SqlDbType.Int),
new SqlParameter("@SignUpCount",SqlDbType.Int),
new SqlParameter("@ViewCount",SqlDbType.BigInt),
new SqlParameter("@VirtualSignUpCount",SqlDbType.Int),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@HiddenAttribute",SqlDbType.NVarChar,300),
new SqlParameter("@IsPrize",SqlDbType.Bit),
new SqlParameter("@PrizeRule",SqlDbType.NVarChar,2000),
new SqlParameter("@PrizeProbability",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
parms[1].Value = model.Title;
parms[2].Value = model.ContentText;
parms[3].Value = model.StartDate;
parms[4].Value = model.EndDate;
parms[5].Value = model.SignUpRule;
parms[6].Value = model.MaxVoteCount;
parms[7].Value = model.VoteMultiple;
parms[8].Value = model.MaxSignUpCount;
parms[9].Value = model.SignUpCount;
parms[10].Value = model.ViewCount;
parms[11].Value = model.VirtualSignUpCount;
parms[12].Value = model.Sort;
parms[13].Value = model.HiddenAttribute;
parms[14].Value = model.IsPrize;
parms[15].Value = model.PrizeRule;
parms[16].Value = model.PrizeProbability;
parms[17].Value = model.Remark;
parms[18].Value = model.IsDisable;
parms[19].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ActivitySubjectNew where Id = @Id");
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
                sb.Append(@"delete from ActivitySubjectNew where Id = @Id" + n + " ;");
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

        public ActivitySubjectNewInfo GetModel(object Id)
        {
            ActivitySubjectNewInfo model = null;

			StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,Title,ContentText,StartDate,EndDate,SignUpRule,MaxVoteCount,VoteMultiple,MaxSignUpCount,SignUpCount,ViewCount,VirtualSignUpCount,Sort,HiddenAttribute,Remark,IsDisable,InsertDate,LastUpdatedDate 
			            from ActivitySubjectNew
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ActivitySubjectNewInfo();
                        model.Id = reader.GetGuid(0);
model.Title = reader.GetString(1);
model.ContentText = reader.GetString(2);
model.StartDate = reader.GetDateTime(3);
model.EndDate = reader.GetDateTime(4);
model.SignUpRule = reader.GetString(5);
model.MaxVoteCount = reader.GetInt32(6);
model.VoteMultiple = reader.GetInt32(7);
model.MaxSignUpCount = reader.GetInt32(8);
model.SignUpCount = reader.GetInt32(9);
model.ViewCount = reader.GetInt64(10);
model.VirtualSignUpCount = reader.GetInt32(11);
model.Sort = reader.GetInt32(12);
model.HiddenAttribute = reader.GetString(13);
model.Remark = reader.GetString(14);
model.IsDisable = reader.GetBoolean(15);
model.InsertDate = reader.GetDateTime(16);
model.LastUpdatedDate = reader.GetDateTime(17);
                    }
                }
            }

            return model;
        }

        public IList<ActivitySubjectNewInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ActivitySubjectNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

			if (totalRecords == 0) return new List<ActivitySubjectNewInfo>();

			sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,Title,ContentText,StartDate,EndDate,SignUpRule,MaxVoteCount,VoteMultiple,MaxSignUpCount,SignUpCount,ViewCount,VirtualSignUpCount,Sort,HiddenAttribute,Remark,IsDisable,InsertDate,LastUpdatedDate
					  from ActivitySubjectNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
			sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivitySubjectNewInfo> list = new List<ActivitySubjectNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivitySubjectNewInfo model = new ActivitySubjectNewInfo();
                        model.Id = reader.GetGuid(1);
model.Title = reader.GetString(2);
model.ContentText = reader.GetString(3);
model.StartDate = reader.GetDateTime(4);
model.EndDate = reader.GetDateTime(5);
model.SignUpRule = reader.GetString(6);
model.MaxVoteCount = reader.GetInt32(7);
model.VoteMultiple = reader.GetInt32(8);
model.MaxSignUpCount = reader.GetInt32(9);
model.SignUpCount = reader.GetInt32(10);
model.ViewCount = reader.GetInt64(11);
model.VirtualSignUpCount = reader.GetInt32(12);
model.Sort = reader.GetInt32(13);
model.HiddenAttribute = reader.GetString(14);
model.Remark = reader.GetString(15);
model.IsDisable = reader.GetBoolean(16);
model.InsertDate = reader.GetDateTime(17);
model.LastUpdatedDate = reader.GetDateTime(18);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivitySubjectNewInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,Title,ContentText,StartDate,EndDate,SignUpRule,MaxVoteCount,VoteMultiple,MaxSignUpCount,SignUpCount,ViewCount,VirtualSignUpCount,Sort,HiddenAttribute,Remark,IsDisable,InsertDate,LastUpdatedDate
					   from ActivitySubjectNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivitySubjectNewInfo> list = new List<ActivitySubjectNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivitySubjectNewInfo model = new ActivitySubjectNewInfo();
                        model.Id = reader.GetGuid(1);
model.Title = reader.GetString(2);
model.ContentText = reader.GetString(3);
model.StartDate = reader.GetDateTime(4);
model.EndDate = reader.GetDateTime(5);
model.SignUpRule = reader.GetString(6);
model.MaxVoteCount = reader.GetInt32(7);
model.VoteMultiple = reader.GetInt32(8);
model.MaxSignUpCount = reader.GetInt32(9);
model.SignUpCount = reader.GetInt32(10);
model.ViewCount = reader.GetInt64(11);
model.VirtualSignUpCount = reader.GetInt32(12);
model.Sort = reader.GetInt32(13);
model.HiddenAttribute = reader.GetString(14);
model.Remark = reader.GetString(15);
model.IsDisable = reader.GetBoolean(16);
model.InsertDate = reader.GetDateTime(17);
model.LastUpdatedDate = reader.GetDateTime(18);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivitySubjectNewInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,ContentText,StartDate,EndDate,SignUpRule,MaxVoteCount,VoteMultiple,MaxSignUpCount,SignUpCount,ViewCount,VirtualSignUpCount,Sort,HiddenAttribute,Remark,IsDisable,InsertDate,LastUpdatedDate
                        from ActivitySubjectNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ActivitySubjectNewInfo> list = new List<ActivitySubjectNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivitySubjectNewInfo model = new ActivitySubjectNewInfo();
                        model.Id = reader.GetGuid(0);
model.Title = reader.GetString(1);
model.ContentText = reader.GetString(2);
model.StartDate = reader.GetDateTime(3);
model.EndDate = reader.GetDateTime(4);
model.SignUpRule = reader.GetString(5);
model.MaxVoteCount = reader.GetInt32(6);
model.VoteMultiple = reader.GetInt32(7);
model.MaxSignUpCount = reader.GetInt32(8);
model.SignUpCount = reader.GetInt32(9);
model.ViewCount = reader.GetInt64(10);
model.VirtualSignUpCount = reader.GetInt32(11);
model.Sort = reader.GetInt32(12);
model.HiddenAttribute = reader.GetString(13);
model.Remark = reader.GetString(14);
model.IsDisable = reader.GetBoolean(15);
model.InsertDate = reader.GetDateTime(16);
model.LastUpdatedDate = reader.GetDateTime(17);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivitySubjectNewInfo> GetList()
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,ContentText,StartDate,EndDate,SignUpRule,MaxVoteCount,VoteMultiple,MaxSignUpCount,SignUpCount,ViewCount,VirtualSignUpCount,Sort,HiddenAttribute,Remark,IsDisable,InsertDate,LastUpdatedDate 
			            from ActivitySubjectNew
					    order by LastUpdatedDate desc ");

            IList<ActivitySubjectNewInfo> list = new List<ActivitySubjectNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivitySubjectNewInfo model = new ActivitySubjectNewInfo();
                        model.Id = reader.GetGuid(0);
model.Title = reader.GetString(1);
model.ContentText = reader.GetString(2);
model.StartDate = reader.GetDateTime(3);
model.EndDate = reader.GetDateTime(4);
model.SignUpRule = reader.GetString(5);
model.MaxVoteCount = reader.GetInt32(6);
model.VoteMultiple = reader.GetInt32(7);
model.MaxSignUpCount = reader.GetInt32(8);
model.SignUpCount = reader.GetInt32(9);
model.ViewCount = reader.GetInt64(10);
model.VirtualSignUpCount = reader.GetInt32(11);
model.Sort = reader.GetInt32(12);
model.HiddenAttribute = reader.GetString(13);
model.Remark = reader.GetString(14);
model.IsDisable = reader.GetBoolean(15);
model.InsertDate = reader.GetDateTime(16);
model.LastUpdatedDate = reader.GetDateTime(17);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
