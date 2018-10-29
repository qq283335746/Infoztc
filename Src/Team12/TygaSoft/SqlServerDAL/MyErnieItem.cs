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
    public partial class ErnieItem
    {
        #region IErnieItem Member

        public int CopyLasted(object ErnieId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select top 1 Id  from dbo.Ernie where Id <> @Id order by LastUpdatedDate desc, EndTime desc");
            SqlParameter parm = new SqlParameter("@Id",SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ErnieId.ToString());
            var obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);
            if (obj == null) return 0;

            Guid lastId = Guid.Parse(obj.ToString());
            sb.Clear();
            sb.Append("select Id,ErnieId,NumType,Num,AppearRatio from ErnieItem where ErnieId = @ErnieId ");
            parm = new SqlParameter("@ErnieId",lastId);

            var ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return 0;

            DataRowCollection drc = ds.Tables[0].Rows;
            foreach (DataRow dr in drc)
            {
                dr["Id"] = Guid.NewGuid();
                dr["ErnieId"] = ErnieId;
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(SqlHelper.HnztcTeamDbConnString))
            {
                bulkCopy.DestinationTableName = "dbo.ErnieItem";
                bulkCopy.ColumnMappings.Add("Id", "Id");
                bulkCopy.ColumnMappings.Add("ErnieId", "ErnieId");
                bulkCopy.ColumnMappings.Add("NumType", "NumType");
                bulkCopy.ColumnMappings.Add("Num", "Num");
                bulkCopy.ColumnMappings.Add("AppearRatio", "AppearRatio");

                bulkCopy.WriteToServer(ds.Tables[0]);
            }

            return 1;
        }

        public int DeleteByErnieId(object ernieId)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ErnieItem where ErnieId = @ErnieId");
            SqlParameter parm = new SqlParameter("@ErnieId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ernieId.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);
        }



        #endregion
    }
}
