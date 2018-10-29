using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;

namespace TygaSoft.BLL
{
    public partial class UserLevelProduce
    {
        #region UserLevelProduce Member

        public bool IsExist(object userId, int funCode, int enumSource)
        {
            return dal.IsExist(userId, funCode, enumSource);
        }

        #endregion
    }
}
