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
    public partial class MenuProduct
    {
        #region MenuProduct Member

        public int Delete(object productId, object menuId)
        {
            return dal.Delete(productId, menuId);
        }

        #endregion
    }
}
