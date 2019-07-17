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
    public partial class Cart
    {
        #region ICart Member

        public IList<CartInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Profiles p join Cart c on c.ProfileId = p.Id ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<CartInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by p.LastUpdatedDate desc) as RowNumber,
                      p.Username,p.IsAnonymous,p.LastActivityDate,p.LastUpdatedDate,
			          c.ProfileId,c.ProductId,c.CategoryId,c.Price,c.Quantity,c.Named,c.IsShoppingCart,(c.Price * c.Quantity) TotalPrice
                      from Profiles p join Cart c on c.ProfileId = p.Id
					  ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<CartInfo> list = new List<CartInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CartInfo model = new CartInfo();
                        model.Username = reader.GetString(1);
                        model.IsAnonymous = reader.GetBoolean(2);
                        model.LastActivityDate = reader.GetDateTime(3);
                        model.LastUpdatedDate = reader.GetDateTime(4);
                        model.ProfileId = reader.GetGuid(5);
                        model.ProductId = reader.GetGuid(6);
                        model.CategoryId = reader.GetGuid(7);
                        model.Price = reader.GetDecimal(8);
                        model.Quantity = reader.GetInt32(9);
                        model.Named = reader.GetString(10);
                        model.IsShoppingCart = reader.GetBoolean(11);
                        model.TotalPrice = reader.GetDecimal(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
