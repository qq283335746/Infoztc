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
    public partial class Advertisement : IAdvertisement
    {
        #region IAdvertisement Member

        public int Insert(AdvertisementInfo model)
        {
            if (IsExist(model.Title, null)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Advertisement (Id,Title,SiteFunId,LayoutPositionId,Timeout,Sort,StartTime,EndTime,VirtualViewCount,IsDisable,LastUpdatedDate)
			            values
						(@Id,@Title,@SiteFunId,@LayoutPositionId,@Timeout,@Sort,@StartTime,@EndTime,@VirtualViewCount,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@Title",SqlDbType.NVarChar,100),
                                        new SqlParameter("@SiteFunId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@LayoutPositionId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Timeout",SqlDbType.Int),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@StartTime",SqlDbType.DateTime),
                                        new SqlParameter("@EndTime",SqlDbType.DateTime),
                                        new SqlParameter("@VirtualViewCount",SqlDbType.Int),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = Guid.Parse(model.Id.ToString());
            parms[1].Value = model.Title;
            parms[2].Value = model.SiteFunId;
            parms[3].Value = model.LayoutPositionId;
            parms[4].Value = model.Timeout;
            parms[5].Value = model.Sort;
            parms[6].Value = model.StartTime;
            parms[7].Value = model.EndTime;
            parms[8].Value = model.VirtualViewCount;
            parms[9].Value = model.IsDisable;
            parms[10].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(AdvertisementInfo model)
        {
            if (IsExist(model.Title, model.Id)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update Advertisement set Title = @Title,SiteFunId = @SiteFunId,LayoutPositionId = @LayoutPositionId,Timeout = @Timeout,Sort = @Sort,StartTime = @StartTime,EndTime = @EndTime,VirtualViewCount = @VirtualViewCount,IsDisable = @IsDisable,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Title",SqlDbType.NVarChar,100),
                                        new SqlParameter("@SiteFunId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@LayoutPositionId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Timeout",SqlDbType.Int),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@StartTime",SqlDbType.DateTime),
                                        new SqlParameter("@EndTime",SqlDbType.DateTime),
                                        new SqlParameter("@VirtualViewCount",SqlDbType.Int),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.Title;
            parms[2].Value = model.SiteFunId;
            parms[3].Value = model.LayoutPositionId;
            parms[4].Value = model.Timeout;
            parms[5].Value = model.Sort;
            parms[6].Value = model.StartTime;
            parms[7].Value = model.EndTime;
            parms[8].Value = model.VirtualViewCount;
            parms[9].Value = model.IsDisable;
            parms[10].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            string cmdText = "delete from Advertisement where Id = @Id";
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
                sb.Append(@"delete from Advertisement where Id = @Id" + n + " ;");
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

        public AdvertisementInfo GetModel(object Id)
        {
            AdvertisementInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,Title,SiteFunId,LayoutPositionId,Timeout,Sort,StartTime,EndTime,VirtualViewCount,ViewCount,IsDisable,LastUpdatedDate 
			            from Advertisement
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new AdvertisementInfo();
                        model.Id = reader.GetGuid(0);
                        model.Title = reader.GetString(1);
                        model.SiteFunId = reader.GetGuid(2);
                        model.LayoutPositionId = reader.GetGuid(3);
                        model.Timeout = reader.GetInt32(4);
                        model.Sort = reader.GetInt32(5);
                        model.StartTime = reader.GetDateTime(6);
                        model.EndTime = reader.GetDateTime(7);
                        model.VirtualViewCount = reader.GetInt32(8);
                        model.ViewCount = reader.GetInt32(9);
                        model.IsDisable = reader.GetBoolean(10);
                        model.LastUpdatedDate = reader.GetDateTime(11);
                    }
                }
            }

            return model;
        }

        public IList<AdvertisementInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Advertisement ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<AdvertisementInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,Title,SiteFunId,LayoutPositionId,Timeout,Sort,StartTime,EndTime,VirtualViewCount,ViewCount,IsDisable,LastUpdatedDate
					  from Advertisement ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<AdvertisementInfo> list = new List<AdvertisementInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdvertisementInfo model = new AdvertisementInfo();
                        model.Id = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.SiteFunId = reader.GetGuid(3);
                        model.LayoutPositionId = reader.GetGuid(4);
                        model.Timeout = reader.GetInt32(5);
                        model.Sort = reader.GetInt32(6);
                        model.StartTime = reader.GetDateTime(7);
                        model.EndTime = reader.GetDateTime(8);
                        model.VirtualViewCount = reader.GetInt32(9);
                        model.ViewCount = reader.GetInt32(10);
                        model.IsDisable = reader.GetBoolean(11);
                        model.LastUpdatedDate = reader.GetDateTime(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AdvertisementInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,Title,SiteFunId,LayoutPositionId,Timeout,Sort,StartTime,EndTime,VirtualViewCount,ViewCount,IsDisable,LastUpdatedDate
					   from Advertisement ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<AdvertisementInfo> list = new List<AdvertisementInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdvertisementInfo model = new AdvertisementInfo();
                        model.Id = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.SiteFunId = reader.GetGuid(3);
                        model.LayoutPositionId = reader.GetGuid(4);
                        model.Timeout = reader.GetInt32(5);
                        model.Sort = reader.GetInt32(6);
                        model.StartTime = reader.GetDateTime(7);
                        model.EndTime = reader.GetDateTime(8);
                        model.VirtualViewCount = reader.GetInt32(9);
                        model.ViewCount = reader.GetInt32(10);
                        model.IsDisable = reader.GetBoolean(11);
                        model.LastUpdatedDate = reader.GetDateTime(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AdvertisementInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,SiteFunId,LayoutPositionId,Timeout,Sort,StartTime,EndTime,VirtualViewCount,ViewCount,IsDisable,LastUpdatedDate
                        from Advertisement ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<AdvertisementInfo> list = new List<AdvertisementInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdvertisementInfo model = new AdvertisementInfo();
                        model.Id = reader.GetGuid(0);
                        model.Title = reader.GetString(1);
                        model.SiteFunId = reader.GetGuid(2);
                        model.LayoutPositionId = reader.GetGuid(3);
                        model.Timeout = reader.GetInt32(4);
                        model.Sort = reader.GetInt32(5);
                        model.StartTime = reader.GetDateTime(6);
                        model.EndTime = reader.GetDateTime(7);
                        model.VirtualViewCount = reader.GetInt32(8);
                        model.ViewCount = reader.GetInt32(9);
                        model.IsDisable = reader.GetBoolean(10);
                        model.LastUpdatedDate = reader.GetDateTime(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AdvertisementInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,SiteFunId,LayoutPositionId,Timeout,Sort,StartTime,EndTime,VirtualViewCount,ViewCount,IsDisable,LastUpdatedDate 
			            from Advertisement
					    order by LastUpdatedDate desc ");

            IList<AdvertisementInfo> list = new List<AdvertisementInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdvertisementInfo model = new AdvertisementInfo();
                        model.Id = reader.GetGuid(0);
                        model.Title = reader.GetString(1);
                        model.SiteFunId = reader.GetGuid(2);
                        model.LayoutPositionId = reader.GetGuid(3);
                        model.Timeout = reader.GetInt32(4);
                        model.Sort = reader.GetInt32(5);
                        model.StartTime = reader.GetDateTime(6);
                        model.EndTime = reader.GetDateTime(7);
                        model.VirtualViewCount = reader.GetInt32(8);
                        model.ViewCount = reader.GetInt32(9);
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
