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
    public partial class ServiceVote
    {
        #region IServiceVote Member

        public ServiceVoteInfo GetModelByJoin(object Id)
        {
            ServiceVoteInfo model = null;

            string cmdText = @"select top 1 sv.Id,sv.ServiceItemId,sv.Named,sv.HeadPictureId,sv.Descr,sv.ContentText,sv.Sort,sv.EnableStartTime,sv.EnableEndTime,sv.IsDisable,sv.LastUpdatedDate,
                               si.Named as ServiceItemName,psv.FileExtension,psv.FileDirectory,psv.RandomFolder
			                   from Service_Vote sv
                               left join Picture_ServiceVote psv on psv.Id = sv.HeadPictureId
                               left join Service_Item si on si.Id = sv.ServiceItemId
							   where sv.Id = @Id ";
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ServiceVoteInfo();
                        model.Id = reader.GetGuid(0);
                        model.ServiceItemId = reader.GetGuid(1);
                        model.Named = reader.GetString(2);
                        model.HeadPictureId = reader.GetGuid(3);
                        model.Descr = reader.GetString(4);
                        model.ContentText = reader.GetString(5);
                        model.Sort = reader.GetInt32(6);
                        model.EnableStartTime = reader.GetDateTime(7);
                        model.EnableEndTime = reader.GetDateTime(8);
                        model.IsDisable = reader.GetBoolean(9);
                        model.LastUpdatedDate = reader.GetDateTime(10);

                        model.ServiceItemName = reader.IsDBNull(11) ? "" : reader.GetString(11);
                        model.FileExtension = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        model.FileDirectory = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        model.RandomFolder = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    }
                }
            }

            return model;
        }

        public IList<ServiceVoteInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(500);
            sb.Append(@"select count(*) from Service_Vote sv
                        left join Picture_ServiceVote psv on psv.Id = sv.HeadPictureId
                        left join Service_Item si on si.Id = sv.ServiceItemId
                     ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ServiceVoteInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by sv.LastUpdatedDate desc,sv.Sort) as RowNumber,
			          sv.Id,sv.ServiceItemId,sv.Named,sv.HeadPictureId,sv.Descr,sv.Sort,sv.LastUpdatedDate,
                      si.Named as ServiceItemName,psv.FileExtension,psv.FileDirectory,psv.RandomFolder
					  from Service_Vote sv
                      left join Picture_ServiceVote psv on psv.Id = sv.HeadPictureId
                      left join Service_Item si on si.Id = sv.ServiceItemId
                      ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ServiceVoteInfo> list = new List<ServiceVoteInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ServiceVoteInfo model = new ServiceVoteInfo();
                        model.Id = reader.GetGuid(1);
                        model.ServiceItemId = reader.GetGuid(2);
                        model.Named = reader.GetString(3);
                        model.HeadPictureId = reader.GetGuid(4);
                        model.Descr = reader.GetString(5);
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
                sb.Append(@" select 1 from [Service_Vote] where lower(Named) = @Named and ServiceItemId = @ServiceItemId and Id <> @Id ");

                Array.Resize(ref parms, 3);
                parms[2] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                parms[2].Value = gId;
            }
            else
            {
                sb.Append(@" select 1 from [Service_Vote] where lower(Named) = @Named and ServiceItemId = @ServiceItemId ");
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
