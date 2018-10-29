using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Newtonsoft.Json;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Handlers.Admin
{
    /// <summary>
    /// HandlerActivity 的摘要说明
    /// </summary>
    public class HandlerInformation : IHttpHandler
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
                    case "SaveInformation":
                        SaveInformation(context);
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

        #region 保存资讯
        public void SaveInformation(HttpContext context)
        {
            try
            {
                string id = context.Request.Form["ctl00$cphMain$hId"].Trim();
                string sTitle = context.Request.Form["ctl00$cphMain$txtTitle"].Trim();
                string sSummary = context.Request.Form["ctl00$cphMain$txtSummary"].Trim();
                string sSource = context.Request.Form["ctl00$cphMain$txtSource"].Trim();
                string sRemark = context.Request.Form["ctl00$cphMain$txtRemark"].Trim();
                string sContent = context.Request.Form["content"].Trim();
                string sIsDisable = context.Request.Form["isDisable"].Trim();
                string sViewType = context.Request.Form["rdViewType"].Trim();
                string sSort = context.Request.Form["ctl00$cphMain$txtSort"].Trim();
                sSort = sSort == "" ? "0" : sSort;
                string sViewCount = context.Request.Form["ctl00$cphMain$txtViewCount"].Trim();
                sViewCount = sViewCount == "" ? "0" : sViewCount;

                string sPictureIdList = context.Request.Form["pictureId"].TrimEnd(',');
                string sIsPush = context.Request.Form["isPush"].Trim();

                sContent = HttpUtility.HtmlDecode(sContent);

                Guid gId = Guid.Empty;
                if (id != "") Guid.TryParse(id, out gId);

                InformationInfo model = new InformationInfo();
                model.LastUpdatedDate = DateTime.Now;
                model.Remark = sRemark;

                model.Id = gId;
                model.Title = sTitle;
                model.Summary = sSummary;
                model.Source = sSource;
                model.ContentText = sContent;
                model.Sort = int.Parse(sSort);
                model.ViewCount = int.Parse(sViewCount);
                model.ViewType = byte.Parse(sViewType);
                model.IsDisable = bool.Parse(sIsDisable);
                model.IsPush = bool.Parse(sIsPush);

                if (string.IsNullOrWhiteSpace(model.ContentText))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                    return;
                }

                Information bll = new Information();
                InformationPicture bllPP = new InformationPicture();
                int effect = -1;

                if (!gId.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                    if (effect > 0)
                    {
                        bllPP.Delete(model.Id);
                        if (!string.IsNullOrWhiteSpace(sPictureIdList))
                        {
                            foreach (string sPictureId in sPictureIdList.Split(','))
                            {
                                InformationPictureInfo infoPP = new InformationPictureInfo();
                                Guid pictureId = Guid.Empty;
                                Guid.TryParse(sPictureId, out pictureId);
                                infoPP.InformationId = model.Id;
                                infoPP.PictureId = pictureId;
                                bllPP.InsertModel(infoPP);
                            }
                        }
                    }
                }
                else
                {
                    model.LastUpdatedDate = DateTime.Now;
                    Guid anformationId = bll.InsertByOutput(model);
                    if (!anformationId.Equals(Guid.Empty))
                    {
                        effect = 1;
                        if (!string.IsNullOrWhiteSpace(sPictureIdList))
                        {
                            foreach (string sPictureId in sPictureIdList.Split(','))
                            {
                                InformationPictureInfo infoPP = new InformationPictureInfo();
                                Guid pictureId = Guid.Empty;
                                Guid.TryParse(sPictureId, out pictureId);
                                infoPP.InformationId = anformationId;
                                infoPP.PictureId = pictureId;
                                bllPP.InsertModel(infoPP);
                            }
                        }
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

                if (model.IsPush)
                {
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
                            "xwfb", "", model.Title, "1");

                        pushProxy.ReceivePushContentAsync(sxml);
                    }
                    catch
                    {

                    }
                    #endregion
                }

                context.Response.Write("{\"success\": true,\"message\": \"" + MessageContent.Submit_Success + "\"}");
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.AlertTitle_Ex_Error + "：" + ex.Message + "\"}");
            }
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}