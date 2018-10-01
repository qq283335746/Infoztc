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
    public partial class Brand
    {
        #region IBrand Member

        public Guid InsertByOutput(BrandInfo model)
        {
            if (IsExist(model.BrandName, model.ParentId, null))
            {
                throw new ArgumentException("已存在相同数据记录，请勿重复提交！");
            }

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"insert into Brand (BrandName,BrandCode,ParentId,PictureId,Sort,Remark,LastUpdatedDate)
			            output inserted.Id values
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

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
            if(obj != null) return Guid.Parse(obj.ToString());

            return Guid.Empty;
        }

        public BrandInfo GetModelByJoin(object Id)
        {
            BrandInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 b.Id,b.BrandName,b.BrandCode,b.ParentId,b.PictureId,b.Sort,b.Remark,b.LastUpdatedDate 
                        ,cb.CategoryId
                        ,p.FileDirectory,p.RandomFolder,p.FileExtension
			            from Brand b
                        left join CategoryBrand cb on cb.BrandId = b.Id
                        left join Picture_Brand p on p.Id = b.PictureId
						where b.Id = @Id ");
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
                        model.CategoryId = reader.IsDBNull(8) ? Guid.Empty : reader.GetGuid(8);
                        model.FileDirectory = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        model.RandomFolder = reader.IsDBNull(10) ? "" : reader.GetString(10);
                        model.FileExtension = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    }
                }
            }

            return model;
        }

        public List<BrandInfo> GetListByJoin(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select b.Id,b.BrandName,b.BrandCode,b.ParentId,b.PictureId,b.Sort,b.Remark,b.LastUpdatedDate,
                        cb.CategoryId 
			            from Brand b
                        left join CategoryBrand cb on cb.BrandId = b.Id
						");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.Append(" where 1=1 " + sqlWhere);
            sb.Append(" order by b.LastUpdatedDate desc ");

            List<BrandInfo> list = new List<BrandInfo>();

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
                        model.PictureId = reader.IsDBNull(4) ? Guid.Empty : reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                        model.CategoryId = reader.IsDBNull(8) ? Guid.Empty : reader.GetGuid(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<BrandInfo> GetListByJoin()
        {
            StringBuilder sb = new StringBuilder(500);
            sb.Append(@"select b.Id,b.BrandName,b.BrandCode,b.ParentId,b.PictureId,b.Sort,b.Remark,b.LastUpdatedDate
                        ,cb.CategoryId
                        ,p.FileDirectory,p.RandomFolder,p.FileExtension
			            from Brand b
                        left join CategoryBrand cb on cb.BrandId = b.Id
                        left join Picture_Brand p on p.Id = b.PictureId
						order by b.LastUpdatedDate desc ");

            List<BrandInfo> list = new List<BrandInfo>();

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
                        model.PictureId = reader.IsDBNull(4) ? Guid.Empty : reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                        model.CategoryId = reader.IsDBNull(8) ? Guid.Empty : reader.GetGuid(8);
                        model.FileDirectory = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        model.RandomFolder = reader.IsDBNull(10) ? "" : reader.GetString(10);
                        model.FileExtension = reader.IsDBNull(11) ? "" : reader.GetString(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        private bool IsExist(string name,object parentId, object Id)
        {
            Guid gId = Guid.Empty;
            if (Id != null)
            {
                Guid.TryParse(Id.ToString(), out gId);
            }

            SqlParameter[] parms = {
                                       new SqlParameter("@Name",SqlDbType.NVarChar, 30),
                                       new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = name;
            parms[1].Value = Guid.Parse(parentId.ToString());

            StringBuilder sb = new StringBuilder(100);
            if (!gId.Equals(Guid.Empty))
            {
                sb.Append(@" select 1 from [Brand] where lower(BrandName) = @Name and ParentId = @ParentId and Id <> @Id ");

                Array.Resize(ref parms, 3);
                parms[2] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                parms[2].Value = gId;
            }
            else
            {
                sb.Append(@" select 1 from [Brand] where lower(BrandName) = @Name and ParentId = @ParentId ");
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
