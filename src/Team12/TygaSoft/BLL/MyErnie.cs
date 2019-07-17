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
    public partial class Ernie
    {
        #region Ernie Member

        public bool IsExistLatest()
        {
            return dal.IsExistLatest();
        }

        public IList<ErnieAllInfo> GetLatest()
        {
            return dal.GetLatest();
        }

        #endregion
    }
}