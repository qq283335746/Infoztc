using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.IDAL
{
    public partial interface ICategoryBrand
    {
        #region ICategoryBrand Member

        int Delete(object brandId, object categoryId);

        #endregion
    }
}
