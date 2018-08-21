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
    public partial class ProductAttr
    {
        #region ProductAttr Member

        public DataSet GetDsByProductId(Guid productId)
        {
            SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier);
            parm.Value = productId;

            return dal.GetDsByProduct("and pa.ProductId = @ProductId", parm);
        }

        #endregion
    }
}
