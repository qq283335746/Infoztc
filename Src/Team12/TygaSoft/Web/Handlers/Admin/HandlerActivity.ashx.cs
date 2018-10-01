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
    public class HandlerActivity : IHttpHandler
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
                    case "SaveActivitySubject":
                        SaveActivitySubject(context);
                        break;
                    case "SaveActivityPlayer":
                        SaveActivityPlayer(context);
                        break;
                    case "SaveActivityPlayerNew":
                        SaveActivityPlayerNew(context);
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

        #region 保存活动
        public void SaveActivitySubject(HttpContext context)
        {
            try
            {
                string id = context.Request.Form["ctl00$cphMain$hId"].Trim();
                string sTitle = context.Request.Form["ctl00$cphMain$txtTitle"].Trim();
                string sContent = context.Request.Form["content"].Trim();
                string sPictureId = context.Request.Form["pictureId"].Trim();
                string startDate = context.Request.Form["ctl00$cphMain$startDate"].Trim();
                string endDate = context.Request.Form["ctl00$cphMain$endDate"].Trim();
                string sSort = context.Request.Form["ctl00$cphMain$txtSort"].Trim();
                string sMaxVoteCount = context.Request.Form["ctl00$cphMain$txtMaxVoteCount"].Trim();
                sMaxVoteCount = sMaxVoteCount == "" ? "0" : sMaxVoteCount;
                string sMaxSignUpCount = context.Request.Form["ctl00$cphMain$txtMaxSignUpCount"].Trim();
                sMaxSignUpCount = sMaxSignUpCount == "" ? "0" : sMaxSignUpCount;
                string sActualSignUpCount = context.Request.Form["ctl00$cphMain$txtActualSignUpCount"].Trim();
                sActualSignUpCount = sActualSignUpCount == "" ? "0" : sActualSignUpCount;
                string sUpdateSignUpCount = context.Request.Form["ctl00$cphMain$txtUpdateSignUpCount"].Trim();
                sUpdateSignUpCount = sUpdateSignUpCount == "" ? "0" : sUpdateSignUpCount;
                string sViewCount = context.Request.Form["ctl00$cphMain$txtViewCount"].Trim();
                sViewCount = sViewCount == "" ? "0" : sViewCount;
                string sSignUpRule = context.Request.Form["signUpRule"].Trim();
                string sHideAttr = context.Request.Form["hideAttr"].Trim();
                string sIsPrize = context.Request.Form["isPrize"].Trim();
                string sPrizeProbability = context.Request.Form["ctl00$cphMain$txtPrizeProbability"].Trim();
                sPrizeProbability = sPrizeProbability == "" ? "0" : sPrizeProbability;
                string sPrizeRule = context.Request.Form["PrizeRule"].Trim();
                string sIsDisable = context.Request.Form["isDisable"].Trim();
                bool isPush = bool.Parse(context.Request.Form["isPush"].Trim());

                sContent = HttpUtility.HtmlDecode(sContent);
                sSignUpRule = HttpUtility.HtmlDecode(sSignUpRule);
                sPrizeRule = HttpUtility.HtmlDecode(sPrizeRule);

                Guid gId = Guid.Empty;
                if (id != "") Guid.TryParse(id, out gId);
                Guid pictureId = Guid.Empty;
                Guid.TryParse(sPictureId, out pictureId);

                ActivitySubjectNewInfo model = new ActivitySubjectNewInfo();
                model.LastUpdatedDate = DateTime.Now;
                model.Remark = "";

                model.Id = gId;
                model.Title = sTitle;
                model.ContentText = sContent;
                model.StartDate = DateTime.Parse(startDate);
                model.EndDate = DateTime.Parse(endDate);
                model.Sort = int.Parse(sSort);
                model.MaxVoteCount = int.Parse(sMaxVoteCount);
                model.MaxSignUpCount = int.Parse(sMaxSignUpCount);
                model.SignUpCount = int.Parse(sActualSignUpCount);
                model.VirtualSignUpCount = int.Parse(sUpdateSignUpCount);
                model.ViewCount = int.Parse(sViewCount);

                model.SignUpRule = sSignUpRule;
                model.HiddenAttribute = sHideAttr.Length > 0 ? sHideAttr.TrimEnd(',') : sHideAttr;
                model.IsPrize = bool.Parse(sIsPrize);
                model.PrizeProbability = int.Parse(sPrizeProbability);
                model.PrizeRule = sPrizeRule;
                model.IsDisable = bool.Parse(sIsDisable);

                if (string.IsNullOrWhiteSpace(model.ContentText))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                    return;
                }

                if (string.IsNullOrWhiteSpace(model.SignUpRule))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                    return;
                }

                ActivitySubjectNew bll = new ActivitySubjectNew();
                ActivityPictureNew bllAP = new ActivityPictureNew();
                ActivityPictureNewInfo infoAP = new ActivityPictureNewInfo();
                int effect = -1;

                if (!gId.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                    if (effect > 0)
                    {
                        infoAP.ActivityId = model.Id;
                        infoAP.PictureId = pictureId;
                        bllAP.Delete(model.Id);
                        bllAP.Insert(infoAP);
                    }
                }
                else
                {
                    model.InsertDate = DateTime.Now;
                    Guid activityId = bll.InsertByOutput(model);
                    if (!activityId.Equals(Guid.Empty))
                    {
                        effect = 1;
                        if (!pictureId.Equals(Guid.Empty))
                        {
                            infoAP.ActivityId = activityId;
                            infoAP.PictureId = pictureId;
                            bllAP.Insert(infoAP);
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

                if (isPush)
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
                            "hd", "", model.Title, "1##" + model.Id);

                        //string rt = pushProxy.ReceivePushContent(sxml);
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

        #region 保存选手
        public void SaveActivityPlayer(HttpContext context)
        {
            try
            {
                string id = context.Request.Form["ctl00$cphMain$hId"].Trim();
                string sActivityId = context.Request.Form["ctl00$cphMain$asId"].Trim();
                string sName = context.Request.Form["ctl00$cphMain$txtName"].Trim();
                string sPictureId = context.Request.Form["pictureId"].Trim();
                string sDetailInfo = context.Request.Form["detailInfo"].Trim();
                string sActualVoteCount = context.Request.Form["ctl00$cphMain$txtActualVoteCount"].Trim();
                sActualVoteCount = sActualVoteCount == "" ? "0" : sActualVoteCount;
                string sUpdateVoteCount = context.Request.Form["ctl00$cphMain$txtUpdateVoteCount"].Trim();
                sUpdateVoteCount = sUpdateVoteCount == "" ? "0" : sUpdateVoteCount;
                string sIsDisable = context.Request.Form["isDisable"].Trim();
                sDetailInfo = HttpUtility.HtmlDecode(sDetailInfo);
                Guid gId = Guid.Empty;
                if (id != "") Guid.TryParse(id, out gId);
                Guid pictureId = Guid.Empty;
                Guid.TryParse(sPictureId, out pictureId);
                Guid activityId = Guid.Empty;
                Guid.TryParse(sActivityId, out activityId);

                ActivityPlayerInfo model = new ActivityPlayerInfo();
                model.LastUpdatedDate = DateTime.Now;
                model.Remark = "";
                model.HeadPicture = "";

                model.Id = gId;
                model.ActivityId = activityId;
                model.Named = sName;
                model.DetailInformation = sDetailInfo;
                model.ActualVoteCount = int.Parse(sActualVoteCount);
                model.UpdateVoteCount = int.Parse(sUpdateVoteCount);
                model.IsDisable = bool.Parse(sIsDisable);

                if (string.IsNullOrWhiteSpace(model.DetailInformation))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                    return;
                }

                ActivityPlayer bll = new ActivityPlayer();
                PlayerPicture bllPP = new PlayerPicture();
                PlayerPictureInfo infoPP = new PlayerPictureInfo();
                int effect = -1;

                if (!gId.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                    if (effect > 0)
                    {
                        infoPP.PlayerId = model.Id;
                        infoPP.PictureId = pictureId;
                        bllPP.Delete(model.Id);
                        bllPP.Insert(infoPP);
                    }
                }
                else
                {
                    Guid playerId = bll.InsertByOutput(model);
                    if (!playerId.Equals(Guid.Empty))
                    {
                        effect = 1;
                        if (!pictureId.Equals(Guid.Empty))
                        {
                            infoPP.PlayerId = playerId;
                            infoPP.PictureId = pictureId;
                            bllPP.Insert(infoPP);
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

        #region 保存选手（新）
        public void SaveActivityPlayerNew(HttpContext context)
        {
            try
            {
                string id = context.Request.Form["ctl00$cphMain$hId"].Trim();
                string sActivityId = context.Request.Form["ctl00$cphMain$asId"].Trim();
                string sName = context.Request.Form["ctl00$cphMain$txtName"].Trim();
                string sAge = context.Request.Form["ctl00$cphMain$txtAge"].Trim();
                string sOccupation = context.Request.Form["ctl00$cphMain$txtOccupation"].Trim();
                string sPhone = context.Request.Form["ctl00$cphMain$txtPhone"].Trim();
                string sLocation = context.Request.Form["ctl00$cphMain$txtLocation"].Trim();
                string sProfessional = context.Request.Form["ctl00$cphMain$txtProfessional"].Trim();
                string sDescr = context.Request.Form["ctl00$cphMain$txtDescr"].Trim();
                string sPictureIdList = context.Request.Form["pictureId"].TrimEnd(',');
                string sVoteCount = context.Request.Form["ctl00$cphMain$txtActualVoteCount"].Trim();
                sVoteCount = sVoteCount == "" ? "0" : sVoteCount;
                string sVirtualVoteCount = context.Request.Form["ctl00$cphMain$txtUpdateVoteCount"].Trim();
                sVirtualVoteCount = sVirtualVoteCount == "" ? "0" : sVirtualVoteCount;
                string sIsDisable = context.Request.Form["isDisable"].Trim();

                ActivitySubjectNew bllAS = new ActivitySubjectNew();
                ActivitySubjectNewInfo info = new ActivitySubjectNewInfo();
                info = bllAS.GetModel(sActivityId);

                if (info.SignUpCount >= info.MaxVoteCount)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"报名数已达上限\"}");
                    return;
                }

                Guid gId = Guid.Empty;
                if (id != "") Guid.TryParse(id, out gId);
                Guid activityId = Guid.Empty;
                Guid.TryParse(sActivityId, out activityId);

                ActivityPlayerNewInfo model = new ActivityPlayerNewInfo();
                model.LastUpdatedDate = DateTime.Now;
                model.Remark = "";

                model.Id = gId;
                model.ActivityId = activityId;
                model.UserId = WebCommon.GetUserId();
                model.Named = sName;
                model.Age = int.Parse(sAge);
                model.Occupation = sOccupation;
                model.Phone = sPhone;
                model.Location = sLocation;
                model.Professional = sProfessional;
                model.Descr = sDescr;
                model.VoteCount = int.Parse(sVoteCount);
                model.VirtualVoteCount = int.Parse(sVirtualVoteCount);
                model.IsDisable = bool.Parse(sIsDisable);

                ActivityPlayerNew bll = new ActivityPlayerNew();
                PlayerPictureNew bllPP = new PlayerPictureNew();
                int effect = -1;

                if (!gId.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                    if (effect > 0)
                    {

                        bllPP.Delete(model.Id);
                        int index = 1;
                        foreach (string sPictureId in sPictureIdList.Split(','))
                        {
                            PlayerPictureNewInfo infoPP = new PlayerPictureNewInfo();
                            Guid pictureId = Guid.Empty;
                            Guid.TryParse(sPictureId, out pictureId);
                            infoPP.PlayerId = model.Id;
                            infoPP.PictureId = pictureId;
                            infoPP.Sort = index;
                            infoPP.IsHeadImg = index == 1 ? true : false;
                            bllPP.Insert(infoPP);
                            index++;
                        }

                    }
                }
                else
                {
                    Guid playerId = bll.InsertByOutput(model);
                    if (!playerId.Equals(Guid.Empty))
                    {
                        effect = 1;
                        int index = 1;
                        foreach (string sPictureId in sPictureIdList.Split(','))
                        {
                            PlayerPictureNewInfo infoPP = new PlayerPictureNewInfo();
                            Guid pictureId = Guid.Empty;
                            Guid.TryParse(sPictureId, out pictureId);
                            infoPP.PlayerId = playerId;
                            infoPP.PictureId = pictureId;
                            infoPP.Sort = index;
                            infoPP.IsHeadImg = index == 1 ? true : false;
                            bllPP.Insert(infoPP);
                            index++;
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}