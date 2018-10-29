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
    public partial class TopicPicture : ITopicPicture
    {
        #region ITopicPicture Member

        public int Insert(TopicPictureInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into TopicPicture (TopicSubjectId,PictureId)
			            values
						(@TopicSubjectId,@PictureId)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@TopicSubjectId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.TopicSubjectId;
            parms[1].Value = model.PictureId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(TopicPictureInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update TopicPicture set PictureId = @PictureId 
			            where TopicSubjectId = @TopicSubjectId
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@TopicSubjectId",SqlDbType.UniqueIdentifier),
new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.TopicSubjectId;
parms[1].Value = model.PictureId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object TopicSubjectId)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from TopicPicture where TopicSubjectId = @TopicSubjectId");
            SqlParameter parm = new SqlParameter("@TopicSubjectId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(TopicSubjectId.ToString());

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
                sb.Append(@"delete from TopicPicture where TopicSubjectId = @TopicSubjectId" + n + " ;");
                SqlParameter parm = new SqlParameter("@TopicSubjectId" + n + "", SqlDbType.UniqueIdentifier);
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

        public TopicPictureInfo GetModel(object TopicSubjectId)
        {
            TopicPictureInfo model = null;

			StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 TopicSubjectId,PictureId 
			                   from TopicPicture
							   where TopicSubjectId = @TopicSubjectId ");
            SqlParameter parm = new SqlParameter("@TopicSubjectId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(TopicSubjectId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new TopicPictureInfo();
                        model.TopicSubjectId = reader.GetGuid(0);
model.PictureId = reader.GetGuid(1);
                    }
                }
            }

            return model;
        }

        public IList<TopicPictureInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from TopicPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

			if (totalRecords == 0) return new List<TopicPictureInfo>();

			sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          TopicSubjectId,PictureId
					  from TopicPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
			sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<TopicPictureInfo> list = new List<TopicPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TopicPictureInfo model = new TopicPictureInfo();
                        model.TopicSubjectId = reader.GetGuid(1);
model.PictureId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<TopicPictureInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           TopicSubjectId,PictureId
					   from TopicPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<TopicPictureInfo> list = new List<TopicPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TopicPictureInfo model = new TopicPictureInfo();
                        model.TopicSubjectId = reader.GetGuid(1);
model.PictureId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<TopicPictureInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select TopicSubjectId,PictureId
                        from TopicPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<TopicPictureInfo> list = new List<TopicPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TopicPictureInfo model = new TopicPictureInfo();
                        model.TopicSubjectId = reader.GetGuid(0);
model.PictureId = reader.GetGuid(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<TopicPictureInfo> GetList()
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select TopicSubjectId,PictureId 
			            from TopicPicture
					    order by LastUpdatedDate desc ");

            IList<TopicPictureInfo> list = new List<TopicPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TopicPictureInfo model = new TopicPictureInfo();
                        model.TopicSubjectId = reader.GetGuid(0);
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
