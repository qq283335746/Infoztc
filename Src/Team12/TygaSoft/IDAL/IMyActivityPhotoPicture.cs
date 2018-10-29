using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IActivityPhotoPicture
    {
        #region IActivityPhotoPicture Member

        bool IsExist(string fileName, int fileSize);

        #endregion
    }
}
