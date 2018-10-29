using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;

namespace TygaSoft.BLL
{
    public partial class AnswerStatistics
    {
        /// <summary>
        /// 获取满足当前条件的记录数
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public int GetCount(string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetCount(sqlWhere, cmdParms);
        }
    }
}
