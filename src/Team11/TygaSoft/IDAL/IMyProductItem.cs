using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IProductItem
    {
        #region IProductItem Member

        Dictionary<string, string> GetKeyValueByProductId(object productId);

        bool DeleteBatchByProductId(IList<object> list);

        ProductItemInfo GetModelByJoin(object Id);

        IList<ProductItemInfo> GetListByProduct(string sqlWhere, params SqlParameter[] cmdParms);

        List<ProductAllInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms);

        #endregion
    }
}
