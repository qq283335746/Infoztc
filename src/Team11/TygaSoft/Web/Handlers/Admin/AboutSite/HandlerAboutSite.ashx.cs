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

namespace TygaSoft.Web.Handlers.Admin.AboutSite
{
    /// <summary>
    /// HandlerAboutSite 的摘要说明
    /// </summary>
    public class HandlerAboutSite : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
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
                    case "SaveAdBase":
                        SaveAdBase(context);
                        break;
                    case "SaveAdItem":
                        SaveAdItem(context);
                        break;
                    case "SaveAdItemLink":
                        SaveAdItemLink(context);
                        break;
                    case "SaveAdItemContent":
                        SaveAdItemContent(context);
                        break;
                    case "SaveContentDetail":
                        SaveContentDetail(context);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 广告基本信息提交
        /// </summary>
        /// <param name="context"></param>
        private void SaveAdBase(HttpContext context)
        {
            try
            {
                string sAdId = context.Request.Form["ctl00$cphMain$hAdId"].Trim();
                string title = context.Request.Form["ctl00$cphMain$txtTitle"].Trim();
                string sSiteFunId = context.Request.Form["siteFunId"].Trim();
                string sLayoutPositionId = context.Request.Form["layoutPositionId"].Trim();
                string sTimeout = context.Request.Form["ctl00$cphMain$txtTimeout"].Trim();
                string sSort = context.Request.Form["ctl00$cphMain$txtSort"].Trim();
                string sStartTime = context.Request.Form["ctl00$cphMain$txtStartTime"].Trim();
                string sEndTime = context.Request.Form["ctl00$cphMain$txtEndTime"].Trim();
                string sVirtualViewCount = context.Request.Form["ctl00$cphMain$txtVirtualViewCount"].Trim();
                string sIsDisable = context.Request.Form["isDisable"].Trim();
                
                int timeout = 0;
                if (!string.IsNullOrWhiteSpace(sTimeout)) int.TryParse(sTimeout, out timeout);
                Guid siteFunId = Guid.Empty;
                Guid.TryParse(sSiteFunId, out siteFunId);
                Guid layoutPositionId = Guid.Empty;
                Guid.TryParse(sLayoutPositionId, out layoutPositionId);
                int sort = 0;
                if (!string.IsNullOrWhiteSpace(sSort)) int.TryParse(sSort, out sort);
                int virtualViewCount = 0;
                if (!string.IsNullOrWhiteSpace(sVirtualViewCount)) int.TryParse(sVirtualViewCount, out virtualViewCount);
                DateTime startTime = DateTime.MinValue;
                if (!string.IsNullOrWhiteSpace(sStartTime)) DateTime.TryParse(sStartTime, out startTime);
                DateTime endTime = DateTime.MinValue;
                if (!string.IsNullOrWhiteSpace(sEndTime)) DateTime.TryParse(sEndTime, out endTime);

                Guid adId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(sAdId)) Guid.TryParse(sAdId, out adId);

                AdBaseInfo model = new AdBaseInfo();
                model.Id = adId;
                model.LastUpdatedDate = DateTime.Now;
                model.Title = title;
                model.Timeout = timeout;
                model.Sort = sort;
                model.StartTime = startTime;
                model.EndTime = endTime;
                model.VirtualViewCount = virtualViewCount;
                model.ViewCount = 0;
                model.SiteFunId = siteFunId;
                model.LayoutPositionId = layoutPositionId;
                model.IsDisable = sIsDisable == "1" ? true : false;

                AdBase bll = new AdBase();
                object adIdOutput = Guid.Empty;
                int effect = -1;

                using (TransactionScope scope = new TransactionScope())
                {
                    if (!adId.Equals(Guid.Empty))
                    {
                        var oldModel = bll.GetModel(adId);
                        if (oldModel == null)
                        {
                            context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.GetString(MessageContent.Request_NotExist, "广告ID【" + adId + "】") + "\"}");
                            return;
                        }
                        model.ViewCount = oldModel.ViewCount;
                        effect = bll.Update(model);
                    }
                    else
                    {
                        model.Id = Guid.NewGuid();
                        effect = bll.InsertByOutput(model);
                    }

                    adIdOutput = model.Id;

                    scope.Complete();
                }

