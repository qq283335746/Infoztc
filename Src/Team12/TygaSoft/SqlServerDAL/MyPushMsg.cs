using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TygaSoft.DBUtility;
using TygaSoft.IDAL;
using TygaSoft.Model;

namespace TygaSoft.SqlServerDAL
{
    public partial class PushMsg
    {
        #region PushMsg Member

        public Guid InsertByOutput(PushMsgInfo model)
        {
            StringBuilder sb = new StringBuilder(250);

            sb.Append(@"insert into PushMsg (Title,PushContent,PushType,IsSendOk,SendRange,LastUpdatedDate)
			            output inserted.Id values
						(@Title,@PushContent,@PushType,@IsSendOk,@SendRange,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@PushContent",SqlDbType.NVarChar,1000),
new SqlParameter("@PushType",SqlDbType.NVarChar,10),
new SqlParameter("@IsSendOk",SqlDbType.Bit),
new SqlParameter("@SendRange",SqlDbType.NVarChar,10),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };

            parms[0].Value = model.Title;
            parms[1].Value = model.PushContent;
            parms[2].Value = model.PushType;
            parms[3].Value = model.IsSendOk;
            parms[4].Value = model.SendRange;
            parms[5].Value = model.LastUpdatedDate;

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return Guid.Parse(obj.ToString());

            return Guid.Empty;
        }

        public DataSet GetModelOW(object Id)
        {
            StringBuilder sb = new StringBuilder(300);

            sb.Append(@"select top 1 Id,Title,PushContent,PushType,IsSendOk,SendRange,LastUpdatedDate 
			            from PushMsg
						where Id = @Id ");

            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from PushMsg ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,Title,PushContent,PushType,IsSendOk,SendRange,LastUpdatedDate
					  from PushMsg ");

            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,Title,PushContent,PushType,IsSendOk,SendRange,LastUpdatedDate
					  from PushMsg ");

            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public DataSet GetUserList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from HnztcAspnetDb.dbo.aspnet_Users ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by UserId desc) as RowNumber,
			          UserId,UserName,LoweredUserName
					  from HnztcAspnetDb.dbo.aspnet_Users ");

            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public DataSet GetUserList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by UserId desc) as RowNumber,
			          UserId,UserName,LoweredUserName
					  from HnztcAspnetDb.dbo.aspnet_Users ");

            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }
        #endregion
    }
}
