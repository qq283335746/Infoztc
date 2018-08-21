using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IContentDetail
    {
        #region IContentDetail Member

        ContentDetailInfo GetModelByJoin(object Id);

        ContentDetailInfo GetModelByTitle(string title);

        ContentDetailInfo GetModelByTypeCode(string typeCode);

        bool IsExist(object contentTypeId, object Id);

        #endregion
    }
}
