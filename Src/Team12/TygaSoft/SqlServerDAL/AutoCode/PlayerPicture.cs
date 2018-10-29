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
    public partial class PlayerPicture : IPlayerPicture
    {
        #region IPlayerPicture Member

        public int Insert(PlayerPictureInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into PlayerPicture (PlayerId,PictureId)
			            values
						(@PlayerId,@PictureId)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@PlayerId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.PlayerId;
            parms[1].Value = model.PictureId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(PlayerPictureInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update PlayerPicture set PictureId = @PictureId 
			            where PlayerId = @PlayerId
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@PlayerId",SqlDbType.UniqueIdentifier),
new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.PlayerId;
parms[1].Value = model.PictureId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object PlayerId)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from PlayerPicture where PlayerId = @PlayerId");
            SqlParameter parm = new SqlParameter("@PlayerId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(PlayerId.ToString());

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
                sb.Append(@"delete from PlayerPicture where PlayerId = @PlayerId" + n + " ;");
                SqlParameter parm = new SqlParameter("@PlayerId" + n + "", SqlDbType.UniqueIdentifier);
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

        public PlayerPictureInfo GetModel(object PlayerId)
        {
            PlayerPictureInfo model = null;

			StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 PlayerId,PictureId 
			            from PlayerPicture
						where PlayerId = @PlayerId ");
            SqlParameter parm = new SqlParameter("@PlayerId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(PlayerId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new PlayerPictureInfo();
                        model.PlayerId = reader.GetGuid(0);
model.PictureId = reader.GetGuid(1);
                    }
                }
            }

            return model;
        }

        public IList<PlayerPictureInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from PlayerPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

			if (totalRecords == 0) return new List<PlayerPictureInfo>();

			sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          PlayerId,PictureId
					  from PlayerPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
			sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<PlayerPictureInfo> list = new List<PlayerPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PlayerPictureInfo model = new PlayerPictureInfo();
                        model.PlayerId = reader.GetGuid(1);
model.PictureId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<PlayerPictureInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           PlayerId,PictureId
					   from PlayerPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<PlayerPictureInfo> list = new List<PlayerPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PlayerPictureInfo model = new PlayerPictureInfo();
                        model.PlayerId = reader.GetGuid(1);
model.PictureId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<PlayerPictureInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select PlayerId,PictureId
                        from PlayerPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<PlayerPictureInfo> list = new List<PlayerPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PlayerPictureInfo model = new PlayerPictureInfo();
                        model.PlayerId = reader.GetGuid(0);
model.PictureId = reader.GetGuid(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<PlayerPictureInfo> GetList()
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select PlayerId,PictureId 
			            from PlayerPicture
					    order by LastUpdatedDate desc ");

            IList<PlayerPictureInfo> list = new List<PlayerPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PlayerPictureInfo model = new PlayerPictureInfo();
                        model.PlayerId = reader.GetGuid(0);
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
