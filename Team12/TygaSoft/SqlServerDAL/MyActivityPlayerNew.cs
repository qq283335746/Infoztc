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
    public partial class ActivityPlayerNew : IActivityPlayerNew
    {
        #region IActivityPlayer Member

        public Guid InsertByOutput(ActivityPlayerNewInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("select Max(No) as MaxNo from ActivityPlayerNew where ActivityId=@ActivityId");
            SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
            parm.Value = model.ActivityId;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        model.No = reader["MaxNo"] is DBNull ? 1 : reader.GetInt32(0) + 1;
                    }
                }
            }

            sb.Clear();
            sb.Append(@"insert into ActivityPlayerNew (ActivityId,UserId,No,Named,Age,Occupation,Phone,Location,Professional,Descr,VoteCount,VirtualVoteCount,Remark,IsDisable,LastUpdatedDate)
			            output inserted.Id values
						(@ActivityId,@UserId,@No,@Named,@Age,@Occupation,@Phone,@Location,@Professional,@Descr,@VoteCount,@VirtualVoteCount,@Remark,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ActivityId",SqlDbType.UniqueIdentifier),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@No",SqlDbType.Int),
new SqlParameter("@Named",SqlDbType.NVarChar,30),
new SqlParameter("@Age",SqlDbType.Int),
new SqlParameter("@Occupation",SqlDbType.NVarChar,50),
new SqlParameter("@Phone",SqlDbType.NVarChar,20),
new SqlParameter("@Location",SqlDbType.NVarChar,80),
new SqlParameter("@Professional",SqlDbType.NVarChar,50),
new SqlParameter("@Descr",SqlDbType.NVarChar,1000),
new SqlParameter("@VoteCount",SqlDbType.Int),
new SqlParameter("@VirtualVoteCount",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.ActivityId;
            parms[1].Value = model.UserId;
            parms[2].Value = model.No;
            parms[3].Value = model.Named;
            parms[4].Value = model.Age;
            parms[5].Value = model.Occupation;
            parms[6].Value = model.Phone;
            parms[7].Value = model.Location;
            parms[8].Value = model.Professional;
            parms[9].Value = model.Descr;
            parms[10].Value = model.VoteCount;
            parms[11].Value = model.VirtualVoteCount;
            parms[12].Value = model.Remark;
            parms[13].Value = model.IsDisable;
            parms[14].Value = model.LastUpdatedDate;

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return Guid.Parse(obj.ToString());

            return Guid.Empty;
        }

        public int UpdateVoteCount(object Id)
        {
            StringBuilder sb = new StringBuilder(250);

            sb.Append(@"update ActivityPlayerNew set VoteCount = VoteCount + 1,VirtualVoteCount = VirtualVoteCount + 1 where Id = @Id");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                   };
            parms[0].Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public DataSet GetModelOW(object Id)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 ASj.MaxVoteCount,ASj.HiddenAttribute,AP.Id,AP.ActivityId,UserId,No,Named,Age,Occupation,Phone,Location,Professional,Descr,VoteCount,VirtualVoteCount,
                        AP.Remark,AP.IsDisable,AP.LastUpdatedDate,P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
			            from ActivitySubjectNew ASj,ActivityPlayerNew AP left join 
                        (select AP.PlayerId,CP.* from PlayerPictureNew AP,ActivityPlayerPhotoPicture CP where AP.PictureId=CP.Id and IsHeadImg=1) as P on AP.Id=P.PlayerId
						where ASj.Id=AP.ActivityId and AP.Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ActivityPlayerNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by AP.LastUpdatedDate desc) as RowNumber,
			          ASj.Title,AP.Id,AP.ActivityId,UserId,No,Named,Age,Occupation,Phone,Location,Professional,Descr,VoteCount,VirtualVoteCount,
                      AP.Remark,AP.IsDisable,AP.LastUpdatedDate,P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
			          from ActivitySubjectNew ASj,ActivityPlayerNew AP left join 
                      (select AP.PlayerId,CP.* from PlayerPictureNew AP,ActivityPlayerPhotoPicture CP where AP.PictureId=CP.Id and IsHeadImg=1) as P on AP.Id=P.PlayerId");
            sb.AppendFormat(" where ASj.Id=AP.ActivityId {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by VirtualVoteCount desc,No asc) as RowNumber,
			           AP.Id,AP.ActivityId,UserId,No,Named,Age,Occupation,Phone,Location,Professional,Descr,VoteCount,VirtualVoteCount,
                       AP.Remark,AP.IsDisable,AP.LastUpdatedDate,P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
			           from ActivitySubjectNew ASj,ActivityPlayerNew AP left join 
                       (select AP.PlayerId,CP.* from PlayerPictureNew AP,ActivityPlayerPhotoPicture CP where AP.PictureId=CP.Id and IsHeadImg=1) as P on AP.Id=P.PlayerId");
            sb.AppendFormat(" where ASj.Id=AP.ActivityId {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public bool IsExist(Guid activityId, string phone)
        {
            SqlParameter[] parms = {
                                       new SqlParameter("@ActivityId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@Phone",SqlDbType.NVarChar, 20)
                                   };
            parms[0].Value = activityId;
            parms[1].Value = phone;

            string cmdText = "select 1 from ActivityPlayerNew where ActivityId=@ActivityId and Phone = @Phone";

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, cmdText, parms);
            if (obj != null) return true;

            return false;
        }
        #endregion
    }
}
