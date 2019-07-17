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
    public partial class InformationAdPicture : IInformationAdPicture
    {
        #region IInformationAdPicture Member

        public int Insert(InformationAdPictureInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into InformationAdPicture (PictureId)
			            values
						(@PictureId)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.PictureId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(InformationAdPictureInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update InformationAdPicture set PictureId = @PictureId 
			            where InformationAdId = @InformationAdId
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@InformationAdId",SqlDbType.UniqueIdentifier),
new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.InformationAdId;
parms[1].Value = model.PictureId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object InformationAdId)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from InformationAdPicture where InformationAdId = @InformationAdId");
            SqlParameter parm = new SqlParameter("@InformationAdId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(InformationAdId.ToString());

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
                sb.Append(@"delete from InformationAdPicture where InformationAdId = @InformationAdId" + n + " ;");
                SqlParameter parm = new SqlParameter("@InformationAdId" + n + "", SqlDbType.UniqueIdentifier);
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

        public InformationAdPictureInfo GetModel(object InformationAdId)
        {
            InformationAdPictureInfo model = null;

			StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 InformationAdId,PictureId 
			            from InformationAdPicture
						where InformationAdId = @InformationAdId ");
            SqlParameter parm = new SqlParameter("@InformationAdId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(InformationAdId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new InformationAdPictureInfo();
                        model.InformationAdId = reader.GetGuid(0);
model.PictureId = reader.GetGuid(1);
                    }
                }
            }

            return model;
        }

        public IList<InformationAdPictureInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from InformationAdPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

			if (totalRecords == 0) return new List<InformationAdPictureInfo>();

			sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          InformationAdId,PictureId
					  from InformationAdPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
			sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<InformationAdPictureInfo> list = new List<InformationAdPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationAdPictureInfo model = new InformationAdPictureInfo();
                        model.InformationAdId = reader.GetGuid(1);
model.PictureId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<InformationAdPictureInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           InformationAdId,PictureId
					   from InformationAdPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<InformationAdPictureInfo> list = new List<InformationAdPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationAdPictureInfo model = new InformationAdPictureInfo();
                        model.InformationAdId = reader.GetGuid(1);
model.PictureId = reader.GetGuid(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<InformationAdPictureInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select InformationAdId,PictureId
                        from InformationAdPicture ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<InformationAdPictureInfo> list = new List<InformationAdPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationAdPictureInfo model = new InformationAdPictureInfo();
                        model.InformationAdId = reader.GetGuid(0);
model.PictureId = reader.GetGuid(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<InformationAdPictureInfo> GetList()
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select InformationAdId,PictureId 
			            from InformationAdPicture
					    order by LastUpdatedDate desc ");

            IList<InformationAdPictureInfo> list = new List<InformationAdPictureInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationAdPictureInfo model = new InformationAdPictureInfo();
                        model.InformationAdId = reader.GetGuid(0);
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
