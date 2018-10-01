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
    public partial class ServiceItem
    {
        #region IServiceItem Member

        public ServiceItemInfo GetModelByJoin(object Id)
        {
            ServiceItemInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 pi.Id,pi.Named,pi.ParentId,pi.PictureId,pi.Sort,pi.HasVote,pi.HasContent,pi.HasLink,pi.EnableStartTime,pi.EnableEndTime,pi.IsDisable,pi.LastUpdatedDate,
                        pip.Named ParentName,psi.FileExtension,psi.FileDirectory,psi.RandomFolder
			            from Service_Item pi
                        left join Service_Item pip on pip.Id = pi.ParentId
                        left join Picture_ServiceItem psi on psi.Id = pi.PictureId
						where pi.Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ServiceItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.Named = reader.GetString(1);
                        model.ParentId = reader.GetGuid(2);
                        model.PictureId = reader.GetGuid(3);
                        model.Sort = reader.GetInt32(4);
                        model.HasVote = reader.GetBoolean(5);
                        model.HasContent = reader.GetBoolean(6);
                        model.HasLink = reader.GetBoolean(7);
                        model.EnableStartTime = reader.GetDateTime(8);
                        model.EnableEndTime = reader.GetDateTime(9);
                        model.IsDisable = reader.GetBoolean(10);
                        model.LastUpdatedDate = reader.GetDateTime(11);

                        model.ParentName = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        model.FileExtension = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        model.FileDirectory = reader.IsDBNull(14) ? "" : reader.GetString(14);
                        model.RandomFolder = reader.IsDBNull(15) ? "" : reader.GetString(15);
                    }
                }
            }

            return model;
        }

//        public IList<ServiceItemInfo> GetListByService(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, string colAppend, params SqlParameter[] cmdParms)
//        {
//            StringBuilder sb = new StringBuilder(500);
//            sb.Append(@"select count(*) from Service_Item si
//                        left join Service_Picture sp on sp.Id = si.PictureId
//                      ");
//            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
//            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

//            if (totalRecords == 0) return new List<ServiceItemInfo>();

//            sb.Clear();
//            int startIndex = (pageIndex - 1) * pageSize + 1;
//            int endIndex = pageIndex * pageSize;

//            sb.AppendFormat(@"select * from(select row_number() over(order by si.LastUpdatedDate desc,si.Sort) as RowNumber,
//			          si.Id,si.Named,si.ParentId,si.PictureId,si.Sort,si.HasVote,si.HasContent,si.HasLink,si.EnableStartTime,si.EnableEndTime,si.IsDisable,si.LastUpdatedDate,
//                      sp.OriginalPicture,sp.BPicture,sp.MPicture,sp.SPicture,sp.OtherPicture,
//                      (select COUNT(1) from Service_UserPraise where ServiceItemId = si.Id) as TotalPraise,
//                      (select COUNT(1) from Service_UserVole where ServiceItemId = si.Id) as TotalVole
//                      {0}
//					  from Service_Item si
//                      left join Service_Picture sp on sp.Id = si.PictureId
//                      ", colAppend);
//            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
//            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

//            IList<ServiceItemInfo> list = new List<ServiceItemInfo>();

//            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
//            {
//                if (reader != null && reader.HasRows)
//                {
//                    while (reader.Read())
//                    {
//                        ServiceItemInfo model = new ServiceItemInfo();
//                        model.Id = reader.GetGuid(1);
//                        model.Named = reader.GetString(2);
//                        model.ParentId = reader.GetGuid(3);
//                        model.PictureId = reader.GetGuid(4);
//                        model.Sort = reader.GetInt32(5);
//                        model.HasVote = reader.GetBoolean(6);
//                        model.HasContent = reader.GetBoolean(7);
//                        model.HasLink = reader.GetBoolean(8);
//                        model.EnableStartTime = reader.GetDateTime(9);
//                        model.EnableEndTime = reader.GetDateTime(10);
//                        model.IsDisable = reader.GetBoolean(11);
//                        model.LastUpdatedDate = reader.GetDateTime(12);

