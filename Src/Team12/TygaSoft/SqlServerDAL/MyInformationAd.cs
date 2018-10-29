using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TygaSoft.DBUtility;
using TygaSoft.IDAL;
using TygaSoft.Model;

namespace TygaSoft.SqlServerDAL
{
    public partial class InformationAd
    {
        #region IInformation Member

        public Guid InsertByOutput(InformationAdInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Information_Ad (Title,Descr,ContentText,Url,ViewType,StartDate,EndDate,UserId,LastUpdatedDate)
			            output inserted.Id values
						(@Title,@Descr,@ContentText,@Url,@ViewType,@StartDate,@EndDate,@UserId,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@Descr",SqlDbType.NVarChar,200),
new SqlParameter("@ContentText",SqlDbType.NVarChar,4000),
new SqlParameter("@Url",SqlDbType.VarChar,200),
new SqlParameter("@ViewType",SqlDbType.Int),
new SqlParameter("@StartDate",SqlDbType.DateTime),
new SqlParameter("@EndDate",SqlDbType.DateTime),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Title;
            parms[1].Value = model.Descr;
            parms[2].Value = model.ContentText;
            parms[3].Value = model.Url;
            parms[4].Value = model.ViewType;
            parms[5].Value = model.StartDate;
            parms[6].Value = model.EndDate;
            parms[7].Value = model.UserId;
            parms[8].Value = model.LastUpdatedDate;

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return Guid.Parse(obj.ToString());

            return Guid.Empty;
        }


        public DataSet GetModelOW(object Id)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,Title,Descr,ContentText,Url,ViewType,StartDate,EndDate,UserId,LastUpdatedDate 
			            from Information_Ad
						where Id = @Id ");

            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Information_Ad ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,Title,Descr,ContentText,Url,ViewType,StartDate,EndDate,UserId,LastUpdatedDate
					  from Information_Ad ");

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

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,Title,Descr,ContentText,Url,ViewType,StartDate,EndDate,UserId,LastUpdatedDate
					  from Information_Ad ");

            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public DataSet GetInforAdListOW(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select InformationId,InformationAdId,Sort,InforAd.Title,InforAd.Descr,InforAd.Url,InforAd.ViewType
                        from InformationAd IA left join Information_Ad InforAd on IA.InformationAdId=InforAd.Id where InformationId = @Id order by Sort asc");

            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());


            IList<InformationAdInfo> list = new List<InformationAdInfo>();

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);

            return ds;
        }

        public DataSet GetValidInforAdListOW(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select InformationId,InformationAdId,Sort,InforAd.Title,InforAd.Descr,InforAd.Url,InforAd.ViewType
                        from InformationAd IA left join Information_Ad InforAd on IA.InformationAdId=InforAd.Id ");

            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<InformationAdInfo> list = new List<InformationAdInfo>();

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public int InsertInformationAd(Guid informationId, Guid informationAdId, int sort)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into InformationAd (InformationId,InformationAdId,sort)
			            values
						(@InformationId,@InformationAdId,@sort)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@InformationId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@InformationAdId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@sort",SqlDbType.Int)
                                   };
            parms[0].Value = informationId;
            parms[1].Value = informationAdId;
            parms[2].Value = sort;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }


        public int DeleteInformationAd(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from InformationAd where InformationId = @InformationId");
            SqlParameter parm = new SqlParameter("@InformationId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);
        }

        #endregion
    }
}
