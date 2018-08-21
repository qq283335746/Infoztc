using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IServiceUserVole
    {
        #region IServiceUserVole Member

        bool IsExist(object userId, object serviceItemId);

        #endregion
    }
}
