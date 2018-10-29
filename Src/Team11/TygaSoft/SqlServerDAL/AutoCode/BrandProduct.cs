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
    public partial class BrandProduct : IBrandProduct
    {
        #region IBrandProduct Member

        public int Insert(BrandProductInfo model)
        {
            string cmdText = @"insert into BrandProduct (BrandId,ProductId)
			                 values
							 (@BrandId,@ProductId)
			                 ";

            SqlParameter[] parms = {
                                       new SqlParameter("@BrandId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.BrandId;
            parms[1].Value = model.ProductId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
        }

        public int Update(BrandProductInfo model)
        {
            string cmdText = @"update BrandProduct set BrandId = @BrandId 
			                 where ProductId = @ProductId";

            SqlParameter[] parms = {
                                     new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@BrandId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.ProductId;
            parms[1].Value = model.BrandId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
        }

        public int Delete(object ProductId)
        {
            string cmdText = "delete from BrandProduct where ProductId = @ProductId";
            SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ProductId.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parm);
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
                sb.Append(@"delete from BrandProduct where ProductId = @ProductId" + n + " ;");
                SqlParameter parm = new SqlParameter("@ProductId" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(item);
                parms.Add(parm);
            }
            using (SqlConnection conn = new SqlConnection(SqlHelper.HnztcShopDbConnString))
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

        public BrandProductInfo GetModel(object ProductId)
        {
            BrandProductInfo model = null;

            string cmdText = @"select top 1 ProductId,BrandId 
			                   from BrandProduct
							   where ProductId = @ProductId ";
            SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ProductId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new BrandProductInfo();
                        model.ProductId = reader.GetGuid(0);
                        model.BrandId = reader.GetGuid(1);
                    }
                }
            }

            return model;
        }

        public List<BrandProductInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = @"select count(*) from BrandProduct ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms);

            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            cmdText = @"select * from(select row_number() over(order by ProductId) as RowNumber,
			          ProductId,BrandId
					  from BrandProduct ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            List<BrandProductInfo> list = new List<BrandProductInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BrandProductInfo model = new BrandProductInfo();
                        model.ProductId = reader.GetGuid(1);
                        model.BrandId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<BrandProductInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            string cmdText = @"select * from(select row_number() over(order by ProductId) as RowNumber,
			                 ProductId,BrandId
							 from BrandProduct";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            List<BrandProductInfo> list = new List<BrandProductInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BrandProductInfo model = new BrandProductInfo();
                        model.ProductId = reader.GetGuid(1);
                        model.BrandId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<BrandProductInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = @"select ProductId,BrandId
                              from BrandProduct";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;

            List<BrandProductInfo> list = new List<BrandProductInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BrandProductInfo model = new BrandProductInfo();
                        model.ProductId = reader.GetGuid(0);
                        model.BrandId = reader.GetGuid(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<BrandProductInfo> GetList()
        {
            string cmdText = @"select ProductId,BrandId 
			                from BrandProduct
							order by ProductId ";

            List<BrandProductInfo> list = new List<BrandProductInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BrandProductInfo model = new BrandProductInfo();
                        model.ProductId = reader.GetGuid(0);
                        model.BrandId = reader.GetGuid(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