                if (effect == 110)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Exist + "\"}");
                    return;
                }
                if (effect != 1)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"操作失败，原因：请正确输入并重试，如果再出现此问题请联系管理员\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"" + MessageContent.Submit_Success + "\",\"data\": \"" + adIdOutput + "\"}");

            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.AlertTitle_Ex_Error + "：" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 广告项提交
        /// </summary>
        /// <param name="context"></param>
        private void SaveAdItem(HttpContext context)
        {
            try
            {
                Guid adId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["adId"])) Guid.TryParse(context.Request.Form["adId"], out adId);
                if (adId.Equals(Guid.Empty))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"请先完成基本信息\"}");
                    return;
                }

                if(string.IsNullOrWhiteSpace(context.Request.Form["ddlActionType"]))
                {
                    context.Response.Write("{\"success\": false,\"message\": \""+MessageContent.Submit_Params_InvalidError+"\"}");
                    return;
                }

                Guid adItemId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["hAdItemId"])) Guid.TryParse(context.Request.Form["hAdItemId"], out adItemId);
                Guid pictureId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["hImgPictureId"])) Guid.TryParse(context.Request.Form["hImgPictureId"], out pictureId);
                Guid actionTypeId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["ddlActionType"])) Guid.TryParse(context.Request.Form["ddlActionType"], out actionTypeId);
                int sort = 0;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["txtSort"])) int.TryParse(context.Request.Form["txtSort"], out sort);
                bool isDisable = false;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["isDisable"])) bool.TryParse(context.Request.Form["isDisable"], out isDisable);

                AdItem bll = new AdItem();

                AdItemInfo model = new AdItemInfo();
                model.AdvertisementId = adId;
                model.Id = adItemId;
                model.PictureId = pictureId;
                model.ActionTypeId = actionTypeId;
                model.Sort = sort;
                model.IsDisable = isDisable;

                int effect = -1;

                if (!adItemId.Equals(Guid.Empty))
                {
                    var oldModel = bll.GetModel(adItemId);
                    if (oldModel.ActionTypeId != model.ActionTypeId)
                    {
                        AdItemLink adlBll = new AdItemLink();
                        if (adlBll.Delete(adItemId) < 1)
                        {
                            AdItemContent adcBll = new AdItemContent();
                            adcBll.Delete(adItemId);
                        }
                    }
                    else
                    {
                        effect = bll.Update(model);
                    }
                }
                else
                {
                    model.Id = Guid.NewGuid();
                    effect = bll.Insert(model);
                }

                if (effect == 110)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Exist + "\"}");
                    return;
                }
                if (effect != 1)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"操作失败，原因：请正确输入并重试，如果再出现此问题请联系管理员\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"" + MessageContent.Submit_Success + "\",\"data\": \"" + model.Id + "\"}");

            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.AlertTitle_Ex_Error + "：" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 广告项链接提交
        /// </summary>
        /// <param name="context"></param>
        private void SaveAdItemLink(HttpContext context)
        {
            try
            {
                Guid adId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["adId"])) Guid.TryParse(context.Request.Form["adId"], out adId);
                if (adId.Equals(Guid.Empty))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"请先完成基本信息\"}");
                    return;
                }

                Guid adItemId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["adItemId"])) Guid.TryParse(context.Request.Form["adItemId"], out adItemId);
                if (adItemId.Equals(Guid.Empty))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"请先完成广告项再继续此操作\"}");
                    return;
                }

                if (string.IsNullOrWhiteSpace(context.Request.Form["txtUrl"]))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                    return;
                }
                var productId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["productId"])) Guid.TryParse(context.Request.Form["productId"], out productId);

                AdItemLink bll = new AdItemLink();

                AdItemLinkInfo model = new AdItemLinkInfo();
                model.AdItemId = adItemId;
                model.Url = context.Request.Form["txtUrl"].Trim();
                model.ProductId = productId;

                int effect = -1;

                if (bll.IsExist(adItemId))
                {
                    effect = bll.Update(model);
                }
                else
                {
                    effect = bll.Insert(model);
                }

                if (effect == 110)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Exist + "\"}");
                    return;
                }
                if (effect != 1)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"操作失败，原因：请正确输入并重试，如果再出现此问题请联系管理员\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"" + MessageContent.Submit_Success + "\"}");

            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.AlertTitle_Ex_Error + "：" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 广告项内容提交
        /// </summary>
        /// <param name="context"></param>
        private void SaveAdItemContent(HttpContext context)
        {
            try
            {
                Guid adId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["adId"])) Guid.TryParse(context.Request.Form["adId"], out adId);
                if (adId.Equals(Guid.Empty))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"请先完成基本信息\"}");
                    return;
                }

                Guid adItemId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["adItemId"])) Guid.TryParse(context.Request.Form["adItemId"], out adItemId);
                if (adItemId.Equals(Guid.Empty))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"请先完成广告项再继续此操作\"}");
                    return;
                }

                if (string.IsNullOrWhiteSpace(context.Request.Form["content"]))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                    return;
                }
                string content = HttpUtility.HtmlDecode(context.Request.Form["content"].Trim());

                AdItemContent bll = new AdItemContent();

                AdItemContentInfo model = new AdItemContentInfo();
                model.AdItemId = adItemId;
                model.Descr = context.Request.Form["txtaDescr"].Trim();
                model.ContentText = content;

                int effect = -1;

                if (bll.IsExist(adItemId))
                {
                    effect = bll.Update(model);
                }
                else
                {
                    effect = bll.Insert(model);
                }

                if (effect == 110)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Exist + "\"}");
                    return;
                }
                if (effect != 1)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"操作失败，原因：请正确输入并重试，如果再出现此问题请联系管理员\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"" + MessageContent.Submit_Success + "\"}");

            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.AlertTitle_Ex_Error + "：" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 站点内容
        /// </summary>
        /// <param name="context"></param>
        private void SaveContentDetail(HttpContext context)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(context.Request.Form["ctl00$cphMain$txtTitle"]))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                    return;
                }
                if (string.IsNullOrWhiteSpace(context.Request.Form["contentTypeId"]))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                    return;
                }

                Guid gId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["ctl00$cphMain$hId"].Trim()))
                {
                    Guid.TryParse(context.Request.Form["ctl00$cphMain$hId"].Trim(), out gId);
                }

                Guid contentTypeId = Guid.Empty;
                Guid.TryParse(context.Request.Form["contentTypeId"], out contentTypeId);
                Guid pictureId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["ctl00$cphMain$hPictureId"]))
                {
                    Guid.TryParse(context.Request.Form["ctl00$cphMain$hPictureId"], out pictureId);
                }
                int virtualViewCount = 0;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["ctl00$cphMain$txtVirtualViewCount"]))
                {
                    int.TryParse(context.Request.Form["ctl00$cphMain$txtVirtualViewCount"], out virtualViewCount);
                }
                int sort = 0;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["ctl00$cphMain$txtSort"]))
                {
                    int.TryParse(context.Request.Form["ctl00$cphMain$txtSort"], out sort);
                }
                bool isDisable = false;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["isDisable"]))
                {
                    bool.TryParse(context.Request.Form["isDisable"], out isDisable);
                }

                ContentDetailInfo model = new ContentDetailInfo();
                model.Id = gId;
                model.LastUpdatedDate = DateTime.Now;
                model.Title = context.Request.Form["ctl00$cphMain$txtTitle"].Trim();
                model.PictureId = pictureId;
                model.Descr = context.Request.Form["ctl00$cphMain$txtaDescr"] == null ? "" : context.Request.Form["ctl00$cphMain$txtaDescr"].Trim();
                model.ContentText = context.Request.Form["txtContent"] == null ? "" : HttpUtility.HtmlDecode(context.Request.Form["txtContent"].Trim());
                model.ContentTypeId = contentTypeId;
                model.VirtualViewCount = virtualViewCount;
                model.Sort = sort;
                model.IsDisable = isDisable;

                ContentDetail bll = new ContentDetail();
                int effect = -1;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (TransactionScope scope2 = new TransactionScope(TransactionScopeOption.Suppress))
                    {
                        if (bll.IsExist(model.ContentTypeId, model.Id))
                        {
                            scope2.Complete();
                            scope.Complete();
                            context.Response.Write("{\"success\": false,\"message\": \"已存在该类别的内容，请勿重复操作\"}");
                            return;
                        }
                    }
                    if (!gId.Equals(Guid.Empty))
                    {
                        effect = bll.Update(model);
                    }
                    else
                    {
                        effect = bll.Insert(model);
                    }

                    scope.Complete();
                }
                if (effect == 110)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Exist + "\"}");
                    return;
                }
                if (effect != 1)
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