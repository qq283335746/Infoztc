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
    public partial class MenuProduct : IMenuProduct
    {
        #region IMenuProduct Member

        public int Insert(MenuProductInfo model)
        {
            StringBuilder sb = new StringBuilder(100);
            sb.Append(@"insert into MenuProduct (MenuId,ProductId)
			            values
						(@MenuId,@ProductId)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@MenuId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.MenuId;
            parms[1].Value = model.ProductId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(MenuProductInfo model)
        {
            StringBuilder sb = new StringBuilder(100);
            sb.Append(@"update MenuProduct set MenuId = @MenuId 
			            where ProductId = @ProductId
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@MenuId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.ProductId;
            parms[1].Value = model.MenuId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object ProductId)
        {
            StringBuilder sb = new StringBuilder(100);
            sb.Append("delete from MenuProduct where ProductId = @ProductId");
            SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ProductId.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm);
        }

        public bool DeleteBatch(IList<object> list)
        {
            if (list == null || list.Count == 0) return false;

            bool result = false;
            StringBuilder sb = new StringBuilder(500);
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from MenuProduct where ProductId = @ProductId" + n + " ;");
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

        public MenuProductInfo GetModel(object ProductId)
        {
            MenuProductInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 ProductId,MenuId 
			            from MenuProduct
						where ProductId = @ProductId ");
            SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ProductId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new MenuProductInfo();
                        model.ProductId = reader.GetGuid(0);
                        model.MenuId = reader.GetGuid(1);
                    }
                }
            }

            return model;
        }

        public IList<MenuProductInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from MenuProduct ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<MenuProductInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          ProductId,MenuId
					  from MenuProduct ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<MenuProductInfo> list = new List<MenuProductInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MenuProductInfo model = new MenuProductInfo();
                        model.ProductId = reader.GetGuid(1);
                        model.MenuId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<MenuProductInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           ProductId,MenuId
					   from MenuProduct ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<MenuProductInfo> list = new List<MenuProductInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MenuProductInfo model = new MenuProductInfo();
                        model.ProductId = reader.GetGuid(1);
                        model.MenuId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<MenuProductInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ProductId,MenuId
                        from MenuProduct ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<MenuProductInfo> list = new List<MenuProductInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MenuProductInfo model = new MenuProductInfo();
                        model.ProductId = reader.GetGuid(0);
                        model.MenuId = reader.GetGuid(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<MenuProductInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ProductId,MenuId 
			            from MenuProduct
					    order by LastUpdatedDate desc ");

            IList<MenuProductInfo> list = new List<MenuProductInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MenuProductInfo model = new MenuProductInfo();
                        model.ProductId = reader.GetGuid(0);
                        model.MenuId = reader.GetGuid(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
