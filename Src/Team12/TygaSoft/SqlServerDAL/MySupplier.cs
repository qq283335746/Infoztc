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
    public partial class Supplier
    {
        #region ISupplier Member

        private bool IsExist(string name, object Id)
        {
            Guid gId = Guid.Empty;
            if (Id != null)
            {
                Guid.TryParse(Id.ToString(), out gId);
            }

            SqlParameter[] parms = {
                                       new SqlParameter("@SupplierName",SqlDbType.NVarChar, 30)
                                   };
            parms[0].Value = name;

            string cmdText = "select 1 from [Supplier] where lower(SupplierName) = @SupplierName";
            if (!gId.Equals(Guid.Empty))
            {
                cmdText = "select 1 from [Supplier] where lower(SupplierName) = @SupplierName and Id <> @Id ";

                Array.Resize(ref parms, 2);
                parms[1] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                parms[1].Value = gId;
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
