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
    public partial class QXCLotteryNumber
    {
        #region IQXCLotteryNumber Member
        public QXCLotteryNumberInfo GetNewModel()
        {
            QXCLotteryNumberInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,QS,HNQS,LotteryTime,LotteryNo,ExpiryClosingDate,SalesVolume,Progressive,ContentText,UserId,LastUpdatedDate 
			                   from QXCLotteryNumber order by QS desc");

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), null))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new QXCLotteryNumberInfo();
                        model.Id = reader.GetGuid(0);
                        model.QS = reader.GetString(1);
                        model.HNQS = reader.GetString(2);
                        model.LotteryTime = reader.GetDateTime(3);
                        model.LotteryNo = reader.GetString(4);
                        model.ExpiryClosingDate = reader.GetDateTime(5);
                        model.SalesVolume = reader.GetInt64(6);
                        model.Progressive = reader.GetInt64(7);
                        model.ContentText = reader.GetString(8);
                        model.UserId = reader.GetGuid(9);
                        model.LastUpdatedDate = reader.GetDateTime(10);
                    }
                }
            }

            return model;
        }

        public IList<QXCLotteryNumberInfo> GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from QXCLotteryNumber ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<QXCLotteryNumberInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by QS desc) as RowNumber,
			          Id,QS,HNQS,LotteryTime,LotteryNo,ExpiryClosingDate,SalesVolume,Progressive,ContentText,Q.UserId,LastUpdatedDate,U.UserName
					  from QXCLotteryNumber Q left join HnztcAspnetDb.dbo.aspnet_Users U on Q.UserId=U.UserId");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<QXCLotteryNumberInfo> list = new List<QXCLotteryNumberInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        QXCLotteryNumberInfo model = new QXCLotteryNumberInfo();
                        model.Id = reader.GetGuid(1);
                        model.QS = reader.GetString(2);
                        model.HNQS = reader.GetString(3);
                        model.LotteryTime = reader.GetDateTime(4);
                        model.LotteryNo = reader.GetString(5);
                        model.ExpiryClosingDate = reader.GetDateTime(6);
                        model.SalesVolume = reader.GetInt64(7);
                        model.Progressive = reader.GetInt64(8);
                        model.ContentText = reader.GetString(9);
                        model.UserId = reader.GetGuid(10);
                        model.LastUpdatedDate = reader.GetDateTime(11);
                        model.UserName = reader["UserName"] is DBNull ? "" : reader.GetString(12); ;
                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<QXCLotteryNumberInfo> GetList(int pageIndex, int pageSize, string sqlWhere, string sort, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.AppendFormat(@"select * from(select row_number() over(order by QS {0}) as RowNumber,
			           Id,QS,HNQS,LotteryTime,LotteryNo,ExpiryClosingDate,SalesVolume,Progressive,ContentText,UserId,LastUpdatedDate
					   from QXCLotteryNumber ", sort);
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1}", startIndex, endIndex);

            IList<QXCLotteryNumberInfo> list = new List<QXCLotteryNumberInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        QXCLotteryNumberInfo model = new QXCLotteryNumberInfo();
                        model.Id = reader.GetGuid(1);
                        model.QS = reader.GetString(2);
                        model.HNQS = reader.GetString(3);
                        model.LotteryTime = reader.GetDateTime(4);
                        model.LotteryNo = reader.GetString(5);
                        model.ExpiryClosingDate = reader.GetDateTime(6);
                        model.SalesVolume = reader.GetInt64(7);
                        model.Progressive = reader.GetInt64(8);
                        model.ContentText = reader.GetString(9);
                        model.UserId = reader.GetGuid(10);
                        model.LastUpdatedDate = reader.GetDateTime(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }
        #endregion
    }
}
