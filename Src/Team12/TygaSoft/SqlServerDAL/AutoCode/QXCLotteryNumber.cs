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
    public partial class QXCLotteryNumber : IQXCLotteryNumber
    {
        #region IQXCLotteryNumber Member

        public int Insert(QXCLotteryNumberInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into QXCLotteryNumber (QS,HNQS,LotteryTime,LotteryNo,ExpiryClosingDate,SalesVolume,Progressive,ContentText,UserId,LastUpdatedDate)
			            values
						(@QS,@HNQS,@LotteryTime,@LotteryNo,@ExpiryClosingDate,@SalesVolume,@Progressive,@ContentText,@UserId,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@QS",SqlDbType.VarChar,30),
new SqlParameter("@HNQS",SqlDbType.VarChar,30),
new SqlParameter("@LotteryTime",SqlDbType.DateTime),
new SqlParameter("@LotteryNo",SqlDbType.VarChar,14),
new SqlParameter("@ExpiryClosingDate",SqlDbType.DateTime),
new SqlParameter("@SalesVolume",SqlDbType.BigInt),
new SqlParameter("@Progressive",SqlDbType.BigInt),
new SqlParameter("@ContentText",SqlDbType.NVarChar,1000),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.QS;
            parms[1].Value = model.HNQS;
            parms[2].Value = model.LotteryTime;
            parms[3].Value = model.LotteryNo;
            parms[4].Value = model.ExpiryClosingDate;
            parms[5].Value = model.SalesVolume;
            parms[6].Value = model.Progressive;
            parms[7].Value = model.ContentText;
            parms[8].Value = model.UserId;
            parms[9].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(QXCLotteryNumberInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update QXCLotteryNumber set QS = @QS,HNQS = @HNQS,LotteryTime = @LotteryTime,LotteryNo = @LotteryNo,ExpiryClosingDate = @ExpiryClosingDate,SalesVolume = @SalesVolume,Progressive = @Progressive,ContentText = @ContentText,UserId = @UserId,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@QS",SqlDbType.VarChar,30),
new SqlParameter("@HNQS",SqlDbType.VarChar,30),
new SqlParameter("@LotteryTime",SqlDbType.DateTime),
new SqlParameter("@LotteryNo",SqlDbType.VarChar,14),
new SqlParameter("@ExpiryClosingDate",SqlDbType.DateTime),
new SqlParameter("@SalesVolume",SqlDbType.BigInt),
new SqlParameter("@Progressive",SqlDbType.BigInt),
new SqlParameter("@ContentText",SqlDbType.NVarChar,1000),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.QS;
            parms[2].Value = model.HNQS;
            parms[3].Value = model.LotteryTime;
            parms[4].Value = model.LotteryNo;
            parms[5].Value = model.ExpiryClosingDate;
            parms[6].Value = model.SalesVolume;
            parms[7].Value = model.Progressive;
            parms[8].Value = model.ContentText;
            parms[9].Value = model.UserId;
            parms[10].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from QXCLotteryNumber where Id = @Id");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);
        }

        public bool DeleteBatch(IList<object> list)
        {
            if (list == null || list.Count == 0) return false;

            bool result = false;
            StringBuilder sb = new StringBuilder(500);
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from QXCLotteryNumber where Id = @Id" + n + " ;");
                SqlParameter parm = new SqlParameter("@Id" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(item);
                parms.Add(parm);
            }
            using (SqlConnection conn = new SqlConnection(SqlHelper.HnztcTeamDbConnString))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        int effect = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, sb.ToString(), parms != null ? parms.ToArray() : null);
                        tran.Commit();
                        if (effect > 0) result = true;
                    }
                    catch
                    {
                        tran.Rollback();
                    }
                }
            }
            return result;
        }

        public QXCLotteryNumberInfo GetModel(object Id)
        {
            QXCLotteryNumberInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,QS,HNQS,LotteryTime,LotteryNo,ExpiryClosingDate,SalesVolume,Progressive,ContentText,UserId,LastUpdatedDate 
			                   from QXCLotteryNumber
							   where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
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

        public IList<QXCLotteryNumberInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from QXCLotteryNumber ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<QXCLotteryNumberInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,QS,HNQS,LotteryTime,LotteryNo,ExpiryClosingDate,SalesVolume,Progressive,ContentText,UserId,LastUpdatedDate
					  from QXCLotteryNumber ");
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

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<QXCLotteryNumberInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,QS,HNQS,LotteryTime,LotteryNo,ExpiryClosingDate,SalesVolume,Progressive,ContentText,UserId,LastUpdatedDate
					   from QXCLotteryNumber ");
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

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<QXCLotteryNumberInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,QS,HNQS,LotteryTime,LotteryNo,ExpiryClosingDate,SalesVolume,Progressive,ContentText,UserId,LastUpdatedDate
                        from QXCLotteryNumber ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<QXCLotteryNumberInfo> list = new List<QXCLotteryNumberInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        QXCLotteryNumberInfo model = new QXCLotteryNumberInfo();
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

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<QXCLotteryNumberInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,QS,HNQS,LotteryTime,LotteryNo,ExpiryClosingDate,SalesVolume,Progressive,ContentText,UserId,LastUpdatedDate 
			            from QXCLotteryNumber
					    order by LastUpdatedDate desc ");

            IList<QXCLotteryNumberInfo> list = new List<QXCLotteryNumberInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        QXCLotteryNumberInfo model = new QXCLotteryNumberInfo();
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

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
