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
        public int UpdateStatus(object Id, int Status)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update WinningRecord set Status = @Status where Id = @Id");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@Status",SqlDbType.Int)
                                   };
            parms[0].Value = Guid.Parse(Id.ToString());
            parms[1].Value = Status;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int UpdateMobilePhone(object Id, string mobilePhone)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update WinningRecord set MobilePhone = @MobilePhone where Id = @Id");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@MobilePhone",SqlDbType.VarChar,20)
                                   };
            parms[0].Value = Guid.Parse(Id.ToString());
            parms[1].Value = mobilePhone;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public bool IsScratch(Guid activityId, string userID)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.AppendFormat(@"select count(*) from WinningRecord where ActivityId=@ActivityId and UserFlag = @UserFlag and
                            LastUpdatedDate>='{0}' and LastUpdatedDate<'{1}'", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));

            SqlParameter[] parms = {
                                     new SqlParameter("@ActivityId",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@UserFlag",SqlDbType.VarChar,50)
                                   };
            parms[0].Value = activityId;
            parms[1].Value = userID
                ;
            int rt = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);

            return rt > 0 ? true : false;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from WinningRecord WR left join (select U.UserId,U.UserName,UB.Nickname,UB.HeadPicture,UB.Sex,UB.MobilePhone 
                      from HnztcAspnetDb.dbo.aspnet_Users U left join HnztcSystemDb.dbo.UserBase UB on U.UserId=UB.UserId) as U on WR.UserId=U.UserId");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by WR.LastUpdatedDate desc) as RowNumber,
			          WR.Id,WR.ActivityId,ActivityPrizeId,WR.UserId,UserFlag,WR.MobilePhone,Status,WR.Remark,WR.LastUpdatedDate,
                      PrizeName,PrizeCount,PrizeContent,UserName,Nickname
					  from ActivityPrize AP, WinningRecord WR left join (select U.UserId,U.UserName,UB.Nickname,UB.HeadPicture,UB.Sex,UB.MobilePhone 
                      from HnztcAspnetDb.dbo.aspnet_Users U left join HnztcSystemDb.dbo.UserBase UB on U.UserId=UB.UserId) as U on WR.UserId=U.UserId ");
            sb.AppendFormat(" where AP.Id=WR.ActivityPrizeId {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by WR.LastUpdatedDate desc) as RowNumber,
			          WR.Id,WR.ActivityId,WR.ActivityPrizeId,WR.UserId,UserFlag,WR.MobilePhone,Status,WR.Remark,WR.LastUpdatedDate,
                      PrizeName,PrizeCount,PrizeContent,BusinessName,BusinessPhone,BusinessAddress,UserName,Nickname,
                      P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
					  from ActivityPrize AP left join (select PP.ActivityPrizeId,PSL.* from PrizePicture PP,Picture_ScratchLotto PSL where PP.PictureId=PSL.Id) as P on AP.Id=P.ActivityPrizeId, 
                      WinningRecord WR left join (select U.UserId,U.UserName,UB.Nickname,UB.HeadPicture,UB.Sex,UB.MobilePhone 
                      from HnztcAspnetDb.dbo.aspnet_Users U left join HnztcSystemDb.dbo.UserBase UB on U.UserId=UB.UserId) as U on WR.UserId=U.UserId ");
            sb.AppendFormat(" where AP.Id=WR.ActivityPrizeId {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }
        #endregion
    }
}
