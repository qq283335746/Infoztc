﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;

namespace TygaSoft.BLL
{
    public partial class AdItemContent
    {
        #region AdItemContent Member

        public bool IsExist(object adItemId)
        {
            return dal.IsExist(adItemId);
        }

        #endregion
    }
}
