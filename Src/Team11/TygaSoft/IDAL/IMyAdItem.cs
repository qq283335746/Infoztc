﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IAdItem
    {
        #region IAdItem Member

        AdItemInfo GetModelByJoin(object Id);

        DataSet GetDsByJoinForAdmin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms);

        bool DeleteBatchByJoin(IList<object> list);

        #endregion
    }
}
