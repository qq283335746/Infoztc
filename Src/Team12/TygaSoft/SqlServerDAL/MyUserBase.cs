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
    public partial class UserBase
    {
        #region IUserBase Member

        public Dictionary<object, string> GetKeyValueByUsersInRoles(string sqlWhere, params SqlParameter[] cmdParms)
        {
            Dictionary<object, string> dic = new Dictionary<object, string>();

            StringBuilder sb = new StringBuilder(500);
            sb.Append(@"select u.UserId,u.UserName from HnztcAspnetDb.dbo.aspnet_Users u
                        join HnztcAspnetDb.dbo.aspnet_UsersInRoles ur on ur.UserId = u.UserId
                        join HnztcAspnetDb.dbo.aspnet_Roles r on r.RoleId = ur.RoleId
                      ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<UserBaseInfo> list = new List<UserBaseInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcSystemDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dic.Add(reader[0], reader.GetString(1));
                    }
                }
            }

            return dic;
        }

        public IList<UserBaseInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from UserBase ub 
                        left join HnztcAspnetDb.dbo.aspnet_Users u on u.UserId = ub.UserId
                     ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcSystemDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<UserBaseInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by ub.LastUpdatedDate desc) as RowNumber,
			          ub.UserId,ub.Nickname,ub.HeadPicture,ub.Sex,ub.MobilePhone,ub.TotalGold,ub.TotalSilver,ub.TotalIntegral,ub.SilverLevel,ub.ColorLevel,ub.IntegralLevel,ub.VIPLevel
                      ,u.UserName
					  from UserBase ub
                      left join HnztcAspnetDb.dbo.aspnet_Users u on u.UserId = ub.UserId
                      ");
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
                        model.Username = reader.IsDBNull(12) ? "" : reader.GetString(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
