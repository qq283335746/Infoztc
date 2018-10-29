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
    public class HandlerInformationAd : IHttpHandler
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
                    case "SaveInformationAd":
                        SaveInformationAd(context);
                        break;
                    case "GetInforAdList":
                        GetInforAdList(context);
                        break;
                    case "GetSelectedInforAdList":
                        GetSelectedInforAdList(context);
                        break;
                    case "SubmitRelInforAd":
                        SubmitRelInforAd(context);
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

        #region 保存资讯广告
        public void SaveInformationAd(HttpContext context)
        {
            try
            {
                string id = context.Request.Form["ctl00$cphMain$hId"].Trim();
                string sTitle = context.Request.Form["ctl00$cphMain$txtTitle"].Trim();
                string sDescr = context.Request.Form["ctl00$cphMain$txtDescr"].Trim();
                string sContent = context.Request.Form["content"].Trim();
                string sViewType = context.Request.Form["rdViewType"].Trim();
                string startDate = context.Request.Form["ctl00$cphMain$startDate"].Trim();
                string endDate = context.Request.Form["ctl00$cphMain$endDate"].Trim();
                string sUrl = context.Request.Form["ctl00$cphMain$txtUrl"].Trim();

                string sPictureIdList = context.Request.Form["pictureId"].TrimEnd(',');

                sContent = HttpUtility.HtmlDecode(sContent);

                Guid gId = Guid.Empty;
                if (id != "") Guid.TryParse(id, out gId);

                InformationAdInfo model = new InformationAdInfo();
                InformationAdPicture bllPP = new InformationAdPicture();
                model.LastUpdatedDate = DateTime.Now;

                model.Id = gId;
                model.Title = sTitle;
                model.Descr = sDescr;
                model.ContentText = sContent;
                model.ViewType = byte.Parse(sViewType);
                model.Url = sUrl;
                model.StartDate = DateTime.Parse(startDate);
                model.EndDate = DateTime.Parse(endDate);

                if (1 == model.ViewType && string.IsNullOrWhiteSpace(model.ContentText))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                    return;
                }

                InformationAd bll = new InformationAd();
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
                                InformationAdPictureInfo infoPP = new InformationAdPictureInfo();
                                Guid pictureId = Guid.Empty;
                                Guid.TryParse(sPictureId, out pictureId);
                                infoPP.InformationAdId = model.Id;
                                infoPP.PictureId = pictureId;
                                bllPP.InsertModel(infoPP);
                            }
                        }
                    }
                }
                else
                {
                    model.LastUpdatedDate = DateTime.Now;
                    Guid anformationAdId = bll.InsertByOutput(model);
                    if (!anformationAdId.Equals(Guid.Empty))
                    {
                        effect = 1;
                        if (!string.IsNullOrWhiteSpace(sPictureIdList))
                        {
                            foreach (string sPictureId in sPictureIdList.Split(','))
                            {
                                InformationAdPictureInfo infoPP = new InformationAdPictureInfo();
                                Guid pictureId = Guid.Empty;
                                Guid.TryParse(sPictureId, out pictureId);
                                infoPP.InformationAdId = anformationAdId;
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

                context.Response.Write("{\"success\": true,\"message\": \"" + MessageContent.Submit_Success + "\"}");
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.AlertTitle_Ex_Error + "：" + ex.Message + "\"}");
            }
        }
        #endregion

        #region 获取广告列表
        public void GetInforAdList(HttpContext context)
        {
            try
            {
                InformationAd iaBll = new InformationAd();
                IList<InformationAdInfo> inforList = iaBll.GetList();

                string strJSON = JsonConvert.SerializeObject(inforList);
                context.Response.Write(strJSON);
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.AlertTitle_Ex_Error + "：" + ex.Message + "\"}");
            }
        }
        #endregion

        #region 获取关联的广告列表
        public void GetSelectedInforAdList(HttpContext context)
        {
            try
            {
                string id = context.Request.Form["id"].Trim();
                InformationAd iaBll = new InformationAd();
                DataSet inforAdDs = iaBll.GetInforAdListOW(id);

                string strJSON = JsonConvert.SerializeObject(inforAdDs);
                context.Response.Write(strJSON);
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.AlertTitle_Ex_Error + "：" + ex.Message + "\"}");
            }
        }
        #endregion

        #region 设置资讯广告关联
        public void SubmitRelInforAd(HttpContext context)
        {
            try
            {
                string informationId = context.Request.Form["InformationId"].Trim();
                string data = context.Request.Form["data"].Trim();
                Guid gInformationId = Guid.Empty;
                Guid.TryParse(informationId, out gInformationId);

                InformationAd iaBll = new InformationAd();
                int effect = -1;

                effect = iaBll.DeleteInformationAd(gInformationId);
                if (effect >= 0)
                {
                    if (!string.IsNullOrWhiteSpace(data))
                    {
                        int index = 0;
                        foreach (string informationAdId in data.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            Guid gInformationAdId = Guid.Empty;
                            Guid.TryParse(informationAdId, out gInformationAdId);
                            iaBll.InsertInformationAd(gInformationId, gInformationAdId, index);
                            index++;
                        }
                    }
                }

                if (effect < 0)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Error + "\"}");
                }
                else
                {
                    context.Response.Write("{\"success\": true,\"message\": \"" + MessageContent.Submit_Success + "\"}");
                }
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