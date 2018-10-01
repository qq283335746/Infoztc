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
    public partial class ErnieItem : IErnieItem
    {
        #region IErnieItem Member

        public int Insert(ErnieItemInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ErnieItem (ErnieId,NumType,Num,AppearRatio)
			            values
						(@ErnieId,@NumType,@Num,@AppearRatio)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ErnieId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@NumType",SqlDbType.NVarChar,4),
                                        new SqlParameter("@Num",SqlDbType.VarChar,20),
                                        new SqlParameter("@AppearRatio",SqlDbType.Float)
                                   };
            parms[0].Value = model.ErnieId;
            parms[1].Value = model.NumType;
            parms[2].Value = model.Num;
            parms[3].Value = model.AppearRatio;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ErnieItemInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ErnieItem set ErnieId = @ErnieId,NumType = @NumType,Num = @Num,AppearRatio = @AppearRatio 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@ErnieId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@NumType",SqlDbType.NVarChar,4),
                                        new SqlParameter("@Num",SqlDbType.VarChar,20),
                                        new SqlParameter("@AppearRatio",SqlDbType.Float)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.ErnieId;
            parms[2].Value = model.NumType;
            parms[3].Value = model.Num;
            parms[4].Value = model.AppearRatio;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ErnieItem where Id = @Id");
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
                sb.Append(@"delete from ErnieItem where Id = @Id" + n + " ;");
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

        public ErnieItemInfo GetModel(object Id)
        {
            ErnieItemInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,ErnieId,NumType,Num,AppearRatio 
			            from ErnieItem
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ErnieItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.ErnieId = reader.GetGuid(1);
                        model.NumType = reader.GetString(2);
                        model.Num = reader.GetString(3);
                        model.AppearRatio = reader.GetDouble(4);
                    }
                }
            }

            return model;
        }

        public IList<ErnieItemInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ErnieItem ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ErnieItemInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by NumType,AppearRatio desc) as RowNumber,
			          Id,ErnieId,NumType,Num,AppearRatio
					  from ErnieItem ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ErnieItemInfo> list = new List<ErnieItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ErnieItemInfo model = new ErnieItemInfo();
                        model.Id = reader.GetGuid(1);
                        model.ErnieId = reader.GetGuid(2);
                        model.NumType = reader.GetString(3);
                        model.Num = reader.GetString(4);
                        model.AppearRatio = reader.GetDouble(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ErnieItemInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by NumType,AppearRatio desc) as RowNumber,
			           Id,ErnieId,NumType,Num,AppearRatio
					   from ErnieItem ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ErnieItemInfo> list = new List<ErnieItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ErnieItemInfo model = new ErnieItemInfo();
                        model.Id = reader.GetGuid(1);
                        model.ErnieId = reader.GetGuid(2);
                        model.NumType = reader.GetString(3);
                        model.Num = reader.GetString(4);
                        model.AppearRatio = reader.GetDouble(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ErnieItemInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ErnieId,NumType,Num,AppearRatio
                        from ErnieItem ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ErnieItemInfo> list = new List<ErnieItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ErnieItemInfo model = new ErnieItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.ErnieId = reader.GetGuid(1);
                        model.NumType = reader.GetString(2);
                        model.Num = reader.GetString(3);
                        model.AppearRatio = reader.GetDouble(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ErnieItemInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ErnieId,NumType,Num,AppearRatio 
			            from ErnieItem
					    order by NumType,AppearRatio desc ");

            IList<ErnieItemInfo> list = new List<ErnieItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ErnieItemInfo model = new ErnieItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.ErnieId = reader.GetGuid(1);
                        model.NumType = reader.GetString(2);
                        model.Num = reader.GetString(3);
                        model.AppearRatio = reader.GetDouble(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
