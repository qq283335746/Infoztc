using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TygaSoft.DBUtility;
using TygaSoft.IDAL;
using TygaSoft.Model;

namespace TygaSoft.SqlServerDAL
{
    public partial class Information
    {
        #region IInformation Member

        public Guid InsertByOutput(InformationInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Information (Title,Summary,ContentText,Source,ViewCount,Sort,ViewType,Remark,IsDisable,UserId,LastUpdatedDate)
			            output inserted.Id values
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

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return Guid.Parse(obj.ToString());

            return Guid.Empty;
        }


        public DataSet GetModelOW(object Id)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,Title,Summary,ContentText,Source,ViewCount,Sort,ViewType,Remark,IsDisable,UserId,LastUpdatedDate 
			            from Information
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Information ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by Sort asc, LastUpdatedDate desc) as RowNumber,
			           Id,Title,Summary,ContentText,Source,ViewCount,Sort,ViewType,Remark,IsDisable,UserId,LastUpdatedDate
					   from Information ");

            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by Sort asc, LastUpdatedDate desc) as RowNumber,
			           Id,Title,Summary,ContentText,Source,ViewCount,Sort,ViewType,Remark,IsDisable,UserId,LastUpdatedDate
					   from Information ");

            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        /// <summary>
        /// –ﬁ∏ƒ∑√Œ ¡ø
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateViewCount(object Id)
        {

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update Information set ViewCount = ViewCount + 1 where Id = @Id");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                   };
            parms[0].Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }
        #endregion
    }
}
