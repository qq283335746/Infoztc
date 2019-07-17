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
    public partial class InformationAd : IInformationAd
    {
        #region IInformationAd Member

        public int Insert(InformationAdInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Information_Ad (Title,Descr,ContentText,Url,ViewType,StartDate,EndDate,UserId,LastUpdatedDate)
			            values
						(@Title,@Descr,@ContentText,@Url,@ViewType,@StartDate,@EndDate,@UserId,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@Descr",SqlDbType.NVarChar,200),
new SqlParameter("@ContentText",SqlDbType.NVarChar,4000),
new SqlParameter("@Url",SqlDbType.VarChar,200),
new SqlParameter("@ViewType",SqlDbType.Int),
new SqlParameter("@StartDate",SqlDbType.DateTime),
new SqlParameter("@EndDate",SqlDbType.DateTime),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Title;
parms[1].Value = model.Descr;
parms[2].Value = model.ContentText;
parms[3].Value = model.Url;
parms[4].Value = model.ViewType;
parms[5].Value = model.StartDate;
parms[6].Value = model.EndDate;
parms[7].Value = model.UserId;
parms[8].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(InformationAdInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update Information_Ad set Title = @Title,Descr = @Descr,ContentText = @ContentText,Url = @Url,ViewType = @ViewType,StartDate = @StartDate,EndDate = @EndDate,UserId = @UserId,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@Descr",SqlDbType.NVarChar,200),
new SqlParameter("@ContentText",SqlDbType.NVarChar,4000),
new SqlParameter("@Url",SqlDbType.VarChar,200),
new SqlParameter("@ViewType",SqlDbType.Int),
new SqlParameter("@StartDate",SqlDbType.DateTime),
new SqlParameter("@EndDate",SqlDbType.DateTime),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
parms[1].Value = model.Title;
parms[2].Value = model.Descr;
parms[3].Value = model.ContentText;
parms[4].Value = model.Url;
parms[5].Value = model.ViewType;
parms[6].Value = model.StartDate;
parms[7].Value = model.EndDate;
parms[8].Value = model.UserId;
parms[9].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from Information_Ad where Id = @Id");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

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
                sb.Append(@"delete from Information_Ad where Id = @Id" + n + " ;");
                SqlParameter parm = new SqlParameter("@Id" + n + "", SqlDbType.UniqueIdentifier);
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

        public InformationAdInfo GetModel(object Id)
        {
            InformationAdInfo model = null;

			StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,Title,Descr,ContentText,Url,ViewType,StartDate,EndDate,UserId,LastUpdatedDate 
			            from Information_Ad
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new InformationAdInfo();
                        model.Id = reader.GetGuid(0);
model.Title = reader.GetString(1);
model.Descr = reader.GetString(2);
model.ContentText = reader.GetString(3);
model.Url = reader.GetString(4);
model.ViewType = reader.GetInt32(5);
model.StartDate = reader.GetDateTime(6);
model.EndDate = reader.GetDateTime(7);
model.UserId = reader.GetGuid(8);
model.LastUpdatedDate = reader.GetDateTime(9);
                    }
                }
            }

            return model;
        }

        public IList<InformationAdInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Information_Ad ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

			if (totalRecords == 0) return new List<InformationAdInfo>();

			sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,Title,Descr,ContentText,Url,ViewType,StartDate,EndDate,UserId,LastUpdatedDate
					  from Information_Ad ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
			sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<InformationAdInfo> list = new List<InformationAdInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationAdInfo model = new InformationAdInfo();
                        model.Id = reader.GetGuid(1);
model.Title = reader.GetString(2);
model.Descr = reader.GetString(3);
model.ContentText = reader.GetString(4);
model.Url = reader.GetString(5);
model.ViewType = reader.GetInt32(6);
model.StartDate = reader.GetDateTime(7);
model.EndDate = reader.GetDateTime(8);
model.UserId = reader.GetGuid(9);
model.LastUpdatedDate = reader.GetDateTime(10);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<InformationAdInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,Title,Descr,ContentText,Url,ViewType,StartDate,EndDate,UserId,LastUpdatedDate
					   from Information_Ad ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<InformationAdInfo> list = new List<InformationAdInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationAdInfo model = new InformationAdInfo();
                        model.Id = reader.GetGuid(1);
model.Title = reader.GetString(2);
model.Descr = reader.GetString(3);
model.ContentText = reader.GetString(4);
model.Url = reader.GetString(5);
model.ViewType = reader.GetInt32(6);
model.StartDate = reader.GetDateTime(7);
model.EndDate = reader.GetDateTime(8);
model.UserId = reader.GetGuid(9);
model.LastUpdatedDate = reader.GetDateTime(10);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<InformationAdInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,Descr,ContentText,Url,ViewType,StartDate,EndDate,UserId,LastUpdatedDate
                        from Information_Ad ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<InformationAdInfo> list = new List<InformationAdInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationAdInfo model = new InformationAdInfo();
                        model.Id = reader.GetGuid(0);
model.Title = reader.GetString(1);
model.Descr = reader.GetString(2);
model.ContentText = reader.GetString(3);
model.Url = reader.GetString(4);
model.ViewType = reader.GetInt32(5);
model.StartDate = reader.GetDateTime(6);
model.EndDate = reader.GetDateTime(7);
model.UserId = reader.GetGuid(8);
model.LastUpdatedDate = reader.GetDateTime(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<InformationAdInfo> GetList()
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,Descr,ContentText,Url,ViewType,StartDate,EndDate,UserId,LastUpdatedDate 
			            from Information_Ad
					    order by LastUpdatedDate desc ");

            IList<InformationAdInfo> list = new List<InformationAdInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationAdInfo model = new InformationAdInfo();
                        model.Id = reader.GetGuid(0);
model.Title = reader.GetString(1);
model.Descr = reader.GetString(2);
model.ContentText = reader.GetString(3);
model.Url = reader.GetString(4);
model.ViewType = reader.GetInt32(5);
model.StartDate = reader.GetDateTime(6);
model.EndDate = reader.GetDateTime(7);
model.UserId = reader.GetGuid(8);
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
