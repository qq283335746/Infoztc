using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IBrand
    {
        #region IBrand Member

        Guid InsertByOutput(BrandInfo model);

        BrandInfo GetModelByJoin(object Id);

        List<BrandInfo> GetListByJoin(string sqlWhere, params SqlParameter[] cmdParms);

        List<BrandInfo> GetListByJoin();

        #endregion
    }
}
