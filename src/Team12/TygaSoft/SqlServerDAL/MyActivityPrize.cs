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

        public Guid InsertByOutput(ActivityPrizeInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ActivityPrize (ActivityId,PrizeName,PrizeCount,PrizeContent,Sort,BusinessName,BusinessPhone,BusinessAddress,WinningTimes,UpdateWinningTimes,Remark,IsDisable,LastUpdatedDate)
			            output inserted.Id values
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

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return Guid.Parse(obj.ToString());

            return Guid.Empty;
        }

        public int UpdateWinningTimes(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ActivityPrize set UpdateWinningTimes = UpdateWinningTimes - 1 where Id = @Id");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                   };
            parms[0].Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public DataSet GetModelOW(object Id)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 AP.Id,ActivityId,PrizeName,PrizeCount,PrizeContent,Sort,BusinessName,BusinessPhone,BusinessAddress,WinningTimes,UpdateWinningTimes,Remark,IsDisable,AP.LastUpdatedDate,
                        P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
			            from ActivityPrize AP left join 
                        (select PP.ActivityPrizeId,PSL.* from PrizePicture PP,Picture_ScratchLotto PSL where PP.PictureId=PSL.Id) as P on AP.Id=P.ActivityPrizeId
						where AP.Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ActivityPrize ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by AP.LastUpdatedDate desc) as RowNumber,
			          AP.Id,ActivityId,PrizeName,PrizeCount,PrizeContent,Sort,BusinessName,BusinessPhone,BusinessAddress,WinningTimes,UpdateWinningTimes,Remark,IsDisable,AP.LastUpdatedDate,
                      P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
			          from ActivityPrize AP left join 
                      (select PP.ActivityPrizeId,PSL.* from PrizePicture PP,Picture_ScratchLotto PSL where PP.PictureId=PSL.Id) as P on AP.Id=P.ActivityPrizeId ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public DataSet GetListOW(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select AP.Id,ActivityId,PrizeName,PrizeCount,PrizeContent,Sort,BusinessName,BusinessPhone,BusinessAddress,WinningTimes,UpdateWinningTimes,Remark,IsDisable,AP.LastUpdatedDate,
                        P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
			            from ActivityPrize AP left join 
                        (select PP.ActivityPrizeId,PSL.* from PrizePicture PP,Picture_ScratchLotto PSL where PP.PictureId=PSL.Id) as P on AP.Id=P.ActivityPrizeId ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        #endregion
    }
}
