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
    public partial class AdBase
    {
        #region AdBase Member

        public DataSet GetDs(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetDs(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }

        public AdBaseInfo GetModelByJoin(object Id)
        {
            return dal.GetModelByJoin(Id);
        }

        public int InsertByOutput(AdBaseInfo model)
        {
            return dal.InsertByOutput(model);
        }

        #endregion
    }
}
