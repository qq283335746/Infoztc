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
    public partial class ServiceUnion
    {
        private static readonly IServiceUnion dal = DataAccess.CreateServiceUnion();

        public IList<ServiceUnionInfo> GetListByService(object userId, int pageIndex, int pageSize, out int totalRecords, object serviceItemId)
        {
            string colAppend = " ,(select 1 from Service_UserPraise sup where sup.ServiceItemId = si.Id and sup.UserId = @UserId ) IsUserPraise ";

            SqlParameter[] parms = {
                                       new SqlParameter("@UserId", SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@ServiceItemId", SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = Guid.Parse(userId.ToString());
            parms[1].Value = Guid.Parse(serviceItemId.ToString());

            return dal.GetListByService(pageIndex, pageSize, out totalRecords, null, colAppend, parms);
        }
    }
}
