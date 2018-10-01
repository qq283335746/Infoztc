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
    public partial class ServiceLink : IServiceLink
    {
        #region IServiceLink Member

        public int Insert(ServiceLinkInfo model)
        {
            if (IsExist(model.Named, model.ServiceItemId, null)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Service_Link (ServiceItemId,PictureId,Named,Url,Sort,EnableStartTime,EnableEndTime,IsDisable,LastUpdatedDate)
			            values
						(@ServiceItemId,@PictureId,@Named,@Url,@Sort,@EnableStartTime,@EnableEndTime,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ServiceItemId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Named",SqlDbType.NVarChar,30),
                                        new SqlParameter("@Url",SqlDbType.VarChar,300),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@EnableStartTime",SqlDbType.DateTime),
                                        new SqlParameter("@EnableEndTime",SqlDbType.DateTime),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.ServiceItemId;
            parms[1].Value = model.PictureId;
            parms[2].Value = model.Named;
            parms[3].Value = model.Url;
            parms[4].Value = model.Sort;
            parms[5].Value = model.EnableStartTime;
            parms[6].Value = model.EnableEndTime;
            parms[7].Value = model.IsDisable;
            parms[8].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ServiceLinkInfo model)
        {
            if (IsExist(model.Named, model.ServiceItemId, model.Id)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update Service_Link set ServiceItemId = @ServiceItemId,PictureId = @PictureId,Named = @Named,Url = @Url,Sort = @Sort,EnableStartTime = @EnableStartTime,EnableEndTime = @EnableEndTime,IsDisable = @IsDisable,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@ServiceItemId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Named",SqlDbType.NVarChar,30),
                                        new SqlParameter("@Url",SqlDbType.VarChar,300),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@EnableStartTime",SqlDbType.DateTime),
                                        new SqlParameter("@EnableEndTime",SqlDbType.DateTime),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.ServiceItemId;
            parms[2].Value = model.PictureId;
            parms[3].Value = model.Named;
            parms[4].Value = model.Url;
            parms[5].Value = model.Sort;
            parms[6].Value = model.EnableStartTime;
            parms[7].Value = model.EnableEndTime;
            parms[8].Value = model.IsDisable;
            parms[9].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from Service_Link where Id = @Id");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm);
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
                sb.Append(@"delete from Service_Link where Id = @Id" + n + " ;");
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

        public ServiceLinkInfo GetModel(object Id)
        {
            ServiceLinkInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,ServiceItemId,PictureId,Named,Url,Sort,EnableStartTime,EnableEndTime,IsDisable,LastUpdatedDate 
			            from Service_Link
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ServiceLinkInfo();
                        model.Id = reader.GetGuid(0);
                        model.ServiceItemId = reader.GetGuid(1);
                        model.PictureId = reader.GetGuid(2);
                        model.Named = reader.GetString(3);
                        model.Url = reader.GetString(4);
                        model.Sort = reader.GetInt32(5);
                        model.EnableStartTime = reader.GetDateTime(6);
                        model.EnableEndTime = reader.GetDateTime(7);
                        model.IsDisable = reader.GetBoolean(8);
                        model.LastUpdatedDate = reader.GetDateTime(9);
                    }
                }
            }

            return model;
        }

        public IList<ServiceLinkInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Service_Link ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ServiceLinkInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,ServiceItemId,PictureId,Named,Url,Sort,EnableStartTime,EnableEndTime,IsDisable,LastUpdatedDate
					  from Service_Link ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ServiceLinkInfo> list = new List<ServiceLinkInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ServiceLinkInfo model = new ServiceLinkInfo();
                        model.Id = reader.GetGuid(1);
                        model.ServiceItemId = reader.GetGuid(2);
                        model.PictureId = reader.GetGuid(3);
                        model.Named = reader.GetString(4);
                        model.Url = reader.GetString(5);
                        model.Sort = reader.GetInt32(6);
                        model.EnableStartTime = reader.GetDateTime(7);
                        model.EnableEndTime = reader.GetDateTime(8);
                        model.IsDisable = reader.GetBoolean(9);
                        model.LastUpdatedDate = reader.GetDateTime(10);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ServiceLinkInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,ServiceItemId,PictureId,Named,Url,Sort,EnableStartTime,EnableEndTime,IsDisable,LastUpdatedDate
					   from Service_Link ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ServiceLinkInfo> list = new List<ServiceLinkInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ServiceLinkInfo model = new ServiceLinkInfo();
                        model.Id = reader.GetGuid(1);
                        model.ServiceItemId = reader.GetGuid(2);
                        model.PictureId = reader.GetGuid(3);
                        model.Named = reader.GetString(4);
                        model.Url = reader.GetString(5);
                        model.Sort = reader.GetInt32(6);
                        model.EnableStartTime = reader.GetDateTime(7);
                        model.EnableEndTime = reader.GetDateTime(8);
                        model.IsDisable = reader.GetBoolean(9);
                        model.LastUpdatedDate = reader.GetDateTime(10);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ServiceLinkInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ServiceItemId,PictureId,Named,Url,Sort,EnableStartTime,EnableEndTime,IsDisable,LastUpdatedDate
                        from Service_Link ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ServiceLinkInfo> list = new List<ServiceLinkInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ServiceLinkInfo model = new ServiceLinkInfo();
                        model.Id = reader.GetGuid(0);
                        model.ServiceItemId = reader.GetGuid(1);
                        model.PictureId = reader.GetGuid(2);
                        model.Named = reader.GetString(3);
                        model.Url = reader.GetString(4);
                        model.Sort = reader.GetInt32(5);
                        model.EnableStartTime = reader.GetDateTime(6);
                        model.EnableEndTime = reader.GetDateTime(7);
                        model.IsDisable = reader.GetBoolean(8);
                        model.LastUpdatedDate = reader.GetDateTime(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ServiceLinkInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ServiceItemId,PictureId,Named,Url,Sort,EnableStartTime,EnableEndTime,IsDisable,LastUpdatedDate 
			            from Service_Link
					    order by LastUpdatedDate desc ");

            IList<ServiceLinkInfo> list = new List<ServiceLinkInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ServiceLinkInfo model = new ServiceLinkInfo();
                        model.Id = reader.GetGuid(0);
                        model.ServiceItemId = reader.GetGuid(1);
                        model.PictureId = reader.GetGuid(2);
                        model.Named = reader.GetString(3);
                        model.Url = reader.GetString(4);
                        model.Sort = reader.GetInt32(5);
                        model.EnableStartTime = reader.GetDateTime(6);
                        model.EnableEndTime = reader.GetDateTime(7);
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
