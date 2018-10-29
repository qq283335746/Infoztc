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
    public partial class Category
    {
        #region ICategory Member

        public CategoryInfo GetModelByJoin(object Id)
        {
            CategoryInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 c.Id,c.CategoryName,c.CategoryCode,c.ParentId,c.PictureId,c.Sort,c.Remark,c.LastUpdatedDate
                        ,p.FileDirectory,p.RandomFolder,p.FileExtension
			            from Category c
                        left join Picture_Category p on p.Id = c.PictureId
						where c.Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new CategoryInfo();
                        model.Id = reader.GetGuid(0);
                        model.CategoryName = reader.GetString(1);
                        model.CategoryCode = reader.GetString(2);
                        model.ParentId = reader.GetGuid(3);
                        model.PictureId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                        model.FileDirectory = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        model.RandomFolder = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        model.FileExtension = reader.IsDBNull(10) ? "" : reader.GetString(10);
                    }
                }
            }

            return model;
        }

        public List<CategoryInfo> GetListByJoin()
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select c.Id,c.CategoryName,c.CategoryCode,c.ParentId,c.PictureId,c.Sort,c.Remark,c.LastUpdatedDate
                        ,p.FileDirectory,p.RandomFolder,p.FileExtension
			            from Category c
                        left join Picture_Category p on p.Id = c.PictureId
						order by c.LastUpdatedDate desc ");

            List<CategoryInfo> list = new List<CategoryInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryInfo model = new CategoryInfo();
                        model.Id = reader.GetGuid(0);
                        model.CategoryName = reader.GetString(1);
                        model.CategoryCode = reader.GetString(2);
                        model.ParentId = reader.IsDBNull(3) ? Guid.Empty : reader.GetGuid(3);
                        model.PictureId = reader.IsDBNull(4) ? Guid.Empty : reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                        model.FileDirectory = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        model.RandomFolder = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        model.FileExtension = reader.IsDBNull(10) ? "" : reader.GetString(10);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        private bool IsExist(string name, object parentId, object Id)
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
                sb.Append(@" select 1 from [Category] where lower(CategoryName) = @Name and ParentId = @ParentId and Id <> @Id ");

                Array.Resize(ref parms, 3);
                parms[2] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                parms[2].Value = gId;
            }
            else
            {
                sb.Append(@" select 1 from [Category] where lower(CategoryName) = @Name and ParentId = @ParentId ");
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
