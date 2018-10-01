using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IQXCLotteryNumber
    {
        #region IQXCLotteryNumber Member
        /// <summary>
        /// 获取最新一条的数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        QXCLotteryNumberInfo GetNewModel();

        /// <summary>
        /// 获取数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        IList<QXCLotteryNumberInfo> GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms);

        /// <summary>
        /// 获取数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="sort"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        IList<QXCLotteryNumberInfo> GetList(int pageIndex, int pageSize, string sqlWhere, string sort, params SqlParameter[] cmdParms);
        #endregion
    }
}
