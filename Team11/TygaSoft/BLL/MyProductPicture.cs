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
    public partial class ProductPicture
    {
        public IList<ProductPictureInfo> GetListInIdAppend(string idAppend)
        {
            string inIds = "";
            foreach (string item in idAppend.Split(','))
            {
                inIds += string.Format("'{0}',", item);
            }
            inIds = inIds.Trim(',');

            SqlParameter parm = new SqlParameter("@Ids", SqlDbType.UniqueIdentifier);
            parm.Value = "" + inIds + "";

            return GetList(" and Id in (" + inIds + ")", null);
        }
    }
}
