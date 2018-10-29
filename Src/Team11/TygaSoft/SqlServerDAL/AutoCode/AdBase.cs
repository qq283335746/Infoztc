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
    public partial class AdBase : IAdBase
    {
        #region IAdBase Member

        public int Insert(AdBaseInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into AdBase (Title,SiteFunId,LayoutPositionId,Timeout,Sort,StartTime,EndTime,VirtualViewCount,ViewCount,IsDisable,LastUpdatedDate)
			            values
						(@Title,@SiteFunId,@LayoutPositionId,@Timeout,@Sort,@StartTime,@EndTime,@VirtualViewCount,@ViewCount,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Title",SqlDbType.NVarChar,100),
                                        new SqlParameter("@SiteFunId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@LayoutPositionId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Timeout",SqlDbType.Int),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@StartTime",SqlDbType.DateTime),
                                        new SqlParameter("@EndTime",SqlDbType.DateTime),
                                        new SqlParameter("@VirtualViewCount",SqlDbType.Int),
                                        new SqlParameter("@ViewCount",SqlDbType.Int),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Title;
            parms[1].Value = model.SiteFunId;
            parms[2].Value = model.LayoutPositionId;
            parms[3].Value = model.Timeout;
            parms[4].Value = model.Sort;
            parms[5].Value = model.StartTime;
            parms[6].Value = model.EndTime;
            parms[7].Value = model.VirtualViewCount;
            parms[8].Value = model.ViewCount;
            parms[9].Value = model.IsDisable;
            parms[10].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(AdBaseInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update AdBase set Title = @Title,SiteFunId = @SiteFunId,LayoutPositionId = @LayoutPositionId,Timeout = @Timeout,Sort = @Sort,StartTime = @StartTime,EndTime = @EndTime,VirtualViewCount = @VirtualViewCount,ViewCount = @ViewCount,IsDisable = @IsDisable,LastUpdatedDate = @LastUpdatedDate 
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
                                        new SqlParameter("@ViewCount",SqlDbType.Int),
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
            parms[9].Value = model.ViewCount;
            parms[10].Value = model.IsDisable;
            parms[11].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from AdBase where Id = @Id");
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
                sb.Append(@"delete from AdBase where Id = @Id" + n + " ;");
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

        public AdBaseInfo GetModel(object Id)
        {
            AdBaseInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,Title,SiteFunId,LayoutPositionId,Timeout,Sort,StartTime,EndTime,VirtualViewCount,ViewCount,IsDisable,LastUpdatedDate 
			            from AdBase
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new AdBaseInfo();
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

        public IList<AdBaseInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from AdBase ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<AdBaseInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,Title,SiteFunId,LayoutPositionId,Timeout,Sort,StartTime,EndTime,VirtualViewCount,ViewCount,IsDisable,LastUpdatedDate
					  from AdBase ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<AdBaseInfo> list = new List<AdBaseInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdBaseInfo model = new AdBaseInfo();
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

        public IList<AdBaseInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,Title,SiteFunId,LayoutPositionId,Timeout,Sort,StartTime,EndTime,VirtualViewCount,ViewCount,IsDisable,LastUpdatedDate
					   from AdBase ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<AdBaseInfo> list = new List<AdBaseInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdBaseInfo model = new AdBaseInfo();
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

        public IList<AdBaseInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,SiteFunId,LayoutPositionId,Timeout,Sort,StartTime,EndTime,VirtualViewCount,ViewCount,IsDisable,LastUpdatedDate
                        from AdBase ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<AdBaseInfo> list = new List<AdBaseInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdBaseInfo model = new AdBaseInfo();
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

        public IList<AdBaseInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,SiteFunId,LayoutPositionId,Timeout,Sort,StartTime,EndTime,VirtualViewCount,ViewCount,IsDisable,LastUpdatedDate 
			            from AdBase
					    order by LastUpdatedDate desc ");

            IList<AdBaseInfo> list = new List<AdBaseInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdBaseInfo model = new AdBaseInfo();
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
