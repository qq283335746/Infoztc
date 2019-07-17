using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TygaSoft.Model;
using TygaSoft.WebHelper;
using TygaSoft.BLL;
using TygaSoft.DBUtility;
using System.Data;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace TygaSoft.Web.Handlers.Admin
{
    /// <summary>
    /// HandlerPushMsg 的摘要说明
    /// </summary>
    public class HandlerPushMsg : IHttpHandler
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
                    case "SavePushMsg":
                        SavePushMsg(context);
                        break;
                    case "GetUserList":
                        GetUserList(context);
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

        #region 保存推送信息
        public void SavePushMsg(HttpContext context)
        {
            try
            {
                string id = context.Request.Form["ctl00$cphMain$hId"].Trim();
                string sTitle = context.Request.Form["ctl00$cphMain$txtTitle"].Trim();
                string sPushContent = context.Request.Form["ctl00$cphMain$txtPushContent"].Trim();
                string sSendRange = context.Request.Form["ctl00$cphMain$txtSendRange"].Trim();
                string sckAll = context.Request.Form["ctl00$cphMain$ckAll"];


                Guid gId = Guid.Empty;

                PushMsgInfo model = new PushMsgInfo();
                model.LastUpdatedDate = DateTime.Now;

                model.Id = gId;
                model.Title = sTitle;
                model.PushContent = sPushContent;
                string PushParam = "1";
                if (null != sckAll)
                {
                    model.SendRange = "全部";
                }
                else
                {
                    model.SendRange = "个人";
                    if (-1 == sSendRange.IndexOf(','))
                    {
                        PushParam = "2@@" + sSendRange;
                    }
                    else
                    {
                        PushParam = "3@@" + sSendRange;
                    }
                }
                model.PushType = "zdy";

                if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.PushContent) )
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                    return;
                }

                PushMsg bll = new PushMsg();
                int effect = -1;

                model.LastUpdatedDate = DateTime.Now;
                Guid pushMsgId = bll.InsertByOutput(model);

                if (!pushMsgId.Equals(Guid.Empty))
                {
                    effect = 1;

                    if (!PushParam.Equals("1"))
                    {
                        PushUser bllPushUser = new PushUser();
                        PushUserInfo pushUserInfo = new PushUserInfo();
                        pushUserInfo.PushId = pushMsgId;
                        pushUserInfo.PushUser = sSendRange;
                        bllPushUser.InsertOW(pushUserInfo);
                    }
                }

                if (effect == 110)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Exist + "\"}");
                    return;
                }

                if (effect < 1)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Error + "\"}");
                    return;
                }
               
                
                #region 上传推送信息到推送服务系统
                try
                {
                    PushContentService pushProxy = new PushContentService();
                    if (System.Configuration.ConfigurationManager.AppSettings["PushServiceUrl"] != null)
                    {
                        pushProxy.Url = System.Configuration.ConfigurationManager.AppSettings["PushServiceUrl"].ToString();
                    }

                    string sxml = "";
                    sxml = string.Format(@"<XmlParameters><ReceivePushContent><PushType>{0}</PushType><PushContent>{1}</PushContent><Title>{2}</Title><PushParam>{3}</PushParam></ReceivePushContent></XmlParameters>",
                        "zdy", model.PushContent, model.Title, PushParam);

                    pushProxy.ReceivePushContentAsync(sxml);
                }
                catch
                {

                }
                #endregion
                
                context.Response.Write("{\"success\": true,\"message\": \"" + MessageContent.Submit_Success + "\"}");
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.AlertTitle_Ex_Error + "：" + ex.Message + "\"}");
            }
        }
        #endregion

        public void GetUserList(HttpContext context)
        {
            try
            {
                ParamsHelper parms = null;
                string sqlWhere = "";
                string UserName = context.Request.QueryString["userName"];
                string sPageIndex = context.Request.QueryString["pageIndex"];
                string sPageSize = context.Request.QueryString["pageSize"];
                if (!string.IsNullOrWhiteSpace(UserName))
                {
                    parms = new ParamsHelper();
                    sqlWhere += "and UserName like @UserName ";
                    SqlParameter parm = new SqlParameter("@UserName", SqlDbType.NVarChar, 100);
                    parm.Value = "%" + UserName + "%";
                    parms.Add(parm);
                }
                PushMsg bll = new PushMsg();
                int totalRecords = 0;
                int pageIndex = Convert.ToInt32(sPageIndex);
                int pageSize = Convert.ToInt32(sPageSize); 
                DataSet inforAdDs = bll.GetUserList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                string strJSON = JsonConvert.SerializeObject(inforAdDs);
                strJSON = strJSON.Substring(0,strJSON.Length-1) + ",\"total\":" + totalRecords + "}";
                context.Response.Write(strJSON);
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.AlertTitle_Ex_Error + "：" + ex.Message + "\"}");
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