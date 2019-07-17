using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IAnswerStatistics
    {
        /// <summary>
        /// 获取满足当前条件的记录数
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        int GetCount(string sqlWhere, params SqlParameter[] cmdParms);
    }
}
