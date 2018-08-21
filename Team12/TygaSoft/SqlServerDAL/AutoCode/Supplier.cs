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
    public partial class Supplier : ISupplier
    {
        #region ISupplier Member

        public int Insert(SupplierInfo model)
        {
            if (IsExist(model.SupplierName, null)) return 110;
            string cmdText = @"insert into Supplier (SupplierName,Phone,ProvinceCityName,Address,LastUpdatedDate)
			                 values
							 (@SupplierName,@Phone,@ProvinceCityName,@Address,@LastUpdatedDate)
			                 ";

            SqlParameter[] parms = {
                                       new SqlParameter("@SupplierName",SqlDbType.NVarChar,30),
                                        new SqlParameter("@Phone",SqlDbType.Char,15),
                                        new SqlParameter("@ProvinceCityName",SqlDbType.NVarChar,20),
                                        new SqlParameter("@Address",SqlDbType.NVarChar,30),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.SupplierName;
            parms[1].Value = model.Phone;
            parms[2].Value = model.ProvinceCityName;
            parms[3].Value = model.Address;
            parms[4].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
        }

        public int Update(SupplierInfo model)
        {
            if (IsExist(model.SupplierName, model.Id)) return 110;

            string cmdText = @"update Supplier set SupplierName = @SupplierName,Phone = @Phone,ProvinceCityName = @ProvinceCityName,Address = @Address,LastUpdatedDate = @LastUpdatedDate 
			                 where Id = @Id";

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@SupplierName",SqlDbType.NVarChar,30),
                                        new SqlParameter("@Phone",SqlDbType.Char,15),
                                        new SqlParameter("@ProvinceCityName",SqlDbType.NVarChar,20),
                                        new SqlParameter("@Address",SqlDbType.NVarChar,30),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.SupplierName;
            parms[2].Value = model.Phone;
            parms[3].Value = model.ProvinceCityName;
            parms[4].Value = model.Address;
            parms[5].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
        }

        public int Delete(object Id)
        {
            string cmdText = "delete from Supplier where Id = @Id";
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

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
                sb.Append(@"delete from Supplier where Id = @Id" + n + " ;");
                SqlParameter parm = new SqlParameter("@Id" + n + "", SqlDbType.UniqueIdentifier);
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

        public SupplierInfo GetModel(object Id)
        {
            SupplierInfo model = null;

            string cmdText = @"select top 1 Id,SupplierName,Phone,ProvinceCityName,Address,LastUpdatedDate 
			                   from Supplier
							   where Id = @Id ";
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new SupplierInfo();
                        model.Id = reader.GetGuid(0);
                        model.SupplierName = reader.GetString(1);
                        model.Phone = reader.GetString(2);
                        model.ProvinceCityName = reader.GetString(3);
                        model.Address = reader.GetString(4);
                        model.LastUpdatedDate = reader.GetDateTime(5);
                    }
                }
            }

            return model;
        }

        public IList<SupplierInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = @"select count(*) from Supplier ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms);

            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            cmdText = @"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,SupplierName,Phone,ProvinceCityName,Address,LastUpdatedDate
					  from Supplier ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            IList<SupplierInfo> list = new List<SupplierInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SupplierInfo model = new SupplierInfo();
                        model.Id = reader.GetGuid(1);
                        model.SupplierName = reader.GetString(2);
                        model.Phone = reader.GetString(3);
                        model.ProvinceCityName = reader.GetString(4);
                        model.Address = reader.GetString(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<SupplierInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            string cmdText = @"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			                 Id,SupplierName,Phone,ProvinceCityName,Address,LastUpdatedDate
							 from Supplier";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            IList<SupplierInfo> list = new List<SupplierInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SupplierInfo model = new SupplierInfo();
                        model.Id = reader.GetGuid(1);
                        model.SupplierName = reader.GetString(2);
                        model.Phone = reader.GetString(3);
                        model.ProvinceCityName = reader.GetString(4);
                        model.Address = reader.GetString(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<SupplierInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = @"select Id,SupplierName,Phone,ProvinceCityName,Address,LastUpdatedDate
                              from Supplier";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += " where 1=1 " + sqlWhere;

            IList<SupplierInfo> list = new List<SupplierInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SupplierInfo model = new SupplierInfo();
                        model.Id = reader.GetGuid(0);
                        model.SupplierName = reader.GetString(1);
                        model.Phone = reader.GetString(2);
                        model.ProvinceCityName = reader.GetString(3);
                        model.Address = reader.GetString(4);
                        model.LastUpdatedDate = reader.GetDateTime(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<SupplierInfo> GetList()
        {
            string cmdText = @"select Id,SupplierName,Phone,ProvinceCityName,Address,LastUpdatedDate 
			                from Supplier
							order by LastUpdatedDate desc ";

            IList<SupplierInfo> list = new List<SupplierInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SupplierInfo model = new SupplierInfo();
                        model.Id = reader.GetGuid(0);
                        model.SupplierName = reader.GetString(1);
                        model.Phone = reader.GetString(2);
                        model.ProvinceCityName = reader.GetString(3);
                        model.Address = reader.GetString(4);
                        model.LastUpdatedDate = reader.GetDateTime(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
