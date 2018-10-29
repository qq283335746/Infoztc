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
    public partial class AnswerStatistics
    {
        public int GetCount(string sqlWhere, params SqlParameter[] cmdParms)
        {
            int rt = 0;
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from (select PaperId from AnswerStatistics ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.Append(" group by PaperId) as objTable");

            IList<AnswerStatisticsInfo> list = new List<AnswerStatisticsInfo>();

            rt = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return rt;
        }
    }
}
