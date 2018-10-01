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
    public partial class ServiceItem : IServiceItem
    {
        #region IServiceItem Member

        public int Insert(ServiceItemInfo model)
        {
            if (IsExist(model.Named, model.ParentId, null)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Service_Item (Named,ParentId,PictureId,Sort,HasVote,HasContent,HasLink,EnableStartTime,EnableEndTime,IsDisable,LastUpdatedDate)
			            values
						(@Named,@ParentId,@PictureId,@Sort,@HasVote,@HasContent,@HasLink,@EnableStartTime,@EnableEndTime,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Named",SqlDbType.NVarChar,30),
                                        new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@HasVote",SqlDbType.Bit),
                                        new SqlParameter("@HasContent",SqlDbType.Bit),
                                        new SqlParameter("@HasLink",SqlDbType.Bit),
                                        new SqlParameter("@EnableStartTime",SqlDbType.DateTime),
                                        new SqlParameter("@EnableEndTime",SqlDbType.DateTime),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Named;
            parms[1].Value = model.ParentId;
            parms[2].Value = model.PictureId;
            parms[3].Value = model.Sort;
            parms[4].Value = model.HasVote;
            parms[5].Value = model.HasContent;
            parms[6].Value = model.HasLink;
            parms[7].Value = model.EnableStartTime;
            parms[8].Value = model.EnableEndTime;
            parms[9].Value = model.IsDisable;
            parms[10].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ServiceItemInfo model)
        {
            if (IsExist(model.Named, model.ParentId, model.Id)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update Service_Item set Named = @Named,ParentId = @ParentId,PictureId = @PictureId,Sort = @Sort,HasVote = @HasVote,HasContent = @HasContent,HasLink = @HasLink,EnableStartTime = @EnableStartTime,EnableEndTime = @EnableEndTime,IsDisable = @IsDisable,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Named",SqlDbType.NVarChar,30),
                                        new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@HasVote",SqlDbType.Bit),
                                        new SqlParameter("@HasContent",SqlDbType.Bit),
                                        new SqlParameter("@HasLink",SqlDbType.Bit),
                                        new SqlParameter("@EnableStartTime",SqlDbType.DateTime),
                                        new SqlParameter("@EnableEndTime",SqlDbType.DateTime),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.Named;
            parms[2].Value = model.ParentId;
            parms[3].Value = model.PictureId;
            parms[4].Value = model.Sort;
            parms[5].Value = model.HasVote;
            parms[6].Value = model.HasContent;
            parms[7].Value = model.HasLink;
            parms[8].Value = model.EnableStartTime;
            parms[9].Value = model.EnableEndTime;
            parms[10].Value = model.IsDisable;
            parms[11].Value = model.LastUpdatedDate;


            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            string cmdText = "delete from Service_Item where Id = @Id";
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm);
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
                sb.Append(@"delete from Service_Item where Id = @Id" + n + " ;");
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

        public ServiceItemInfo GetModel(object Id)
        {
            ServiceItemInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,Named,ParentId,PictureId,Sort,HasVote,HasContent,HasLink,EnableStartTime,EnableEndTime,IsDisable,LastUpdatedDate 
			            from Service_Item
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ServiceItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.Named = reader.GetString(1);
                        model.ParentId = reader.GetGuid(2);
                        model.PictureId = reader.GetGuid(3);
                        model.Sort = reader.GetInt32(4);
                        model.HasVote = reader.GetBoolean(5);
                        model.HasContent = reader.GetBoolean(6);
                        model.HasLink = reader.GetBoolean(7);
                        model.EnableStartTime = reader.GetDateTime(8);
                        model.EnableEndTime = reader.GetDateTime(9);
                        model.IsDisable = reader.GetBoolean(10);
                        model.LastUpdatedDate = reader.GetDateTime(11);
                    }
                }
            }

            return model;
        }

        public IList<ServiceItemInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Service_Item ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ServiceItemInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,Named,ParentId,PictureId,Sort,HasVote,HasContent,HasLink,EnableStartTime,EnableEndTime,IsDisable,LastUpdatedDate
					  from Service_Item ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ServiceItemInfo> list = new List<ServiceItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ServiceItemInfo model = new ServiceItemInfo();
                        model.Id = reader.GetGuid(1);
                        model.Named = reader.GetString(2);
                        model.ParentId = reader.GetGuid(3);
                        model.PictureId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.HasVote = reader.GetBoolean(6);
                        model.HasContent = reader.GetBoolean(7);
                        model.HasLink = reader.GetBoolean(8);
                        model.EnableStartTime = reader.GetDateTime(9);
                        model.EnableEndTime = reader.GetDateTime(10);
                        model.IsDisable = reader.GetBoolean(11);
                        model.LastUpdatedDate = reader.GetDateTime(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ServiceItemInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,Named,ParentId,PictureId,Sort,HasVote,HasContent,HasLink,EnableStartTime,EnableEndTime,IsDisable,LastUpdatedDate
					   from Service_Item ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ServiceItemInfo> list = new List<ServiceItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ServiceItemInfo model = new ServiceItemInfo();
                        model.Id = reader.GetGuid(1);
                        model.Named = reader.GetString(2);
                        model.ParentId = reader.GetGuid(3);
                        model.PictureId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.HasVote = reader.GetBoolean(6);
                        model.HasContent = reader.GetBoolean(7);
                        model.HasLink = reader.GetBoolean(8);
                        model.EnableStartTime = reader.GetDateTime(9);
                        model.EnableEndTime = reader.GetDateTime(10);
                        model.IsDisable = reader.GetBoolean(11);
                        model.LastUpdatedDate = reader.GetDateTime(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ServiceItemInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Named,ParentId,PictureId,Sort,HasVote,HasContent,HasLink,EnableStartTime,EnableEndTime,IsDisable,LastUpdatedDate
                        from Service_Item ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ServiceItemInfo> list = new List<ServiceItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ServiceItemInfo model = new ServiceItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.Named = reader.GetString(1);
                        model.ParentId = reader.GetGuid(2);
                        model.PictureId = reader.GetGuid(3);
                        model.Sort = reader.GetInt32(4);
                        model.HasVote = reader.GetBoolean(5);
                        model.HasContent = reader.GetBoolean(6);
                        model.HasLink = reader.GetBoolean(7);
                        model.EnableStartTime = reader.GetDateTime(8);
                        model.EnableEndTime = reader.GetDateTime(9);
                        model.IsDisable = reader.GetBoolean(10);
                        model.LastUpdatedDate = reader.GetDateTime(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ServiceItemInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Named,ParentId,PictureId,Sort,HasVote,HasContent,HasLink,EnableStartTime,EnableEndTime,IsDisable,LastUpdatedDate 
			            from Service_Item
					    order by LastUpdatedDate desc ");

            IList<ServiceItemInfo> list = new List<ServiceItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ServiceItemInfo model = new ServiceItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.Named = reader.GetString(1);
                        model.ParentId = reader.GetGuid(2);
                        model.PictureId = reader.GetGuid(3);
                        model.Sort = reader.GetInt32(4);
                        model.HasVote = reader.GetBoolean(5);
                        model.HasContent = reader.GetBoolean(6);
                        model.HasLink = reader.GetBoolean(7);
                        model.EnableStartTime = reader.GetDateTime(8);
                        model.EnableEndTime = reader.GetDateTime(9);
                        model.IsDisable = reader.GetBoolean(10);
                        model.LastUpdatedDate = reader.GetDateTime(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
