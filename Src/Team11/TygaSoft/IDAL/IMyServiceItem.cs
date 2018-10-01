using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IServiceItem
    {
        #region IServiceItem Member

        ServiceItemInfo GetModelByJoin(object Id);

        IList<ServiceItemInfo> GetListByService(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, string colAppend, params SqlParameter[] cmdParms);

        IList<ServiceItemInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms);

        List<ServiceItemInfo> GetListByJoin();

        int DeleteByJoin(object Id);

        void UpdateHasVote(Guid Id, bool hasVote);

        void UpdateHasContent(Guid Id, bool hasContent);

        void UpdateHasLink(Guid Id, bool hasLink);

        #endregion
    }
}
