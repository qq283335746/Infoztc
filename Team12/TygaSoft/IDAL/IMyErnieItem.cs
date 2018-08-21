using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IErnieItem
    {
        #region IErnieItem Member

        int CopyLasted(object ErnieId);

        int DeleteByErnieId(object ernieId);

        #endregion
    }
}
