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
    public partial class ProductStock
    {
        #region ProductStock Member

        public DataSet GetDsByProductId(Guid productId)
        {
            SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier);
            parm.Value = productId;

            return dal.GetDsByProduct("and pst.ProductId = @ProductId", parm);
        }

        public bool DeleteBatchByProduct(IList<object> list)
        {
            return dal.DeleteBatchByProduct(list);
        }

        public bool IsExist(object productId, object productItemId, string productSize, object Id)
        {
            return dal.IsExist(productId, productItemId, productSize,Id);
        }

        #endregion
    }
}
