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
    public partial class AdBase
    {
        #region IAdBase Member

        public DataSet GetDs(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(1000);
            sb.Append(@"select count(*) 
                        from AdBase ad
                        left join ContentType at on at.Id = ad.SiteFunId
                        left join ContentType at1 on at1.Id = ad.LayoutPositionId
                        ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return null;

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by ad.LastUpdatedDate desc,ad.Sort) as RowNumber,
			          ad.Id,ad.Title,ad.SiteFunId,ad.LayoutPositionId,ad.Timeout,ad.Sort,ad.StartTime,ad.EndTime,ad.VirtualViewCount,ad.ViewCount,ad.IsDisable,ad.LastUpdatedDate,
                      at.TypeCode as SiteFunCode,at.TypeName as SiteFunName,at.TypeValue as SiteFunValue,at1.TypeCode as LayoutPositionCode,at1.TypeName as LayoutPositionName,at1.TypeValue as LayoutPositionValue
					  from AdBase ad
                        left join ContentType at on at.Id = ad.SiteFunId
                        left join ContentType at1 on at1.Id = ad.LayoutPositionId
                      ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            return SqlHelper.ExecuteDataset(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);
        }

        public AdBaseInfo GetModelByJoin(object Id)
        {
            AdBaseInfo model = null;

            string cmdText = @"select top 1 ad.Id,ad.Title,ad.SiteFunId,ad.LayoutPositionId,ad.Timeout,ad.Sort,ad.StartTime,ad.EndTime,ad.VirtualViewCount,ad.ViewCount,ad.IsDisable,ad.LastUpdatedDate,
                               at.TypeCode as SiteFunName,at1.TypeCode as LayoutPositionName,adi.Descr,adi.ContentText
                               from AdBase ad
                               left join ContentType at on at.Id = ad.SiteFunId
                               left join ContentType at1 on at1.Id = ad.LayoutPositionId
                               left join AdvertisementItem adi on adi.AdvertisementId = ad.Id
							   where ad.Id = @Id ";
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new AdBaseInfo();
                        model.Id = reader.GetGuid(0);
                        model.Title = reader.GetString(1);
                        model.SiteFunId = reader.GetGuid(2);
                        model.LayoutPositionId = reader.GetGuid(3);
                        model.Timeout = reader.GetInt32(4);
                        model.Sort = reader.GetInt32(5);
                        model.StartTime = reader.GetDateTime(6);
                        model.EndTime = reader.GetDateTime(7);
                        model.VirtualViewCount = reader.GetInt32(8);
                        model.ViewCount = reader.GetInt32(9);
                        model.IsDisable = reader.GetBoolean(10);
                        model.LastUpdatedDate = reader.GetDateTime(11);
                        model.SiteFunName = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        model.LayoutPositionName = reader.IsDBNull(13) ? "" : reader.GetString(13);

                    }
                }
            }

            return model;
        }

        public int InsertByOutput(AdBaseInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into AdBase (Id,Title,SiteFunId,LayoutPositionId,Timeout,Sort,StartTime,EndTime,VirtualViewCount,ViewCount,IsDisable,LastUpdatedDate)
			            values
						(@Id,@Title,@SiteFunId,@LayoutPositionId,@Timeout,@Sort,@StartTime,@EndTime,@VirtualViewCount,@ViewCount,@IsDisable,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@Title",SqlDbType.NVarChar,100),
                                        new SqlParameter("@SiteFunId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@LayoutPositionId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Timeout",SqlDbType.Int),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@StartTime",SqlDbType.DateTime),
                                        new SqlParameter("@EndTime",SqlDbType.DateTime),
                                        new SqlParameter("@VirtualViewCount",SqlDbType.Int),
                                        new SqlParameter("@ViewCount",SqlDbType.Int),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = Guid.Parse(model.Id.ToString());
            parms[1].Value = model.Title;
            parms[2].Value = model.SiteFunId;
            parms[3].Value = model.LayoutPositionId;
            parms[4].Value = model.Timeout;
            parms[5].Value = model.Sort;
            parms[6].Value = model.StartTime;
            parms[7].Value = model.EndTime;
            parms[8].Value = model.VirtualViewCount;
            parms[9].Value = model.ViewCount;
            parms[10].Value = model.IsDisable;
            parms[11].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        #endregion
    }
}
