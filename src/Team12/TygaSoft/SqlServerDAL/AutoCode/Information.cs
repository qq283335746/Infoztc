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
    public partial class Information : IInformation
    {
        #region IInformation Member

        public int Insert(InformationInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Information (Title,Summary,ContentText,Source,ViewCount,Sort,ViewType,Remark,IsDisable,UserId,LastUpdatedDate)
			            values
						(@Title,@Summary,@ContentText,@Source,@ViewCount,@Sort,@ViewType,@Remark,@IsDisable,@UserId,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@Summary",SqlDbType.NVarChar,200),
new SqlParameter("@ContentText",SqlDbType.NVarChar,4000),
new SqlParameter("@Source",SqlDbType.NVarChar,100),
new SqlParameter("@ViewCount",SqlDbType.BigInt),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@ViewType",SqlDbType.TinyInt),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Title;
parms[1].Value = model.Summary;
parms[2].Value = model.ContentText;
parms[3].Value = model.Source;
parms[4].Value = model.ViewCount;
parms[5].Value = model.Sort;
parms[6].Value = model.ViewType;
parms[7].Value = model.Remark;
parms[8].Value = model.IsDisable;
parms[9].Value = model.UserId;
parms[10].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(InformationInfo model)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update Information set Title = @Title,Summary = @Summary,ContentText = @ContentText,Source = @Source,ViewCount = @ViewCount,Sort = @Sort,ViewType = @ViewType,Remark = @Remark,IsDisable = @IsDisable,UserId = @UserId,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@Summary",SqlDbType.NVarChar,200),
new SqlParameter("@ContentText",SqlDbType.NVarChar,4000),
new SqlParameter("@Source",SqlDbType.NVarChar,100),
new SqlParameter("@ViewCount",SqlDbType.BigInt),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@ViewType",SqlDbType.TinyInt),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
parms[1].Value = model.Title;
parms[2].Value = model.Summary;
parms[3].Value = model.ContentText;
parms[4].Value = model.Source;
parms[5].Value = model.ViewCount;
parms[6].Value = model.Sort;
parms[7].Value = model.ViewType;
parms[8].Value = model.Remark;
parms[9].Value = model.IsDisable;
parms[10].Value = model.UserId;
parms[11].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from Information where Id = @Id");
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
                sb.Append(@"delete from Information where Id = @Id" + n + " ;");
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

        public InformationInfo GetModel(object Id)
        {
            InformationInfo model = null;

			StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,Title,Summary,ContentText,Source,ViewCount,Sort,ViewType,Remark,IsDisable,UserId,LastUpdatedDate 
			            from Information
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new InformationInfo();
                        model.Id = reader.GetGuid(0);
model.Title = reader.GetString(1);
model.Summary = reader.GetString(2);
model.ContentText = reader.GetString(3);
model.Source = reader.GetString(4);
model.ViewCount = reader.GetInt64(5);
model.Sort = reader.GetInt32(6);
model.ViewType = reader.GetByte(7);
model.Remark = reader.GetString(8);
model.IsDisable = reader.GetBoolean(9);
model.UserId = reader.GetGuid(10);
model.LastUpdatedDate = reader.GetDateTime(11);
                    }
                }
            }

            return model;
        }

        public IList<InformationInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Information ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

			if (totalRecords == 0) return new List<InformationInfo>();

			sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,Title,Summary,ContentText,Source,ViewCount,Sort,ViewType,Remark,IsDisable,UserId,LastUpdatedDate
					  from Information ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
			sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<InformationInfo> list = new List<InformationInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationInfo model = new InformationInfo();
                        model.Id = reader.GetGuid(1);
model.Title = reader.GetString(2);
model.Summary = reader.GetString(3);
model.ContentText = reader.GetString(4);
model.Source = reader.GetString(5);
model.ViewCount = reader.GetInt64(6);
model.Sort = reader.GetInt32(7);
model.ViewType = reader.GetByte(8);
model.Remark = reader.GetString(9);
model.IsDisable = reader.GetBoolean(10);
model.UserId = reader.GetGuid(11);
model.LastUpdatedDate = reader.GetDateTime(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<InformationInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,Title,Summary,ContentText,Source,ViewCount,Sort,ViewType,Remark,IsDisable,UserId,LastUpdatedDate
					   from Information ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<InformationInfo> list = new List<InformationInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationInfo model = new InformationInfo();
                        model.Id = reader.GetGuid(1);
model.Title = reader.GetString(2);
model.Summary = reader.GetString(3);
model.ContentText = reader.GetString(4);
model.Source = reader.GetString(5);
model.ViewCount = reader.GetInt64(6);
model.Sort = reader.GetInt32(7);
model.ViewType = reader.GetByte(8);
model.Remark = reader.GetString(9);
model.IsDisable = reader.GetBoolean(10);
model.UserId = reader.GetGuid(11);
model.LastUpdatedDate = reader.GetDateTime(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<InformationInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,Summary,ContentText,Source,ViewCount,Sort,ViewType,Remark,IsDisable,UserId,LastUpdatedDate
                        from Information ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<InformationInfo> list = new List<InformationInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationInfo model = new InformationInfo();
                        model.Id = reader.GetGuid(0);
model.Title = reader.GetString(1);
model.Summary = reader.GetString(2);
model.ContentText = reader.GetString(3);
model.Source = reader.GetString(4);
model.ViewCount = reader.GetInt64(5);
model.Sort = reader.GetInt32(6);
model.ViewType = reader.GetByte(7);
model.Remark = reader.GetString(8);
model.IsDisable = reader.GetBoolean(9);
model.UserId = reader.GetGuid(10);
model.LastUpdatedDate = reader.GetDateTime(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<InformationInfo> GetList()
        {
		    StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,Summary,ContentText,Source,ViewCount,Sort,ViewType,Remark,IsDisable,UserId,LastUpdatedDate 
			            from Information
					    order by LastUpdatedDate desc ");

            IList<InformationInfo> list = new List<InformationInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InformationInfo model = new InformationInfo();
                        model.Id = reader.GetGuid(0);
model.Title = reader.GetString(1);
model.Summary = reader.GetString(2);
model.ContentText = reader.GetString(3);
model.Source = reader.GetString(4);
model.ViewCount = reader.GetInt64(5);
model.Sort = reader.GetInt32(6);
model.ViewType = reader.GetByte(7);
model.Remark = reader.GetString(8);
model.IsDisable = reader.GetBoolean(9);
model.UserId = reader.GetGuid(10);
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
