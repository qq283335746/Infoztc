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
    public partial class ActivityPictureNew : IActivityPictureNew
    {
        #region IActivityPictureNew Member

        public int Insert(ActivityPictureNewInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ActivityPictureNew (ActivityId,PictureId)
			            values
						(@ActivityId,@PictureId)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ActivityId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.ActivityId;
            parms[1].Value = model.PictureId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ActivityPictureNewInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ActivityPictureNew set PictureId = @PictureId 
			            where ActivityId = @ActivityId
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@ActivityId",SqlDbType.UniqueIdentifier),
new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.ActivityId;
            parms[1].Value = model.PictureId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object ActivityId)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ActivityPictureNew where ActivityId = @ActivityId");
            SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ActivityId.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);
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
                sb.Append(@"delete from ActivityPictureNew where ActivityId = @ActivityId" + n + " ;");
                SqlParameter parm = new SqlParameter("@ActivityId" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(item);
                parms.Add(parm);
            }
            using (SqlConnection conn = new SqlConnection(SqlHelper.HnztcTeamDbConnString))
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

        public ActivityPictureNewInfo GetModel(object ActivityId)
        {
            ActivityPictureNewInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 ActivityId,PictureId 
			            from ActivityPictureNew
						where ActivityId = @ActivityId ");
            SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ActivityId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ActivityPictureNewInfo();
                        model.ActivityId = reader.GetGuid(0);
                        model.PictureId = reader.GetGuid(1);
                    }
                }
            }

            return model;
        }

        public IList<ActivityPictureNewInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ActivityPictureNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ActivityPictureNewInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          ActivityId,PictureId
					  from ActivityPictureNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivityPictureNewInfo> list = new List<ActivityPictureNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPictureNewInfo model = new ActivityPictureNewInfo();
                        model.ActivityId = reader.GetGuid(1);
                        model.PictureId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityPictureNewInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           ActivityId,PictureId
					   from ActivityPictureNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ActivityPictureNewInfo> list = new List<ActivityPictureNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPictureNewInfo model = new ActivityPictureNewInfo();
                        model.ActivityId = reader.GetGuid(1);
                        model.PictureId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityPictureNewInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ActivityId,PictureId
                        from ActivityPictureNew ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ActivityPictureNewInfo> list = new List<ActivityPictureNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPictureNewInfo model = new ActivityPictureNewInfo();
                        model.ActivityId = reader.GetGuid(0);
                        model.PictureId = reader.GetGuid(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ActivityPictureNewInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ActivityId,PictureId 
			            from ActivityPictureNew
					    order by LastUpdatedDate desc ");

            IList<ActivityPictureNewInfo> list = new List<ActivityPictureNewInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityPictureNewInfo model = new ActivityPictureNewInfo();
                        model.ActivityId = reader.GetGuid(0);
                        model.PictureId = reader.GetGuid(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
