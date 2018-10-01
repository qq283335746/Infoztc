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
    public partial class VoteToPlayerNew
    {
        #region VoteToPlayer Member

        /// <summary>
        /// 修改用户投票数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateVoteCount(VoteToPlayerNewInfo model)
        {
            return dal.UpdateVoteCount(model);
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
        /// 获取满足当前条件的数据数量
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public int GetCount(string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetCount(sqlWhere, cmdParms);
        }

        #endregion
    }
}
