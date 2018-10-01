using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;

namespace TygaSoft.BLL
{
    public partial class UserErnie
    {
        #region UserErnie Member

        public int GetTotalBetCount(object userId, object ernieId)
        {
            return dal.GetTotalBetCount(userId, ernieId);
        }

        #endregion
    }
}
