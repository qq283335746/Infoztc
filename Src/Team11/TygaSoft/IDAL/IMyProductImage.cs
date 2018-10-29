using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IProductImage
    {
        #region IProductImage Member

        DataSet GetDsByProduct(string sqlWhere, params SqlParameter[] cmdParms);

        bool IsExist(object productId, object productItemId);

        #endregion
    }
}
