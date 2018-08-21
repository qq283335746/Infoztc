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
    public partial class ProductItem : IProductItem
    {
        #region IProductItem Member

        public int Insert(ProductItemInfo model)
        {
            if (IsExist(model.ProductId, model.Named, null)) return 110;
                 
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ProductItem (ProductId,Named,PictureId,Sort,EnableStartTime,EnableEndTime,IsEnable,IsDisable)
			            values
						(@ProductId,@Named,@PictureId,@Sort,@EnableStartTime,@EnableEndTime,@IsEnable,@IsDisable)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Named",SqlDbType.NVarChar,50),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@EnableStartTime",SqlDbType.DateTime),
                                        new SqlParameter("@EnableEndTime",SqlDbType.DateTime),
                                        new SqlParameter("@IsEnable",SqlDbType.Bit),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit)
                                   };
            parms[0].Value = model.ProductId;
            parms[1].Value = model.Named;
            parms[2].Value = model.PictureId;
            parms[3].Value = model.Sort;
            parms[4].Value = model.EnableStartTime;
            parms[5].Value = model.EnableEndTime;
            parms[6].Value = model.IsEnable;
            parms[7].Value = model.IsDisable;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ProductItemInfo model)
        {
            if (IsExist(model.ProductId, model.Named, model.Id)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ProductItem set ProductId = @ProductId,Named = @Named,PictureId = @PictureId,Sort = @Sort,EnableStartTime = @EnableStartTime,EnableEndTime = @EnableEndTime,IsEnable = @IsEnable,IsDisable = @IsDisable 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Named",SqlDbType.NVarChar,50),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@EnableStartTime",SqlDbType.DateTime),
                                        new SqlParameter("@EnableEndTime",SqlDbType.DateTime),
                                        new SqlParameter("@IsEnable",SqlDbType.Bit),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.ProductId;
            parms[2].Value = model.Named;
            parms[3].Value = model.PictureId;
            parms[4].Value = model.Sort;
            parms[5].Value = model.EnableStartTime;
            parms[6].Value = model.EnableEndTime;
            parms[7].Value = model.IsEnable;
            parms[8].Value = model.IsDisable;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ProductItem where Id = @Id");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

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
                sb.Append(@"delete from ProductItem where Id = @Id" + n + " ;");
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

        public ProductItemInfo GetModel(object Id)
        {
            ProductItemInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,ProductId,Named,PictureId,Sort,EnableStartTime,EnableEndTime,IsEnable,IsDisable 
			            from ProductItem
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ProductItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.ProductId = reader.GetGuid(1);
                        model.Named = reader.GetString(2);
                        model.PictureId = reader.GetGuid(3);
                        model.Sort = reader.GetInt32(4);
                        model.EnableStartTime = reader.GetDateTime(5);
                        model.EnableEndTime = reader.GetDateTime(6);
                        model.IsEnable = reader.GetBoolean(7);
                        model.IsDisable = reader.GetBoolean(8);
                    }
                }
            }

            return model;
        }

        public IList<ProductItemInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ProductItem ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ProductItemInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,ProductId,Named,PictureId,Sort,EnableStartTime,EnableEndTime,IsEnable,IsDisable
					  from ProductItem ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProductItemInfo> list = new List<ProductItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductItemInfo model = new ProductItemInfo();
                        model.Id = reader.GetGuid(1);
                        model.ProductId = reader.GetGuid(2);
                        model.Named = reader.GetString(3);
                        model.PictureId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.EnableStartTime = reader.GetDateTime(6);
                        model.EnableEndTime = reader.GetDateTime(7);
                        model.IsEnable = reader.GetBoolean(8);
                        model.IsDisable = reader.GetBoolean(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductItemInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,ProductId,Named,PictureId,Sort,EnableStartTime,EnableEndTime,IsEnable,IsDisable
					   from ProductItem ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProductItemInfo> list = new List<ProductItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductItemInfo model = new ProductItemInfo();
                        model.Id = reader.GetGuid(1);
                        model.ProductId = reader.GetGuid(2);
                        model.Named = reader.GetString(3);
                        model.PictureId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.EnableStartTime = reader.GetDateTime(6);
                        model.EnableEndTime = reader.GetDateTime(7);
                        model.IsEnable = reader.GetBoolean(8);
                        model.IsDisable = reader.GetBoolean(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductItemInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ProductId,Named,PictureId,Sort,EnableStartTime,EnableEndTime,IsEnable,IsDisable
                        from ProductItem ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ProductItemInfo> list = new List<ProductItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductItemInfo model = new ProductItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.ProductId = reader.GetGuid(1);
                        model.Named = reader.GetString(2);
                        model.PictureId = reader.GetGuid(3);
                        model.Sort = reader.GetInt32(4);
                        model.EnableStartTime = reader.GetDateTime(5);
                        model.EnableEndTime = reader.GetDateTime(6);
                        model.IsEnable = reader.GetBoolean(7);
                        model.IsDisable = reader.GetBoolean(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductItemInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ProductId,Named,PictureId,Sort,EnableStartTime,EnableEndTime,IsEnable,IsDisable 
			            from ProductItem
					    order by LastUpdatedDate desc ");

            IList<ProductItemInfo> list = new List<ProductItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductItemInfo model = new ProductItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.ProductId = reader.GetGuid(1);
                        model.Named = reader.GetString(2);
                        model.PictureId = reader.GetGuid(3);
                        model.Sort = reader.GetInt32(4);
                        model.EnableStartTime = reader.GetDateTime(5);
                        model.EnableEndTime = reader.GetDateTime(6);
                        model.IsEnable = reader.GetBoolean(7);
                        model.IsDisable = reader.GetBoolean(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
