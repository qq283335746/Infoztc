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
    public partial class ActivityQuestionBank
    {
        public int InsertOW(ActivityQuestionBankInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ActivityQuestionBank (ActivityReleaseId,QuestionBankId,QuestionCount)
			            values
						(@ActivityReleaseId,@QuestionBankId,@QuestionCount)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ActivityReleaseId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@QuestionBankId",SqlDbType.UniqueIdentifier),
new SqlParameter("@QuestionCount",SqlDbType.Int)
                                   };
            parms[0].Value = model.ActivityReleaseId;
            parms[1].Value = model.QuestionBankId;
            parms[2].Value = model.QuestionCount;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }
    }
}
