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
    public partial class OrderStatus : IOrderStatus
    {
        #region IOrderStatus Member

        public int Insert(OrderStatusInfo model)
        {
            string cmdText = @"insert into OrderStatus (LineNum,Status,LastUpdatedDate)
			                 values
							 (@LineNum,@Status,@LastUpdatedDate)
			                 ";

            SqlParameter[] parms = {
                                       new SqlParameter("@LineNum",SqlDbType.Char,12),
new SqlParameter("@Status",SqlDbType.TinyInt),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.LineNum;
parms[1].Value = model.Status;
parms[2].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        public int Update(OrderStatusInfo model)
        {
            string cmdText = @"update OrderStatus set LineNum = @LineNum,Status = @Status,LastUpdatedDate = @LastUpdatedDate 
			                 where OrderId = @OrderId";

            SqlParameter[] parms = {
                                     new SqlParameter("@OrderId",SqlDbType.UniqueIdentifier),
new SqlParameter("@LineNum",SqlDbType.Char,12),
new SqlParameter("@Status",SqlDbType.TinyInt),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.OrderId;
parms[1].Value = model.LineNum;
parms[2].Value = model.Status;
parms[3].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        public int Delete(object OrderId)
        {
            string cmdText = "delete from OrderStatus where OrderId = @OrderId";
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
                sb.Append(@"delete from OrderStatus where OrderId = @OrderId" + n + " ;");
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

        public OrderStatusInfo GetModel(object OrderId)
        {
            OrderStatusInfo model = null;

            string cmdText = @"select top 1 OrderId,LineNum,Status,LastUpdatedDate 
			                   from OrderStatus
							   where OrderId = @OrderId ";
            SqlParameter parm = new SqlParameter("@OrderId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(OrderId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new OrderStatusInfo();
                        model.OrderId = reader.GetGuid(0);
model.LineNum = reader.GetString(1);
model.Status = reader.GetByte(2);
model.LastUpdatedDate = reader.GetDateTime(3);
                    }
                }
            }

            return model;
        }

        public IList<OrderStatusInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = @"select count(*) from OrderStatus ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms);

            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            cmdText = @"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          OrderId,LineNum,Status,LastUpdatedDate
					  from OrderStatus ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            IList<OrderStatusInfo> list = new List<OrderStatusInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrderStatusInfo model = new OrderStatusInfo();
                        model.OrderId = reader.GetGuid(1);
model.LineNum = reader.GetString(2);
model.Status = reader.GetByte(3);
model.LastUpdatedDate = reader.GetDateTime(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<OrderStatusInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            string cmdText = @"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			                 OrderId,LineNum,Status,LastUpdatedDate
							 from OrderStatus";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            IList<OrderStatusInfo> list = new List<OrderStatusInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrderStatusInfo model = new OrderStatusInfo();
                        model.OrderId = reader.GetGuid(1);
model.LineNum = reader.GetString(2);
model.Status = reader.GetByte(3);
model.LastUpdatedDate = reader.GetDateTime(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<OrderStatusInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = @"select OrderId,LineNum,Status,LastUpdatedDate
                              from OrderStatus";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;

            IList<OrderStatusInfo> list = new List<OrderStatusInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrderStatusInfo model = new OrderStatusInfo();
                        model.OrderId = reader.GetGuid(0);
model.LineNum = reader.GetString(1);
model.Status = reader.GetByte(2);
model.LastUpdatedDate = reader.GetDateTime(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<OrderStatusInfo> GetList()
        {
            string cmdText = @"select OrderId,LineNum,Status,LastUpdatedDate 
			                from OrderStatus
							order by LastUpdatedDate desc ";

            IList<OrderStatusInfo> list = new List<OrderStatusInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrderStatusInfo model = new OrderStatusInfo();
                        model.OrderId = reader.GetGuid(0);
model.LineNum = reader.GetString(1);
model.Status = reader.GetByte(2);
model.LastUpdatedDate = reader.GetDateTime(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
