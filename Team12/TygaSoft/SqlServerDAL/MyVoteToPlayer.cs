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
    public partial class VoteToPlayer : IVoteToPlayer
    {
        #region IVoteToPlayer Member

        public int UpdateVoteCount(VoteToPlayerInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.AppendFormat(@"update VoteToPlayer set VoteCount = VoteCount + {0},LastUpdatedDate = @LastUpdatedDate where PlayerId=@PlayerId and UserId = @UserId", model.VoteCount);

            SqlParameter[] parms = {
                                     new SqlParameter("@PlayerId",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@userId",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.PlayerId;
            parms[1].Value = model.UserId;
            parms[2].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from VoteToPlayer ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

			sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by VTP.LastUpdatedDate desc) as RowNumber,
			          AP.Named,VTP.Id,VTP.PlayerId,VTP.UserId,VoteCount,VTP.Remark,VTP.LastUpdatedDate,
                      U.UserName,U.Nickname,U.HeadPicture,U.Sex,U.MobilePhone
					  from ActivityPlayer AP, VoteToPlayer VTP, (select U.UserId,U.UserName,UB.Nickname,UB.HeadPicture,UB.Sex,UB.MobilePhone 
                        from HnztcAspnetDb.dbo.aspnet_Users U left join HnztcSystemDb.dbo.UserBase UB on U.UserId=UB.UserId) as U");
            sb.AppendFormat(" where AP.Id=VTP.PlayerId and VTP.UserId=U.UserId {0} ", sqlWhere);
			sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public int GetCount(string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select sum(VoteCount) from ActivityPlayer AP,VoteToPlayer VP");
            sb.AppendFormat(" where AP.Id=VP.PlayerId {0} ", sqlWhere);

            object obj= SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (obj != null && !string.IsNullOrEmpty(obj.ToString())) return int.Parse(obj.ToString());

            return 0;
        }

        #endregion
    }
}
