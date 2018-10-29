using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TygaSoft.Model;
using TygaSoft.BLL;

namespace TygaSoft.Web.Handlers
{
    /// <summary>
    /// HandlerProvinceCiy 的摘要说明
    /// </summary>
    public class HandlerProvinceCiy : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string msg = "";
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
                    case "GetJsonForProvince":
                        GetJsonForProvince(context);
                        break;
                    case "GetJsonByParentId":
                        GetJsonByParentId(context);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            if (msg != "")
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + msg + "\"}");
            }
        }

        private void GetJsonForProvince(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            ProvinceCity bll = new ProvinceCity();
            var dic = bll.GetProvince();
            if (dic == null || dic.Count == 0)
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in dic)
            {
                sb.Append("{\"Id\":\"" + item.Key + "\",\"Name\":\"" + item.Value + "\"},");
            }

            context.Response.Write("{\"total\":" + dic.Count + ",\"rows\":[" + sb.ToString().Trim(',') + "]}");
        }

        private void GetJsonByParentId(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            Guid parentId = Guid.Empty;
            Guid.TryParse(context.Request.QueryString["parentId"], out parentId);
            if (parentId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }

            ProvinceCity bll = new ProvinceCity();
            var dic = bll.GetChild(parentId);
            if (dic == null || dic.Count == 0)
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in dic)
            {
                sb.Append("{\"Id\":\"" + item.Key + "\",\"Name\":\"" + item.Value + "\"},");
            }

            context.Response.Write("{\"total\":" + dic.Count + ",\"rows\":[" + sb.ToString().Trim(',') + "]}");
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