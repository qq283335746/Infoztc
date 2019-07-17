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
    public partial class OrderItem : IOrderItem
    {
        #region IOrderItem Member

        public int Insert(OrderItemInfo model)
        {
            string cmdText = @"insert into OrderItem (LineNum,ProductId,Price,Quantity)
			                 values
							 (@LineNum,@ProductId,@Price,@Quantity)
			                 ";

            SqlParameter[] parms = {
                                       new SqlParameter("@LineNum",SqlDbType.Char,12),
new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Price",SqlDbType.Decimal),
new SqlParameter("@Quantity",SqlDbType.Int)
                                   };
            parms[0].Value = model.LineNum;
parms[1].Value = model.ProductId;
parms[2].Value = model.Price;
parms[3].Value = model.Quantity;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        public int Update(OrderItemInfo model)
        {
            string cmdText = @"update OrderItem set LineNum = @LineNum,ProductId = @ProductId,Price = @Price,Quantity = @Quantity 
			                 where OrderId = @OrderId";

            SqlParameter[] parms = {
                                     new SqlParameter("@OrderId",SqlDbType.UniqueIdentifier),
new SqlParameter("@LineNum",SqlDbType.Char,12),
new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Price",SqlDbType.Decimal),
new SqlParameter("@Quantity",SqlDbType.Int)
                                   };
            parms[0].Value = model.OrderId;
parms[1].Value = model.LineNum;
parms[2].Value = model.ProductId;
parms[3].Value = model.Price;
parms[4].Value = model.Quantity;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        public int Delete(object OrderId)
        {
            string cmdText = "delete from OrderItem where OrderId = @OrderId";
            SqlParameter parm = new SqlParameter("@OrderId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(OrderId.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm);
        }

        public bool DeleteBatch(IList<object> list)
        {
            if (list == null || list.Count == 0) return false;

            bool result = false;
            StringBuilder sb = new StringBuilder();
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from OrderItem where OrderId = @OrderId" + n + " ;");
                SqlParameter parm = new SqlParameter("@OrderId" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(item);
                parms.Add(parm);
            }
            using (SqlConnection conn = new SqlConnection(SqlHelper.SqlProviderConnString))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        int effect = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, sb.ToString(), parms != null ? parms.ToArray() : null);
                        tran.Commit();
                        if (effect > 0) result = true;
                    }
                    catch
                    {
                        tran.Rollback();
                    }
                }
            }
            return result;
        }

        public OrderItemInfo GetModel(object OrderId)
        {
            OrderItemInfo model = null;

            string cmdText = @"select top 1 OrderId,LineNum,ProductId,Price,Quantity 
			                   from OrderItem
							   where OrderId = @OrderId ";
            SqlParameter parm = new SqlParameter("@OrderId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(OrderId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new OrderItemInfo();
                        model.OrderId = reader.GetGuid(0);
model.LineNum = reader.GetString(1);
model.ProductId = reader.GetGuid(2);
model.Price = reader.GetDecimal(3);
model.Quantity = reader.GetInt32(4);
                    }
                }
            }

            return model;
        }

        public IList<OrderItemInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = @"select count(*) from OrderItem ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms);

            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            cmdText = @"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          OrderId,LineNum,ProductId,Price,Quantity
					  from OrderItem ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            IList<OrderItemInfo> list = new List<OrderItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrderItemInfo model = new OrderItemInfo();
                        model.OrderId = reader.GetGuid(1);
model.LineNum = reader.GetString(2);
model.ProductId = reader.GetGuid(3);
model.Price = reader.GetDecimal(4);
model.Quantity = reader.GetInt32(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<OrderItemInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            string cmdText = @"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			                 OrderId,LineNum,ProductId,Price,Quantity
							 from OrderItem";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            IList<OrderItemInfo> list = new List<OrderItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrderItemInfo model = new OrderItemInfo();
                        model.OrderId = reader.GetGuid(1);
model.LineNum = reader.GetString(2);
model.ProductId = reader.GetGuid(3);
model.Price = reader.GetDecimal(4);
model.Quantity = reader.GetInt32(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<OrderItemInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = @"select OrderId,LineNum,ProductId,Price,Quantity
                              from OrderItem";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;

            IList<OrderItemInfo> list = new List<OrderItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrderItemInfo model = new OrderItemInfo();
                        model.OrderId = reader.GetGuid(0);
model.LineNum = reader.GetString(1);
model.ProductId = reader.GetGuid(2);
model.Price = reader.GetDecimal(3);
model.Quantity = reader.GetInt32(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<OrderItemInfo> GetList()
        {
            string cmdText = @"select OrderId,LineNum,ProductId,Price,Quantity 
			                from OrderItem
							order by LastUpdatedDate desc ";

            IList<OrderItemInfo> list = new List<OrderItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrderItemInfo model = new OrderItemInfo();
                        model.OrderId = reader.GetGuid(0);
model.LineNum = reader.GetString(1);
model.ProductId = reader.GetGuid(2);
model.Price = reader.GetDecimal(3);
model.Quantity = reader.GetInt32(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
