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
    public partial class ActivityPrize : IActivityPrize
    {
        #region IActivityPrize Member

        public int Insert(ActivityPrizeInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ActivityPrize (ActivityId,PrizeName,PrizeCount,PrizeContent,Sort,BusinessName,BusinessPhone,BusinessAddress,WinningTimes,UpdateWinningTimes,Remark,IsDisable,LastUpdatedDate)
			            values
						(@ActivityId,@PrizeName,@PrizeCount,@PrizeContent,@Sort,@BusinessName,@BusinessPhone,@BusinessAddress,@WinningTimes,@UpdateWinningTimes,@Remark,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ActivityId",SqlDbType.UniqueIdentifier),
new SqlParameter("@PrizeName",SqlDbType.NVarChar,50),
new SqlParameter("@PrizeCount",SqlDbType.Int),
new SqlParameter("@PrizeContent",SqlDbType.NVarChar,300),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@BusinessName",SqlDbType.NVarChar,50),
new SqlParameter("@BusinessPhone",SqlDbType.NVarChar,20),
new SqlParameter("@BusinessAddress",SqlDbType.NVarChar,80),
new SqlParameter("@WinningTimes",SqlDbType.Int),
new SqlParameter("@UpdateWinningTimes",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.ActivityId;
            parms[1].Value = model.PrizeName;
            parms[2].Value = model.PrizeCount;
            parms[3].Value = model.PrizeContent;
            parms[4].Value = model.Sort;
            parms[5].Value = model.BusinessName;
            parms[6].Value = model.BusinessPhone;
            parms[7].Value = model.BusinessAddress;
            parms[8].Value = model.WinningTimes;
            parms[9].Value = model.UpdateWinningTimes;
            parms[10].Value = model.Remark;
            parms[11].Value = model.IsDisable;
            parms[12].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ActivityPrizeInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ActivityPrize set ActivityId = @ActivityId,PrizeName = @PrizeName,PrizeCount = @PrizeCount,PrizeContent = @PrizeContent,Sort = @Sort,BusinessName = @BusinessName,BusinessPhone = @BusinessPhone,BusinessAddress = @BusinessAddress,WinningTimes = @WinningTimes,Remark = @Remark,IsDisable = @IsDisable,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@ActivityId",SqlDbType.UniqueIdentifier),
new SqlParameter("@PrizeName",SqlDbType.NVarChar,50),
new SqlParameter("@PrizeCount",SqlDbType.Int),
new SqlParameter("@PrizeContent",SqlDbType.NVarChar,300),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@BusinessName",SqlDbType.NVarChar,50),
new SqlParameter("@BusinessPhone",SqlDbType.NVarChar,20),
new SqlParameter("@BusinessAddress",SqlDbType.NVarChar,80),
new SqlParameter("@WinningTimes",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.ActivityId;
            parms[2].Value = model.PrizeName;
            parms[3].Value = model.PrizeCount;
            parms[4].Value = model.PrizeContent;
            parms[5].Value = model.Sort;
            parms[6].Value = model.BusinessName;
            parms[7].Value = model.BusinessPhone;
            parms[8].Value = model.BusinessAddress;
            parms[9].Value = model.WinningTimes;
            parms[10].Value = model.Remark;
            parms[11].Value = model.IsDisable;
            parms[12].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ActivityPrize where Id = @Id");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

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
                sb.Append(@"delete from ActivityPrize where Id = @Id" + n + " ;");
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

        public ActivityPrizeInfo GetModel(object Id)
        {
            ActivityPrizeInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,ActivityId,PrizeName,PrizeCount,PrizeContent,Sort,BusinessName,BusinessPhone,BusinessAddress,WinningTimes,Remark,IsDisable,LastUpdatedDate 
			            from ActivityPrize
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new ActivityPrizeInfo();
                        model.Id = reader.GetGuid(0);
                        model.ActivityId = reader.GetGuid(1);
                        model.PrizeName = reader.GetString(2);
                        model.PrizeCount = reader.GetInt32(3);
                        model.PrizeContent = reader.GetString(4);
                        model.Sort = reader.GetInt32(5);
                        model.BusinessName = reader.GetString(6);
                        model.BusinessPhone = reader.GetString(7);
                        model.BusinessAddress = reader.GetString(8);
                        model.WinningTimes = reader.GetInt32(9);
                        model.Remark = reader.GetString(10);
                        model.IsDisable = reader.GetBoolean(11);
                        model.LastUpdatedDate = reader.GetDateTime(12);
                    }
                }
            }

            return model;
        }

        public IList<ActivityPrizeInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ActivityPrize ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ActivityPrizeInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,ActivityId,PrizeName,PrizeCount,PrizeContent,Sort,BusinessName,BusinessPhone,BusinessAddress,WinningTimes,Remark,IsDisable,LastUpdatedDate
					  from ActivityPrize ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivityPrizeInfo> list = new List<ActivityPrizeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPrizeInfo model = new ActivityPrizeInfo();
                        model.Id = reader.GetGuid(1);
                        model.ActivityId = reader.GetGuid(2);
                        model.PrizeName = reader.GetString(3);
                        model.PrizeCount = reader.GetInt32(4);
                        model.PrizeContent = reader.GetString(5);
                        model.Sort = reader.GetInt32(6);
                        model.BusinessName = reader.GetString(7);
                        model.BusinessPhone = reader.GetString(8);
                        model.BusinessAddress = reader.GetString(9);
                        model.WinningTimes = reader.GetInt32(10);
                        model.Remark = reader.GetString(11);
                        model.IsDisable = reader.GetBoolean(12);
                        model.LastUpdatedDate = reader.GetDateTime(13);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityPrizeInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,ActivityId,PrizeName,PrizeCount,PrizeContent,Sort,BusinessName,BusinessPhone,BusinessAddress,WinningTimes,Remark,IsDisable,LastUpdatedDate
					   from ActivityPrize ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivityPrizeInfo> list = new List<ActivityPrizeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPrizeInfo model = new ActivityPrizeInfo();
                        model.Id = reader.GetGuid(1);
                        model.ActivityId = reader.GetGuid(2);
                        model.PrizeName = reader.GetString(3);
                        model.PrizeCount = reader.GetInt32(4);
                        model.PrizeContent = reader.GetString(5);
                        model.Sort = reader.GetInt32(6);
                        model.BusinessName = reader.GetString(7);
                        model.BusinessPhone = reader.GetString(8);
                        model.BusinessAddress = reader.GetString(9);
                        model.WinningTimes = reader.GetInt32(10);
                        model.Remark = reader.GetString(11);
                        model.IsDisable = reader.GetBoolean(12);
                        model.LastUpdatedDate = reader.GetDateTime(13);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityPrizeInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ActivityId,PrizeName,PrizeCount,PrizeContent,Sort,BusinessName,BusinessPhone,BusinessAddress,WinningTimes,Remark,IsDisable,LastUpdatedDate
                        from ActivityPrize ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ActivityPrizeInfo> list = new List<ActivityPrizeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPrizeInfo model = new ActivityPrizeInfo();
                        model.Id = reader.GetGuid(0);
                        model.ActivityId = reader.GetGuid(1);
                        model.PrizeName = reader.GetString(2);
                        model.PrizeCount = reader.GetInt32(3);
                        model.PrizeContent = reader.GetString(4);
                        model.Sort = reader.GetInt32(5);
                        model.BusinessName = reader.GetString(6);
                        model.BusinessPhone = reader.GetString(7);
                        model.BusinessAddress = reader.GetString(8);
                        model.WinningTimes = reader.GetInt32(9);
                        model.Remark = reader.GetString(10);
                        model.IsDisable = reader.GetBoolean(11);
                        model.LastUpdatedDate = reader.GetDateTime(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityPrizeInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ActivityId,PrizeName,PrizeCount,PrizeContent,Sort,BusinessName,BusinessPhone,BusinessAddress,WinningTimes,Remark,IsDisable,LastUpdatedDate 
			            from ActivityPrize
					    order by LastUpdatedDate desc ");

            IList<ActivityPrizeInfo> list = new List<ActivityPrizeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPrizeInfo model = new ActivityPrizeInfo();
                        model.Id = reader.GetGuid(0);
                        model.ActivityId = reader.GetGuid(1);
                        model.PrizeName = reader.GetString(2);
                        model.PrizeCount = reader.GetInt32(3);
                        model.PrizeContent = reader.GetString(4);
                        model.Sort = reader.GetInt32(5);
                        model.BusinessName = reader.GetString(6);
                        model.BusinessPhone = reader.GetString(7);
                        model.BusinessAddress = reader.GetString(8);
                        model.WinningTimes = reader.GetInt32(9);
                        model.Remark = reader.GetString(10);
                        model.IsDisable = reader.GetBoolean(11);
                        model.LastUpdatedDate = reader.GetDateTime(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
