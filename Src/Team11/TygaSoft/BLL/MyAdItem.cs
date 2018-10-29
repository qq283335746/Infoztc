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
    public partial class AdItem
    {
        #region AdItem Member

        public AdItemInfo GetModelByJoin(object Id)
        {
            return dal.GetModelByJoin(Id);
        }

        public DataSet GetDsByJoinForAdmin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetDsByJoinForAdmin(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }

        public bool DeleteBatchByJoin(IList<object> list)
        {
            return dal.DeleteBatchByJoin(list);
        }

        #endregion
    }
}
