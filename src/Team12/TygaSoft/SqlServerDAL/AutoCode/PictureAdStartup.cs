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
    public partial class PictureAdStartup : IPictureAdStartup
    {
        #region IPictureAdStartup Member

        public int Insert(PictureAdStartupInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Picture_AdStartup (UserId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder,LastUpdatedDate)
			            values
						(@UserId,@FileName,@FileSize,@FileExtension,@FileDirectory,@RandomFolder,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@FileName",SqlDbType.NVarChar,100),
new SqlParameter("@FileSize",SqlDbType.Int),
new SqlParameter("@FileExtension",SqlDbType.VarChar,10),
new SqlParameter("@FileDirectory",SqlDbType.NVarChar,100),
new SqlParameter("@RandomFolder",SqlDbType.VarChar,20),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.UserId;
            parms[1].Value = model.FileName;
            parms[2].Value = model.FileSize;
            parms[3].Value = model.FileExtension;
            parms[4].Value = model.FileDirectory;
            parms[5].Value = model.RandomFolder;
            parms[6].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(PictureAdStartupInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update Picture_AdStartup set UserId = @UserId,FileName = @FileName,FileSize = @FileSize,FileExtension = @FileExtension,FileDirectory = @FileDirectory,RandomFolder = @RandomFolder,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@FileName",SqlDbType.NVarChar,100),
new SqlParameter("@FileSize",SqlDbType.Int),
new SqlParameter("@FileExtension",SqlDbType.VarChar,10),
new SqlParameter("@FileDirectory",SqlDbType.NVarChar,100),
new SqlParameter("@RandomFolder",SqlDbType.VarChar,20),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.UserId;
            parms[2].Value = model.FileName;
            parms[3].Value = model.FileSize;
            parms[4].Value = model.FileExtension;
            parms[5].Value = model.FileDirectory;
            parms[6].Value = model.RandomFolder;
            parms[7].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from Picture_AdStartup where Id = @Id");
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
                sb.Append(@"delete from Picture_AdStartup where Id = @Id" + n + " ;");
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

        public PictureAdStartupInfo GetModel(object Id)
        {
            PictureAdStartupInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,UserId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder,LastUpdatedDate 
			            from Picture_AdStartup
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new PictureAdStartupInfo();
                        model.Id = reader.GetGuid(0);
                        model.UserId = reader.GetGuid(1);
                        model.FileName = reader.GetString(2);
                        model.FileSize = reader.GetInt32(3);
                        model.FileExtension = reader.GetString(4);
                        model.FileDirectory = reader.GetString(5);
                        model.RandomFolder = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                    }
                }
            }

            return model;
        }

        public IList<PictureAdStartupInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Picture_AdStartup ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<PictureAdStartupInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,UserId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder,LastUpdatedDate
					  from Picture_AdStartup ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<PictureAdStartupInfo> list = new List<PictureAdStartupInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PictureAdStartupInfo model = new PictureAdStartupInfo();
                        model.Id = reader.GetGuid(1);
                        model.UserId = reader.GetGuid(2);
                        model.FileName = reader.GetString(3);
                        model.FileSize = reader.GetInt32(4);
                        model.FileExtension = reader.GetString(5);
                        model.FileDirectory = reader.GetString(6);
                        model.RandomFolder = reader.GetString(7);
                        model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<PictureAdStartupInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,UserId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder,LastUpdatedDate
					   from Picture_AdStartup ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<PictureAdStartupInfo> list = new List<PictureAdStartupInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PictureAdStartupInfo model = new PictureAdStartupInfo();
                        model.Id = reader.GetGuid(1);
                        model.UserId = reader.GetGuid(2);
                        model.FileName = reader.GetString(3);
                        model.FileSize = reader.GetInt32(4);
                        model.FileExtension = reader.GetString(5);
                        model.FileDirectory = reader.GetString(6);
                        model.RandomFolder = reader.GetString(7);
                        model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<PictureAdStartupInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,UserId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder,LastUpdatedDate
                        from Picture_AdStartup ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<PictureAdStartupInfo> list = new List<PictureAdStartupInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PictureAdStartupInfo model = new PictureAdStartupInfo();
                        model.Id = reader.GetGuid(0);
                        model.UserId = reader.GetGuid(1);
                        model.FileName = reader.GetString(2);
                        model.FileSize = reader.GetInt32(3);
                        model.FileExtension = reader.GetString(4);
                        model.FileDirectory = reader.GetString(5);
                        model.RandomFolder = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<PictureAdStartupInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,UserId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder,LastUpdatedDate 
			            from Picture_AdStartup
					    order by LastUpdatedDate desc ");

            IList<PictureAdStartupInfo> list = new List<PictureAdStartupInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PictureAdStartupInfo model = new PictureAdStartupInfo();
                        model.Id = reader.GetGuid(0);
                        model.UserId = reader.GetGuid(1);
                        model.FileName = reader.GetString(2);
                        model.FileSize = reader.GetInt32(3);
                        model.FileExtension = reader.GetString(4);
                        model.FileDirectory = reader.GetString(5);
                        model.RandomFolder = reader.GetString(6);
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
