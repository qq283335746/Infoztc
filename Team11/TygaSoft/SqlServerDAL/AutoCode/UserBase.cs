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
    public partial class UserBase : IUserBase
    {
        #region IUserBase Member

        public int Insert(UserBaseInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into UserBase (UserId,Nickname,HeadPicture,Sex,MobilePhone,TotalGold,TotalSilver,TotalIntegral,SilverLevel,ColorLevel,IntegralLevel,VIPLevel)
			            values
						(@UserId,@Nickname,@HeadPicture,@Sex,@MobilePhone,@TotalGold,@TotalSilver,@TotalIntegral,@SilverLevel,@ColorLevel,@IntegralLevel,@VIPLevel)
			            ");

            SqlParameter[] parms = {
                                        new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Nickname",SqlDbType.NVarChar,30),
                                        new SqlParameter("@HeadPicture",SqlDbType.VarChar,100),
                                        new SqlParameter("@Sex",SqlDbType.NChar,2),
                                        new SqlParameter("@MobilePhone",SqlDbType.VarChar,20),
                                        new SqlParameter("@TotalGold",SqlDbType.Int),
                                        new SqlParameter("@TotalSilver",SqlDbType.Int),
                                        new SqlParameter("@TotalIntegral",SqlDbType.Int),
                                        new SqlParameter("@SilverLevel",SqlDbType.Int),
                                        new SqlParameter("@ColorLevel",SqlDbType.Int),
                                        new SqlParameter("@IntegralLevel",SqlDbType.Int),
                                        new SqlParameter("@VIPLevel",SqlDbType.NVarChar,10)
                                   };
            parms[0].Value = model.UserId;
            parms[1].Value = model.Nickname;
            parms[2].Value = model.HeadPicture;
            parms[3].Value = model.Sex;
            parms[4].Value = model.MobilePhone;
            parms[5].Value = model.TotalGold;
            parms[6].Value = model.TotalSilver;
            parms[7].Value = model.TotalIntegral;
            parms[8].Value = model.SilverLevel;
            parms[9].Value = model.ColorLevel;
            parms[10].Value = model.IntegralLevel;
            parms[11].Value = model.VIPLevel;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcSystemDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(UserBaseInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update UserBase set Nickname = @Nickname,HeadPicture = @HeadPicture,Sex = @Sex,MobilePhone = @MobilePhone,TotalGold = @TotalGold,TotalSilver = @TotalSilver,TotalIntegral = @TotalIntegral,SilverLevel = @SilverLevel,ColorLevel = @ColorLevel,IntegralLevel = @IntegralLevel,VIPLevel = @VIPLevel 
			            where UserId = @UserId
					    ");

            SqlParameter[] parms = {
                                        new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Nickname",SqlDbType.NVarChar,30),
                                        new SqlParameter("@HeadPicture",SqlDbType.VarChar,100),
                                        new SqlParameter("@Sex",SqlDbType.NChar,2),
                                        new SqlParameter("@MobilePhone",SqlDbType.VarChar,20),
                                        new SqlParameter("@TotalGold",SqlDbType.Int),
                                        new SqlParameter("@TotalSilver",SqlDbType.Int),
                                        new SqlParameter("@TotalIntegral",SqlDbType.Int),
                                        new SqlParameter("@SilverLevel",SqlDbType.Int),
                                        new SqlParameter("@ColorLevel",SqlDbType.Int),
                                        new SqlParameter("@IntegralLevel",SqlDbType.Int),
                                        new SqlParameter("@VIPLevel",SqlDbType.NVarChar,10)
                                   };
            parms[0].Value = model.UserId;
            parms[1].Value = model.Nickname;
            parms[2].Value = model.HeadPicture;
            parms[3].Value = model.Sex;
            parms[4].Value = model.MobilePhone;
            parms[5].Value = model.TotalGold;
            parms[6].Value = model.TotalSilver;
            parms[7].Value = model.TotalIntegral;
            parms[8].Value = model.SilverLevel;
            parms[9].Value = model.ColorLevel;
            parms[10].Value = model.IntegralLevel;
            parms[11].Value = model.VIPLevel;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcSystemDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object UserId)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from UserBase where UserId = @UserId");
            SqlParameter parm = new SqlParameter("@UserId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(UserId.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcSystemDbConnString, CommandType.Text, sb.ToString(), parm);
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
                sb.Append(@"delete from UserBase where UserId = @UserId" + n + " ;");
                SqlParameter parm = new SqlParameter("@UserId" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(item);
                parms.Add(parm);
            }
            using (SqlConnection conn = new SqlConnection(SqlHelper.HnztcSystemDbConnString))
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

        public UserBaseInfo GetModel(object UserId)
        {
            UserBaseInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 UserId,Nickname,HeadPicture,Sex,MobilePhone,TotalGold,TotalSilver,TotalIntegral,SilverLevel,ColorLevel,IntegralLevel,VIPLevel 
			            from UserBase
						where UserId = @UserId ");
            SqlParameter parm = new SqlParameter("@UserId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(UserId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcSystemDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new UserBaseInfo();
                        model.UserId = reader.GetGuid(0);
                        model.Nickname = reader.GetString(1);
                        model.HeadPicture = reader.GetString(2);
                        model.Sex = reader.GetString(3);
                        model.MobilePhone = reader.GetString(4);
                        model.TotalGold = reader.GetInt32(5);
                        model.TotalSilver = reader.GetInt32(6);
                        model.TotalIntegral = reader.GetInt32(7);
                        model.SilverLevel = reader.GetInt32(8);
                        model.ColorLevel = reader.GetInt32(9);
                        model.IntegralLevel = reader.GetInt32(10);
                        model.VIPLevel = reader.GetString(11);
                    }
                }
            }

            return model;
        }

        public IList<UserBaseInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from UserBase ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcSystemDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<UserBaseInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          UserId,Nickname,HeadPicture,Sex,MobilePhone,TotalGold,TotalSilver,TotalIntegral,SilverLevel,ColorLevel,IntegralLevel,VIPLevel
					  from UserBase ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<UserBaseInfo> list = new List<UserBaseInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcSystemDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserBaseInfo model = new UserBaseInfo();
                        model.UserId = reader.GetGuid(1);
                        model.Nickname = reader.GetString(2);
                        model.HeadPicture = reader.GetString(3);
                        model.Sex = reader.GetString(4);
                        model.MobilePhone = reader.GetString(5);
                        model.TotalGold = reader.GetInt32(6);
                        model.TotalSilver = reader.GetInt32(7);
                        model.TotalIntegral = reader.GetInt32(8);
                        model.SilverLevel = reader.GetInt32(9);
                        model.ColorLevel = reader.GetInt32(10);
                        model.IntegralLevel = reader.GetInt32(11);
                        model.VIPLevel = reader.GetString(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<UserBaseInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           UserId,Nickname,HeadPicture,Sex,MobilePhone,TotalGold,TotalSilver,TotalIntegral,SilverLevel,ColorLevel,IntegralLevel,VIPLevel
					   from UserBase ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<UserBaseInfo> list = new List<UserBaseInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcSystemDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserBaseInfo model = new UserBaseInfo();
                        model.UserId = reader.GetGuid(1);
                        model.Nickname = reader.GetString(2);
                        model.HeadPicture = reader.GetString(3);
                        model.Sex = reader.GetString(4);
                        model.MobilePhone = reader.GetString(5);
                        model.TotalGold = reader.GetInt32(6);
                        model.TotalSilver = reader.GetInt32(7);
                        model.TotalIntegral = reader.GetInt32(8);
                        model.SilverLevel = reader.GetInt32(9);
                        model.ColorLevel = reader.GetInt32(10);
                        model.IntegralLevel = reader.GetInt32(11);
                        model.VIPLevel = reader.GetString(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<UserBaseInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select UserId,Nickname,HeadPicture,Sex,MobilePhone,TotalGold,TotalSilver,TotalIntegral,SilverLevel,ColorLevel,IntegralLevel,VIPLevel
                        from UserBase ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<UserBaseInfo> list = new List<UserBaseInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcSystemDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserBaseInfo model = new UserBaseInfo();
                        model.UserId = reader.GetGuid(0);
                        model.Nickname = reader.GetString(1);
                        model.HeadPicture = reader.GetString(2);
                        model.Sex = reader.GetString(3);
                        model.MobilePhone = reader.GetString(4);
                        model.TotalGold = reader.GetInt32(5);
                        model.TotalSilver = reader.GetInt32(6);
                        model.TotalIntegral = reader.GetInt32(7);
                        model.SilverLevel = reader.GetInt32(8);
                        model.ColorLevel = reader.GetInt32(9);
                        model.IntegralLevel = reader.GetInt32(10);
                        model.VIPLevel = reader.GetString(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<UserBaseInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select UserId,Nickname,HeadPicture,Sex,MobilePhone,TotalGold,TotalSilver,TotalIntegral,SilverLevel,ColorLevel,IntegralLevel,VIPLevel 
			            from UserBase
					    order by LastUpdatedDate desc ");

            IList<UserBaseInfo> list = new List<UserBaseInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcSystemDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserBaseInfo model = new UserBaseInfo();
                        model.UserId = reader.GetGuid(0);
                        model.Nickname = reader.GetString(1);
                        model.HeadPicture = reader.GetString(2);
                        model.Sex = reader.GetString(3);
                        model.MobilePhone = reader.GetString(4);
                        model.TotalGold = reader.GetInt32(5);
                        model.TotalSilver = reader.GetInt32(6);
                        model.TotalIntegral = reader.GetInt32(7);
                        model.SilverLevel = reader.GetInt32(8);
                        model.ColorLevel = reader.GetInt32(9);
                        model.IntegralLevel = reader.GetInt32(10);
                        model.VIPLevel = reader.GetString(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
