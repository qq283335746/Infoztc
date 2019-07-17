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
    public partial class Announcement
    {
        public void UpdateViewCount(Guid Id)
        {
            dal.UpdateViewCount(Id);
        }

        public List<AnnouncementInfo> GetListForTitle(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListForTitle(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }
    }
}
