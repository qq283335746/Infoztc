using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;
using TygaSoft.BLL;

namespace TygaSoft.Web.Handlers.Admin.ECShop
{
    /// <summary>
    /// HandlerCategory 的摘要说明
    /// </summary>
    public class HandlerCategory : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

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
                    case "GetCategoryTreeJson":
                        GetCategoryTreeJson(context);
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
                context.Response.Write("[]");
            }
        }

        private void GetCategoryTreeJson(HttpContext context)
        {
            Category bll = new Category();
            context.Response.Write(bll.GetTreeJson());
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