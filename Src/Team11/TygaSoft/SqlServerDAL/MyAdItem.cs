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
    public partial class AdItem
    {
        #region IAdItem Member

        public AdItemInfo GetModelByJoin(object Id)
        {
            AdItemInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 adi.Id,adi.AdvertisementId,adi.PictureId,adi.ActionTypeId,adi.Sort,adi.IsDisable 
                        ,adp.FileDirectory,adp.FileExtension,adp.RandomFolder
			            from AdItem adi
                        left join AdvertisementPicture adp on adp.Id = adi.PictureId
						where adi.Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new AdItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.AdvertisementId = reader.GetGuid(1);
                        model.PictureId = reader.GetGuid(2);
                        model.ActionTypeId = reader.GetGuid(3);
                        model.Sort = reader.GetInt32(4);
                        model.IsDisable = reader.GetBoolean(5);
                        model.FileDirectory = reader.IsDBNull(6) ? null : reader.GetString(6);
                        model.FileExtension = reader.IsDBNull(7) ? null : reader.GetString(7);
                        model.RandomFolder = reader.IsDBNull(8) ? null : reader.GetString(8);
                    }
                }
            }

            return model;
        }

        public DataSet GetDsByJoinForAdmin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*)
                from Advertisement ad
                left join AdItem adi on adi.AdvertisementId = ad.Id
                left join AdItemLink adl on adl.AdItemId = adi.Id
                left join AdItemContent adc on adc.AdItemId = adi.Id
                left join AdvertisementPicture adp on adp.Id = adi.PictureId
                left join ContentType ct on ct.Id = adi.ActionTypeId
                ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return null;

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by ad.LastUpdatedDate desc) as RowNumber,
			    adp.FileDirectory,adp.FileExtension,adp.RandomFolder
                ,ct.TypeName,ct.TypeCode,ct.TypeValue ActionTypeName,adi.Id,adi.Sort,adi.IsDisable
                from AdItem adi 
                left join Advertisement ad on adi.AdvertisementId = ad.Id
                left join AdItemLink adl on adl.AdItemId = adi.Id
                left join AdItemContent adc on adc.AdItemId = adi.Id
                left join AdvertisementPicture adp on adp.Id = adi.PictureId
                left join ContentType ct on ct.Id = adi.ActionTypeId
                ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            return SqlHelper.ExecuteDataset(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);
        }

        public bool DeleteBatchByJoin(IList<object> list)
        {
            if (list == null || list.Count == 0) return false;

            bool result = false;
            StringBuilder sb = new StringBuilder(500);
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from AdItem where Id = @Id" + n + " ;");
                sb.Append(@"delete from AdItemContent where AdItemId = @Id" + n + " ;");
                sb.Append(@"delete from AdItemLink where AdItemId = @Id" + n + " ;");
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

        #endregion
    }
}
