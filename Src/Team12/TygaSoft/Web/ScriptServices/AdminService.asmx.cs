using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.DBUtility;
using TygaSoft.WebHelper;
using TygaSoft.CustomExceptions;

namespace TygaSoft.Web.ScriptServices
{
    /// <summary>
    /// AdminService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    [System.Web.Script.Services.ScriptService]
    public class AdminService : System.Web.Services.WebService
    {

        #region 菜单导航

        [WebMethod]
        public string GetTreeJsonForMenu()
        {
            try
            {
                string[] roles = Roles.GetRolesForUser("283335746");
                var roleList = roles.ToList();
                //if (roleList.Contains("Administrators"))
                //{
                //    roleList.Remove("Administrators");
                //}
                //roleList.Add("Users");
                SitemapHelper.Roles = roleList;
                return SitemapHelper.GetTreeJsonForMenu();
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
            }

            return "[]";
        }

        #endregion

        #region 供应商

        [WebMethod]
        public string DelSupplier(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Supplier bll = new Supplier();
                bll.DeleteBatch(items.ToList<object>());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string SaveSupplier(SupplierInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.SupplierName))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            if (!string.IsNullOrWhiteSpace(model.Address))
            {
                if (string.IsNullOrWhiteSpace(model.ProvinceCityName))
                {
                    return "请选择省市区";
                }
            }

            Guid gId = Guid.Empty;
            Guid.TryParse(model.Id.ToString(), out gId);
            model.Id = gId;

            model.LastUpdatedDate = DateTime.Now;

            Supplier bll = new Supplier();
            int effect = -1;

            if (!gId.Equals(Guid.Empty))
            {
                effect = bll.Update(model);
            }
            else
            {
                effect = bll.Insert(model);
            }

            if (effect == 110)
            {
                return MessageContent.Submit_Exist;
            }

            if (effect < 1) return MessageContent.Submit_Error;

            return "1";
        }

        #endregion

        #region 知识竞猜

        #region 题库

