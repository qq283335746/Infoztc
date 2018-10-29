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
    public partial class InformationPicture : IInformationPicture
    {
        #region IInformationPicture Member

        public int Insert(InformationPictureInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into InformationPicture (PictureId)
			            values
						(@PictureId)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.PictureId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(InformationPictureInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update InformationPicture set PictureId = @PictureId 
			            where InformationId = @InformationId
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@InformationId",SqlDbType.UniqueIdentifier),
new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.InformationId;
parms[1].Value = model.PictureId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object InformationId)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from InformationPicture where InformationId = @InformationId");
            SqlParameter parm = new SqlParameter("@InformationId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(InformationId.ToString());

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
                sb.Append(@"delete from InformationPicture where InformationId = @InformationId" + n + " ;");
                SqlParameter parm = new SqlParameter("@InformationId" + n + "", SqlDbType.UniqueIdentifier);
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

        public InformationPictureInfo GetModel(object InformationId)
        {
            InformationPictureInfo model = null;

			StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 InformationId,PictureId 
			            from InformationPicture
						where InformationId = @InformationId ");
            SqlParameter parm = new SqlParameter("@InformationId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(InformationId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new InformationPictureInfo();
                        model.InformationId = reader.GetGuid(0);
model.PictureId = reader.GetGuid(1);
                    }
                }
            }

            return model;
        }

        public IList<InformationPictureInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from InformationPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

			if (totalRecords == 0) return new List<InformationPictureInfo>();

			sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          InformationId,PictureId
					  from InformationPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
			sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<InformationPictureInfo> list = new List<InformationPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationPictureInfo model = new InformationPictureInfo();
                        model.InformationId = reader.GetGuid(1);
model.PictureId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<InformationPictureInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           InformationId,PictureId
					   from InformationPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<InformationPictureInfo> list = new List<InformationPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationPictureInfo model = new InformationPictureInfo();
                        model.InformationId = reader.GetGuid(1);
model.PictureId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<InformationPictureInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select InformationId,PictureId
                        from InformationPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<InformationPictureInfo> list = new List<InformationPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationPictureInfo model = new InformationPictureInfo();
                        model.InformationId = reader.GetGuid(0);
model.PictureId = reader.GetGuid(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<InformationPictureInfo> GetList()
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select InformationId,PictureId 
			            from InformationPicture
					    order by LastUpdatedDate desc ");

            IList<InformationPictureInfo> list = new List<InformationPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationPictureInfo model = new InformationPictureInfo();
                        model.InformationId = reader.GetGuid(0);
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
