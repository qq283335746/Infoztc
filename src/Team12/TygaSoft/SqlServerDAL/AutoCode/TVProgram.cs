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
    public partial class TVProgram : ITVProgram
    {
        #region ITVProgram Member

        public int Insert(TVProgramInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into TVProgram (ProgramName,HWTVId,ProgramAddress,Time,IsDisable,Sort,LastUpdatedDate,TVScID )
			            values
						(@ProgramName,@HWTVId,@ProgramAddress,@Time,@IsDisable,@Sort,@LastUpdatedDate, @TVScID)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ProgramName",SqlDbType.NVarChar,100),
new SqlParameter("@HWTVId",SqlDbType.UniqueIdentifier),
new SqlParameter("@ProgramAddress",SqlDbType.NVarChar,300),
new SqlParameter("@Time",SqlDbType.Int),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime),
new SqlParameter("@TVScID" ,SqlDbType.NVarChar,100)
                                   };
            parms[0].Value = model.ProgramName;
            parms[1].Value = model.HWTVId;
            parms[2].Value = model.ProgramAddress;
            parms[3].Value = model.Time;
            parms[4].Value = model.IsDisable;
            parms[5].Value = model.Sort;
            parms[6].Value = model.LastUpdatedDate;
            parms[7].Value = model.TVScID;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(TVProgramInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update TVProgram set ProgramName = @ProgramName,HWTVId = @HWTVId,ProgramAddress = @ProgramAddress,Time = @Time,IsDisable = @IsDisable,Sort = @Sort,LastUpdatedDate = @LastUpdatedDate ,TVScID = @TVScID
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@ProgramName",SqlDbType.NVarChar,100),
new SqlParameter("@HWTVId",SqlDbType.UniqueIdentifier),
new SqlParameter("@ProgramAddress",SqlDbType.NVarChar,300),
new SqlParameter("@Time",SqlDbType.Int),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime),
new SqlParameter("@TVScID" ,SqlDbType.NVarChar,100)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.ProgramName;
            parms[2].Value = model.HWTVId;
            parms[3].Value = model.ProgramAddress;
            parms[4].Value = model.Time;
            parms[5].Value = model.IsDisable;
            parms[6].Value = model.Sort;
            parms[7].Value = model.LastUpdatedDate;
            parms[8].Value = model.TVScID;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from TVProgram where Id = @Id");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

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
                sb.Append(@"delete from TVProgram where Id = @Id" + n + " ;");
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

        public TVProgramInfo GetModel(object Id)
        {
            TVProgramInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,ProgramName,HWTVId,ProgramAddress,Time,IsDisable,Sort,LastUpdatedDate ,TVScID
			                   from TVProgram
							   where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new TVProgramInfo();
                        model.Id = reader.GetGuid(0);
                        model.ProgramName = reader.GetString(1);
                        model.HWTVId = reader.GetGuid(2);
                        model.ProgramAddress = reader.GetString(3);
                        model.Time = reader.GetInt32(4);
                        model.IsDisable = reader.GetBoolean(5);
                        if (!reader.IsDBNull(6))
                        {
                            model.Sort = reader.GetInt32(6);
                        }
                        model.LastUpdatedDate = reader.GetDateTime(7);
                        if (!reader.IsDBNull(8))
                        {
                            model.TVScID = reader.GetString(8);
                        }
                    }
                }
            }

            return model;
        }

        public IList<TVProgramInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from TVProgram ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<TVProgramInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by t.Sort,t.LastUpdatedDate desc) as RowNumber,h.HWTVName,
			          t.Id,ProgramName,HWTVId,t.ProgramAddress,Time,t.IsDisable,t.Sort,t.LastUpdatedDate
					  from TVProgram AS t
					  LEFT JOIN HWTV AS h ON h.Id = t.HWTVId");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<TVProgramInfo> list = new List<TVProgramInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TVProgramInfo model = new TVProgramInfo();
                        model.Id = reader.GetGuid(1);
                        model.ProgramName = reader.GetString(2);
                        model.HWTVId = reader.GetGuid(3);
                        model.ProgramAddress = reader.GetString(4);
                        model.Time = reader.GetInt32(5);
                        model.IsDisable = reader.GetBoolean(6);
                        model.Sort = reader.GetInt32(7);
                        model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public DataTable GetTable(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from TVProgram t ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return null;

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by t.Sort,t.LastUpdatedDate desc) as RowNumber,h.HWTVName,
			          t.Id,ProgramName,HWTVId,t.ProgramAddress,Time,t.IsDisable,t.Sort,t.LastUpdatedDate,TVScID
					  from TVProgram AS t
					  LEFT JOIN HWTV AS h ON h.Id = t.HWTVId");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataTable dt = null;

            using (DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (ds != null && !ds.HasErrors)
                {
                    dt = ds.Tables[0];
                }
            }

            return dt;
        }


        public IList<TVProgramInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,ProgramName,HWTVId,ProgramAddress,Time,IsDisable,Sort,LastUpdatedDate
					   from TVProgram ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<TVProgramInfo> list = new List<TVProgramInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TVProgramInfo model = new TVProgramInfo();
                        model.Id = reader.GetGuid(1);
                        model.ProgramName = reader.GetString(2);
                        model.HWTVId = reader.GetGuid(3);
                        model.ProgramAddress = reader.GetString(4);
                        model.Time = reader.GetInt32(5);
                        model.IsDisable = reader.GetBoolean(6);
                        model.Sort = reader.GetInt32(7);
                        model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<TVProgramInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ProgramName,HWTVId,ProgramAddress,Time,IsDisable,Sort,LastUpdatedDate,TVScID
                        from TVProgram ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            sb.Append(" ORDER BY Sort  ");

            IList<TVProgramInfo> list = new List<TVProgramInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TVProgramInfo model = new TVProgramInfo();
                        model.Id = reader.GetGuid(0);
                        model.ProgramName = reader.GetString(1);
                        model.HWTVId = reader.GetGuid(2);
                        model.ProgramAddress = reader.GetString(3);
                        model.Time = reader.GetInt32(4);
                        model.IsDisable = reader.GetBoolean(5);
                        model.Sort = reader.GetInt32(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                        if (!reader.IsDBNull(8))
                        {
                            model.TVScID = reader.GetString(8);
                        }

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<TVProgramInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ProgramName,HWTVId,ProgramAddress,Time,IsDisable,Sort,LastUpdatedDate 
			            from TVProgram
					    order by LastUpdatedDate desc ");

            IList<TVProgramInfo> list = new List<TVProgramInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TVProgramInfo model = new TVProgramInfo();
                        model.Id = reader.GetGuid(0);
                        model.ProgramName = reader.GetString(1);
                        model.HWTVId = reader.GetGuid(2);
                        model.ProgramAddress = reader.GetString(3);
                        model.Time = reader.GetInt32(4);
                        model.IsDisable = reader.GetBoolean(5);
                        model.Sort = reader.GetInt32(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
