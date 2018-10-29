using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IServiceUnion
    {
        #region IServiceUnion Member

        IList<ServiceUnionInfo> GetListByService(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, string colAppend, params SqlParameter[] cmdParms);

        #endregion
    }
}
