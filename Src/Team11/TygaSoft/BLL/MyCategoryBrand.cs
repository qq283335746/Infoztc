using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.BLL
{
    public partial class CategoryBrand
    {
        #region CategoryBrand Member

        public int Delete(object brandId, object categoryId)
        {
            return dal.Delete(brandId, categoryId);
        }

        #endregion
    }
}
