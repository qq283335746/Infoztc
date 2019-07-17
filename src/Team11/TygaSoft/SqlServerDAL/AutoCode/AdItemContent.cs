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
    public partial class AdItemContent : IAdItemContent
    {
        #region IAdItemContent Member

        public int Insert(AdItemContentInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into AdItemContent (AdItemId, Descr,ContentText)
			            values
						(@AdItemId,@Descr,@ContentText)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@AdItemId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@Descr",SqlDbType.NVarChar,300),
                                       new SqlParameter("@ContentText",SqlDbType.NVarChar,3000)
                                   };
            parms[0].Value = model.AdItemId;
            parms[1].Value = model.Descr;
            parms[2].Value = model.ContentText;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(AdItemContentInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update AdItemContent set Descr = @Descr,ContentText = @ContentText 
			            where AdItemId = @AdItemId
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@AdItemId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Descr",SqlDbType.NVarChar,300),
new SqlParameter("@ContentText",SqlDbType.NVarChar,3000)
                                   };
            parms[0].Value = model.AdItemId;
            parms[1].Value = model.Descr;
            parms[2].Value = model.ContentText;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object AdItemId)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from AdItemContent where AdItemId = @AdItemId");
            SqlParameter parm = new SqlParameter("@AdItemId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(AdItemId.ToString());

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
                sb.Append(@"delete from AdItemContent where AdItemId = @AdItemId" + n + " ;");
                SqlParameter parm = new SqlParameter("@AdItemId" + n + "", SqlDbType.UniqueIdentifier);
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

        public AdItemContentInfo GetModel(object AdItemId)
        {
            AdItemContentInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 AdItemId,Descr,ContentText 
			            from AdItemContent
						where AdItemId = @AdItemId ");
            SqlParameter parm = new SqlParameter("@AdItemId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(AdItemId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new AdItemContentInfo();
                        model.AdItemId = reader.GetGuid(0);
                        model.Descr = reader.GetString(1);
                        model.ContentText = reader.GetString(2);
                    }
                }
            }

            return model;
        }

        public IList<AdItemContentInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from AdItemContent ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<AdItemContentInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          AdItemId,Descr,ContentText
					  from AdItemContent ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<AdItemContentInfo> list = new List<AdItemContentInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdItemContentInfo model = new AdItemContentInfo();
                        model.AdItemId = reader.GetGuid(1);
                        model.Descr = reader.GetString(2);
                        model.ContentText = reader.GetString(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AdItemContentInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           AdItemId,Descr,ContentText
					   from AdItemContent ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<AdItemContentInfo> list = new List<AdItemContentInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdItemContentInfo model = new AdItemContentInfo();
                        model.AdItemId = reader.GetGuid(1);
                        model.Descr = reader.GetString(2);
                        model.ContentText = reader.GetString(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AdItemContentInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select AdItemId,Descr,ContentText
                        from AdItemContent ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<AdItemContentInfo> list = new List<AdItemContentInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdItemContentInfo model = new AdItemContentInfo();
                        model.AdItemId = reader.GetGuid(0);
                        model.Descr = reader.GetString(1);
                        model.ContentText = reader.GetString(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AdItemContentInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select AdItemId,Descr,ContentText 
			            from AdItemContent
					    order by LastUpdatedDate desc ");

            IList<AdItemContentInfo> list = new List<AdItemContentInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdItemContentInfo model = new AdItemContentInfo();
                        model.AdItemId = reader.GetGuid(0);
                        model.Descr = reader.GetString(1);
                        model.ContentText = reader.GetString(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
