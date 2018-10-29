using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;

namespace TygaSoft.BLL
{
    public partial class Brand
    {
        #region Brand Member

        public List<BrandInfo> GetListByCategoryId(object categoryId)
        {
            string sqlWhere = "and cb.CategoryId = @CategoryId ";
            SqlParameter parm = new SqlParameter("@CategoryId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(categoryId.ToString());

            return GetListByJoin(sqlWhere, parm);
        }

        public List<BrandInfo> GetListByParentId(object parentId)
        {
            string sqlWhere = "and b.ParentId = @ParentId ";
            SqlParameter parm = new SqlParameter("@ParentId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(parentId.ToString());

            return GetListByJoin(sqlWhere, parm);
        }

        public string GetTreeJson()
        {
            StringBuilder jsonAppend = new StringBuilder();
            List<BrandInfo> list = GetListByJoin();
            if (list != null && list.Count > 0)
            {
                CreateTreeJson(list, Guid.Empty, ref jsonAppend);
            }
            else
            {
                jsonAppend.Append("[{\"id\":\"" + Guid.Empty + "\",\"text\":\"请选择\",\"state\":\"open\",\"attributes\":{\"parentId\":\"" + Guid.Empty + "\",\"parentName\":\"请选择\"}}]");
            }

            return jsonAppend.ToString();
        }

        private void CreateTreeJson(List<BrandInfo> list, object parentId, ref StringBuilder jsonAppend)
        {
            jsonAppend.Append("[");
            var childList = list.FindAll(x => x.ParentId.Equals(parentId));
            if (childList.Count > 0)
            {
                int index = 0;
                foreach (var model in childList)
                {
                    jsonAppend.Append("{\"id\":\"" + model.Id + "\",\"text\":\"" + model.BrandName + "\",\"state\":\"open\",\"attributes\":{\"parentId\":\"" + model.ParentId + "\"}");
                    if (list.Any(r => r.ParentId.Equals(model.Id)))
                    {
                        jsonAppend.Append(",\"children\":");
                        CreateTreeJson(list, model.Id, ref jsonAppend);
                    }
                    jsonAppend.Append("}");
                    if (index < childList.Count - 1) jsonAppend.Append(",");
                    index++;
                }
            }
            jsonAppend.Append("]");
        }

        public BrandInfo GetModelByJoin(object Id)
        {
            return dal.GetModelByJoin(Id);
        }

        public List<BrandInfo> GetListByJoin(string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListByJoin(sqlWhere, cmdParms);
        }

        public List<BrandInfo> GetListByJoin()
        {
            return dal.GetListByJoin();
        }

        public Guid InsertAndGetId(BrandInfo model)
        {
            return dal.InsertByOutput(model);
        }

        #endregion
    }
}
