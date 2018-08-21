using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.IDAL
{
    public partial interface IPictureInformation
    {
        #region IPictureInformation Member

        bool IsExist(string fileName, int size);

        #endregion
    }
}
