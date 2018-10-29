using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.IDAL
{
    public partial interface IAdItemContent
    {
        #region IAdItemContent Member

        bool IsExist(object adItemId);

        #endregion
    }
}
