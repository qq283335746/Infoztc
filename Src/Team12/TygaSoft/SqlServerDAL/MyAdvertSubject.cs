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
    public partial class AdvertSubject : IAdvertSubject
    {
        #region IAdvertSubject Member

        public Guid InsertByOutput(AdvertSubjectInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into AdvertSubject (Title,Sort,PlayTime,Remark,IsDisable,LastUpdatedDate)
			            output inserted.Id values
						(@Title,@Sort,@PlayTime,@Remark,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Title",SqlDbType.NVarChar,100),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@PlayTime",SqlDbType.Int),
new SqlParameter("@Remark",SqlDbType.NVarChar,300),
new SqlParameter("@IsDisable",SqlDbType.Bit),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Title;
            parms[1].Value = model.Sort;
            parms[2].Value = model.PlayTime;
            parms[3].Value = model.Remark;
            parms[4].Value = model.IsDisable;
            parms[5].Value = model.LastUpdatedDate;

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return Guid.Parse(obj.ToString());

            return Guid.Empty;
        }

        public int UpdateDisable(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update AdvertSubject set IsDisable = 1 where Id <> @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public DataSet GetModelOW(object Id)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 ASj.Id,Title,Sort,PlayTime,Remark,IsDisable,ASj.LastUpdatedDate,
                        P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
			            from AdvertSubject ASj left join 
                        (select AP.AdvertId,CP.* from AdvertPicture AP,Picture_AdStartup CP where AP.PictureId=CP.Id) as P on ASj.Id=P.AdvertId
						where ASj.Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm);

            return ds;
        }

        public DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from AdvertSubject ASj");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by ASj.LastUpdatedDate desc) as RowNumber,
			          ASj.Id,Title,Sort,PlayTime,Remark,IsDisable,ASj.LastUpdatedDate,
                      P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
					  from AdvertSubject ASj left join 
                      (select AP.AdvertId,CP.* from AdvertPicture AP,Picture_AdStartup CP where AP.PictureId=CP.Id) as P on ASj.Id=P.AdvertId");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public DataSet GetListOW(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ASj.Id,Title,Sort,PlayTime,Remark,IsDisable,ASj.LastUpdatedDate,
                      P.Id as PictureId,FileName,FileSize,FileExtension,FileDirectory,RandomFolder
					  from AdvertSubject ASj left join 
                      (select AP.AdvertId,CP.* from AdvertPicture AP,Picture_AdStartup CP where AP.PictureId=CP.Id) as P on ASj.Id=P.AdvertId");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        #endregion
    }
}
