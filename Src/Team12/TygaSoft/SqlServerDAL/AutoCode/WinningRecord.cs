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
    public partial class WinningRecord : IWinningRecord
    {
        #region IWinningRecord Member

        public int Insert(WinningRecordInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into WinningRecord (ActivityId,ActivityPrizeId,UserId,UserFlag,MobilePhone,Status,Remark,LastUpdatedDate)
			            values
						(@ActivityId,@ActivityPrizeId,@UserId,@UserFlag,@MobilePhone,@Status,@Remark,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ActivityId",SqlDbType.UniqueIdentifier),
new SqlParameter("@ActivityPrizeId",SqlDbType.UniqueIdentifier),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@UserFlag",SqlDbType.VarChar,50),
new SqlParameter("@MobilePhone",SqlDbType.VarChar,20),
new SqlParameter("@Status",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.ActivityId;
            parms[1].Value = model.ActivityPrizeId;
            parms[2].Value = model.UserId;
            parms[3].Value = model.UserFlag;
            parms[4].Value = model.MobilePhone;
            parms[5].Value = model.Status;
            parms[6].Value = model.Remark;
            parms[7].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(WinningRecordInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update WinningRecord set ActivityId = @ActivityId,ActivityPrizeId = @ActivityPrizeId,UserId = @UserId,Status = @Status,Remark = @Remark,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@ActivityId",SqlDbType.UniqueIdentifier),
new SqlParameter("@ActivityPrizeId",SqlDbType.UniqueIdentifier),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Status",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.ActivityId;
            parms[2].Value = model.ActivityPrizeId;
            parms[3].Value = model.UserId;
            parms[4].Value = model.Status;
            parms[5].Value = model.Remark;
            parms[6].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from WinningRecord where Id = @Id");
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
                sb.Append(@"delete from WinningRecord where Id = @Id" + n + " ;");
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

        public WinningRecordInfo GetModel(object Id)
        {
            WinningRecordInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,ActivityId,ActivityPrizeId,UserId,Status,Remark,LastUpdatedDate 
			            from WinningRecord
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new WinningRecordInfo();
                        model.Id = reader.GetGuid(0);
                        model.ActivityId = reader.GetGuid(1);
                        model.ActivityPrizeId = reader.GetGuid(2);
                        model.UserId = reader.GetGuid(3);
                        model.Status = reader.GetInt32(4);
                        model.Remark = reader.GetString(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);
                    }
                }
            }

            return model;
        }

        public IList<WinningRecordInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from WinningRecord ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<WinningRecordInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,ActivityId,ActivityPrizeId,UserId,Status,Remark,LastUpdatedDate
					  from WinningRecord ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<WinningRecordInfo> list = new List<WinningRecordInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        WinningRecordInfo model = new WinningRecordInfo();
                        model.Id = reader.GetGuid(1);
                        model.ActivityId = reader.GetGuid(2);
                        model.ActivityPrizeId = reader.GetGuid(3);
                        model.UserId = reader.GetGuid(4);
                        model.Status = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<WinningRecordInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,ActivityId,ActivityPrizeId,UserId,Status,Remark,LastUpdatedDate
					   from WinningRecord ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<WinningRecordInfo> list = new List<WinningRecordInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        WinningRecordInfo model = new WinningRecordInfo();
                        model.Id = reader.GetGuid(1);
                        model.ActivityId = reader.GetGuid(2);
                        model.ActivityPrizeId = reader.GetGuid(3);
                        model.UserId = reader.GetGuid(4);
                        model.Status = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<WinningRecordInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ActivityId,ActivityPrizeId,UserId,Status,Remark,LastUpdatedDate
                        from WinningRecord ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<WinningRecordInfo> list = new List<WinningRecordInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        WinningRecordInfo model = new WinningRecordInfo();
                        model.Id = reader.GetGuid(0);
                        model.ActivityId = reader.GetGuid(1);
                        model.ActivityPrizeId = reader.GetGuid(2);
                        model.UserId = reader.GetGuid(3);
                        model.Status = reader.GetInt32(4);
                        model.Remark = reader.GetString(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<WinningRecordInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ActivityId,ActivityPrizeId,UserId,Status,Remark,LastUpdatedDate 
			            from WinningRecord
					    order by LastUpdatedDate desc ");

            IList<WinningRecordInfo> list = new List<WinningRecordInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        WinningRecordInfo model = new WinningRecordInfo();
                        model.Id = reader.GetGuid(0);
                        model.ActivityId = reader.GetGuid(1);
                        model.ActivityPrizeId = reader.GetGuid(2);
                        model.UserId = reader.GetGuid(3);
                        model.Status = reader.GetInt32(4);
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
