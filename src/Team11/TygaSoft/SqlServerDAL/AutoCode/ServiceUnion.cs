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
    public partial class ServiceUnion : IServiceUnion
    {
        #region IServiceUnion Member

        public IList<ServiceUnionInfo> GetListByService(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, string colAppend, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(1000);
            sb.AppendFormat(@"select COUNT(1) from
                        (
                        select si.Id,si.Named,'' as Descr,si.Sort,si.PictureId,si.LastUpdatedDate,'si' as Flag
                        {0}
                        from dbo.Service_Item si where si.ParentId=@ServiceItemId
                         union all
                         select sv.Id, sv.Named,sv.Descr,sv.Sort,sv.HeadPictureId,sv.LastUpdatedDate,'sv' as Flag 
                         ,null IsUserPraise
                         from dbo.Service_Vote sv where sv.ServiceItemId = @ServiceItemId
                         union all
                         select sl.Id, sl.Named,sl.Url,sl.Sort,sl.PictureId,sl.LastUpdatedDate,'sl' as Flag 
                         ,null IsUserPraise
                          from dbo.Service_Link sl where sl.ServiceItemId = @ServiceItemId
                          union all
                         select sc.Id, sc.Named,sc.Descr,sc.Sort,sc.PictureId,sc.LastUpdatedDate,'sc' as Flag 
                         ,null IsUserPraise
                         from dbo.Service_Content sc where sc.ServiceItemId = @ServiceItemId
                         ) objt
                        ", colAppend);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ServiceUnionInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.AppendFormat(@"select * from(
                        select row_number() over(order by objt.Sort) as RowNumber,
                        objt.* from
                        (
                        select si.Id,si.Named,'' as Descr,si.Sort,si.PictureId,si.LastUpdatedDate,psi.FileExtension,psi.FileDirectory,psi.RandomFolder,'si' as Flag
                        ,(select COUNT(1) from Service_UserPraise where ServiceItemId = si.Id) as TotalPraise
                        ,(select COUNT(1) from Service_UserVole where ServiceItemId = si.Id) as TotalVole  
                         {0}
                        from dbo.Service_Item si 
                         left join Picture_ServiceItem psi on psi.Id = si.PictureId
                          where si.ParentId=@ServiceItemId
                         union all
                         select sv.Id, sv.Named,sv.Descr,sv.Sort,sv.HeadPictureId,sv.LastUpdatedDate,psv.FileExtension,psv.FileDirectory,psv.RandomFolder,'sv' as Flag 
                         ,(select COUNT(1) from Service_UserPraise where ServiceItemId = sv.Id) as TotalPraise
                         ,(select COUNT(1) from Service_UserVole where ServiceItemId = sv.Id) as TotalVole
                         ,null IsUserPraise
                         from dbo.Service_Vote sv 
                         left join Picture_ServiceVote psv on psv.Id = sv.HeadPictureId
                         where sv.ServiceItemId = @ServiceItemId
                         union all
                         select sl.Id, sl.Named,sl.Url,sl.Sort,sl.PictureId,sl.LastUpdatedDate,psl.FileExtension,psl.FileDirectory,psl.RandomFolder,'sl' as Flag 
                          ,(select COUNT(1) from Service_UserPraise where ServiceItemId = sl.Id) as TotalPraise
                          ,(select COUNT(1) from Service_UserVole where ServiceItemId = sl.Id) as TotalVole
                          ,null IsUserPraise
                          from dbo.Service_Link sl 
                          left join Picture_ServiceLink psl on psl.Id = sl.PictureId
                          where sl.ServiceItemId = @ServiceItemId
                          union all
                         select sc.Id, sc.Named,sc.Descr,sc.Sort,sc.PictureId,sc.LastUpdatedDate,psc.FileExtension,psc.FileDirectory,psc.RandomFolder,'sc' as Flag 
                          ,(select COUNT(1) from Service_UserPraise where ServiceItemId = sc.Id) as TotalPraise
                         ,(select COUNT(1) from Service_UserVole where ServiceItemId = sc.Id) as TotalVole
                         ,null IsUserPraise
                         from dbo.Service_Content sc 
                         left join Picture_ServiceContent psc on psc.Id = sc.PictureId
                         where sc.ServiceItemId = @ServiceItemId
                         ) objt
                      ", colAppend);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ServiceUnionInfo> list = new List<ServiceUnionInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ServiceUnionInfo model = new ServiceUnionInfo();
                        model.Id = reader.IsDBNull(1) ? Guid.Empty : reader.GetGuid(1);
                        model.Named = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        model.Descr = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        model.Sort = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                        model.PictureId = reader.IsDBNull(5) ? Guid.Empty : reader.GetGuid(5);
                        model.LastUpdatedDate = reader.IsDBNull(6) ? DateTime.MinValue : reader.GetDateTime(6);
                        model.FileExtension = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        model.FileDirectory = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        model.RandomFolder = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        model.Flag = reader.IsDBNull(10) ? "" : reader.GetString(10);
                        model.TotalPraise = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                        model.TotalVole = reader.IsDBNull(12) ? 0 : reader.GetInt32(12);
                        model.IsUserPraise = reader.IsDBNull(13) ? false : reader.GetInt32(13) == 1;

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
