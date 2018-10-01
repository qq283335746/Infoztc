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
    public partial class ContentDetail
    {
        #region IContentDetail Member

        public ContentDetailInfo GetModelByJoin(object Id)
        {
            ContentDetailInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 cd.Id,cd.ContentTypeId,cd.Title,cd.PictureId,cd.Descr,cd.ContentText,cd.VirtualViewCount,cd.ViewCount,cd.Sort,cd.IsDisable,cd.LastUpdatedDate 
			            ,pc.FileExtension,pc.FileDirectory,pc.RandomFolder
                        from ContentDetail cd
                        left join Picture_Content pc on pc.Id = cd.PictureId
						where cd.Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ContentDetailInfo();
                        model.Id = reader.GetGuid(0);
                        model.ContentTypeId = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.PictureId = reader.GetGuid(3);
                        model.Descr = reader.GetString(4);
                        model.ContentText = reader.GetString(5);
                        model.VirtualViewCount = reader.GetInt32(6);
                        model.ViewCount = reader.GetInt32(7);
                        model.Sort = reader.GetInt32(8);
                        model.IsDisable = reader.GetBoolean(9);
                        model.LastUpdatedDate = reader.GetDateTime(10);
                        model.FileExtension = reader.IsDBNull(11) ? string.Empty : reader.GetString(11);
                        model.FileDirectory = reader.IsDBNull(12) ? string.Empty : reader.GetString(12);
                        model.RandomFolder = reader.IsDBNull(13) ? string.Empty : reader.GetString(13);
                    }
                }
            }

            return model;
        }

        public ContentDetailInfo GetModelByTitle(string title)
        {
            ContentDetailInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 cd.Id,cd.ContentTypeId,cd.Title,cd.PictureId,cd.Descr,cd.ContentText,cd.VirtualViewCount,cd.ViewCount,cd.Sort,cd.IsDisable,cd.LastUpdatedDate 
			            ,pc.FileExtension,pc.FileDirectory,pc.RandomFolder
                        from ContentDetail cd
                        left join ContentType ct on ct.Id = cd.ContentTypeId
                        left join Picture_Content pc on pc.Id = cd.PictureId
						where ct.TypeValue = @TypeValue ");
            SqlParameter parm = new SqlParameter("@TypeValue", SqlDbType.NVarChar,50);
            parm.Value = title;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ContentDetailInfo();
                        model.Id = reader.GetGuid(0);
                        model.ContentTypeId = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.PictureId = reader.GetGuid(3);
                        model.Descr = reader.GetString(4);
                        model.ContentText = reader.GetString(5);
                        model.VirtualViewCount = reader.GetInt32(6);
                        model.ViewCount = reader.GetInt32(7);
                        model.Sort = reader.GetInt32(8);
                        model.IsDisable = reader.GetBoolean(9);
                        model.LastUpdatedDate = reader.GetDateTime(10);
                        model.FileExtension = reader.IsDBNull(11) ? string.Empty : reader.GetString(11);
                        model.FileDirectory = reader.IsDBNull(12) ? string.Empty : reader.GetString(12);
                        model.RandomFolder = reader.IsDBNull(13) ? string.Empty : reader.GetString(13);
                    }
                }
            }

            return model;
        }

        public ContentDetailInfo GetModelByTypeCode(string typeCode)
        {
            ContentDetailInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 cd.Id,cd.ContentTypeId,cd.Title,cd.PictureId,cd.Descr,cd.ContentText,cd.VirtualViewCount,cd.ViewCount,cd.Sort,cd.IsDisable,cd.LastUpdatedDate 
			            ,pc.FileExtension,pc.FileDirectory,pc.RandomFolder
                        from ContentDetail cd
                        left join ContentType ct on ct.Id = cd.ContentTypeId
                        left join Picture_Content pc on pc.Id = cd.PictureId
						where ct.TypeCode = @TypeCode ");
            SqlParameter parm = new SqlParameter("@TypeCode", SqlDbType.VarChar, 50);
            parm.Value = typeCode;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ContentDetailInfo();
                        model.Id = reader.GetGuid(0);
                        model.ContentTypeId = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.PictureId = reader.GetGuid(3);
                        model.Descr = reader.GetString(4);
                        model.ContentText = reader.GetString(5);
                        model.VirtualViewCount = reader.GetInt32(6);
                        model.ViewCount = reader.GetInt32(7);
                        model.Sort = reader.GetInt32(8);
                        model.IsDisable = reader.GetBoolean(9);
                        model.LastUpdatedDate = reader.GetDateTime(10);
                        model.FileExtension = reader.IsDBNull(11) ? string.Empty : reader.GetString(11);
                        model.FileDirectory = reader.IsDBNull(12) ? string.Empty : reader.GetString(12);
                        model.RandomFolder = reader.IsDBNull(13) ? string.Empty : reader.GetString(13);
                    }
                }
            }

            return model;
        }

        public bool IsExist(object contentTypeId, object Id)
        {
            Guid gId = Guid.Empty;
            if (Id != null)
            {
                Guid.TryParse(Id.ToString(), out gId);
            }
            Guid ctId = Guid.Empty;
            if (contentTypeId != null)
            {
                Guid.TryParse(contentTypeId.ToString(), out ctId);
            }

            SqlParameter[] parms = {
                                       new SqlParameter("@ContentTypeId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = Guid.Parse(contentTypeId.ToString());

            StringBuilder sb = new StringBuilder(100);
            if (!gId.Equals(Guid.Empty))
            {
                sb.Append(@" select 1 from [ContentDetail] where ContentTypeId = @ContentTypeId and Id <> @Id ");

                Array.Resize(ref parms, 2);
                parms[1] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                parms[1].Value = gId;
            }
            else
            {
                sb.Append(@" select 1 from [ContentDetail] where ContentTypeId = @ContentTypeId ");
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
