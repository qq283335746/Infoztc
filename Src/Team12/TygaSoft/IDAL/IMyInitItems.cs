using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IInitItems
    {
        #region IInitItems Member

        /// <summary>
        /// 批量修改数据（启用事务
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool UpdateBatch(IList<InitItemsInfo> list);

        #endregion
    }
}
