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
    public partial class ServiceItem
    {
        #region ServiceItem Member

        public ServiceItemInfo GetModelByJoin(object Id)
        {
            return dal.GetModelByJoin(Id);
        }

        public IList<ServiceItemInfo> GetListByRoot(object userId, int pageIndex, int pageSize, out int totalRecords)
        {
            string colAppend = " ,(select 1 from Service_UserPraise sup where sup.ServiceItemId = si.Id and sup.UserId = @UserId ) IsUserPraise ";
            string sqlWhere = " and si.ParentId = (select Id from Service_Item where ParentId = @ParentId) ";

            SqlParameter[] parms = {
                                       new SqlParameter("@ParentId", SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = Guid.Empty;
            parms[1].Value = Guid.Parse(userId.ToString());

            return dal.GetListByService(pageIndex, pageSize, out totalRecords, sqlWhere, colAppend, parms);
        }

        public IList<ServiceItemInfo> GetListByParentId(int pageIndex, int pageSize, out int totalRecords, Guid parentId)
        {
            string sqlWhere = " and si.ParentId = @ParentId ";
            SqlParameter parm = new SqlParameter("@ParentId", SqlDbType.UniqueIdentifier);
            parm.Value = parentId;

            return dal.GetListByJoin(pageIndex, pageSize, out totalRecords, sqlWhere, parm);
        }

        public string GetTreeJson()
        {
            StringBuilder jsonAppend = new StringBuilder(500);
            List<ServiceItemInfo> list = GetListByJoin(); ;
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

        private void CreateTreeJson(List<ServiceItemInfo> list, object parentId, ref StringBuilder jsonAppend)
        {
            jsonAppend.Append("[");
            var childList = list.FindAll(x => x.ParentId.Equals(parentId));
            if (childList.Count > 0)
            {
                int temp = 0;
                foreach (var model in childList)
                {
                    jsonAppend.Append("{\"id\":\"" + model.Id + "\",\"text\":\"" + model.Named + "\",\"state\":\"open\",\"attributes\":{\"ParentId\":\"" + model.ParentId + "\",\"PictureId\":\"" + model.PictureId + "\",\"Sort\":\"" + model.Sort + "\",\"HasVote\":\"" + model.HasVote + "\",\"HasContent\":\"" + model.HasContent + "\",\"HasLink\":\"" + model.HasLink + "\"}");
                    if (list.Any(r => r.ParentId.Equals(model.Id)))
                    {
                        jsonAppend.Append(",\"children\":");
                        CreateTreeJson(list, model.Id, ref jsonAppend);
                    }
                    jsonAppend.Append("}");
                    if (temp < childList.Count - 1) jsonAppend.Append(",");
                    temp++;
                }
            }
            jsonAppend.Append("]");
        }

        public List<ServiceItemInfo> GetListByJoin()
        {
            return dal.GetListByJoin();
        }

        public int DeleteByJoin(object Id)
        {
            return dal.DeleteByJoin(Id);
        }

        public void UpdateHasVote(Guid Id, bool hasVote)
        {
            dal.UpdateHasVote(Id, hasVote);
        }

        public void UpdateHasContent(Guid Id, bool hasContent)
        {
            dal.UpdateHasContent(Id, hasContent);
        }

        public void UpdateHasLink(Guid Id, bool hasLink)
        {
            dal.UpdateHasLink(Id, hasLink);
        }

        #endregion
    }
}