//                        model.OriginalPicture = reader.IsDBNull(13) ? "" : reader.GetString(13);
//                        model.BPicture = reader.IsDBNull(14) ? "" : reader.GetString(14);
//                        model.MPicture = reader.IsDBNull(15) ? "" : reader.GetString(15);
//                        model.SPicture = reader.IsDBNull(16) ? "" : reader.GetString(16);
//                        model.OtherPicture = reader.IsDBNull(17) ? "" : reader.GetString(17);
//                        model.TotalPraise = reader.IsDBNull(18) ? 0 : reader.GetInt32(18);
//                        model.TotalVole = reader.IsDBNull(19) ? 0 : reader.GetInt32(19);
//                        model.IsUserPraise = reader.IsDBNull(20) ? false : true;

//                        list.Add(model);
//                    }
//                }
//            }

//            return list;
//        }

        public IList<ServiceItemInfo> GetListByService(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, string colAppend, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(500);
            sb.Append(@"select count(*) from Service_Item si
                        left join Picture_ServiceItem psi on psi.Id = si.PictureId
                      ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ServiceItemInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.AppendFormat(@"select * from(select row_number() over(order by si.Sort) as RowNumber,
			          si.Id,si.Named,si.ParentId,si.PictureId,si.Sort,si.HasVote,si.HasContent,si.HasLink,si.EnableStartTime,si.EnableEndTime,si.IsDisable,si.LastUpdatedDate,
                       psi.FileExtension,psi.FileDirectory,psi.RandomFolder,
                      (select COUNT(1) from Service_UserPraise where ServiceItemId = si.Id) as TotalPraise,
                      (select COUNT(1) from Service_UserVole where ServiceItemId = si.Id) as TotalVole
                      {0}
					  from Service_Item si
                      left join Picture_ServiceItem psi on psi.Id = si.PictureId
                      ", colAppend);
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ServiceItemInfo> list = new List<ServiceItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ServiceItemInfo model = new ServiceItemInfo();
                        model.Id = reader.GetGuid(1);
                        model.Named = reader.GetString(2);
                        model.ParentId = reader.GetGuid(3);
                        model.PictureId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.HasVote = reader.GetBoolean(6);
                        model.HasContent = reader.GetBoolean(7);
                        model.HasLink = reader.GetBoolean(8);
                        model.EnableStartTime = reader.GetDateTime(9);
                        model.EnableEndTime = reader.GetDateTime(10);
                        model.IsDisable = reader.GetBoolean(11);
                        model.LastUpdatedDate = reader.GetDateTime(12);

                        model.FileExtension = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        model.FileDirectory = reader.IsDBNull(14) ? "" : reader.GetString(14);
                        model.RandomFolder = reader.IsDBNull(15) ? "" : reader.GetString(15);
                        model.TotalPraise = reader.IsDBNull(16) ? 0 : reader.GetInt32(16);
                        model.TotalVole = reader.IsDBNull(17) ? 0 : reader.GetInt32(17);
                        model.IsUserPraise = reader.IsDBNull(18) ? false : true;

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ServiceItemInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(500);
            sb.Append(@"select count(*) from Service_Item si
                        left join Service_Picture sp on sp.Id = si.PictureId
                      ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ServiceItemInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by si.Sort) as RowNumber,
			          si.Id,si.Named,si.ParentId,si.PictureId,si.Sort,si.HasVote,si.HasContent,si.HasLink,si.EnableStartTime,si.EnableEndTime,si.IsDisable,si.LastUpdatedDate,
                      psi.FileExtension,psi.FileDirectory,psi.RandomFolder,
                      (select COUNT(1) from Service_UserPraise where ServiceItemId = si.Id) as TotalPraise,
                      (select COUNT(1) from Service_UserVole where ServiceItemId = si.Id) as TotalVole
					  from Service_Item si
                      left join Picture_ServiceItem psi on psi.Id = si.PictureId
                      ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ServiceItemInfo> list = new List<ServiceItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ServiceItemInfo model = new ServiceItemInfo();
                        model.Id = reader.GetGuid(1);
                        model.Named = reader.GetString(2);
                        model.ParentId = reader.GetGuid(3);
                        model.PictureId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.HasVote = reader.GetBoolean(6);
                        model.HasContent = reader.GetBoolean(7);
                        model.HasLink = reader.GetBoolean(8);
                        model.EnableStartTime = reader.GetDateTime(9);
                        model.EnableEndTime = reader.GetDateTime(10);
                        model.IsDisable = reader.GetBoolean(11);
                        model.LastUpdatedDate = reader.GetDateTime(12);

                        model.FileExtension = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        model.FileDirectory = reader.IsDBNull(14) ? "" : reader.GetString(14);
                        model.RandomFolder = reader.IsDBNull(15) ? "" : reader.GetString(15);
                        model.TotalPraise = reader.IsDBNull(16) ? 0 : reader.GetInt32(16);
                        model.TotalVole = reader.IsDBNull(17) ? 0 : reader.GetInt32(17);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<ServiceItemInfo> GetListByJoin()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select si.Id,si.Named,si.ParentId,si.PictureId,si.Sort,si.HasVote,si.HasContent,si.HasLink,si.EnableStartTime,si.EnableEndTime,si.IsDisable,si.LastUpdatedDate,
                        psi.FileExtension,psi.FileDirectory,psi.RandomFolder
			            from Service_Item si
                        left join Picture_ServiceItem psi on psi.Id = si.PictureId
					    order by si.Sort ");

            List<ServiceItemInfo> list = new List<ServiceItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ServiceItemInfo model = new ServiceItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.Named = reader.GetString(1);
                        model.ParentId = reader.GetGuid(2);
                        model.PictureId = reader.GetGuid(3);
                        model.Sort = reader.GetInt32(4);
                        model.HasVote = reader.GetBoolean(5);
                        model.HasContent = reader.GetBoolean(6);
                        model.HasLink = reader.GetBoolean(7);
                        model.EnableStartTime = reader.GetDateTime(8);
                        model.EnableEndTime = reader.GetDateTime(9);
                        model.IsDisable = reader.GetBoolean(10);
                        model.LastUpdatedDate = reader.GetDateTime(11);

                        model.FileExtension = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        model.FileDirectory = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        model.RandomFolder = reader.IsDBNull(14) ? "" : reader.GetString(14);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public int DeleteByJoin(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from Service_Item where Id = @Id;");
            sb.Append("delete from Service_Vote where ServiceItemId = @Id;");
            sb.Append("delete from Service_Content where ServiceItemId = @Id;");
            sb.Append("delete from Service_Link where ServiceItemId = @Id;");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm);
        }

        public void UpdateHasVote(Guid Id, bool hasVote)
        {
            string cmdText = @"update Service_Item set HasVote = @HasVote 
                             where Id = @Id";

            SqlParameter[] parms = {
                                       new SqlParameter("@Id", SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@HasVote", SqlDbType.Bit)
                                   };
            parms[0].Value = Id;
            parms[1].Value = hasVote;

            SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        public void UpdateHasContent(Guid Id, bool hasContent)
        {
            string cmdText = @"update Service_Item set HasContent = @HasContent 
                             where Id = @Id";

            SqlParameter[] parms = {
                                       new SqlParameter("@Id", SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@HasContent", SqlDbType.Bit)
                                   };
            parms[0].Value = Id;
            parms[1].Value = hasContent;

            SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        public void UpdateHasLink(Guid Id, bool hasLink)
        {
            string cmdText = @"update Service_Item set HasLink = @HasLink 
                             where Id = @Id";

            SqlParameter[] parms = {
                                       new SqlParameter("@Id", SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@HasContent", SqlDbType.Bit)
                                   };
            parms[0].Value = Id;
            parms[1].Value = hasLink;

            SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        private bool IsExist(string name, object parentId, object Id)
        {
            Guid gId = Guid.Empty;
            if (Id != null)
            {
                Guid.TryParse(Id.ToString(), out gId);
            }

            SqlParameter[] parms = {
                                       new SqlParameter("@Name",SqlDbType.NVarChar, 30),
                                       new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = name;
            parms[1].Value = Guid.Parse(parentId.ToString());

            StringBuilder sb = new StringBuilder(100);
            if (!gId.Equals(Guid.Empty))
            {
                sb.Append(@" select 1 from [Service_Item] where lower(Named) = @Name and ParentId = @ParentId and Id <> @Id ");

                Array.Resize(ref parms, 3);
                parms[2] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                parms[2].Value = gId;
            }
            else
            {
                sb.Append(@" select 1 from [Service_Item] where lower(Named) = @Name and ParentId = @ParentId ");
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }


        #endregion
    }
}
