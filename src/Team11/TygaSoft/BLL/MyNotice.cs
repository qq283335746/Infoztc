using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;

namespace TygaSoft.BLL
{
    public partial class Notice
    {
        public IList<NoticeInfo> GetListExceptContent(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListExceptContent(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }
    }
}
