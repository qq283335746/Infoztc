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
    public partial class TopicSubject
    {
        #region ITopicSubject Member
        public Guid InsertByOutput(TopicSubjectInfo model)
        {
            StringBuilder sb = new StringBuilder(300);

            sb.Append(@"insert into TopicSubject (UserId,Title,ContentText,IsTop,Sort,Remark,IsDisable,LastUpdatedDate)
			            output inserted.Id values
						(@UserId,@Title,@ContentText,@IsTop,@Sort,@Remark,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@ContentText",SqlDbType.NVarChar,2000),
new SqlParameter("@IsTop",SqlDbType.Bit),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.UserId;
            parms[1].Value = model.Title;
            parms[2].Value = model.ContentText;
            parms[3].Value = model.IsTop;
            parms[4].Value = model.Sort;
            parms[5].Value = model.Remark;
            parms[6].Value = model.IsDisable;
            parms[7].Value = model.LastUpdatedDate;

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return Guid.Parse(obj.ToString());

            return Guid.Empty;
        }

        public int UpdateIsTop(TopicSubjectInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update TopicSubject set IsTop = @IsTop where Id = @Id");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@IsTop",SqlDbType.Bit)
                                   };
            parms[0].Value = Guid.Parse(model.Id.ToString());
            parms[1].Value = model.IsTop;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public DataSet GetModelOW(object Id)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 TS.Id,TS.UserId,Title,ContentText,IsTop,Sort,Remark,IsDisable,TS.LastUpdatedDate,
                      P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
			          from TopicSubject TS left join 
                      (select TP.TopicSubjectId,CP.* from TopicPicture TP,CommunionPicture CP where TP.PictureId=CP.Id) as P on TS.Id=P.TopicSubjectId
					  where TS.Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from TopicSubject ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            //if (totalRecords == 0) return new DataSet();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by IsTop desc,TS.LastUpdatedDate desc) as RowNumber,
			          TS.Id,TS.UserId,Title,ContentText,IsTop,Sort,Remark,IsDisable,TS.LastUpdatedDate,
                      P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder,U.UserName
					  from TopicSubject TS left join 
                      (select TP.TopicSubjectId,CP.* from TopicPicture TP,CommunionPicture CP where TP.PictureId=CP.Id) as P on TS.Id=P.TopicSubjectId
                      left join HnztcAspnetDb.dbo.aspnet_Users U on TS.UserId=U.UserId");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by IsTop desc,TS.LastUpdatedDate desc) as RowNumber,
			           TS.Id,TS.UserId,Title,ContentText,IsTop,Sort,Remark,IsDisable,TS.LastUpdatedDate,
                       P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder,U.UserName
					   from TopicSubject TS left join 
                       (select TP.TopicSubjectId,CP.* from TopicPicture TP,CommunionPicture CP where TP.PictureId=CP.Id) as P on TS.Id=P.TopicSubjectId
                       left join HnztcAspnetDb.dbo.aspnet_Users U on TS.UserId=U.UserId");
            sb.AppendFormat(" where IsDisable=0 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }
        #endregion
    }
}
