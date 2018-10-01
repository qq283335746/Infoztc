using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;

namespace TygaSoft.BLL
{
    public partial class ContentDetail
    {
        #region ContentDetail Member

        public ContentDetailInfo GetModelByJoin(object Id)
        {
            return dal.GetModelByJoin(Id);
        }

        public ContentDetailInfo GetModelByTitle(string title)
        {
            return dal.GetModelByTitle(title);
        }

        public ContentDetailInfo GetModelByTypeCode(string typeCode)
        {
            return dal.GetModelByTypeCode(typeCode);
        }

        public bool IsExist(object contentTypeId, object Id)
        {
            return dal.IsExist(contentTypeId, Id);
        }

        #endregion
    }
}
