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
    public partial class ProductImage
    {
        #region ProductImage Member

        public DataSet GetDsByProductId(Guid productId)
        {
            SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier);
            parm.Value = productId;

            return dal.GetDsByProduct("and pim.ProductId = @ProductId", parm);
        }

        public bool IsExist(object productId, object productItemId)
        {
            return dal.IsExist(productId, productItemId);
        }

        #endregion
    }
}
