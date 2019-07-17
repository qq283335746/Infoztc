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
    public partial class ErnieItem
    {
        #region ErnieItem Member

        public int CopyLasted(object ErnieId)
        {
            return dal.CopyLasted(ErnieId);
        }

        public int DeleteByErnieId(object ernieId)
        {
            return dal.DeleteByErnieId(ernieId);
        }

        #endregion
    }
}
