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
    public partial class Brand : IBrand
    {
        #region IBrand Member

        public int Insert(BrandInfo model)
        {
            if (IsExist(model.BrandName,model.ParentId, null)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Brand (BrandName,BrandCode,ParentId,PictureId,Sort,Remark,LastUpdatedDate)
			            values
						(@BrandName,@BrandCode,@ParentId,@PictureId,@Sort,@Remark,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@BrandName",SqlDbType.NVarChar,30),
                                        new SqlParameter("@BrandCode",SqlDbType.VarChar,50),
                                        new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@Remark",SqlDbType.NVarChar,50),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.BrandName;
            parms[1].Value = model.BrandCode;
            parms[2].Value = model.ParentId;
            parms[3].Value = model.PictureId;
            parms[4].Value = model.Sort;
            parms[5].Value = model.Remark;
            parms[6].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(BrandInfo model)
        {
            if (IsExist(model.BrandName,model.ParentId, model.Id)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update Brand set BrandName = @BrandName,BrandCode = @BrandCode,ParentId = @ParentId,PictureId = @PictureId,Sort = @Sort,Remark = @Remark,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@BrandName",SqlDbType.NVarChar,30),
                                        new SqlParameter("@BrandCode",SqlDbType.VarChar,50),
                                        new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@Remark",SqlDbType.NVarChar,50),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.BrandName;
            parms[2].Value = model.BrandCode;
            parms[3].Value = model.ParentId;
            parms[4].Value = model.PictureId;
            parms[5].Value = model.Sort;
            parms[6].Value = model.Remark;
            parms[7].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from Brand where Id = @Id");
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
                sb.Append(@"delete from Brand where Id = @Id" + n + " ;");
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

        public BrandInfo GetModel(object Id)
        {
            BrandInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,BrandName,BrandCode,ParentId,PictureId,Sort,Remark,LastUpdatedDate 
			            from Brand
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new BrandInfo();
                        model.Id = reader.GetGuid(0);
                        model.BrandName = reader.GetString(1);
                        model.BrandCode = reader.GetString(2);
                        model.ParentId = reader.GetGuid(3);
                        model.PictureId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                    }
                }
            }

            return model;
        }

        public IList<BrandInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Brand ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<BrandInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,BrandName,BrandCode,ParentId,PictureId,Sort,Remark,LastUpdatedDate
					  from Brand ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<BrandInfo> list = new List<BrandInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BrandInfo model = new BrandInfo();
                        model.Id = reader.GetGuid(1);
                        model.BrandName = reader.GetString(2);
                        model.BrandCode = reader.GetString(3);
                        model.ParentId = reader.GetGuid(4);
                        model.PictureId = reader.GetGuid(5);
                        model.Sort = reader.GetInt32(6);
                        model.Remark = reader.GetString(7);
                        model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<BrandInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,BrandName,BrandCode,ParentId,PictureId,Sort,Remark,LastUpdatedDate
					   from Brand ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<BrandInfo> list = new List<BrandInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BrandInfo model = new BrandInfo();
                        model.Id = reader.GetGuid(1);
                        model.BrandName = reader.GetString(2);
                        model.BrandCode = reader.GetString(3);
                        model.ParentId = reader.GetGuid(4);
                        model.PictureId = reader.GetGuid(5);
                        model.Sort = reader.GetInt32(6);
                        model.Remark = reader.GetString(7);
                        model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<BrandInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,BrandName,BrandCode,ParentId,PictureId,Sort,Remark,LastUpdatedDate
                        from Brand ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<BrandInfo> list = new List<BrandInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BrandInfo model = new BrandInfo();
                        model.Id = reader.GetGuid(0);
                        model.BrandName = reader.GetString(1);
                        model.BrandCode = reader.GetString(2);
                        model.ParentId = reader.GetGuid(3);
                        model.PictureId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<BrandInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,BrandName,BrandCode,ParentId,PictureId,Sort,Remark,LastUpdatedDate 
			            from Brand
					    order by LastUpdatedDate desc ");

            IList<BrandInfo> list = new List<BrandInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BrandInfo model = new BrandInfo();
                        model.Id = reader.GetGuid(0);
                        model.BrandName = reader.GetString(1);
                        model.BrandCode = reader.GetString(2);
                        model.ParentId = reader.GetGuid(3);
                        model.PictureId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
