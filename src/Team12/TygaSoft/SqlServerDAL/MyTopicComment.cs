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
    public partial class TopicComment
    {
        #region ITopicComment Member
        public int UpdateIsTop(TopicCommentInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update TopicComment set IsTop = @IsTop where Id = @Id");

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
            sb.Append(@"select top 1 Id,TopicSubjectId,TC.UserId,ContentText,IsTop,Sort,Remark,IsDisable,LastUpdatedDate,
                        U.UserName,U.Nickname,U.HeadPicture,U.Sex,U.MobilePhone
			            from TopicComment TC,(select U.UserId,U.UserName,UB.Nickname,UB.HeadPicture,UB.Sex,UB.MobilePhone 
                        from HnztcAspnetDb.dbo.aspnet_Users U left join HnztcSystemDb.dbo.UserBase UB on U.UserId=UB.UserId) as U
						where TC.UserId=U.UserId and Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from TopicComment ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by TC.IsTop desc,TC.LastUpdatedDate desc) as RowNumber,
			          TC.Id,TopicSubjectId,TC.UserId,TC.ContentText,TC.IsTop,TC.Sort,TC.Remark,TC.IsDisable,TC.LastUpdatedDate,TS.Title,TS.ContentText as TSContentText,
                      U.UserName,U.Nickname,U.HeadPicture,U.Sex,U.MobilePhone
					  from TopicComment TC,TopicSubject TS,(select U.UserId,U.UserName,UB.Nickname,UB.HeadPicture,UB.Sex,UB.MobilePhone 
                      from HnztcAspnetDb.dbo.aspnet_Users U left join HnztcSystemDb.dbo.UserBase UB on U.UserId=UB.UserId) as U");
            sb.AppendFormat(" where TC.TopicSubjectId=TS.Id and TC.UserId=U.UserId {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by IsTop desc,LastUpdatedDate desc) as RowNumber,
			           Id,TopicSubjectId,TC.UserId,ContentText,IsTop,Sort,Remark,IsDisable,LastUpdatedDate,U.UserName,U.Nickname,U.HeadPicture,U.Sex,U.MobilePhone
					   from TopicComment TC,(select U.UserId,U.UserName,UB.Nickname,UB.HeadPicture,UB.Sex,UB.MobilePhone 
                      from HnztcAspnetDb.dbo.aspnet_Users U left join HnztcSystemDb.dbo.UserBase UB on U.UserId=UB.UserId) as U");
            sb.AppendFormat(" where TC.UserId=U.UserId and IsDisable=0 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }
        #endregion
    }
}
