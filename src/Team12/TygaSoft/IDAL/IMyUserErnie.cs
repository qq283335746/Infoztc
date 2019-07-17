using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IUserErnie
    {
        #region IUserErnie Member

        int GetTotalBetCount(object userId, object ernieId);

        #endregion
    }
}
