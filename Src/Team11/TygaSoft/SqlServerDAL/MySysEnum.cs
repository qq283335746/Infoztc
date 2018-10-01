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
    public partial class SysEnum
    {
        #region ISysEnum Member

        public SysEnumInfo GetModel(string enumCode)
        {
            SysEnumInfo model = null;

            string cmdText = @"select top 1 Id,EnumCode,EnumName,EnumValue,ParentId,Sort,Remark from [Sys_Enum] where EnumCode = @EnumCode ";
            SqlParameter parm = new SqlParameter("@EnumCode", SqlDbType.VarChar, 50);
            parm.Value = enumCode;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new SysEnumInfo();
                        model.Id = reader["Id"];
                        model.EnumCode = reader.GetString(1);
                        model.EnumName = reader.GetString(2);
                        model.EnumValue = reader.GetString(3);
                        model.ParentId = reader[4];
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6).Trim();
                    }
                }
            }

            return model;
        }

        public Dictionary<object, string> GetKeyValue(string sqlWhere, params SqlParameter[] cmdParms)
        {
            Dictionary<object, string> dic = new Dictionary<object, string>();

            string cmdText = @"select t1.Id,t1.EnumValue 
                                from Sys_Enum t1
                                join Sys_Enum t2 on t2.Id = t1.ParentId ";

            if (!string.IsNullOrEmpty(sqlWhere))
            {
                cmdText += " where 1=1 " + sqlWhere;
            }

            cmdText += " order by t1.Sort ";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dic.Add(reader[0], reader.GetString(1));

                    }
                }
            }

            return dic;
        }

        private bool IsExist(string name, object parentId, object Id)
        {
            Guid gId = Guid.Empty;
            if (Id != null)
            {
                Guid.TryParse(Id.ToString(), out gId);
            }

            SqlParameter[] parms = {
                                       new SqlParameter("@Named",SqlDbType.NVarChar, 30),
                                       new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = name;
            parms[1].Value = Guid.Parse(parentId.ToString());

            StringBuilder sb = new StringBuilder(100);
            if (!gId.Equals(Guid.Empty))
            {
                sb.Append(@" select 1 from [Sys_Enum] where lower(EnumCode) = @EnumCode and ParentId = @ParentId and Id <> @Id ");

                Array.Resize(ref parms, 3);
                parms[2] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                parms[2].Value = gId;
            }
            else
            {
                sb.Append(@" select 1 from [Sys_Enum] where lower(EnumCode) = @EnumCode and ParentId = @ParentId ");
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
