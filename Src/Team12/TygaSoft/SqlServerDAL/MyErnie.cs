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
    public partial class Ernie
    {
        #region IErnie Member

        public bool IsExistLatest()
        {
            StringBuilder sb = new StringBuilder(100);
            sb.Append(@" select 1 from [Ernie] where IsOver = 0 and IsDisable = 0 
                      and (GETDATE() < EndTime) ");

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), null);
            if (obj != null) return true;

            return false;
        }

        public ErnieInfo GetModelByLast(object ErnieId)
        {
            ErnieInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,StartTime,EndTime,UserBetMaxCount,IsOver,IsDisable,LastUpdatedDate 
                        from dbo.Ernie 
                        where Id <> @Id
                        order by EndTime desc ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ErnieId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ErnieInfo();
                        model.Id = reader.GetGuid(0);
                        model.StartTime = reader.GetDateTime(1);
                        model.EndTime = reader.GetDateTime(2);
                        model.UserBetMaxCount = reader.GetInt32(3);
                        model.IsOver = reader.GetBoolean(4);
                        model.IsDisable = reader.GetBoolean(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);
                    }
                }
            }

            return model;
        }

        public IList<ErnieAllInfo> GetLatest()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select e.Id ErnieId,e.StartTime,e.EndTime,e.UserBetMaxCount,e.IsOver,e.IsDisable,e.LastUpdatedDate
                        ,ei.Id ErnieItemId,ei.NumType,ei.Num,ei.AppearRatio 
                        from Ernie e
                        left join ErnieItem ei on ei.ErnieId = e.Id
                        where e.Id = 
                        (select top 1 Id from Ernie where IsOver = 0 and IsDisable = 0 
                        order by LastUpdatedDate desc)
                        order by NumType,Num
                        ");

            IList<ErnieAllInfo> list = new List<ErnieAllInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), null))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ErnieAllInfo model = new ErnieAllInfo();
                        model.ErnieId = reader.GetGuid(0);
                        model.StartTime = reader.GetDateTime(1);
                        model.EndTime = reader.GetDateTime(2);
                        model.UserBetMaxCount = reader.GetInt32(3);
                        model.IsOver = reader.GetBoolean(4);
                        model.IsDisable = reader.GetBoolean(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);
                        model.ErnieItemId = reader.IsDBNull(7) ? Guid.Empty : reader.GetGuid(7);
                        model.NumType = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        model.Num = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                        model.AppearRatio = reader.IsDBNull(10) ? 0 : Math.Round(reader.GetDouble(10), 2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
