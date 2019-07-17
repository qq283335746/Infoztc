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
    public partial class AdvertSubject : IAdvertSubject
    {
        #region IAdvertSubject Member

        public int Insert(AdvertSubjectInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into AdvertSubject (Title,Sort,PlayTime,Remark,IsDisable,LastUpdatedDate)
			            values
						(@Title,@Sort,@PlayTime,@Remark,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@PlayTime",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Title;
            parms[1].Value = model.Sort;
            parms[2].Value = model.PlayTime;
            parms[3].Value = model.Remark;
            parms[4].Value = model.IsDisable;
            parms[5].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(AdvertSubjectInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update AdvertSubject set Title = @Title,Sort = @Sort,PlayTime = @PlayTime,Remark = @Remark,IsDisable = @IsDisable,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@PlayTime",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.Title;
            parms[2].Value = model.Sort;
            parms[3].Value = model.PlayTime;
            parms[4].Value = model.Remark;
            parms[5].Value = model.IsDisable;
            parms[6].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from AdvertSubject where Id = @Id");
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
                sb.Append(@"delete from AdvertSubject where Id = @Id" + n + " ;");
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

        public AdvertSubjectInfo GetModel(object Id)
        {
            AdvertSubjectInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,Title,Sort,PlayTime,Remark,IsDisable,LastUpdatedDate 
			            from AdvertSubject
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new AdvertSubjectInfo();
                        model.Id = reader.GetGuid(0);
                        model.Title = reader.GetString(1);
                        model.Sort = reader.GetInt32(2);
                        model.PlayTime = reader.GetInt32(3);
                        model.Remark = reader.GetString(4);
                        model.IsDisable = reader.GetBoolean(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);
                    }
                }
            }

            return model;
        }

        public IList<AdvertSubjectInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from AdvertSubject ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<AdvertSubjectInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,Title,Sort,PlayTime,Remark,IsDisable,LastUpdatedDate
					  from AdvertSubject ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<AdvertSubjectInfo> list = new List<AdvertSubjectInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdvertSubjectInfo model = new AdvertSubjectInfo();
                        model.Id = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.Sort = reader.GetInt32(3);
                        model.PlayTime = reader.GetInt32(4);
                        model.Remark = reader.GetString(5);
                        model.IsDisable = reader.GetBoolean(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AdvertSubjectInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,Title,Sort,PlayTime,Remark,IsDisable,LastUpdatedDate
					   from AdvertSubject ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<AdvertSubjectInfo> list = new List<AdvertSubjectInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdvertSubjectInfo model = new AdvertSubjectInfo();
                        model.Id = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.Sort = reader.GetInt32(3);
                        model.PlayTime = reader.GetInt32(4);
                        model.Remark = reader.GetString(5);
                        model.IsDisable = reader.GetBoolean(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AdvertSubjectInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,Sort,PlayTime,Remark,IsDisable,LastUpdatedDate
                        from AdvertSubject ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<AdvertSubjectInfo> list = new List<AdvertSubjectInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdvertSubjectInfo model = new AdvertSubjectInfo();
                        model.Id = reader.GetGuid(0);
                        model.Title = reader.GetString(1);
                        model.Sort = reader.GetInt32(2);
                        model.PlayTime = reader.GetInt32(3);
                        model.Remark = reader.GetString(4);
                        model.IsDisable = reader.GetBoolean(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AdvertSubjectInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,Title,Sort,PlayTime,Remark,IsDisable,LastUpdatedDate 
			            from AdvertSubject
					    order by LastUpdatedDate desc ");

            IList<AdvertSubjectInfo> list = new List<AdvertSubjectInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdvertSubjectInfo model = new AdvertSubjectInfo();
                        model.Id = reader.GetGuid(0);
                        model.Title = reader.GetString(1);
                        model.Sort = reader.GetInt32(2);
                        model.PlayTime = reader.GetInt32(3);
                        model.Remark = reader.GetString(4);
                        model.IsDisable = reader.GetBoolean(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
