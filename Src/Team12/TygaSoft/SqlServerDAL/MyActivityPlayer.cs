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

        public Guid InsertByOutput(ActivityPlayerInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ActivityPlayer (ActivityId,Named,HeadPicture,DetailInformation,ActualVoteCount,UpdateVoteCount,Remark,IsDisable,LastUpdatedDate)
			            output inserted.Id values
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

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return Guid.Parse(obj.ToString());

            return Guid.Empty;
        }

        public int UpdateVoteCount(object Id, int count)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("select VoteMultiple from ActivitySubject ASj,ActivityPlayer AP where ASj.Id = AP.ActivityId and AP.Id = @Id");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());
            int voteMultiple = 1;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        voteMultiple = reader.GetInt32(0);
                    }
                }
            }

            sb.Clear();
            sb.AppendFormat(@"update ActivityPlayer set ActualVoteCount = ActualVoteCount + {0},UpdateVoteCount = UpdateVoteCount + {1} where Id = @Id", count, voteMultiple * count);

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                   };
            parms[0].Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public DataSet GetModelOW(object Id)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 ASj.MaxVoteCount,ASj.VoteMultiple,AP.Id,AP.ActivityId,Named,HeadPicture,DetailInformation,ActualVoteCount,UpdateVoteCount,AP.Remark,AP.IsDisable,AP.LastUpdatedDate,
                        P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
			            from ActivitySubject ASj,ActivityPlayer AP left join 
                        (select AP.PlayerId,CP.* from PlayerPicture AP,ActivityPlayerPhotoPicture CP where AP.PictureId=CP.Id) as P on AP.Id=P.PlayerId
						where ASj.Id=AP.ActivityId and AP.Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ActivityPlayer ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by AP.LastUpdatedDate desc) as RowNumber,
			          ASj.Title,AP.Id,AP.ActivityId,AP.Named,HeadPicture,DetailInformation,ActualVoteCount,UpdateVoteCount,AP.Remark,AP.IsDisable,AP.LastUpdatedDate,
                      P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
					  from ActivitySubject ASj,ActivityPlayer AP left join 
                      (select AP.PlayerId,CP.* from PlayerPicture AP,ActivityPlayerPhotoPicture CP where AP.PictureId=CP.Id) as P on AP.Id=P.PlayerId");
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

            sb.Append(@"select * from(select row_number() over(order by UpdateVoteCount desc) as RowNumber,
			           AP.Id,AP.ActivityId,Named,HeadPicture,DetailInformation,ActualVoteCount,UpdateVoteCount,Remark,IsDisable,AP.LastUpdatedDate,
                       P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
					   from ActivityPlayer AP left join 
                      (select AP.PlayerId,CP.* from PlayerPicture AP,ActivityPlayerPhotoPicture CP where AP.PictureId=CP.Id) as P on AP.Id=P.PlayerId");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public DataSet GetListOW(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select AP.Id,AP.ActivityId,Named,HeadPicture,DetailInformation,ActualVoteCount,UpdateVoteCount,Remark,IsDisable,AP.LastUpdatedDate,
                       P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
					   from ActivityPlayer AP left join 
                      (select AP.PlayerId,CP.* from PlayerPicture AP,ActivityPlayerPhotoPicture CP where AP.PictureId=CP.Id) as P on AP.Id=P.PlayerId");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} order by UpdateVoteCount desc", sqlWhere);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        #endregion
    }
}
