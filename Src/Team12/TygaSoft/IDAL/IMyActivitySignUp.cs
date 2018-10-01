using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IActivitySignUp
    {
        #region IActivitySignUp Member
        /// <summary>
        /// 获取数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms);

        DataSet ExportExcel(string sqlWhere, params SqlParameter[] cmdParms);

        bool IsAlreadySignUp(string UserId, string ActivityId);

        bool IsNotAtFull(string ActivityId);

        int SignUpCount(string ActivityId);
        #endregion
    }
}
