using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DBUtility;

namespace TygaSoft.SqlServerDAL
{
    public partial class ProductAttrTemplate
    {
        #region IProductAttrTemplate Member

        private bool IsExist(string name, object Id)
        {
            Guid gId = Guid.Empty;
            if (Id != null)
            {
                Guid.TryParse(Id.ToString(), out gId);
            }

            SqlParameter[] parms = {
                                       new SqlParameter("@Name",SqlDbType.NVarChar, 100)
                                   };
            parms[0].Value = name;

            StringBuilder sb = new StringBuilder(100);
            if (!gId.Equals(Guid.Empty))
            {
                sb.Append(@" select 1 from [ProductAttrTemplate] where lower(TName) = @Name and Id <> @Id ");

                Array.Resize(ref parms, 2);
                parms[1] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                parms[1].Value = gId;
            }
            else
            {
                sb.Append(@" select 1 from [ProductAttrTemplate] where lower(TName) = @Name ");
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
