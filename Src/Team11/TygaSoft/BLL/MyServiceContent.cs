﻿using System;
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
    public partial class ServiceContent
    {
        #region ServiceContent Member

        public IList<ServiceContentInfo> GetListByServiceItemId(int pageIndex, int pageSize, out int totalRecords, Guid serviceItemId)
        {
            string sqlWhere = " and sc.ServiceItemId = @ServiceItemId ";
            SqlParameter parm = new SqlParameter("@ServiceItemId", SqlDbType.UniqueIdentifier);
            parm.Value = serviceItemId;

            return GetListByJoin(pageIndex, pageSize, out totalRecords, sqlWhere, parm);
        }

        public ServiceContentInfo GetModelByJoin(object Id)
        {
            return dal.GetModelByJoin(Id);
        }

        public IList<ServiceContentInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListByJoin(pageIndex,pageSize,out totalRecords,sqlWhere,cmdParms);
        }

        #endregion
    }
}
