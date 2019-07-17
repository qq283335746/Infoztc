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
    public partial class QXCLotteryNumber
    {
        #region QXCLotteryNumber Member
        /// <summary>
        /// 获取最新一条的数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public QXCLotteryNumberInfo GetNewModel()
        {
            return dal.GetNewModel();
        }

        /// <summary>
        /// 获取数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public IList<QXCLotteryNumberInfo> GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListOW(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }

        /// <summary>
        /// 获取数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="sort"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public IList<QXCLotteryNumberInfo> GetList(int pageIndex, int pageSize, string sqlWhere, string sort, params SqlParameter[] cmdParms)
        {
            return dal.GetList(pageIndex, pageSize, sqlWhere, sort, cmdParms);
        }
        #endregion
    }
}
