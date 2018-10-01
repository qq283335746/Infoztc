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
    public partial class ServiceLink
    {
        #region IServiceLink Member

        public ServiceLinkInfo GetModelByJoin(object Id)
        {
            ServiceLinkInfo model = null;

            string cmdText = @"select top 1 sl.Id,sl.ServiceItemId,sl.PictureId,sl.Named,sl.Url,sl.Sort,sl.EnableStartTime,sl.EnableEndTime,sl.IsDisable,sl.LastUpdatedDate, 
                               si.Named as ServiceItemName,psl.FileExtension,psl.FileDirectory,psl.RandomFolder
			                   from Service_Link sl
                               left join Service_Item si on si.Id = sl.ServiceItemId
                               left join Picture_ServiceLink psl on psl.Id = sl.PictureId 
							   where sl.Id = @Id ";
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ServiceLinkInfo();
                        model.Id = reader.GetGuid(0);
                        model.ServiceItemId = reader.GetGuid(1);
                        model.PictureId = reader.GetGuid(2);
                        model.Named = reader.GetString(3);
                        model.Url = reader.GetString(4);
                        model.Sort = reader.GetInt32(5);
                        model.EnableStartTime = reader.GetDateTime(6);
                        model.EnableEndTime = reader.GetDateTime(7);
                        model.IsDisable = reader.GetBoolean(8);
                        model.LastUpdatedDate = reader.GetDateTime(9);

                        model.ServiceItemName = reader.IsDBNull(10) ? "" : reader.GetString(10);
                        model.FileExtension = reader.IsDBNull(11) ? "" : reader.GetString(11);
                        model.FileDirectory = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        model.RandomFolder = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    }
                }
            }

            return model;
        }

        public IList<ServiceLinkInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Service_Link sl
                        left join Picture_ServiceLink psl on psl.Id = sl.PictureId
                        left join Service_Item si on si.Id = sl.ServiceItemId 
                      ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ServiceLinkInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by sl.LastUpdatedDate desc,sl.Sort desc) as RowNumber,
			          sl.Id,sl.ServiceItemId,sl.PictureId,sl.Named,sl.Url,sl.Sort,sl.LastUpdatedDate,
                      si.Named as ServiceItemName,psl.FileExtension,psl.FileDirectory,psl.RandomFolder
					  from Service_Link sl
                      left join Picture_ServiceLink psl on psl.Id = sl.PictureId 
                      left join Service_Item si on si.Id = sl.ServiceItemId
                      ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ServiceLinkInfo> list = new List<ServiceLinkInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ServiceLinkInfo model = new ServiceLinkInfo();
                        model.Id = reader.GetGuid(1);
                        model.ServiceItemId = reader.GetGuid(2);
                        model.PictureId = reader.GetGuid(3);
                        model.Named = reader.GetString(4);
                        model.Url = reader.GetString(5);
                        model.Sort = reader.GetInt32(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                       
                        model.ServiceItemName = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        model.FileExtension = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        model.FileDirectory = reader.IsDBNull(10) ? "" : reader.GetString(10);
                        model.RandomFolder = reader.IsDBNull(11) ? "" : reader.GetString(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        private bool IsExist(string name, object serviceItemId, object Id)
        {
            Guid gId = Guid.Empty;
            if (Id != null)
            {
                Guid.TryParse(Id.ToString(), out gId);
            }

            SqlParameter[] parms = {
                                       new SqlParameter("@Named",SqlDbType.NVarChar, 30),
                                       new SqlParameter("@ServiceItemId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = name;
            parms[1].Value = Guid.Parse(serviceItemId.ToString());

            StringBuilder sb = new StringBuilder(100);
            if (!gId.Equals(Guid.Empty))
            {
                sb.Append(@" select 1 from [Service_Link] where lower(Named) = @Named and ServiceItemId = @ServiceItemId and Id <> @Id ");

                Array.Resize(ref parms, 3);
                parms[2] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                parms[2].Value = gId;
            }
            else
            {
                sb.Append(@" select 1 from [Service_Link] where lower(Named) = @Named and ServiceItemId = @ServiceItemId ");
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
