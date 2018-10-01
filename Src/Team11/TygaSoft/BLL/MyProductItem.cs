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
    public partial class ProductItem
    {
        #region ProductItem Member

        public Dictionary<string, string> GetKeyValueByProductId(object productId)
        {
            return dal.GetKeyValueByProductId(productId);
        }

        public bool DeleteBatchByProductId(IList<object> list)
        {
            return dal.DeleteBatchByProductId(list);
        }

        public ProductItemInfo GetModelByJoin(object Id)
        {
            return dal.GetModelByJoin(Id);
        }

        public IList<ProductItemInfo> GetListByProductId(Guid productId)
        {
            SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier);
            parm.Value = productId;

            return dal.GetListByProduct("and ProductId = @ProductId", parm);
        }

        public List<ProductAllInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListByJoin(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }

        #endregion
    }
}
