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
    public partial class ActivityRelease
    {
        public Guid InsertByOutput(ActivityReleaseInfo model)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"insert into ActivityRelease (Title,StartDate,EndDate,QuestionCount,Remark,IsDisable,LastUpdatedDate)
			            output inserted.Id values
						(@Title,@StartDate,@EndDate,@QuestionCount,@Remark,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Title",SqlDbType.NVarChar,100),
                                       new SqlParameter("@StartDate",SqlDbType.DateTime),
                                       new SqlParameter("@EndDate",SqlDbType.DateTime),
                                       new SqlParameter("@QuestionCount",SqlDbType.Int),
                                       new SqlParameter("@Remark",SqlDbType.NVarChar,300),
                                       new SqlParameter("@IsDisable",SqlDbType.Bit),
                                       new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Title;
            parms[1].Value = model.StartDate;
            parms[2].Value = model.EndDate;
            parms[3].Value = model.QuestionCount;
            parms[4].Value = model.Remark;
            parms[5].Value = model.IsDisable;
            parms[6].Value = model.LastUpdatedDate;

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return Guid.Parse(obj.ToString());

            return Guid.Empty;
        }
    }
}
