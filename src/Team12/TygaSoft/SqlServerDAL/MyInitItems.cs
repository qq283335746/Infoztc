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
    public partial class InitItems : IInitItems
    {
        #region IInitItems Member

        public bool UpdateBatch(IList<InitItemsInfo> list)
        {
            if (list == null || list.Count == 0) return false;

            bool result = false;
            StringBuilder sb = new StringBuilder(500);
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (InitItemsInfo item in list)
            {
                n++;
                sb.Append(@"update InitItems set ItemKey=@ItemKey" + n + " where ItemCode = @ItemCode" + n + " ;");
                SqlParameter parmKey = new SqlParameter("@ItemKey" + n + "", SqlDbType.VarChar, 50);
                parmKey.Value = item.ItemKey;
                parms.Add(parmKey);
                SqlParameter parmCode = new SqlParameter("@ItemCode" + n + "", SqlDbType.VarChar, 1000);
                parmCode.Value = item.ItemCode;
                parms.Add(parmCode);
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
        #endregion
    }
}
