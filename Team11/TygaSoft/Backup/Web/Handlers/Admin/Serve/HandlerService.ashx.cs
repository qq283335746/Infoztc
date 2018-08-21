using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;
using TygaSoft.DBUtility;

namespace TygaSoft.Web.Handlers.Admin.Serve
{
    /// <summary>
    /// HandlerService 的摘要说明
    /// </summary>
    public class HandlerService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string errorMsg = "";
            try
            {
                string reqName = "";
                switch (context.Request.HttpMethod.ToUpper())
                {
                    case "GET":
                        reqName = context.Request.QueryString["reqName"];
                        break;
                    case "POST":
                        reqName = context.Request.Form["reqName"];
                        break;
                    default:
                        break;
                }

                switch (reqName)
                {
                    case "GetDatagridForServiceVote":
                        GetDatagridForServiceVote(context);
                        break;
                    case "GetDatagridForServiceContent":
                        GetDatagridForServiceContent(context);
                        break;
                    case "GetDatagridForServiceLink":
                        GetDatagridForServiceLink(context);
                        break;
                    case "SaveServiceVote":
                        SaveServiceVote(context);
                        break;
                    case "SaveServiceContent":
                        SaveServiceContent(context);
                        break;
                    case "SaveServiceLink":
                        SaveServiceLink(context);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            if (errorMsg != "")
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + errorMsg + "\"}");
            }
        }

        private void GetDatagridForServiceVote(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int totalRecords = 0;
            int pageIndex = 1;
            int pageSize = 10;
            int.TryParse(context.Request.Form["page"], out pageIndex);
            int.TryParse(context.Request.Form["rows"], out pageSize);

            Guid serviceItemId = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(context.Request.Form["serviceItemId"]))
            {
                Guid.TryParse(context.Request.Form["serviceItemId"], out serviceItemId);
            }
            if (serviceItemId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return; 
            }
            string keyword = context.Request.Form["keyword"];

            string sqlWhere = string.Empty;
            ParamsHelper parms = null;
            if (!serviceItemId.Equals(Guid.Empty))
            {
                sqlWhere += "and sv.ServiceItemId = @ServiceItemId ";
                SqlParameter parm = new SqlParameter("@ServiceItemId",SqlDbType.UniqueIdentifier);
                parm.Value = serviceItemId;
                if (parms == null) parms = new ParamsHelper();
                parms.Add(parm);
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                if (parms == null) parms = new ParamsHelper();
                sqlWhere += "and (sv.Named like @Named or sv.Descr like @Descr) ";
                SqlParameter parm = new SqlParameter("@Named", SqlDbType.NVarChar, 30);
                parm.Value = "%" + keyword.Trim() + "%";
                parms.Add(parm);
                parm = new SqlParameter("@Descr", SqlDbType.NVarChar, 300);
                parm.Value = "%" + keyword.Trim() + "%";

                parms.Add(parm);
            }

            ServiceVote bll = new ServiceVote();
            var list = bll.GetListByJoin(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
            if (list == null || list.Count == 0)
            {
                ServiceItem siBll = new ServiceItem();
                siBll.UpdateHasVote(serviceItemId,false);
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var model in list)
            {
                sb.Append("{\"Id\":\"" + model.Id + "\",\"ServiceItemId\":\"" + model.ServiceItemId + "\",\"ServiceItemName\":\"" + model.ServiceItemName + "\",\"Named\":\"" + model.Named + "\",\"Descr\":\"" + model.Descr + "\",\"Sort\":\"" + model.Sort + "\",\"OriginalPicture\":\"" + model.OriginalPicture.Replace("~", "") + "\",\"BPicture\":\"" + model.BPicture + "\",\"MPicture\":\"" + model.MPicture + "\",\"SPicture\":\"" + model.SPicture + "\",\"OtherPicture\":\"" + model.OtherPicture + "\",\"LastUpdatedDate\":\"" + model.LastUpdatedDate.ToString("yyyy-MM-dd HH:mm") + "\"},");
            }
            context.Response.Write("{\"total\":" + totalRecords + ",\"rows\":[" + sb.ToString().Trim(',') + "]}");
        }

        private void SaveServiceVote(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            try
            {
                string svId = context.Request.Form["hServiceVoteId"].Trim();
                string sServiceItemId = context.Request.Form["ServiceItemId"].Trim();
                string sNamed = context.Request.Form["txtNamed_ServiceVote"].Trim();
                string sHeadPictureId = context.Request.Form["hHeadPictureId_ServiceVote"].Trim();
                string sSort = context.Request.Form["Sort"].Trim();

                if (string.IsNullOrWhiteSpace(sNamed))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                    return;
                }
               
                Guid serviceItemId = Guid.Empty;
                Guid.TryParse(sServiceItemId, out serviceItemId);
                Guid headPictureId = Guid.Empty;
                Guid.TryParse(sHeadPictureId, out headPictureId);
                int sort = 0;
                if (!string.IsNullOrWhiteSpace(sSort)) int.TryParse(sSort, out sort);

                string sDescr = context.Request.Form["txtaDescr_ServiceVote"].Trim();
                string content = context.Request.Form["content"].Trim();
                content = HttpUtility.HtmlDecode(content);

                Guid gId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(svId)) Guid.TryParse(svId, out gId);

                ServiceVoteInfo model = new ServiceVoteInfo();
                model.Id = gId;
                model.LastUpdatedDate = DateTime.Now;
                model.ServiceItemId = serviceItemId;
                model.Named = sNamed;
                model.HeadPictureId = headPictureId;
                model.Descr = sDescr;
                model.ContentText = content;
                model.Sort = sort;

                ServiceVote bll = new ServiceVote();
                ServiceItem siBll = new ServiceItem();

                var siModel = siBll.GetModel(serviceItemId);
                if (siModel == null)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"服务分类【"+serviceItemId+"】" + MessageContent.Submit_Data_NotExists + "\"}");
                    return;
                }

                int effect = -1;
                using (TransactionScope scope = new TransactionScope())
                {
                    if (!gId.Equals(Guid.Empty))
                    {
                        effect = bll.Update(model);
                        if ((!siModel.HasVote) && effect > 0)
                        {
                            siModel.HasVote = true;
                            siBll.Update(siModel);
                        }
                    }
                    else
                    {
                        effect = bll.Insert(model);
                        if ((!siModel.HasVote) && effect > 0)
                        {
                            siModel.HasVote = true;
                            siBll.Update(siModel);
                        }
                    }

                    scope.Complete();
                }

                if (effect < 1)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"操作失败，请正确输入\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"操作成功\"}");
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"异常：" + ex.Message + "\"}");
            }
        }

        private void GetDatagridForServiceContent(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int totalRecords = 0;
            int pageIndex = 1;
            int pageSize = 10;
            int.TryParse(context.Request.Form["page"], out pageIndex);
            int.TryParse(context.Request.Form["rows"], out pageSize);

            Guid serviceItemId = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(context.Request.Form["serviceItemId"]))
            {
                Guid.TryParse(context.Request.Form["serviceItemId"], out serviceItemId);
            }
            if (serviceItemId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }
            string keyword = context.Request.Form["keyword"];

            string sqlWhere = string.Empty;
            ParamsHelper parms = null;
            if (!serviceItemId.Equals(Guid.Empty))
            {
                if (parms == null) parms = new ParamsHelper();
                sqlWhere += "and sc.ServiceItemId = @ServiceItemId ";
                SqlParameter parm = new SqlParameter("@ServiceItemId", SqlDbType.UniqueIdentifier);
                parm.Value = serviceItemId;
                
                parms.Add(parm);
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                if (parms == null) parms = new ParamsHelper();
                sqlWhere += "and (sc.Named like @Named or sc.Descr like @Descr) ";
                SqlParameter parm = new SqlParameter("@Named", SqlDbType.NVarChar,30);
                parm.Value = "%"+keyword.Trim()+"%";
                parms.Add(parm);
                parm = new SqlParameter("@Descr", SqlDbType.NVarChar, 300);
                parm.Value = "%" + keyword.Trim() + "%";

                parms.Add(parm);
            }

            ServiceContent bll = new ServiceContent();
            var list = bll.GetListByJoin(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
            if (list == null || list.Count == 0)
            {
                ServiceItem siBll = new ServiceItem();
                siBll.UpdateHasContent(serviceItemId, false);
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var model in list)
            {
                sb.Append("{\"Id\":\"" + model.Id + "\",\"ServiceItemId\":\"" + model.ServiceItemId + "\",\"ServiceItemName\":\"" + model.ServiceItemName + "\",\"Named\":\"" + model.Named + "\",\"Descr\":\"" + model.Descr + "\",\"Sort\":\"" + model.Sort + "\",\"LastUpdatedDate\":\"" + model.LastUpdatedDate.ToString("yyyy-MM-dd HH:mm") + "\",\"OriginalPicture\":\"" + model.OriginalPicture.Replace("~", "") + "\",\"BPicture\":\"" + model.BPicture + "\",\"MPicture\":\"" + model.MPicture + "\",\"SPicture\":\"" + model.SPicture + "\",\"OtherPicture\":\"" + model.OtherPicture + "\"},");
            }
            context.Response.Write("{\"total\":" + totalRecords + ",\"rows\":[" + sb.ToString().Trim(',') + "]}");
        }

        private void SaveServiceContent(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            try
            {
                string scId = context.Request.Form["hServiceContentId"].Trim();
                string sServiceItemId = context.Request.Form["ServiceItemId"].Trim();
                string sNamed = context.Request.Form["txtNamed_ServiceContent"].Trim();
                string sPictureId = context.Request.Form["hPictureId_ServiceContent"].Trim();
                string sSort = context.Request.Form["Sort"].Trim();

                if (string.IsNullOrWhiteSpace(sNamed))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                    return;
                }

                Guid serviceItemId = Guid.Empty;
                Guid.TryParse(sServiceItemId, out serviceItemId);
                Guid pictureId = Guid.Empty;
                Guid.TryParse(sPictureId, out pictureId);
                int sort = 0;
                if (!string.IsNullOrWhiteSpace(sSort)) int.TryParse(sSort, out sort);

                string sDescr = context.Request.Form["txtaDescr_ServiceContent"].Trim();
                string content = context.Request.Form["content"].Trim();
                content = HttpUtility.HtmlDecode(content);

                Guid gId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(scId)) Guid.TryParse(scId, out gId);

                ServiceContentInfo model = new ServiceContentInfo();
                model.Id = gId;
                model.LastUpdatedDate = DateTime.Now;
                model.ServiceItemId = serviceItemId;
                model.Named = sNamed;
                model.PictureId = pictureId;
                model.Descr = sDescr;
                model.ContentText = content;
                model.Sort = sort;

                ServiceContent bll = new ServiceContent();
                ServiceItem siBll = new ServiceItem();

                var siModel = siBll.GetModel(serviceItemId);
                if (siModel == null)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"服务分类【" + serviceItemId + "】" + MessageContent.Submit_Data_NotExists + "\"}");
                    return;
                }

                int effect = -1;
                using (TransactionScope scope = new TransactionScope())
                {
                    if (!gId.Equals(Guid.Empty))
                    {
                        effect = bll.Update(model);
                        if ((!siModel.HasContent) && effect > 0)
                        {
                            siModel.HasContent = true;
                            siBll.Update(siModel);
                        }
                    }
                    else
                    {
                        effect = bll.Insert(model);
                        if ((!siModel.HasContent) && effect > 0)
                        {
                            siModel.HasContent = true;
                            siBll.Update(siModel);
                        }
                    }

                    scope.Complete();
                }

                if (effect < 1)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"操作失败，请正确输入\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"操作成功\"}");
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"异常：" + ex.Message + "\"}");
            }
        }

        private void GetDatagridForServiceLink(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int totalRecords = 0;
            int pageIndex = 1;
            int pageSize = 10;
            int.TryParse(context.Request.Form["page"], out pageIndex);
            int.TryParse(context.Request.Form["rows"], out pageSize);

            Guid serviceItemId = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(context.Request.Form["serviceItemId"]))
            {
                Guid.TryParse(context.Request.Form["serviceItemId"], out serviceItemId);
            }
            if (serviceItemId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }
            string keyword = context.Request.Form["keyword"];

            string sqlWhere = string.Empty;
            ParamsHelper parms = null;
            if (!serviceItemId.Equals(Guid.Empty))
            {
                if (parms == null) parms = new ParamsHelper();
                sqlWhere += "and sl.ServiceItemId = @ServiceItemId ";
                SqlParameter parm = new SqlParameter("@ServiceItemId", SqlDbType.UniqueIdentifier);
                parm.Value = serviceItemId;

                parms.Add(parm);
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                if (parms == null) parms = new ParamsHelper();
                sqlWhere += "and (sl.Named like @Named or sl.Url like @Url) ";
                SqlParameter parm = new SqlParameter("@Named", SqlDbType.NVarChar, 30);
                parm.Value = "%" + keyword + "%";
                parms.Add(parm);
                parm = new SqlParameter("@Url", SqlDbType.VarChar, 300);
                parm.Value = "%" + keyword + "%";
                parms.Add(parm);
            }

            ServiceLink bll = new ServiceLink();
            var list = bll.GetListByJoin(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
            if (list == null || list.Count == 0)
            {
                ServiceItem siBll = new ServiceItem();
                siBll.UpdateHasContent(serviceItemId, false);
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var model in list)
            {
                sb.Append("{\"Id\":\"" + model.Id + "\",\"ServiceItemId\":\"" + model.ServiceItemId + "\",\"ServiceItemName\":\"" + model.ServiceItemName + "\",\"Named\":\"" + model.Named + "\",\"Url\":\"" + model.Url + "\",\"Sort\":\"" + model.Sort + "\",\"LastUpdatedDate\":\"" + model.LastUpdatedDate.ToString("yyyy-MM-dd HH:mm") + "\",\"OriginalPicture\":\"" + model.OriginalPicture.Replace("~", "") + "\",\"BPicture\":\"" + model.BPicture + "\",\"MPicture\":\"" + model.MPicture + "\",\"SPicture\":\"" + model.SPicture + "\",\"OtherPicture\":\"" + model.OtherPicture + "\"},");
            }
            context.Response.Write("{\"total\":" + totalRecords + ",\"rows\":[" + sb.ToString().Trim(',') + "]}");
        }

        private void SaveServiceLink(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            try
            {
                string scId = context.Request.Form["hServiceLinkId"].Trim();
                string sServiceItemId = context.Request.Form["ServiceItemId"].Trim();
                string sNamed = context.Request.Form["txtNamed_ServiceLink"].Trim();
                string sPictureId = context.Request.Form["hPictureId_ServiceLink"].Trim();
                string sSort = context.Request.Form["Sort"].Trim();

                if (string.IsNullOrWhiteSpace(sNamed))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                    return;
                }

                Guid serviceItemId = Guid.Empty;
                Guid.TryParse(sServiceItemId, out serviceItemId);
                Guid pictureId = Guid.Empty;
                Guid.TryParse(sPictureId, out pictureId);
                int sort = 0;
                if (!string.IsNullOrWhiteSpace(sSort)) int.TryParse(sSort, out sort);

                string sUrl = context.Request.Form["txtUrl_ServiceLink"].Trim();

                Guid gId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(scId)) Guid.TryParse(scId, out gId);

                ServiceLinkInfo model = new ServiceLinkInfo();
                model.Id = gId;
                model.LastUpdatedDate = DateTime.Now;
                model.ServiceItemId = serviceItemId;
                model.Named = sNamed;
                model.PictureId = pictureId;
                model.Url = sUrl;
                model.Sort = sort;

                ServiceLink bll = new ServiceLink();
                ServiceItem siBll = new ServiceItem();

                var siModel = siBll.GetModel(serviceItemId);
                if (siModel == null)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"服务分类【" + serviceItemId + "】" + MessageContent.Submit_Data_NotExists + "\"}");
                    return;
                }

                int effect = -1;
                using (TransactionScope scope = new TransactionScope())
                {
                    if (!gId.Equals(Guid.Empty))
                    {
                        effect = bll.Update(model);
                        if ((!siModel.HasContent) && effect > 0)
                        {
                            siModel.HasLink = true;
                            siBll.Update(siModel);
                        }
                    }
                    else
                    {
                        effect = bll.Insert(model);
                        if ((!siModel.HasLink) && effect > 0)
                        {
                            siModel.HasLink = true;
                            siBll.Update(siModel);
                        }
                    }

                    scope.Complete();
                }

                if (effect < 1)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"操作失败，请正确输入\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"操作成功\"}");
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"异常：" + ex.Message + "\"}");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}