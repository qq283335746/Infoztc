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
    public partial class BrandProduct
    {
        #region BrandProduct Member

        public int Delete(object brandId, object productId)
        {
            return dal.Delete(brandId, productId);
        }

        #endregion
    }
}
