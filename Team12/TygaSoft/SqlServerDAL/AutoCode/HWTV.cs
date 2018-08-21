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
    public partial class HWTV : IHWTV
    {
        #region IHWTV Member

        public int Insert(HWTVInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into HWTV (HWTVName,HWTVIcon,ProgramAddress,IsTurnTo,IsDisable,Sort,LastUpdatedDate)
			            values
						(@HWTVName,@HWTVIcon,@ProgramAddress,@IsTurnTo,@IsDisable,@Sort,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@HWTVName",SqlDbType.NVarChar,100),
new SqlParameter("@HWTVIcon",SqlDbType.UniqueIdentifier),
new SqlParameter("@ProgramAddress",SqlDbType.NVarChar,300),
new SqlParameter("@IsTurnTo",SqlDbType.Bit),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.HWTVName;
            if (model.HWTVIcon != null && !string.IsNullOrEmpty(model.HWTVIcon.ToString()))
            {
                parms[1].Value = Guid.Parse(model.HWTVIcon.ToString());
            }
            else
            {
                parms[1].Value = null;
            }
            parms[2].Value = model.ProgramAddress;
            parms[3].Value = model.IsTurnTo;
            parms[4].Value = model.IsDisable;
            parms[5].Value = model.Sort;
            parms[6].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(HWTVInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update HWTV set HWTVName = @HWTVName,HWTVIcon = @HWTVIcon,ProgramAddress = @ProgramAddress,IsTurnTo = @IsTurnTo,IsDisable = @IsDisable,Sort = @Sort,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@HWTVName",SqlDbType.NVarChar,100),
new SqlParameter("@HWTVIcon",SqlDbType.UniqueIdentifier),
new SqlParameter("@ProgramAddress",SqlDbType.NVarChar,300),
new SqlParameter("@IsTurnTo",SqlDbType.Bit),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = Guid.Parse(model.Id.ToString());
            parms[1].Value = model.HWTVName;
            if (model.HWTVIcon != null && !string.IsNullOrEmpty(model.HWTVIcon.ToString()))
            {
                parms[2].Value = Guid.Parse(model.HWTVIcon.ToString());
            }
            else
            {
                parms[2].Value = null;
            }
            parms[3].Value = model.ProgramAddress;
            parms[4].Value = model.IsTurnTo;
            parms[5].Value = model.IsDisable;
            parms[6].Value = model.Sort;
            parms[7].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from HWTV where Id = @Id");
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
                sb.Append(@"delete from HWTV where Id = @Id" + n + " ;");
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

        public HWTVInfo GetModel(object Id)
        {
            HWTVInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,HWTVName,HWTVIcon,ProgramAddress,IsTurnTo,IsDisable,Sort,LastUpdatedDate 
			                   from HWTV
							   where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new HWTVInfo();
                        model.Id = reader.GetGuid(0);
                        model.HWTVName = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                        {
                            model.HWTVIcon = reader.GetGuid(2);
                        }
                        model.ProgramAddress = reader.GetString(3);
                        model.IsTurnTo = reader.GetBoolean(4);
                        model.IsDisable = reader.GetBoolean(5);
                        model.Sort = reader.GetInt32(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                    }
                }
            }

            return model;
        }

        public IList<HWTVInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from HWTV ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<HWTVInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by Sort,LastUpdatedDate desc) as RowNumber,
			          Id,HWTVName,HWTVIcon,ProgramAddress,IsTurnTo,IsDisable,Sort,LastUpdatedDate
					  from HWTV ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<HWTVInfo> list = new List<HWTVInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        HWTVInfo model = new HWTVInfo();
                        model.Id = reader.GetGuid(1);
                        model.HWTVName = reader.GetString(2);
                        if (!reader.IsDBNull(3))
                        {
                            model.HWTVIcon = reader.GetGuid(3);
                        }
                        model.ProgramAddress = reader.GetString(4);
                        model.IsTurnTo = reader.GetBoolean(5);
                        model.IsDisable = reader.GetBoolean(6);
                        model.Sort = reader.GetInt32(7);
                        model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<HWTVInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by Sort,LastUpdatedDate desc) as RowNumber,
			           Id,HWTVName,HWTVIcon,ProgramAddress,IsTurnTo,IsDisable,Sort,LastUpdatedDate
					   from HWTV ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<HWTVInfo> list = new List<HWTVInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        HWTVInfo model = new HWTVInfo();
                        model.Id = reader.GetGuid(1);
                        model.HWTVName = reader.GetString(2);
                        if (!reader.IsDBNull(3))
                        {
                            model.HWTVIcon = reader.GetGuid(3);
                        }
                        model.ProgramAddress = reader.GetString(4);
                        model.IsTurnTo = reader.GetBoolean(5);
                        model.IsDisable = reader.GetBoolean(6);
                        model.Sort = reader.GetInt32(7);
                        model.LastUpdatedDate = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<HWTVInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,HWTVName,HWTVIcon,ProgramAddress,IsTurnTo,IsDisable,Sort,LastUpdatedDate
                        from HWTV ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<HWTVInfo> list = new List<HWTVInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        HWTVInfo model = new HWTVInfo();
                        model.Id = reader.GetGuid(0);
                        model.HWTVName = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                        {
                            model.HWTVIcon = reader.GetGuid(2);
                        }
                        model.ProgramAddress = reader.GetString(3);
                        if (!reader.IsDBNull(4))
                        {
                            model.IsTurnTo = reader.GetBoolean(4);
                        }
                        if (!reader.IsDBNull(5))
                        {
                            model.IsDisable = reader.GetBoolean(5);
                        }
                        if (!reader.IsDBNull(6))
                        {
                            model.Sort = reader.GetInt32(6);
                        }
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<HWTVInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,HWTVName,HWTVIcon,ProgramAddress,IsTurnTo,IsDisable,Sort,LastUpdatedDate 
			            from HWTV
					    order by Sort ");

            IList<HWTVInfo> list = new List<HWTVInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        HWTVInfo model = new HWTVInfo();
                        model.Id = reader.GetGuid(0);
                        model.HWTVName = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                        {
                            model.HWTVIcon = reader.GetGuid(2);
                        }
                        model.ProgramAddress = reader.GetString(3);
                        model.IsTurnTo = reader.GetBoolean(4);
                        model.IsDisable = reader.GetBoolean(5);
                        model.Sort = reader.GetInt32(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<MyHWTVInfo> GetMyList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"SELECT h.Id, h.HWTVName, h.HWTVIcon, h.ProgramAddress, h.IsTurnTo, h.Sort,
                               cp.[FileName], cp.FileSize, cp.FileExtension, cp.FileDirectory,
                               cp.RandomFolder
                          FROM HWTV AS h 
                        LEFT JOIN CommunionPicture AS cp ON cp.Id=h.HWTVIcon
                        WHERE h.IsDisable=0
                        ORDER BY h.Sort");

            IList<MyHWTVInfo> list = new List<MyHWTVInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MyHWTVInfo model = new MyHWTVInfo();
                        if (!reader.IsDBNull(0))
                        {
                            model.Id = reader.GetGuid(0);
                        }
                        model.HWTVName = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                        {
                            model.HWTVIcon = reader.GetGuid(2);
                        }
                        model.ProgramAddress = reader.GetString(3);
                        if (!reader.IsDBNull(4))
                        {
                            model.IsTurnTo = reader.GetBoolean(4);
                        }
                        if (!reader.IsDBNull(5))
                        {
                            model.Sort = reader.GetInt32(5);
                        }
                        model.FileName = reader.GetString(6);
                        if (!reader.IsDBNull(7))
                        {
                            model.FileSize = reader.GetInt32(7);
                        }
                        model.FileExtension = reader.GetString(8);
                        model.FileDirectory = reader.GetString(9);
                        model.RandomFolder = reader.GetString(10);
                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
