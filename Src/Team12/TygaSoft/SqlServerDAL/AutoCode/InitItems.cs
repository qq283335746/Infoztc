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
    public partial class InitItems : IInitItems
    {
        #region IInitItems Member

        public int Insert(InitItemsInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into InitItems (ItemType,ItemTypeName,ItemName,ItemCode,ItemKey,IsDisable,EditTime)
			            values
						(@ItemType,@ItemTypeName,@ItemName,@ItemCode,@ItemKey,@IsDisable,@EditTime)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@ItemType",SqlDbType.VarChar,30),
new SqlParameter("@ItemTypeName",SqlDbType.VarChar,50),
new SqlParameter("@ItemName",SqlDbType.VarChar,50),
new SqlParameter("@ItemCode",SqlDbType.VarChar,50),
new SqlParameter("@ItemKey",SqlDbType.VarChar,1000),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@EditTime",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.ItemType;
            parms[1].Value = model.ItemTypeName;
            parms[2].Value = model.ItemName;
            parms[3].Value = model.ItemCode;
            parms[4].Value = model.ItemKey;
            parms[5].Value = model.IsDisable;
            parms[6].Value = model.EditTime;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(InitItemsInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update InitItems set ItemType = @ItemType,ItemTypeName = @ItemTypeName,ItemName = @ItemName,ItemCode = @ItemCode,ItemKey = @ItemKey,IsDisable = @IsDisable,EditTime = @EditTime 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@ItemType",SqlDbType.VarChar,30),
new SqlParameter("@ItemTypeName",SqlDbType.VarChar,50),
new SqlParameter("@ItemName",SqlDbType.VarChar,50),
new SqlParameter("@ItemCode",SqlDbType.VarChar,50),
new SqlParameter("@ItemKey",SqlDbType.VarChar,1000),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@EditTime",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.ItemType;
            parms[2].Value = model.ItemTypeName;
            parms[3].Value = model.ItemName;
            parms[4].Value = model.ItemCode;
            parms[5].Value = model.ItemKey;
            parms[6].Value = model.IsDisable;
            parms[7].Value = model.EditTime;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from InitItems where Id = @Id");
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
                sb.Append(@"delete from InitItems where Id = @Id" + n + " ;");
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

        public InitItemsInfo GetModel(object Id)
        {
            InitItemsInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,ItemType,ItemTypeName,ItemName,ItemCode,ItemKey,IsDisable,EditTime 
			            from InitItems
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new InitItemsInfo();
                        model.Id = reader.GetGuid(0);
                        model.ItemType = reader.GetString(1);
                        model.ItemTypeName = reader.GetString(2);
                        model.ItemName = reader.GetString(3);
                        model.ItemCode = reader.GetString(4);
                        model.ItemKey = reader.GetString(5);
                        model.IsDisable = reader.GetBoolean(6);
                        model.EditTime = reader.GetDateTime(7);
                    }
                }
            }

            return model;
        }

        public IList<InitItemsInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from InitItems ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<InitItemsInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,ItemType,ItemTypeName,ItemName,ItemCode,ItemKey,IsDisable,EditTime
					  from InitItems ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<InitItemsInfo> list = new List<InitItemsInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InitItemsInfo model = new InitItemsInfo();
                        model.Id = reader.GetGuid(1);
                        model.ItemType = reader.GetString(2);
                        model.ItemTypeName = reader.GetString(3);
                        model.ItemName = reader.GetString(4);
                        model.ItemCode = reader.GetString(5);
                        model.ItemKey = reader.GetString(6);
                        model.IsDisable = reader.GetBoolean(7);
                        model.EditTime = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<InitItemsInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,ItemType,ItemTypeName,ItemName,ItemCode,ItemKey,IsDisable,EditTime
					   from InitItems ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<InitItemsInfo> list = new List<InitItemsInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InitItemsInfo model = new InitItemsInfo();
                        model.Id = reader.GetGuid(1);
                        model.ItemType = reader.GetString(2);
                        model.ItemTypeName = reader.GetString(3);
                        model.ItemName = reader.GetString(4);
                        model.ItemCode = reader.GetString(5);
                        model.ItemKey = reader.GetString(6);
                        model.IsDisable = reader.GetBoolean(7);
                        model.EditTime = reader.GetDateTime(8);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<InitItemsInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ItemType,ItemTypeName,ItemName,ItemCode,ItemKey,IsDisable,EditTime
                        from InitItems ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<InitItemsInfo> list = new List<InitItemsInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InitItemsInfo model = new InitItemsInfo();
                        model.Id = reader.GetGuid(0);
                        model.ItemType = reader.GetString(1);
                        model.ItemTypeName = reader.GetString(2);
                        model.ItemName = reader.GetString(3);
                        model.ItemCode = reader.GetString(4);
                        model.ItemKey = reader.GetString(5);
                        model.IsDisable = reader.GetBoolean(6);
                        model.EditTime = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<InitItemsInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,ItemType,ItemTypeName,ItemName,ItemCode,ItemKey,IsDisable,EditTime 
			            from InitItems
					    order by LastUpdatedDate desc ");

            IList<InitItemsInfo> list = new List<InitItemsInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InitItemsInfo model = new InitItemsInfo();
                        model.Id = reader.GetGuid(0);
                        model.ItemType = reader.GetString(1);
                        model.ItemTypeName = reader.GetString(2);
                        model.ItemName = reader.GetString(3);
                        model.ItemCode = reader.GetString(4);
                        model.ItemKey = reader.GetString(5);
                        model.IsDisable = reader.GetBoolean(6);
                        model.EditTime = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
