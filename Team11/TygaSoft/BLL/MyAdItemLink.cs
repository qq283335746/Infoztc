using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.BLL
{
    public partial class AdItemLink
    {
        #region AdItemLink Member

        public bool IsExist(object adItemId)
        {
            return dal.IsExist(adItemId);
        }

        #endregion
    }
}
