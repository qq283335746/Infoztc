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
    public partial class CategoryProduct
    {
        #region CategoryBrand Member

        public int Delete(object categoryId, object productId)
        {
            return dal.Delete(categoryId, productId);
        }

        #endregion
    }
}
