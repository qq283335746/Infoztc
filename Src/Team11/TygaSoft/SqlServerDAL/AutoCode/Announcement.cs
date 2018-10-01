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
    public partial class Announcement : IAnnouncement
    {
        #region IAnnouncement Member

        public int Insert(AnnouncementInfo model)
        {
            if (IsExist(model.Title, null)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Announcement (ContentTypeId,Title,Descr,ContentText,VirtualViewCount,Sort,IsDisable,LastUpdatedDate)
			            values
						(@ContentTypeId,@Title,@Descr,@ContentText,@VirtualViewCount,@Sort,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ContentTypeId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Title",SqlDbType.NVarChar,100),
                                        new SqlParameter("@Descr",SqlDbType.NVarChar,300),
                                        new SqlParameter("@ContentText",SqlDbType.NText,1073741823),
                                        new SqlParameter("@VirtualViewCount",SqlDbType.Int),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.ContentTypeId;
            parms[1].Value = model.Title;
            parms[2].Value = model.Descr;
            parms[3].Value = model.ContentText;
            parms[4].Value = model.VirtualViewCount;
            parms[5].Value = model.Sort;
            parms[6].Value = model.IsDisable;
            parms[7].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(AnnouncementInfo model)
        {
            if (IsExist(model.Title, model.Id)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update Announcement set ContentTypeId = @ContentTypeId,Title = @Title,Descr = @Descr,ContentText = @ContentText,VirtualViewCount = @VirtualViewCount,Sort = @Sort,IsDisable = @IsDisable,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@ContentTypeId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Title",SqlDbType.NVarChar,100),
                                        new SqlParameter("@Descr",SqlDbType.NVarChar,300),
                                        new SqlParameter("@ContentText",SqlDbType.NText,1073741823),
                                        new SqlParameter("@VirtualViewCount",SqlDbType.Int),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.ContentTypeId;
            parms[2].Value = model.Title;
            parms[3].Value = model.Descr;
            parms[4].Value = model.ContentText;
            parms[5].Value = model.VirtualViewCount;
            parms[6].Value = model.Sort;
            parms[7].Value = model.IsDisable;
            parms[8].Value = model.LastUpdatedDate;


            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            string cmdText = "delete from Announcement where Id = @Id";
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
                sb.Append(@"delete from Announcement where Id = @Id" + n + " ;");
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

        public AnnouncementInfo GetModel(object Id)
        {
            AnnouncementInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,ContentTypeId,Title,Descr,ContentText,VirtualViewCount,ViewCount,Sort,IsDisable,LastUpdatedDate 
			            from Announcement
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new AnnouncementInfo();
                        model.Id = reader.GetGuid(0);
                        model.ContentTypeId = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.Descr = reader.GetString(3);
                        model.ContentText = reader.GetString(4);
                        model.VirtualViewCount = reader.GetInt32(5);
                        model.ViewCount = reader.GetInt32(6);
                        model.Sort = reader.GetInt32(7);
                        model.IsDisable = reader.GetBoolean(8);
                        model.LastUpdatedDate = reader.GetDateTime(9);
                    }
                }
            }

            return model;
        }

        public IList<AnnouncementInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Announcement ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<AnnouncementInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc,Sort) as RowNumber,
			          Id,ContentTypeId,Title,Descr,ContentText,VirtualViewCount,ViewCount,Sort,IsDisable,LastUpdatedDate
					  from Announcement ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<AnnouncementInfo> list = new List<AnnouncementInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AnnouncementInfo model = new AnnouncementInfo();
                        model.Id = reader.GetGuid(1);
                        model.ContentTypeId = reader.GetGuid(2);
                        model.Title = reader.GetString(3);
                        model.Descr = reader.GetString(4);
                        model.ContentText = reader.GetString(5);
                        model.VirtualViewCount = reader.GetInt32(6);
                        model.ViewCount = reader.GetInt32(7);
                        model.Sort = reader.GetInt32(8);
                        model.IsDisable = reader.GetBoolean(9);
                        model.LastUpdatedDate = reader.GetDateTime(10);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AnnouncementInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc,Sort) as RowNumber,
			           Id,ContentTypeId,Title,Descr,ContentText,VirtualViewCount,ViewCount,Sort,IsDisable,LastUpdatedDate
					   from Announcement ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<AnnouncementInfo> list = new List<AnnouncementInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AnnouncementInfo model = new AnnouncementInfo();
                        model.Id = reader.GetGuid(1);
                        model.ContentTypeId = reader.GetGuid(2);
                        model.Title = reader.GetString(3);
                        model.Descr = reader.GetString(4);
                        model.ContentText = reader.GetString(5);
                        model.VirtualViewCount = reader.GetInt32(6);
                        model.ViewCount = reader.GetInt32(7);
                        model.Sort = reader.GetInt32(8);
                        model.IsDisable = reader.GetBoolean(9);
                        model.LastUpdatedDate = reader.GetDateTime(10);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AnnouncementInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ContentTypeId,Title,Descr,ContentText,VirtualViewCount,ViewCount,Sort,IsDisable,LastUpdatedDate
                        from Announcement ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.Append("order by LastUpdatedDate desc,Sort");

            IList<AnnouncementInfo> list = new List<AnnouncementInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AnnouncementInfo model = new AnnouncementInfo();
                        model.Id = reader.GetGuid(0);
                        model.ContentTypeId = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.Descr = reader.GetString(3);
                        model.ContentText = reader.GetString(4);
                        model.VirtualViewCount = reader.GetInt32(5);
                        model.ViewCount = reader.GetInt32(6);
                        model.Sort = reader.GetInt32(7);
                        model.IsDisable = reader.GetBoolean(8);
                        model.LastUpdatedDate = reader.GetDateTime(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AnnouncementInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ContentTypeId,Title,Descr,ContentText,VirtualViewCount,ViewCount,Sort,IsDisable,LastUpdatedDate 
			            from Announcement
					    order by LastUpdatedDate desc,Sort ");

            IList<AnnouncementInfo> list = new List<AnnouncementInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AnnouncementInfo model = new AnnouncementInfo();
                        model.Id = reader.GetGuid(0);
                        model.ContentTypeId = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.Descr = reader.GetString(3);
                        model.ContentText = reader.GetString(4);
                        model.VirtualViewCount = reader.GetInt32(5);
                        model.ViewCount = reader.GetInt32(6);
                        model.Sort = reader.GetInt32(7);
                        model.IsDisable = reader.GetBoolean(8);
                        model.LastUpdatedDate = reader.GetDateTime(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
