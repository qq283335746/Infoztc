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
    public partial class PrizePicture : IPrizePicture
    {
        #region IPrizePicture Member

        public int Insert(PrizePictureInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into PrizePicture (ActivityPrizeId,PictureId)
			            values
						(@ActivityPrizeId,@PictureId)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ActivityPrizeId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.ActivityPrizeId;
            parms[1].Value = model.PictureId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(PrizePictureInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update PrizePicture set PictureId = @PictureId 
			            where ActivityPrizeId = @ActivityPrizeId
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@ActivityPrizeId",SqlDbType.UniqueIdentifier),
new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.ActivityPrizeId;
            parms[1].Value = model.PictureId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object ActivityPrizeId)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from PrizePicture where ActivityPrizeId = @ActivityPrizeId");
            SqlParameter parm = new SqlParameter("@ActivityPrizeId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ActivityPrizeId.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);
        }

        public bool DeleteBatch(IList<object> list)
        {
            bool result = false;
            StringBuilder sb = new StringBuilder(500);
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from PrizePicture where ActivityPrizeId = @ActivityPrizeId" + n + " ;");
                SqlParameter parm = new SqlParameter("@ActivityPrizeId" + n + "", SqlDbType.UniqueIdentifier);
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

        public PrizePictureInfo GetModel(object ActivityPrizeId)
        {
            PrizePictureInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 ActivityPrizeId,PictureId 
			            from PrizePicture
						where ActivityPrizeId = @ActivityPrizeId ");
            SqlParameter parm = new SqlParameter("@ActivityPrizeId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ActivityPrizeId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new PrizePictureInfo();
                        model.ActivityPrizeId = reader.GetGuid(0);
                        model.PictureId = reader.GetGuid(1);
                    }
                }
            }

            return model;
        }

        public IList<PrizePictureInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from PrizePicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<PrizePictureInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          ActivityPrizeId,PictureId
					  from PrizePicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<PrizePictureInfo> list = new List<PrizePictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PrizePictureInfo model = new PrizePictureInfo();
                        model.ActivityPrizeId = reader.GetGuid(1);
                        model.PictureId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<PrizePictureInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           ActivityPrizeId,PictureId
					   from PrizePicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<PrizePictureInfo> list = new List<PrizePictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PrizePictureInfo model = new PrizePictureInfo();
                        model.ActivityPrizeId = reader.GetGuid(1);
                        model.PictureId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<PrizePictureInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ActivityPrizeId,PictureId
                        from PrizePicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<PrizePictureInfo> list = new List<PrizePictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PrizePictureInfo model = new PrizePictureInfo();
                        model.ActivityPrizeId = reader.GetGuid(0);
                        model.PictureId = reader.GetGuid(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<PrizePictureInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ActivityPrizeId,PictureId 
			            from PrizePicture
					    order by LastUpdatedDate desc ");

            IList<PrizePictureInfo> list = new List<PrizePictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PrizePictureInfo model = new PrizePictureInfo();
                        model.ActivityPrizeId = reader.GetGuid(0);
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
