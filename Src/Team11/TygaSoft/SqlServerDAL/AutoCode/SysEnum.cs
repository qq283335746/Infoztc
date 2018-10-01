using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;
using TygaSoft.IDAL;
using TygaSoft.DBUtility;

namespace TygaSoft.SqlServerDAL
{
    public partial class SysEnum : ISysEnum
    {
        #region ISysEnum Member

        public int Insert(SysEnumInfo model)
        {
            if (IsExist(model.EnumCode, model.ParentId, null)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Sys_Enum (EnumCode,EnumName,EnumValue,ParentId,Sort,Remark)
			            values
						(@EnumCode,@EnumName,@EnumValue,@ParentId,@Sort,@Remark)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@EnumCode",SqlDbType.NVarChar,50),
                                        new SqlParameter("@EnumName",SqlDbType.NVarChar,50),
                                        new SqlParameter("@EnumValue",SqlDbType.NVarChar,256),
                                        new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@Remark",SqlDbType.NVarChar,256)
                                   };
            parms[0].Value = model.EnumCode;
            parms[1].Value = model.EnumName;
            parms[2].Value = model.EnumValue;
            parms[3].Value = model.ParentId;
            parms[4].Value = model.Sort;
            parms[5].Value = model.Remark;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(SysEnumInfo model)
        {
            if (IsExist(model.EnumCode, model.ParentId, model.Id)) return 110;

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update Sys_Enum set EnumCode = @EnumCode,EnumName = @EnumName,EnumValue = @EnumValue,ParentId = @ParentId,Sort = @Sort,Remark = @Remark 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@EnumCode",SqlDbType.NVarChar,50),
                                        new SqlParameter("@EnumName",SqlDbType.NVarChar,50),
                                        new SqlParameter("@EnumValue",SqlDbType.NVarChar,256),
                                        new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@Remark",SqlDbType.NVarChar,256)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.EnumCode;
            parms[2].Value = model.EnumName;
            parms[3].Value = model.EnumValue;
            parms[4].Value = model.ParentId;
            parms[5].Value = model.Sort;
            parms[6].Value = model.Remark;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from Sys_Enum where Id = @Id");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

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
                sb.Append(@"delete from Sys_Enum where Id = @Id" + n + " ;");
                SqlParameter parm = new SqlParameter("@Id" + n + "", SqlDbType.UniqueIdentifier);
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

        public SysEnumInfo GetModel(object Id)
        {
            SysEnumInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,EnumCode,EnumName,EnumValue,ParentId,Sort,Remark 
			            from Sys_Enum
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new SysEnumInfo();
                        model.Id = reader.GetGuid(0);
                        model.EnumCode = reader.GetString(1);
                        model.EnumName = reader.GetString(2);
                        model.EnumValue = reader.GetString(3);
                        model.ParentId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
                    }
                }
            }

            return model;
        }

        public List<SysEnumInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = "select count(*) from [Sys_Enum] t1 join dbo.Sys_Enum t2 on t1.ParentId = t2.Id ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms);

            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by t1.Sort) as RowNumber,
                      t1.Id,t1.EnumCode,t1.EnumName,t1.EnumValue,t1.ParentId,t1.Sort,t1.Remark 
                      from [Sys_Enum] t1 join dbo.Sys_Enum t2 on t1.ParentId = t2.Id ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            List<SysEnumInfo> list = new List<SysEnumInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SysEnumInfo model = new SysEnumInfo();
                        model.Id = reader["Id"];
                        model.EnumCode = reader.GetString(2);
                        model.EnumName = reader.GetString(3);
                        model.EnumValue = reader.GetString(4);
                        model.ParentId = reader[5];
                        model.Sort = reader.GetInt32(6);
                        model.Remark = reader.GetString(7).Trim();

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<SysEnumInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            string cmdText = @"select * from(select row_number() over(order by t1.Sort) as RowNumber,
                             t1.Id,t1.EnumCode,t1.EnumName,t1.EnumValue,t1.ParentId,t1.Sort,t1.Remark 
                             from [Sys_Enum] t1 join dbo.Sys_Enum t2 on t1.ParentId = t2.Id ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            List<SysEnumInfo> list = new List<SysEnumInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SysEnumInfo model = new SysEnumInfo();
                        model.Id = reader["Id"];
                        model.EnumCode = reader.GetString(2);
                        model.EnumName = reader.GetString(3);
                        model.EnumValue = reader.GetString(4);
                        model.ParentId = reader[5];
                        model.Sort = reader.GetInt32(6);
                        model.Remark = reader.GetString(7).Trim();

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<SysEnumInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            string cmdText = @"select t1.Id,t1.EnumCode,t1.EnumName,t1.EnumValue,t1.ParentId,t1.Sort,t1.Remark 
                             from [Sys_Enum] t1 join [Sys_Enum] t2 on t1.ParentId = t2.Id ";

            if (!string.IsNullOrEmpty(sqlWhere))
            {
                cmdText += " where 1=1 " + sqlWhere;
            }

            cmdText += " order by Sort ";

            List<SysEnumInfo> list = new List<SysEnumInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SysEnumInfo model = new SysEnumInfo();
                        model.Id = reader["Id"];
                        model.EnumCode = reader.GetString(1);
                        model.EnumName = reader.GetString(2);
                        model.EnumValue = reader.GetString(3);
                        model.ParentId = reader[4];
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6).Trim();

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<SysEnumInfo> GetList()
        {
            string cmdText = @"select Id,EnumCode,EnumName,EnumValue,ParentId,Sort,Remark 
                             from [Sys_Enum] order by Sort ";

            List<SysEnumInfo> list = new List<SysEnumInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SysEnumInfo model = new SysEnumInfo();
                        model.Id = reader["Id"];
                        model.EnumCode = reader.GetString(1);
                        model.EnumName = reader.GetString(2);
                        model.EnumValue = reader.GetString(3);
                        model.ParentId = reader[4];
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6).Trim();

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
