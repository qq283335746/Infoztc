using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IAdItemLink
    {
        #region IAdItemLink Member

        bool IsExist(object adItemId);

        #endregion
    }
}
