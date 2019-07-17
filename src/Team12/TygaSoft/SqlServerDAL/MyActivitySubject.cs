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

        public Guid InsertByOutput(ActivitySubjectInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ActivitySubject (Title,ContentText,StartDate,EndDate,ActivityType,MaxVoteCount,VoteMultiple,MaxSignUpCount,ActualSignUpCount,UpdateSignUpCount,Sort,Remark,IsDisable,InsertDate,LastUpdatedDate)
			            output inserted.Id values
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

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return Guid.Parse(obj.ToString());

            return Guid.Empty;
        }

        public int UpdateSignUpCount(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ActivitySubject set ActualSignUpCount = ActualSignUpCount + 1,UpdateSignUpCount = UpdateSignUpCount + 1 where Id = @Id");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                   };
            parms[0].Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public DataSet GetModelOW(object Id)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 ASj.Id,Title,ContentText,StartDate,EndDate,ActivityType,MaxVoteCount,VoteMultiple,MaxSignUpCount,ActualSignUpCount,UpdateSignUpCount,Sort,Remark,IsDisable,
                        InsertDate,ASj.LastUpdatedDate,P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
			            from ActivitySubject ASj left join 
                        (select AP.ActivityId,CP.* from ActivityPicture AP,ActivityPhotoPicture CP where AP.PictureId=CP.Id) as P on ASj.Id=P.ActivityId
						where ASj.Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ActivitySubject ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by Sort asc, InsertDate desc) as RowNumber,
			          ASj.Id,Title,ContentText,StartDate,EndDate,ActivityType,MaxVoteCount,VoteMultiple,MaxSignUpCount,ActualSignUpCount,UpdateSignUpCount,Sort,Remark,IsDisable,InsertDate,ASj.LastUpdatedDate
					  from ActivitySubject ASj left join 
                      (select AP.ActivityId,CP.* from ActivityPicture AP,ActivityPhotoPicture CP where AP.PictureId=CP.Id) as P on ASj.Id=P.ActivityId");
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

            sb.Append(@"select * from(select row_number() over(order by Sort asc, InsertDate desc) as RowNumber,
			           ASj.Id,Title,ContentText,StartDate,EndDate,ActivityType,MaxVoteCount,VoteMultiple,MaxSignUpCount,ActualSignUpCount,UpdateSignUpCount,Sort,Remark,IsDisable,
                       InsertDate,ASj.LastUpdatedDate,P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
					   from ActivitySubject ASj left join 
                       (select AP.ActivityId,CP.* from ActivityPicture AP,ActivityPhotoPicture CP where AP.PictureId=CP.Id) as P on ASj.Id=P.ActivityId");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        #endregion
    }
}
