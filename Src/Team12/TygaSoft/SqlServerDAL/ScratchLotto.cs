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
    public class ScratchLotto : IScratchLotto
    {
        public int UpdateWinningTimes()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ActivityPrize set UpdateWinningTimes = WinningTimes where 
                        ActivityId in (select Id from ActivitySubjectNew where StartDate <= @Date and DATEADD(day,1,EndDate) > @Date and IsDisable=0)");

            SqlParameter[] parms = {
                                     new SqlParameter("@Date",SqlDbType.DateTime),
                                   };
            parms[0].Value = DateTime.Now;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }
    }
}
