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
    public partial class UserLevelView
    {
        #region UserLevelView Member

        public bool IsExist(object userId, int funCode, int enumSource)
        {
            return dal.IsExist(userId, funCode, enumSource);
        }

        public UserLevelViewInfo GetModel(object userId, int funCode, int enumSource)
        {
            return dal.GetModel(userId, funCode, enumSource);
        }

        #endregion
    }
}