        [WebMethod]
        public string DelQuestionBank(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                QuestionBank bll = new QuestionBank();
                bll.DeleteBatch(items.ToList<object>());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string SaveQuestionBank(QuestionBankInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.Named))
            {
                return MessageContent.Submit_Params_InvalidError;
            }
            try
            {
                Guid gId = Guid.Empty;
                Guid.TryParse(model.Id.ToString(), out gId);
                model.Id = gId;

                model.Remark = "";
                model.LastUpdatedDate = DateTime.Now;

                QuestionBank bll = new QuestionBank();
                int effect = -1;

                if (!gId.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                }
                else
                {
                    effect = bll.Insert(model);
                }

                if (effect == 110)
                {
                    return MessageContent.Submit_Exist;
                }

                if (effect < 1) return MessageContent.Submit_Error;

            }
            catch (Exception ex)
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }
            return "1";
        }

        [WebMethod]
        public string GetQuestionBankList()
        {
            string errorMsg = string.Empty;
            string strReturn = string.Empty;
            try
            {
                QuestionBank bll = new QuestionBank();
                List<QuestionBankInfo> qbList = bll.GetList().ToList();
                strReturn = JsonConvert.SerializeObject(qbList);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return strReturn;
        }

        #endregion

        #region 题目

        [WebMethod]
        public string DelQuestionSubject(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                QuestionSubject bll = new QuestionSubject();
                if (bll.DeleteBatch(items.ToList<object>()))
                {
                    //删除题目对应的答案选项
                    AnswerOption bllAO = new AnswerOption();
                    bllAO.DeleteBatchForQS(items.ToList<object>());
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string SaveQuestionSubject(QuestionSubjectInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.QuestionContent))
            {
                return MessageContent.Submit_Params_InvalidError;
            }
            try
            {
                Guid gId = Guid.Empty;
                Guid.TryParse(model.Id.ToString(), out gId);
                model.Id = gId;

                model.Sort = 0;
                model.Remark = "";
                model.LastUpdatedDate = DateTime.Now;

                QuestionSubject bll = new QuestionSubject();
                int effect = -1;

                if (!gId.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                }
                else
                {
                    effect = bll.Insert(model);
                }

                if (effect == 110)
                {
                    return MessageContent.Submit_Exist;
                }

                if (effect < 1) return MessageContent.Submit_Error;
            }
            catch (Exception ex)
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }
            return "1";
        }
        #endregion+

        #region 答案

        [WebMethod]
        public string DelAnswerOption(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                AnswerOption bll = new AnswerOption();
                bll.DeleteBatch(items.ToList<object>());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string SaveAnswerOption(AnswerOptionInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.OptionContent))
            {
                return MessageContent.Submit_Params_InvalidError;
            }
            try
            {
                Guid gId = Guid.Empty;
                Guid.TryParse(model.Id.ToString(), out gId);
                model.Id = gId;

                model.Remark = "";
                model.LastUpdatedDate = DateTime.Now;
                AnswerOption bll = new AnswerOption();

                #region 单选题只能有一个正确答案
                QuestionSubject bllQS = new QuestionSubject();
                QuestionSubjectInfo modelQS = bllQS.GetModel(model.QuestionSubjectId);
                if (modelQS.QuestionType == 0 && model.IsTrue == true)
                {
                    ParamsHelper parms = new ParamsHelper();

                    string sqlWhere = " and QuestionSubjectId = @QuestionSubjectId and IsTrue=1";
                    SqlParameter parm = new SqlParameter("@QuestionSubjectId", SqlDbType.UniqueIdentifier);
                    parm.Value = model.QuestionSubjectId;
                    parms.Add(parm);

                    List<AnswerOptionInfo> list = bll.GetList(sqlWhere, parm).ToList();
                    bool isError = false;
                    foreach (AnswerOptionInfo info in list)
                    {
                        isError = true;
                        if (info.Id.Equals(model.Id))
                        {
                            isError = false;
                            break;
                        }
                    }
                    if (isError)
                    {
                        return "该题目是单选题，已存在正确答案";
                    }
                }
                #endregion

                int effect = -1;

                if (!gId.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                }
                else
                {
                    effect = bll.InsertOW(model);
                }

                if (effect == 110)
                {
                    return MessageContent.Submit_Exist;
                }

                if (effect < 1) return MessageContent.Submit_Error;
            }
            catch (Exception ex)
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }
            return "1";
        }
        #endregion

        #region 发布

        [WebMethod]
        public string DelIssue(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                ActivityRelease bll = new ActivityRelease();
                if (bll.DeleteBatch(items.ToList<object>()))
                {
                    //删除发布下的题库列表
                    ActivityQuestionBank bllAB = new ActivityQuestionBank();
                    bllAB.DeleteBatch(items.ToList<object>());
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string SaveIssue(ActivityReleaseInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                return MessageContent.Submit_Params_InvalidError;
            }
            try
            {
                Guid gId = Guid.Empty;
                Guid.TryParse(model.Id.ToString(), out gId);
                model.Id = gId;

                model.Remark = "";
                model.LastUpdatedDate = DateTime.Now;

                ActivityRelease bll = new ActivityRelease();

                ParamsHelper parms = new ParamsHelper();
                string sqlWhere = " and (StartDate <= @StartDate and EndDate>@StartDate) or (StartDate < @EndDate and EndDate>=@EndDate)";
                SqlParameter parm = new SqlParameter("@StartDate", SqlDbType.DateTime);
                parm.Value = model.StartDate;
                parms.Add(parm);

                SqlParameter parm1 = new SqlParameter("@EndDate", SqlDbType.DateTime);
                parm1.Value = model.EndDate;
                parms.Add(parm1);

                List<ActivityReleaseInfo> listRS = bll.GetList(sqlWhere, parms.ToArray()).ToList();
                if (listRS != null && listRS.Count > 0 && !listRS[0].Id.Equals(model.Id))
                {
                    return "该有效期已发布";
                }

                ActivityQuestionBank bllAQB = new ActivityQuestionBank();
                int effect = -1;

                if (!gId.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                    if (effect > 0)
                    {
                        bllAQB.Delete(model.Id);
                        foreach (ActivityQuestionBankInfo info in model.listAQB)
                        {
                            info.ActivityReleaseId = model.Id;
                            bllAQB.InsertOW(info);
                        }
                    }
                }
                else
                {
                    Guid id = bll.InsertByOutput(model);
                    if (!id.Equals(Guid.Empty))
                    {
                        effect = 1;
                        foreach (ActivityQuestionBankInfo info in model.listAQB)
                        {
                            info.ActivityReleaseId = id;
                            bllAQB.InsertOW(info);
                        }
                    }
                }

                if (effect == 110)
                {
                    return MessageContent.Submit_Exist;
                }

                if (effect < 1) return MessageContent.Submit_Error;
            }
            catch (Exception ex)
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }
            return "1";
        }
        #endregion

        #endregion

        #region 彩票
        [WebMethod]
        public string DelLottery(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                QXCLotteryNumber bll = new QXCLotteryNumber();
                bll.DeleteBatch(items.ToList<object>());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string SaveLottery(QXCLotteryNumberInfo model)
        {
            //if (string.IsNullOrWhiteSpace(model.Named))
            //{
            //    return MessageContent.Submit_Params_InvalidError;
            //}
            try
            {
                Guid gId = Guid.Empty;
                Guid.TryParse(model.Id.ToString(), out gId);
                model.Id = gId;

                model.ExpiryClosingDate = model.ExpiryClosingDate.CompareTo(DateTime.MinValue) == 0 ? DateTime.Parse("1754-01-01") : model.ExpiryClosingDate;
                model.UserId = WebCommon.GetUserId();
                model.LastUpdatedDate = DateTime.Now;

                QXCLotteryNumber bll = new QXCLotteryNumber();
                int effect = -1;

                if (!gId.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                }
                else
                {
                    effect = bll.Insert(model);
                }

                if (effect == 110)
                {
                    return MessageContent.Submit_Exist;
                }

                if (effect < 1) return MessageContent.Submit_Error;

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

                        string content = string.Format("第{0}期开奖结果：{1}", model.HNQS, model.LotteryNo.Substring(0, 4));

                        string sxml = "";
                        sxml = string.Format(@"<XmlParameters><ReceivePushContent><PushType>{0}</PushType><PushContent>{1}</PushContent><Title>{2}</Title><PushParam>{3}</PushParam></ReceivePushContent></XmlParameters>",
                            "cpkj", content, "", "1");

                        pushProxy.ReceivePushContentAsync(sxml);
                    }
                    catch
                    { 
                        
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }
            return "1";
        }

        [WebMethod]
        public string SaveLotteryInitItems(string itemAppend)
        {
            try
            {
                List<InitItemsInfo> list = new List<InitItemsInfo>();
                string[] items = itemAppend.Split(new string[] { "*,*" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in items)
                {
                    InitItemsInfo model = new InitItemsInfo();
                    model.ItemCode = item.Split(new string[] { "*|*" }, StringSplitOptions.None)[0];
                    model.ItemKey = item.Split(new string[] { "*|*" }, StringSplitOptions.None)[1];
                    list.Add(model);
                }
                InitItems bll = new InitItems();
                if (bll.UpdateBatch(list))
                {
                    return "1";
                }
                else
                {
                    return MessageContent.Submit_Error;
                }
            }
            catch (Exception ex)
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }
        }
        #endregion

        #region 电视台

        #region 保存频道
        [WebMethod]
        public string SaveHWTV(HWTVInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.HWTVName))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            Guid gId = Guid.Empty;
            Guid.TryParse(model.Id.ToString(), out gId);
            model.Id = gId;
            model.LastUpdatedDate = DateTime.Now;

            HWTV bll = new HWTV();
            int effect = -1;

            if (!gId.Equals(Guid.Empty))
            {
                effect = bll.Update(model);
            }
            else
            {
                effect = bll.Insert(model);
            }

            if (effect == 110)
            {
                return MessageContent.Submit_Exist;
            }

            if (effect < 1) return MessageContent.Submit_Error;

            return "1";
        }
        #endregion

        #region 删除频道
        [WebMethod]
        public string DelHWTV(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                HWTV bll = new HWTV();
                bll.DeleteBatch(items.ToList<object>());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }
        #endregion

        #region 保存节目
        [WebMethod]
        public string SaveTVProgram(TVProgramInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.ProgramName))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            Guid gId = Guid.Empty;
            Guid.TryParse(model.Id.ToString(), out gId);
            model.Id = gId;
            model.LastUpdatedDate = DateTime.Now;

            TVProgram bll = new TVProgram();
            int effect = -1;

            if (!gId.Equals(Guid.Empty))
            {
                effect = bll.Update(model);
            }
            else
            {
                effect = bll.Insert(model);
            }

            if (effect == 110)
            {
                return MessageContent.Submit_Exist;
            }

            if (effect < 1) return MessageContent.Submit_Error;

            return "1";
        }
        #endregion

        #region 删除节目
        [WebMethod]
        public string DelTVProgram(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                TVProgram bll = new TVProgram();
                bll.DeleteBatch(items.ToList<object>());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }
        #endregion

        #endregion

        #region 话题

        #region 话题
        [WebMethod]
        public string DelTopic(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                TopicSubject bll = new TopicSubject();
                if (bll.DeleteBatch(items.ToList<object>()))
                {
                    TopicPicture bllTP = new TopicPicture();
                    bllTP.DeleteBatch(items.ToList<object>());
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string SaveTopic(TopicSubjectInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.ContentText))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            try
            {
                Guid gId = Guid.Empty;
                Guid.TryParse(model.Id.ToString(), out gId);
                model.Id = gId;

                model.UserId = WebCommon.GetUserId();
                model.Sort = 0;
                model.LastUpdatedDate = DateTime.Now;

                TopicSubject bll = new TopicSubject();
                TopicPicture bllTP = new TopicPicture();
                TopicPictureInfo infoTP = new TopicPictureInfo();
                int effect = -1;

                if (!gId.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                    if (effect > 0)
                    {
                        bllTP.Delete(model.Id);
                        if (!model.PictureId.Equals(Guid.Empty))
                        {
                            infoTP.TopicSubjectId = model.Id;
                            infoTP.PictureId = model.PictureId;
                            bllTP.Insert(infoTP);
                        }
                    }
                }
                else
                {
                    Guid topicSubjectId = bll.InsertByOutput(model);
                    if (!topicSubjectId.Equals(Guid.Empty))
                    {
                        effect = 1;
                        if (!model.PictureId.Equals(Guid.Empty))
                        {
                            infoTP.TopicSubjectId = topicSubjectId;
                            infoTP.PictureId = model.PictureId;
                            bllTP.Insert(infoTP);
                        }
                    }
                }

                if (effect == 110)
                {
                    return MessageContent.Submit_Exist;
                }

                if (effect < 1) return MessageContent.Submit_Error;

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
                            "yhzs", "", model.Title, "1");

                        pushProxy.ReceivePushContentAsync(sxml);
                    }
                    catch
                    {

                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }

            return "1";
        }

        [WebMethod]
        public string GetTopicUserList()
        {
            string errorMsg = string.Empty;
            string strReturn = string.Empty;
            try
            {
                List<UserRoleInfo> list = new List<UserRoleInfo>();
                UserBase bll = new UserBase();
                Dictionary<object, string> dicUser = bll.GetTopicUsers();
                foreach (object key in dicUser.Keys)
                {
                    UserRoleInfo info = new UserRoleInfo();
                    info.UserId = key;
                    info.Username = dicUser[key];
                    list.Add(info);
                }

                strReturn = JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return strReturn;
        }

        [WebMethod]
        public string SaveTopicIsTop(TopicSubjectInfo model)
        {
            try
            {
                TopicSubject bll = new TopicSubject();
                int effect = -1;

                effect = bll.UpdateIsTop(model);

                if (effect < 1) return MessageContent.Submit_Error;
            }
            catch (Exception ex)
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }

            return "1";
        }
        #endregion

        #region 评论
        [WebMethod]
        public string DelTopicComment(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                TopicComment bll = new TopicComment();
                bll.DeleteBatch(items.ToList<object>());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string SaveTopicComment(TopicCommentInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.ContentText))
            {
                return MessageContent.Submit_Params_InvalidError;
            }
            try
            {
                Guid gId = Guid.Empty;
                Guid.TryParse(model.Id.ToString(), out gId);
                model.Id = gId;

                model.Sort = 0;
                model.LastUpdatedDate = DateTime.Now;

                TopicComment bll = new TopicComment();
                int effect = -1;

                if (!gId.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                }
                else
                {
                    effect = bll.Insert(model);
                }

                if (effect == 110)
                {
                    return MessageContent.Submit_Exist;
                }

                if (effect < 1) return MessageContent.Submit_Error;
            }
            catch (Exception ex)
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }

            return "1";
        }

        [WebMethod]
        public string SaveTopicCommentIsTop(TopicCommentInfo model)
        {
            try
            {
                TopicComment bll = new TopicComment();
                int effect = -1;

                effect = bll.UpdateIsTop(model);

                if (effect < 1) return MessageContent.Submit_Error;
            }
            catch (Exception ex)
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }
            return "1";
        }
        #endregion

        #endregion

        #region 摇奖

        [WebMethod]
        public string DelErnie(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Ernie bll = new Ernie();
                bll.DeleteBatch(items.ToList<object>());

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
        }

        [WebMethod]
        public string DelErnieItem(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                ErnieItem bll = new ErnieItem();
                bll.DeleteBatch(items.ToList<object>());

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
        }

        [WebMethod]
        public string CopyErnieItemByLasted(Guid ernieId)
        {
            if (ernieId.Equals(Guid.Empty))
            {
                return MessageContent.GetString(MessageContent.Request_InvalidArgument, "摇奖ID");
            }

            try
            {
                ErnieItem bll = new ErnieItem();

                using (TransactionScope scope = new TransactionScope())
                {
                    bll.DeleteByErnieId(ernieId);
                    if (bll.CopyLasted(ernieId) < 1)
                    {
                        return "未找到任何可复制的数据，请检查";
                    }

                    scope.Complete();
                }

                return "1";
            }
            catch (Exception ex)
            {
                new CustomException("方法：AdminService.CopyErnieItemByLatest，异常：" + ex.Message + "", ex);
                return ex.Message;
            }
        }

        #endregion

        #region 图片相册

        [WebMethod]
        public string DelCommunionPicture(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                if (!HttpContext.Current.User.IsInRole("Administrators"))
                {
                    return MessageContent.Role_InvalidError;
                }

                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string inIds = "";
                Guid gId = Guid.Empty;
                foreach (string item in items)
                {
                    if (!Guid.TryParse(item, out gId))
                    {
                        throw new ArgumentException(MessageContent.GetString(MessageContent.Submit_Params_GetInvalidRegex, item));
                    }
                    inIds += string.Format("'{0}',", item);
                }

                CommunionPicture bll = new CommunionPicture();
                var list = bll.GetList(" and Id in (" + inIds.Trim(',') + ")");
                if (list != null || list.Count() > 0)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        foreach (var model in list)
                        {
                            string dir = Server.MapPath("~" + string.Format("{0}{1}", model.FileDirectory, model.RandomFolder));

                            if (Directory.Exists(dir))
                            {
                                string[] subDirArr = Directory.GetDirectories(dir);
                                if (subDirArr != null)
                                {
                                    foreach (string subDir in subDirArr)
                                    {
                                        Directory.Delete(subDir, true);
                                    }
                                }
                                Directory.Delete(dir, true);
                            }
                            dir = Server.MapPath("~" + string.Format("{0}{1}", model.FileDirectory, model.FileName));
                            if (File.Exists(dir))
                            {
                                File.Delete(dir);
                            }

                        }

                        bll.DeleteBatch(items.ToList<object>());

                        scope.Complete();
                    }
                }

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return MessageContent.GetString(MessageContent.Submit_Ex_Error, errorMsg);
        }

        [WebMethod]
        public string DelUserHeadPicture(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                if (!HttpContext.Current.User.IsInRole("Administrators"))
                {
                    return MessageContent.Role_InvalidError;
                }

                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string inIds = "";
                Guid gId = Guid.Empty;
                foreach (string item in items)
                {
                    if (!Guid.TryParse(item, out gId))
                    {
                        throw new ArgumentException(MessageContent.GetString(MessageContent.Submit_Params_GetInvalidRegex, item));
                    }
                    inIds += string.Format("'{0}',", item);
                }

                UserHeadPicture bll = new UserHeadPicture();
                var list = bll.GetList(" and Id in (" + inIds.Trim(',') + ")");
                if (list != null || list.Count() > 0)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        foreach (var model in list)
                        {
                            string dir = Server.MapPath("~" + string.Format("{0}{1}", model.FileDirectory, model.RandomFolder));

                            if (Directory.Exists(dir))
                            {
                                string[] subDirArr = Directory.GetDirectories(dir);
                                if (subDirArr != null)
                                {
                                    foreach (string subDir in subDirArr)
                                    {
                                        Directory.Delete(subDir, true);
                                    }
                                }
                                Directory.Delete(dir, true);
                            }
                            dir = Server.MapPath("~" + string.Format("{0}{1}", model.FileDirectory, model.FileName));
                            if (File.Exists(dir))
                            {
                                File.Delete(dir);
                            }

                        }

                        bll.DeleteBatch(items.ToList<object>());

                        scope.Complete();
                    }
                }

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return MessageContent.GetString(MessageContent.Submit_Ex_Error, errorMsg);
        }

        [WebMethod]
        public string DelActivityPhotoPicture(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                if (!HttpContext.Current.User.IsInRole("Administrators"))
                {
                    return MessageContent.Role_InvalidError;
                }

                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string inIds = "";
                Guid gId = Guid.Empty;
                foreach (string item in items)
                {
                    if (!Guid.TryParse(item, out gId))
                    {
                        throw new ArgumentException(MessageContent.GetString(MessageContent.Submit_Params_GetInvalidRegex, item));
                    }
                    inIds += string.Format("'{0}',", item);
                }

                ActivityPhotoPicture bll = new ActivityPhotoPicture();
                var list = bll.GetList(" and Id in (" + inIds.Trim(',') + ")");
                if (list != null || list.Count() > 0)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        foreach (var model in list)
                        {
                            string dir = Server.MapPath("~" + string.Format("{0}{1}", model.FileDirectory, model.RandomFolder));

                            if (Directory.Exists(dir))
                            {
                                string[] subDirArr = Directory.GetDirectories(dir);
                                if (subDirArr != null)
                                {
                                    foreach (string subDir in subDirArr)
                                    {
                                        Directory.Delete(subDir, true);
                                    }
                                }
                                Directory.Delete(dir, true);
                            }
                            dir = Server.MapPath("~" + string.Format("{0}{1}", model.FileDirectory, model.FileName));
                            if (File.Exists(dir))
                            {
                                File.Delete(dir);
                            }

                        }

                        bll.DeleteBatch(items.ToList<object>());

                        scope.Complete();
                    }
                }

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return MessageContent.GetString(MessageContent.Submit_Ex_Error, errorMsg);
        }

        [WebMethod]
        public string DelActivityPlayerPhotoPicture(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                if (!HttpContext.Current.User.IsInRole("Administrators"))
                {
                    return MessageContent.Role_InvalidError;
                }

                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string inIds = "";
                Guid gId = Guid.Empty;
                foreach (string item in items)
                {
                    if (!Guid.TryParse(item, out gId))
                    {
                        throw new ArgumentException(MessageContent.GetString(MessageContent.Submit_Params_GetInvalidRegex, item));
                    }
                    inIds += string.Format("'{0}',", item);
                }

                ActivityPlayerPhotoPicture bll = new ActivityPlayerPhotoPicture();
                var list = bll.GetList(" and Id in (" + inIds.Trim(',') + ")");
                if (list != null || list.Count() > 0)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        foreach (var model in list)
                        {
                            string dir = Server.MapPath("~" + string.Format("{0}{1}", model.FileDirectory, model.RandomFolder));

                            if (Directory.Exists(dir))
                            {
                                string[] subDirArr = Directory.GetDirectories(dir);
                                if (subDirArr != null)
                                {
                                    foreach (string subDir in subDirArr)
                                    {
                                        Directory.Delete(subDir, true);
                                    }
                                }
                                Directory.Delete(dir, true);
                            }
                            dir = Server.MapPath("~" + string.Format("{0}{1}", model.FileDirectory, model.FileName));
                            if (File.Exists(dir))
                            {
                                File.Delete(dir);
                            }

                        }

                        bll.DeleteBatch(items.ToList<object>());

                        scope.Complete();
                    }
                }

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return MessageContent.GetString(MessageContent.Submit_Ex_Error, errorMsg);
        }

        [WebMethod]
        public string DelPictureAdStartup(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                if (!HttpContext.Current.User.IsInRole("Administrators"))
                {
                    return MessageContent.Role_InvalidError;
                }

                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string inIds = "";
                Guid gId = Guid.Empty;
                foreach (string item in items)
                {
                    if (!Guid.TryParse(item, out gId))
                    {
                        throw new ArgumentException(MessageContent.GetString(MessageContent.Submit_Params_GetInvalidRegex, item));
                    }
                    inIds += string.Format("'{0}',", item);
                }

                var bll = new PictureAdStartup();
                var list = bll.GetList(" and Id in (" + inIds.Trim(',') + ")");
                if (list != null || list.Count() > 0)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        foreach (var model in list)
                        {
                            string dir = Server.MapPath("~" + string.Format("{0}{1}", model.FileDirectory, model.RandomFolder));

                            if (Directory.Exists(dir))
                            {
                                string[] subDirArr = Directory.GetDirectories(dir);
                                if (subDirArr != null)
                                {
                                    foreach (string subDir in subDirArr)
                                    {
                                        Directory.Delete(subDir, true);
                                    }
                                }
                                Directory.Delete(dir, true);
                            }
                            dir = Server.MapPath("~" + string.Format("{0}{1}", model.FileDirectory, model.FileName));
                            if (File.Exists(dir))
                            {
                                File.Delete(dir);
                            }

                        }

                        bll.DeleteBatch(items.ToList<object>());

                        scope.Complete();
                    }
                }

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return MessageContent.GetString(MessageContent.Submit_Ex_Error, errorMsg);
        }

        #endregion

        #region 系统

        [WebMethod]
        public string SaveUserBase(UserBaseInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.Nickname))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            if (string.IsNullOrWhiteSpace(model.MobilePhone))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            MembershipUser user = Membership.GetUser(model.Username);
            if (user == null)
            {
                return MessageContent.Submit_Data_NotExists;
            }

            UserBase bll = new UserBase();
            int effect = -1;

            var oldModel = bll.GetModel(user.ProviderUserKey);
            if (oldModel == null)
            {
                model.UserId = Guid.Parse(user.ProviderUserKey.ToString());
                effect = bll.Insert(model);
            }
            else
            {
                model.UserId = Guid.Parse(user.ProviderUserKey.ToString());
                effect = bll.Update(model);
            }

            if (effect < 1) return MessageContent.Submit_Error;

            return "1";
        }

        #endregion

        #region 活动

        #region 活动
        [WebMethod]
        public string DelActivity(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                ActivitySubject bll = new ActivitySubject();
                if (bll.DeleteBatch(items.ToList<object>()))
                {
                    ActivityPicture bllTP = new ActivityPicture();
                    bllTP.DeleteBatch(items.ToList<object>());
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string SaveActivity(ActivitySubjectInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.ContentText))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            try
            {
                Guid gId = Guid.Empty;
                Guid.TryParse(model.Id.ToString(), out gId);
                model.Id = gId;

                model.Remark = "";
                model.LastUpdatedDate = DateTime.Now;

                ActivitySubject bll = new ActivitySubject();
                ActivityPicture bllTP = new ActivityPicture();
                ActivityPictureInfo infoTP = new ActivityPictureInfo();
                int effect = -1;

                if (!gId.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                    if (effect > 0)
                    {
                        bllTP.Delete(model.Id);
                        if (!model.PictureId.Equals(Guid.Empty))
                        {
                            infoTP.ActivityId = model.Id;
                            infoTP.PictureId = model.PictureId;
                            bllTP.Delete(model.Id);
                            bllTP.Insert(infoTP);
                        }
                    }
                }
                else
                {
                    //if (model.StartDate.CompareTo(DateTime.Now) < 0)
                    //{
                    //    return "有效期开始时间不能小于当前时间！";
                    //}

                    model.InsertDate = DateTime.Now;
                    Guid activityId = bll.InsertByOutput(model);
                    if (!activityId.Equals(Guid.Empty))
                    {
                        effect = 1;
                        if (!model.PictureId.Equals(Guid.Empty))
                        {
                            infoTP.ActivityId = activityId;
                            infoTP.PictureId = model.PictureId;
                            bllTP.Insert(infoTP);
                        }
                    }
                }

                if (effect == 110)
                {
                    return MessageContent.Submit_Exist;
                }

                if (effect < 1) return MessageContent.Submit_Error;
            }
            catch (Exception ex)
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }

            return "1";
        }
        #endregion

        #region 选手
        [WebMethod]
        public string DelActivityPlayer(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                ActivityPlayer bll = new ActivityPlayer();
                if (bll.DeleteBatch(items.ToList<object>()))
                {
                    PlayerPicture bllTP = new PlayerPicture();
                    bllTP.DeleteBatch(items.ToList<object>());
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }
        #endregion

        #endregion

        #region 活动（新）

        #region 活动
        [WebMethod]
        public string DelActivityNew(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                ActivitySubjectNew bll = new ActivitySubjectNew();
                if (bll.DeleteBatch(items.ToList<object>()))
                {
                    ActivityPictureNew bllAP = new ActivityPictureNew();
                    bllAP.DeleteBatch(items.ToList<object>());
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string SaveActivityPush(ActivityPushInfo model)
        {
            //if (string.IsNullOrWhiteSpace(model.Content))
            //{
            //    return MessageContent.Submit_Params_InvalidError;
            //}

            try
            {
                PushContentService pushProxy = new PushContentService();
                if (System.Configuration.ConfigurationManager.AppSettings["PushServiceUrl"] != null)
                {
                    pushProxy.Url = System.Configuration.ConfigurationManager.AppSettings["PushServiceUrl"].ToString();
                }

                string sxml = "";
                sxml = string.Format(@"<XmlParameters><ReceivePushContent><PushType>{0}</PushType><PushContent>{1}</PushContent><Title>{2}</Title><PushParam>{3}</PushParam></ReceivePushContent></XmlParameters>",
                    "hd", model.Content, model.Title, "1##" + model.ActivityId);

                string rt = pushProxy.ReceivePushContent(sxml);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(rt);

                string retCode = doc.SelectSingleNode("/ResponseMsg/RetCode").InnerText;

                if (retCode == "0")
                {
                    string isSendOk = doc.SelectSingleNode("/ResponseMsg/RetData/Push/IsSendOK").InnerText;
                    if (isSendOk.ToLower() == "false")
                    {
                        return "推送失败";
                    }
                }
                else
                {
                    return "推送接口异常";
                }
            }
            catch (Exception ex)
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }

            return "1";
        }
        #endregion

        #region 选手
        [WebMethod]
        public string DelActivityPlayerNew(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                ActivityPlayerNew bll = new ActivityPlayerNew();
                if (bll.DeleteBatch(items.ToList<object>()))
                {
                    PlayerPictureNew bllTP = new PlayerPictureNew();
                    bllTP.DeleteBatch(items.ToList<object>());
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }
        #endregion

        #region 刮刮奖
        [WebMethod]
        public string DelActivityPrize(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                ActivityPrize bll = new ActivityPrize();
                if (bll.DeleteBatch(items.ToList<object>()))
                {
                    PrizePicture bllPP = new PrizePicture();
                    bllPP.DeleteBatch(items.ToList<object>());
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string SaveActivityPrize(ActivityPrizeInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.PrizeContent))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            try
            {
                Guid gId = Guid.Empty;
                Guid.TryParse(model.Id.ToString(), out gId);
                model.Id = gId;

                model.Remark = "";
                model.LastUpdatedDate = DateTime.Now;

                ActivityPrize bll = new ActivityPrize();
                PrizePicture bllPP = new PrizePicture();
                PrizePictureInfo infoPP = new PrizePictureInfo();
                int effect = -1;

                if (!gId.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                    if (effect > 0)
                    {
                        bllPP.Delete(model.Id);
                        if (!model.PictureId.Equals(Guid.Empty))
                        {
                            infoPP.ActivityPrizeId = model.Id;
                            infoPP.PictureId = model.PictureId;
                            bllPP.Insert(infoPP);
                        }
                    }
                }
                else
                {
                    model.UpdateWinningTimes = model.WinningTimes;
                    Guid topicSubjectId = bll.InsertByOutput(model);
                    if (!topicSubjectId.Equals(Guid.Empty))
                    {
                        effect = 1;
                        if (!model.PictureId.Equals(Guid.Empty))
                        {
                            infoPP.ActivityPrizeId = topicSubjectId;
                            infoPP.PictureId = model.PictureId;
                            bllPP.Insert(infoPP);
                        }
                    }
                }

                if (effect == 110)
                {
                    return MessageContent.Submit_Exist;
                }

                if (effect < 1) return MessageContent.Submit_Error;
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }

            return "1";
        }

        [WebMethod]
        public string UpdateWinningRecordStatus(string sId, string sStatus)
        {
            try
            {
                Guid gId = Guid.Empty;
                Guid.TryParse(sId, out gId);
                int status = int.Parse(sStatus);

                WinningRecord bll = new WinningRecord();
                int effect = -1;

                if (!gId.Equals(Guid.Empty))
                {
                    effect = bll.UpdateStatus(gId, status);
                }

                if (effect == 110)
                {
                    return MessageContent.Submit_Exist;
                }

                if (effect < 1) return MessageContent.Submit_Error;
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }

            return "1";
        }
        #endregion

        #endregion

        #region 资讯管理

        #region 资讯
        [WebMethod]
        public string DelInformation(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Information bll = new Information();
                if (bll.DeleteBatch(items.ToList<object>()))
                {
                    InformationPicture bllTP = new InformationPicture();
                    bllTP.DeleteBatch(items.ToList<object>());
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }
        #endregion

        #region 资讯广告
        [WebMethod]
        public string DelInformationAd(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                InformationAd bll = new InformationAd();
                if (bll.DeleteBatch(items.ToList<object>()))
                {
                    InformationAdPicture bllTP = new InformationAdPicture();
                    bllTP.DeleteBatch(items.ToList<object>());
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }
        #endregion

        #endregion

        #region 广告
        [WebMethod]
        public string DelAdvert(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                AdvertSubject bll = new AdvertSubject();
                if (bll.DeleteBatch(items.ToList<object>()))
                {
                    AdvertPicture bllAP = new AdvertPicture();
                    bllAP.DeleteBatch(items.ToList<object>());
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string SaveAdvert(AdvertSubjectInfo model)
        {
            try
            {
                Guid gId = Guid.Empty;
                Guid.TryParse(model.Id.ToString(), out gId);
                model.Id = gId;

                model.Sort = 0;
                model.Remark = "";
                model.LastUpdatedDate = DateTime.Now;

                AdvertSubject bll = new AdvertSubject();
                AdvertPicture bllPP = new AdvertPicture();
                AdvertPictureInfo infoPP = new AdvertPictureInfo();
                int effect = -1;

                if (!gId.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                    if (effect > 0)
                    {
                        if (!model.IsDisable)
                        {
                            bll.UpdateDisable(model.Id);
                        }

                        bllPP.Delete(model.Id);
                        if (!model.PictureId.Equals(Guid.Empty))
                        {
                            infoPP.AdvertId = model.Id;
                            infoPP.PictureId = model.PictureId;
                            bllPP.Insert(infoPP);
                        }
                    }
                }
                else
                {
                    Guid AdvertSubjectId = bll.InsertByOutput(model);
                    if (!AdvertSubjectId.Equals(Guid.Empty))
                    {
                        effect = 1;
                        if (!model.IsDisable)
                        {
                            bll.UpdateDisable(AdvertSubjectId);
                        }

                        if (!model.PictureId.Equals(Guid.Empty))
                        {
                            infoPP.AdvertId = AdvertSubjectId;
                            infoPP.PictureId = model.PictureId;
                            bllPP.Insert(infoPP);
                        }
                    }
                }

                if (effect == 110)
                {
                    return MessageContent.Submit_Exist;
                }

                if (effect < 1) return MessageContent.Submit_Error;
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }

            return "1";
        }
        #endregion
    }
}
