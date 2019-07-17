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
    public partial class AdItem : IAdItem
    {
        #region IAdItem Member

        public int Insert(AdItemInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into AdItem (Id, AdvertisementId,PictureId,ActionTypeId,Sort,IsDisable)
			            values
						(@Id,@AdvertisementId,@PictureId,@ActionTypeId,@Sort,@IsDisable)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@AdvertisementId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@ActionTypeId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.AdvertisementId;
            parms[2].Value = model.PictureId;
            parms[3].Value = model.ActionTypeId;
            parms[4].Value = model.Sort;
            parms[5].Value = model.IsDisable;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(AdItemInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update AdItem set AdvertisementId = @AdvertisementId,PictureId = @PictureId,ActionTypeId = @ActionTypeId,Sort = @Sort,IsDisable = @IsDisable 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@AdvertisementId",SqlDbType.UniqueIdentifier),
new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier),
new SqlParameter("@ActionTypeId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@IsDisable",SqlDbType.Bit)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.AdvertisementId;
            parms[2].Value = model.PictureId;
            parms[3].Value = model.ActionTypeId;
            parms[4].Value = model.Sort;
            parms[5].Value = model.IsDisable;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from AdItem where Id = @Id");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parm);
        }

        public bool DeleteBatch(IList<object> list)
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

        public AdItemInfo GetModel(object Id)
        {
            AdItemInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,AdvertisementId,PictureId,ActionTypeId,Sort,IsDisable 
			            from AdItem
						where Id = @Id ");
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
                    }
                }
            }

            return model;
        }

        public IList<AdItemInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from AdItem ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<AdItemInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,AdvertisementId,PictureId,ActionTypeId,Sort,IsDisable
					  from AdItem ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<AdItemInfo> list = new List<AdItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdItemInfo model = new AdItemInfo();
                        model.Id = reader.GetGuid(1);
                        model.AdvertisementId = reader.GetGuid(2);
                        model.PictureId = reader.GetGuid(3);
                        model.ActionTypeId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.IsDisable = reader.GetBoolean(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AdItemInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,AdvertisementId,PictureId,ActionTypeId,Sort,IsDisable
					   from AdItem ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<AdItemInfo> list = new List<AdItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdItemInfo model = new AdItemInfo();
                        model.Id = reader.GetGuid(1);
                        model.AdvertisementId = reader.GetGuid(2);
                        model.PictureId = reader.GetGuid(3);
                        model.ActionTypeId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.IsDisable = reader.GetBoolean(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AdItemInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,AdvertisementId,PictureId,ActionTypeId,Sort,IsDisable
                        from AdItem ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<AdItemInfo> list = new List<AdItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdItemInfo model = new AdItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.AdvertisementId = reader.GetGuid(1);
                        model.PictureId = reader.GetGuid(2);
                        model.ActionTypeId = reader.GetGuid(3);
                        model.Sort = reader.GetInt32(4);
                        model.IsDisable = reader.GetBoolean(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AdItemInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,AdvertisementId,PictureId,ActionTypeId,Sort,IsDisable 
			            from AdItem
					    order by LastUpdatedDate desc ");

            IList<AdItemInfo> list = new List<AdItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdItemInfo model = new AdItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.AdvertisementId = reader.GetGuid(1);
                        model.PictureId = reader.GetGuid(2);
                        model.ActionTypeId = reader.GetGuid(3);
                        model.Sort = reader.GetInt32(4);
                        model.IsDisable = reader.GetBoolean(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
