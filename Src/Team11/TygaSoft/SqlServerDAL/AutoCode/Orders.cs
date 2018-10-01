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
    public partial class Orders : IOrders
    {
        #region IOrders Member

        public int Insert(OrdersInfo model)
        {
            string cmdText = @"insert into Orders (OrderNum,UserId,Receiver,ProviceCity,Address,Mobilephone,Telephone,TotalPrice,PayOption,PayDate,LastUpdatedDate)
			                 values
							 (@OrderNum,@UserId,@Receiver,@ProviceCity,@Address,@Mobilephone,@Telephone,@TotalPrice,@PayOption,@PayDate,@LastUpdatedDate)
			                 ";

            SqlParameter[] parms = {
                                       new SqlParameter("@OrderNum",SqlDbType.Char,12),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Receiver",SqlDbType.NVarChar,30),
new SqlParameter("@ProviceCity",SqlDbType.NVarChar,30),
new SqlParameter("@Address",SqlDbType.NVarChar,50),
new SqlParameter("@Mobilephone",SqlDbType.VarChar,15),
new SqlParameter("@Telephone",SqlDbType.VarChar,15),
new SqlParameter("@TotalPrice",SqlDbType.Decimal),
new SqlParameter("@PayOption",SqlDbType.NVarChar,20),
new SqlParameter("@PayDate",SqlDbType.DateTime),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.OrderNum;
parms[1].Value = model.UserId;
parms[2].Value = model.Receiver;
parms[3].Value = model.ProviceCity;
parms[4].Value = model.Address;
parms[5].Value = model.Mobilephone;
parms[6].Value = model.Telephone;
parms[7].Value = model.TotalPrice;
parms[8].Value = model.PayOption;
parms[9].Value = model.PayDate;
parms[10].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        public int Update(OrdersInfo model)
        {
            string cmdText = @"update Orders set OrderNum = @OrderNum,UserId = @UserId,Receiver = @Receiver,ProviceCity = @ProviceCity,Address = @Address,Mobilephone = @Mobilephone,Telephone = @Telephone,TotalPrice = @TotalPrice,PayOption = @PayOption,PayDate = @PayDate,LastUpdatedDate = @LastUpdatedDate 
			                 where Id = @Id";

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@OrderNum",SqlDbType.Char,12),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Receiver",SqlDbType.NVarChar,30),
new SqlParameter("@ProviceCity",SqlDbType.NVarChar,30),
new SqlParameter("@Address",SqlDbType.NVarChar,50),
new SqlParameter("@Mobilephone",SqlDbType.VarChar,15),
new SqlParameter("@Telephone",SqlDbType.VarChar,15),
new SqlParameter("@TotalPrice",SqlDbType.Decimal),
new SqlParameter("@PayOption",SqlDbType.NVarChar,20),
new SqlParameter("@PayDate",SqlDbType.DateTime),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
parms[1].Value = model.OrderNum;
parms[2].Value = model.UserId;
parms[3].Value = model.Receiver;
parms[4].Value = model.ProviceCity;
parms[5].Value = model.Address;
parms[6].Value = model.Mobilephone;
parms[7].Value = model.Telephone;
parms[8].Value = model.TotalPrice;
parms[9].Value = model.PayOption;
parms[10].Value = model.PayDate;
parms[11].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        public int Delete(object Id)
        {
            string cmdText = "delete from Orders where Id = @Id";
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

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
                sb.Append(@"delete from Orders where Id = @Id" + n + " ;");
                SqlParameter parm = new SqlParameter("@Id" + n + "", SqlDbType.UniqueIdentifier);
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

        public OrdersInfo GetModel(object Id)
        {
            OrdersInfo model = null;

            string cmdText = @"select top 1 Id,OrderNum,UserId,Receiver,ProviceCity,Address,Mobilephone,Telephone,TotalPrice,PayOption,PayDate,LastUpdatedDate 
			                   from Orders
							   where Id = @Id ";
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new OrdersInfo();
                        model.Id = reader.GetGuid(0);
model.OrderNum = reader.GetString(1);
model.UserId = reader.GetGuid(2);
model.Receiver = reader.GetString(3);
model.ProviceCity = reader.GetString(4);
model.Address = reader.GetString(5);
model.Mobilephone = reader.GetString(6);
model.Telephone = reader.GetString(7);
model.TotalPrice = reader.GetDecimal(8);
model.PayOption = reader.GetString(9);
model.PayDate = reader.GetDateTime(10);
model.LastUpdatedDate = reader.GetDateTime(11);
                    }
                }
            }

            return model;
        }

        public IList<OrdersInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = @"select count(*) from Orders ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms);

            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            cmdText = @"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,OrderNum,UserId,Receiver,ProviceCity,Address,Mobilephone,Telephone,TotalPrice,PayOption,PayDate,LastUpdatedDate
					  from Orders ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            IList<OrdersInfo> list = new List<OrdersInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrdersInfo model = new OrdersInfo();
                        model.Id = reader.GetGuid(1);
model.OrderNum = reader.GetString(2);
model.UserId = reader.GetGuid(3);
model.Receiver = reader.GetString(4);
model.ProviceCity = reader.GetString(5);
model.Address = reader.GetString(6);
model.Mobilephone = reader.GetString(7);
model.Telephone = reader.GetString(8);
model.TotalPrice = reader.GetDecimal(9);
model.PayOption = reader.GetString(10);
model.PayDate = reader.GetDateTime(11);
model.LastUpdatedDate = reader.GetDateTime(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<OrdersInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            string cmdText = @"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			                 Id,OrderNum,UserId,Receiver,ProviceCity,Address,Mobilephone,Telephone,TotalPrice,PayOption,PayDate,LastUpdatedDate
							 from Orders";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            IList<OrdersInfo> list = new List<OrdersInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrdersInfo model = new OrdersInfo();
                        model.Id = reader.GetGuid(1);
model.OrderNum = reader.GetString(2);
model.UserId = reader.GetGuid(3);
model.Receiver = reader.GetString(4);
model.ProviceCity = reader.GetString(5);
model.Address = reader.GetString(6);
model.Mobilephone = reader.GetString(7);
model.Telephone = reader.GetString(8);
model.TotalPrice = reader.GetDecimal(9);
model.PayOption = reader.GetString(10);
model.PayDate = reader.GetDateTime(11);
model.LastUpdatedDate = reader.GetDateTime(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<OrdersInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = @"select Id,OrderNum,UserId,Receiver,ProviceCity,Address,Mobilephone,Telephone,TotalPrice,PayOption,PayDate,LastUpdatedDate
                              from Orders";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;

            IList<OrdersInfo> list = new List<OrdersInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrdersInfo model = new OrdersInfo();
                        model.Id = reader.GetGuid(0);
model.OrderNum = reader.GetString(1);
model.UserId = reader.GetGuid(2);
model.Receiver = reader.GetString(3);
model.ProviceCity = reader.GetString(4);
model.Address = reader.GetString(5);
model.Mobilephone = reader.GetString(6);
model.Telephone = reader.GetString(7);
model.TotalPrice = reader.GetDecimal(8);
model.PayOption = reader.GetString(9);
model.PayDate = reader.GetDateTime(10);
model.LastUpdatedDate = reader.GetDateTime(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<OrdersInfo> GetList()
        {
            string cmdText = @"select Id,OrderNum,UserId,Receiver,ProviceCity,Address,Mobilephone,Telephone,TotalPrice,PayOption,PayDate,LastUpdatedDate 
			                from Orders
							order by LastUpdatedDate desc ";

            IList<OrdersInfo> list = new List<OrdersInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrdersInfo model = new OrdersInfo();
                        model.Id = reader.GetGuid(0);
model.OrderNum = reader.GetString(1);
model.UserId = reader.GetGuid(2);
model.Receiver = reader.GetString(3);
model.ProviceCity = reader.GetString(4);
model.Address = reader.GetString(5);
model.Mobilephone = reader.GetString(6);
model.Telephone = reader.GetString(7);
model.TotalPrice = reader.GetDecimal(8);
model.PayOption = reader.GetString(9);
model.PayDate = reader.GetDateTime(10);
model.LastUpdatedDate = reader.GetDateTime(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
