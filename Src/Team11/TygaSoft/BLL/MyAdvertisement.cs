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
    public partial class Advertisement
    {
        #region Advertisement Member

        public bool DeleteBatchByJoin(IList<object> list)
        {
            return dal.DeleteBatchByJoin(list);
        }

        public AdvertisementInfo GetModelByJoin(object Id)
        {
            return dal.GetModelByJoin(Id);
        }

        public DataSet GetDs(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetDs(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }

        public IList<AdvertisementInfo> GetListByFunId(int pageIndex, int pageSize, out int totalRecords, Guid siteFunId)
        {
            string sqlWhere = "and ad.SiteFunId = @SiteFunId and (ad.IsDisable = 0 and GETDATE() between StartTime and EndTime) ";
            SqlParameter parm = new SqlParameter("@SiteFunId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(siteFunId.ToString());

            return dal.GetListByJoin(pageIndex, pageSize, out totalRecords, sqlWhere, parm);
        }

        public IList<AdvertisementInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListByJoin(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }

        public int UpdateViewCount(Guid Id)
        {
            return dal.UpdateViewCount(Id);
        }

        #endregion
    }
}
