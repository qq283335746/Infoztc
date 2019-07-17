using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IAdBase
    {
        #region IAdBase Member

        DataSet GetDs(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms);

        AdBaseInfo GetModelByJoin(object Id);

        int InsertByOutput(AdBaseInfo model);

        #endregion
    }
}
