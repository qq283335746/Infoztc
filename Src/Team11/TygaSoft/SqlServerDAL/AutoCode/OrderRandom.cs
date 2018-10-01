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
    public partial class OrderRandom : IOrderRandom
    {
        #region IOrderRandom Member

        public bool IsExist(string orderCode)
        {
            var cmdText = @"select 1 from [OrderRandom] where OrderCode = @OrderCode ";
            var parm = new SqlParameter("@OrderCode", SqlDbType.VarChar, 20);
            parm.Value = orderCode;

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm);
            if (obj != null) return true;

            return false;
        }

        public int Insert(OrderRandomInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into OrderRandom (OrderCode,Prefix,LastUpdatedDate)
			            values
						(@OrderCode,@Prefix,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@OrderCode",SqlDbType.VarChar,20),
                                       new SqlParameter("@Prefix",SqlDbType.VarChar,10),
                                       new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.OrderCode;
            parms[1].Value = model.Prefix;
            parms[2].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(OrderRandomInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update OrderRandom set LastUpdatedDate = @LastUpdatedDate,Prefix = @Prefix, 
			            where OrderCode = @OrderCode
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@OrderCode",SqlDbType.VarChar,20),
                                     new SqlParameter("@Prefix",SqlDbType.VarChar,10),
                                     new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.OrderCode;
            parms[1].Value = model.Prefix;
            parms[2].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object OrderCode)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from OrderRandom where OrderCode = @OrderCode");
            SqlParameter parm = new SqlParameter("@OrderCode", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(OrderCode.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm);
        }

        public bool DeleteBatch(IList<object> list)
        {
            bool result = false;
            StringBuilder sb = new StringBuilder(500);
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from OrderRandom where OrderCode = @OrderCode" + n + " ;");
                SqlParameter parm = new SqlParameter("@OrderCode" + n + "", SqlDbType.UniqueIdentifier);
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

        public OrderRandomInfo GetModel(object OrderCode)
        {
            OrderRandomInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 OrderCode,Prefix,LastUpdatedDate
			            from OrderRandom
						where OrderCode = @OrderCode ");
            SqlParameter parm = new SqlParameter("@OrderCode", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(OrderCode.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new OrderRandomInfo();
                        model.OrderCode = reader.GetString(0);
                        model.Prefix = reader.GetString(1);
                        model.LastUpdatedDate = reader.GetDateTime(2);
                    }
                }
            }

            return model;
        }

        public IList<OrderRandomInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from OrderRandom ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<OrderRandomInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          OrderCode,Prefix,LastUpdatedDate
					  from OrderRandom ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<OrderRandomInfo> list = new List<OrderRandomInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrderRandomInfo model = new OrderRandomInfo();
                        model.OrderCode = reader.GetString(1);
                        model.Prefix = reader.GetString(2);
                        model.LastUpdatedDate = reader.GetDateTime(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<OrderRandomInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           OrderCode,Prefix,LastUpdatedDate
					   from OrderRandom ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<OrderRandomInfo> list = new List<OrderRandomInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrderRandomInfo model = new OrderRandomInfo();
                        model.OrderCode = reader.GetString(1);
                        model.Prefix = reader.GetString(2);
                        model.LastUpdatedDate = reader.GetDateTime(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<OrderRandomInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select OrderCode,Prefix,LastUpdatedDate
                        from OrderRandom ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<OrderRandomInfo> list = new List<OrderRandomInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrderRandomInfo model = new OrderRandomInfo();
                        model.OrderCode = reader.GetString(0);
                        model.Prefix = reader.GetString(1);
                        model.LastUpdatedDate = reader.GetDateTime(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<OrderRandomInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select OrderCode,Prefix,LastUpdatedDate 
			            from OrderRandom
					    order by LastUpdatedDate desc ");

            IList<OrderRandomInfo> list = new List<OrderRandomInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrderRandomInfo model = new OrderRandomInfo();
                        model.OrderCode = reader.GetString(0);
                        model.Prefix = reader.GetString(1);
                        model.LastUpdatedDate = reader.GetDateTime(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
