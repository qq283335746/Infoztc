using System;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IPushUser
    {
        #region IPushMsg Member

        int InsertOW(PushUserInfo model);

        #endregion
    }
}
