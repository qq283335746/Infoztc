using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;
using TygaSoft.IDAL;
using TygaSoft.DBUtility;

namespace TygaSoft.SqlServerDAL
{
    public partial class ProvinceCity
    {
        public Dictionary<object, string> GetProvince()
        {
            var model = GetModel("中国");
            if (model == null) return new Dictionary<object, string>();

            return GetChild((Guid)model.Id);
        }

        public Dictionary<object, string> GetChild(Guid parentId)
        {
            Dictionary<object, string> dic = new Dictionary<object, string>();

            SqlParameter parm = new SqlParameter("@ParentId", SqlDbType.UniqueIdentifier);
            parm.Value = parentId;
            var list = GetList("and ParentId = @ParentId", parm);

            if (list != null)
            {
                foreach (var item in list)
                {
                    dic.Add(item.Id, item.Named);
                }
            }

            return dic;
        }

        public ProvinceCityInfo GetModel(string name)
        {
            ProvinceCityInfo model = null;

            string cmdText = @"select top 1 Id,Named,Pinyin,FirstChar,ParentId,Sort,Remark,LastUpdatedDate 
			                   from ProvinceCity
							   where Named = @Named ";
            SqlParameter parm = new SqlParameter("@Named", SqlDbType.NVarChar,50);
            parm.Value = name;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ProvinceCityInfo();
                        model.Id = reader.GetGuid(0);
                        model.Named = reader.GetString(1);
                        model.Pinyin = reader.GetString(2);
                        model.FirstChar = reader.GetString(3);
                        model.ParentId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                    }
                }
            }

            return model;
        }

        private bool IsExist(string name, object parentId, object Id)
        {
            Guid gId = Guid.Empty;
            if (Id != null)
            {
                Guid.TryParse(Id.ToString(), out gId);
            }

            SqlParameter[] parms = {
                                       new SqlParameter("@Named",SqlDbType.NVarChar, 30),
                                       new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = name;
            parms[1].Value = Guid.Parse(parentId.ToString());

            StringBuilder sb = new StringBuilder(100);
            if (!gId.Equals(Guid.Empty))
            {
                sb.Append(@" select 1 from [ProvinceCity] where lower(Named) = @Named and ParentId = @ParentId and Id <> @Id ");

                Array.Resize(ref parms, 3);
                parms[2] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                parms[2].Value = gId;
            }
            else
            {
                sb.Append(@" select 1 from [ProvinceCity] where lower(Named) = @Named and ParentId = @ParentId ");
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }

    }
}
