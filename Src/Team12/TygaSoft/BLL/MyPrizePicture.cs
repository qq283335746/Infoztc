using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.BLL
{
    public partial class PrizePicture
    {
        #region PrizePicture Member

        public bool IsExist(string fileName, int fileSize)
        {
            return dal.IsExist(fileName, fileSize);
        }

        #endregion
    }
}
