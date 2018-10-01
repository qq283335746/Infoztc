using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;

namespace TygaSoft.BLL
{
    public partial class ActivitySignUp
    {
        #region ActivitySignUp Member

        /// <summary>
        /// 获取数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListOW(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }

        public DataSet ExportExcel(string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.ExportExcel(sqlWhere, cmdParms);
        }

        public bool IsAlreadySignUp(string UserId, string ActivityId)
        {
            return dal.IsAlreadySignUp(UserId, ActivityId);
        }

        public bool IsNotAtFull(string ActivityId)
        {
            return dal.IsNotAtFull(ActivityId);
        }

        public int SignUpCount(string ActivityId)
        {
            return dal.SignUpCount(ActivityId);
        }
        #endregion
    }
}
