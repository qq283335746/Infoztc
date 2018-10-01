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
    public partial class PictureProduct
    {
        #region PictureProduct Member

        public IList<PictureProductInfo> GetListInIds(string inIds)
        {
            return dal.GetList("and Id in (" + inIds + ") ", null);
        }

        public bool IsExist(string fileName, int fileSize)
        {
            return dal.IsExist(fileName, fileSize);
        }

        #endregion
    }
}
