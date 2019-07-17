using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface ISysEnum
    {
        #region ISysEnum Member

        Dictionary<object, string> GetKeyValue(string sqlWhere, params SqlParameter[] cmdParms);

        #endregion
    }
}
