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
    public partial class WinningRecord
    {
        #region WinningRecord Member

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateStatus(object Id, int Status)
        {
            return dal.UpdateStatus(Id, Status);
        }

        /// <summary>
        /// 修改手机号码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateMobilePhone(object Id, string mobilePhone)
        {
            return dal.UpdateMobilePhone(Id, mobilePhone);
        }

        /// <summary>
        /// 是否刮过奖
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool IsScratch(Guid activityId, string userID)
        {
            return dal.IsScratch(activityId, userID);
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
        public DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListOW(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }

        /// <summary>
        /// 获取满足当前条件的数据列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public DataSet GetListOW(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListOW(pageIndex, pageSize, sqlWhere, cmdParms);
        }

        #endregion
    }
}
