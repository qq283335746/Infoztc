using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Xml;
using TygaSoft.BLL;
using TygaSoft.CustomExceptions;
using TygaSoft.DBUtility;
using TygaSoft.Model;
using TygaSoft.SysHelper;
using TygaSoft.Services.HnztcQueueService;

namespace TygaSoft.WcfService
{
    public partial class HnztcTeamService : IHnztcTeam
    {
        #region 成员变量

        private static readonly string _webSiteHost = ConfigurationManager.AppSettings["WebSiteHost"].TrimEnd('/');

        #endregion

        #region 知识竞猜

        public string GetQuestionList(string userId)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                Guid gUserId = GetUserId(userId);

                sb.Append("<QuestionList>");

                //获取当前时间是否有活动发布
                ActivityRelease bllAR = new ActivityRelease();
                ParamsHelper parms = new ParamsHelper();
                string sqlWhere = " and StartDate <= @Date and EndDate > @Date and IsDisable=0";
                SqlParameter parm = new SqlParameter("@Date", SqlDbType.DateTime);
                parm.Value = DateTime.Now;
                parms.Add(parm);

                List<ActivityReleaseInfo> listRS = bllAR.GetList(sqlWhere, parms.ToArray()).ToList();
                if (listRS != null && listRS.Count > 0)
                {
                    ActivityReleaseInfo infoAR = listRS[0];

                    //获取用户当天获取题目次数
                    AnswerStatistics bllAS = new AnswerStatistics();
                    ParamsHelper parmsAS = new ParamsHelper();
                    sqlWhere = " and UserId = @UserId and LastUpdatedDate>=@ToDay and LastUpdatedDate<@Tomorrow";
                    parm = new SqlParameter("@UserId", SqlDbType.UniqueIdentifier);
                    parm.Value = gUserId;
                    parmsAS.Add(parm);

                    parm = new SqlParameter("@ToDay", SqlDbType.DateTime);
                    parm.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                    parmsAS.Add(parm);

                    parm = new SqlParameter("@Tomorrow", SqlDbType.DateTime);
                    parm.Value = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                    parmsAS.Add(parm);

                    int count = bllAS.GetCount(sqlWhere, parmsAS.ToArray());

                    sb.AppendFormat("<PaperID><![CDATA[{0}]]></PaperID>", Guid.NewGuid().ToString());
                    sb.AppendFormat("<Title><![CDATA[{0}]]></Title>", infoAR.Title);
                    sb.AppendFormat("<UpperLimit><![CDATA[{0}]]></UpperLimit>", count < 10 ? "false" : "true");
                    sb.AppendFormat("<CurrentNumber><![CDATA[{0}]]></CurrentNumber>", count + 1);
                    sb.Append("<List>");

                    if (count < 10)
                    {
                        //获取该活动下每个题库题目数
                        ActivityQuestionBank bllAQB = new ActivityQuestionBank();
                        ParamsHelper parmsAQB = new ParamsHelper();
                        sqlWhere = " and ActivityReleaseId like @ActivityReleaseId";
                        parm = new SqlParameter("@ActivityReleaseId", SqlDbType.UniqueIdentifier);
                        parm.Value = infoAR.Id;
                        parmsAQB.Add(parm);
                        List<ActivityQuestionBankInfo> listAQB = bllAQB.GetList(sqlWhere, parmsAQB.ToArray()).ToList();

                        foreach (ActivityQuestionBankInfo infoAQB in listAQB)
                        {
                            QuestionSubject bllQS = new QuestionSubject();
                            if (infoAQB.QuestionCount > 0)
                            {
                                //获取题库随机题目列表
                                List<QuestionSubjectInfo> listQS = bllQS.GetRandomList(infoAQB.QuestionCount, infoAQB.QuestionBankId).ToList();
                                foreach (QuestionSubjectInfo infoQS in listQS)
                                {
                                    sb.Append("<QuestionInfo>");
                                    sb.AppendFormat("<QuestionID><![CDATA[{0}]]></QuestionID>", infoQS.Id);
                                    sb.AppendFormat("<QuestionContent><![CDATA[{0}]]></QuestionContent>", infoQS.QuestionContent);
                                    sb.AppendFormat("<QuestionType><![CDATA[{0}]]></QuestionType>", infoQS.QuestionType);
                                    sb.Append("<AnswerList>");

                                    //获取题目答案选项列表
                                    AnswerOption bllAO = new AnswerOption();
                                    ParamsHelper parmsAO = new ParamsHelper();
                                    sqlWhere = " and QuestionSubjectId like @QuestionSubjectId and IsDisable=0 order by Sort";
                                    parm = new SqlParameter("@QuestionSubjectId", SqlDbType.UniqueIdentifier);
                                    parm.Value = infoQS.Id;
                                    parmsAO.Add(parm);
                                    List<AnswerOptionInfo> listAO = bllAO.GetList(sqlWhere, parmsAO.ToArray()).ToList();

                                    foreach (AnswerOptionInfo infoAO in listAO)
                                    {
                                        sb.Append("<AnswerInfo>");
                                        sb.AppendFormat("<AnswerID><![CDATA[{0}]]></AnswerID>", infoAO.Id);
                                        sb.AppendFormat("<AnswerContent><![CDATA[{0}]]></AnswerContent>", infoAO.OptionContent);
                                        sb.AppendFormat("<IsTrue><![CDATA[{0}]]></IsTrue>", infoAO.IsTrue.ToString().ToLower());
                                        sb.Append("</AnswerInfo>");
                                    }
                                    sb.Append("</AnswerList>");
                                    sb.Append("</QuestionInfo>");
                                }
                            }
                        }
                    }
                    sb.Append("</List>");
                }
                sb.Append("</QuestionList>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string InsertUserAnswer(string userId, string questionSubjectId, string paperId, bool isTrue)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<Rsp>");
                if (string.IsNullOrEmpty(userId))
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>登录ID为空</RtMsg>");
                    sb.Append("</Rsp>");
                    return FormatStrReturn("0", sb.ToString());
                }
                Guid gUserId = GetUserId(userId);

                AnswerStatisticsInfo model = new AnswerStatisticsInfo();
                model.UserId = gUserId;
                model.QuestionSubjectId = Guid.Parse(questionSubjectId);
                model.PaperId = Guid.Parse(paperId);
                model.IsTrue = isTrue;
                model.Integral = 0;
                model.LastUpdatedDate = DateTime.Now;

                AnswerStatistics bll = new AnswerStatistics();
                int rt = bll.Insert(model);

                if (rt > 0)
                {
                    sb.Append("<IsOk>true</IsOk>");
                    sb.Append("<RtMsg>提交成功</RtMsg>");
                    if (isTrue)
                    {
                        UserLevelInfo info = new UserLevelInfo();
                        info.UserId = gUserId;
                        info.TotalGold = 1;

                        UserBaseQueueClient client = new UserBaseQueueClient();
                        client.SaveUserLevel(info);
                    }
                }
                else
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>提交失败</RtMsg>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        #endregion

        #region 彩票

        public string GetQXCLotteryList(string userId, string qs, int qsCount, string sort)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sort = sort == "0" ? "asc" : "desc";
                QXCLotteryNumber bll = new QXCLotteryNumber();
                ParamsHelper parmsAQB = new ParamsHelper();
                string sqlWhere = sort == "asc" ? " and QS > @QS" : " and QS < @QS";
                SqlParameter parm = new SqlParameter("@QS", SqlDbType.VarChar, 30);
                parm.Value = qs;
                parmsAQB.Add(parm);
                List<QXCLotteryNumberInfo> list = bll.GetList(1, qsCount, sqlWhere, sort, parmsAQB.ToArray()).ToList();

                sb.Append("<Rsp>");
                foreach (QXCLotteryNumberInfo info in list)
                {
                    sb.Append("<N>");
                    sb.AppendFormat("<QS><![CDATA[{0}]]></QS>", info.QS);
                    sb.AppendFormat("<HNQS><![CDATA[{0}]]></HNQS>", info.HNQS);
                    sb.AppendFormat("<LotteryNo><![CDATA[{0}]]></LotteryNo>", info.LotteryNo);
                    sb.AppendFormat("<HNLotteryNo><![CDATA[{0}]]></HNLotteryNo>", info.LotteryNo.Substring(0, 4));
                    sb.AppendFormat("<LotteryTime><![CDATA[{0}]]></LotteryTime>", info.LotteryTime.ToString("yyyy-MM-dd HH:mm"));
                    sb.Append("</N>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string GetQXCLotteryInfo(string userId)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                QXCLotteryNumber bll = new QXCLotteryNumber();
                QXCLotteryNumberInfo info = bll.GetNewModel();

                sb.Append("<Rsp>");
                sb.AppendFormat("<QS><![CDATA[{0}]]></QS>", info.QS);
                sb.AppendFormat("<HNQS><![CDATA[{0}]]></HNQS>", info.HNQS);
                sb.AppendFormat("<LotteryNo><![CDATA[{0}]]></LotteryNo>", info.LotteryNo);
                sb.AppendFormat("<HNLotteryNo><![CDATA[{0}]]></HNLotteryNo>", info.LotteryNo.Substring(0, 4));
                sb.AppendFormat("<LotteryTime><![CDATA[{0}]]></LotteryTime>", info.LotteryTime.ToString("yyyy-MM-dd HH:mm"));
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        #endregion

        #region 电视

        #region 获取电视台

        public string GetHWTVList()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                HWTV bll = new HWTV();
                List<MyHWTVInfo> listHWTV = bll.GetMyList().ToList();
                sb.Append("<Rsp>");
                foreach (MyHWTVInfo info in listHWTV)
                {
                    sb.Append("<N>");
                    sb.AppendFormat("<Id><![CDATA[{0}]]></Id>", info.Id);
                    sb.AppendFormat("<HWTVName><![CDATA[{0}]]></HWTVName>", info.HWTVName);
                    sb.AppendFormat("<HWTVIcon><![CDATA[{0}]]></HWTVIcon>", string.Format(_webSiteHost.TrimEnd('/') + "{0}{1}/{1}{2}", info.FileDirectory, info.RandomFolder, info.FileExtension));
                    sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", _webSiteHost.TrimEnd('/') + PictureUrlHelper.GetUrl(info.FileDirectory, info.RandomFolder, info.FileExtension, EnumData.PictureType.OriginalPicture, EnumData.Platform.Android));
                    sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", _webSiteHost.TrimEnd('/') + PictureUrlHelper.GetUrl(info.FileDirectory, info.RandomFolder, info.FileExtension, EnumData.PictureType.BPicture, EnumData.Platform.Android));
                    sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", _webSiteHost.TrimEnd('/') + PictureUrlHelper.GetUrl(info.FileDirectory, info.RandomFolder, info.FileExtension, EnumData.PictureType.MPicture, EnumData.Platform.Android));
                    sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", _webSiteHost.TrimEnd('/') + PictureUrlHelper.GetUrl(info.FileDirectory, info.RandomFolder, info.FileExtension, EnumData.PictureType.SPicture, EnumData.Platform.Android));
                    sb.AppendFormat("<ProgramAddress><![CDATA[{0}]]></ProgramAddress>", info.ProgramAddress);
                    sb.AppendFormat("<IsTurnTo><![CDATA[{0}]]></IsTurnTo>", info.IsTurnTo);
                    sb.AppendFormat("<Sort><![CDATA[{0}]]></Sort>", info.Sort);
                    sb.Append("</N>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }
        #endregion

        #region 获取节目列表
        public string GetTVProgramList(string HWTVId)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                TVProgram bll = new TVProgram();
                ParamsHelper parms = new ParamsHelper();
                string sqlWhere = " and HWTVId=@hwtvid ";
                SqlParameter parm = new SqlParameter("@hwtvid", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(HWTVId);
                parms.Add(parm);
                List<TVProgramInfo> list = bll.GetList(sqlWhere, parms == null ? null : parms.ToArray()).ToList();

                sb.Append("<Rsp>");
                foreach (TVProgramInfo info in list)
                {
                    sb.Append("<N>");
                    sb.AppendFormat("<ProgramName><![CDATA[{0}]]></ProgramName>", info.ProgramName);
                    sb.AppendFormat("<ProgramAddress><![CDATA[{0}]]></ProgramAddress>", info.ProgramAddress);
                    sb.AppendFormat("<TVScID><![CDATA[{0}]]></TVScID>", info.TVScID);
                    sb.AppendFormat("<Time><![CDATA[{0}]]></Time>", info.Time);
                    sb.AppendFormat("<Sort><![CDATA[{0}]]></Sort>", info.Sort);
                    sb.Append("</N>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }
        #endregion
        #endregion

        #region 话题
        public string GetTopicList(int pageIndex, int pageCount)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                TopicSubject bll = new TopicSubject();
                DataSet ds = bll.GetListOW(pageIndex, pageCount, "", null);

                sb.Append("<Rsp>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append("<N>");
                    sb.AppendFormat("<TopicID><![CDATA[{0}]]></TopicID>", row["Id"].ToString());
                    sb.AppendFormat("<Title><![CDATA[{0}]]></Title>", row["Title"].ToString());
                    sb.AppendFormat("<Content><![CDATA[{0}]]></Content>", row["ContentText"].ToString());
                    sb.AppendFormat("<IsTop><![CDATA[{0}]]></IsTop>", row["IsTop"].ToString());

                    string oriImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string bImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string mImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string sImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);

                    sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                    sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                    sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                    sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);
                    sb.AppendFormat("<InsertTime><![CDATA[{0}]]></InsertTime>", Convert.ToDateTime(row["LastUpdatedDate"]).ToString("yyyy-MM-dd HH:mm"));
                    sb.Append("</N>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string GetTopicInfo(string topicID)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                TopicSubject bll = new TopicSubject();
                DataSet ds = bll.GetModelOW(topicID);

                sb.Append("<Rsp>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.AppendFormat("<TopicID><![CDATA[{0}]]></TopicID>", row["Id"].ToString());
                    sb.AppendFormat("<Title><![CDATA[{0}]]></Title>", row["Title"].ToString());
                    sb.AppendFormat("<Content><![CDATA[{0}]]></Content>", row["ContentText"].ToString());

                    string oriImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string bImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string mImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string sImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);

                    sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                    sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                    sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                    sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);
                    sb.AppendFormat("<InsertTime><![CDATA[{0}]]></InsertTime>", Convert.ToDateTime(row["LastUpdatedDate"]).ToString("yyyy-MM-dd HH:mm"));
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string GetTopicCommentList(string topicID, int pageIndex, int pageCount)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                TopicComment bll = new TopicComment();
                ParamsHelper parms = new ParamsHelper();
                string sqlWhere = " and TopicSubjectId = @TopicSubjectId";
                SqlParameter parm = new SqlParameter("@TopicSubjectId", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(topicID);
                parms.Add(parm);

                DataSet ds = bll.GetListOW(pageIndex, pageCount, sqlWhere, parms.ToArray());

                sb.Append("<Rsp>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append("<N>");
                    sb.AppendFormat("<CommentID><![CDATA[{0}]]></CommentID>", row["Id"].ToString());
                    sb.AppendFormat("<LoginID><![CDATA[{0}]]></LoginID>", row["UserName"].ToString());
                    sb.AppendFormat("<NikeName><![CDATA[{0}]]></NikeName>", row["Nickname"].ToString());
                    sb.AppendFormat("<HeadImg><![CDATA[{0}]]></HeadImg>", row["HeadPicture"] is DBNull ? "" : _webSiteHost + row["HeadPicture"].ToString());
                    sb.AppendFormat("<Content><![CDATA[{0}]]></Content>", row["ContentText"].ToString());
                    sb.AppendFormat("<IsTop><![CDATA[{0}]]></IsTop>", row["IsTop"].ToString());
                    sb.AppendFormat("<InsertTime><![CDATA[{0}]]></InsertTime>", Convert.ToDateTime(row["LastUpdatedDate"]).ToString("yyyy-MM-dd HH:mm"));
                    sb.Append("</N>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string AddTopicComment(string userId, string topicID, string content)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<Rsp>");
                if (string.IsNullOrEmpty(userId))
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>登录ID为空</RtMsg>");
                    sb.Append("</Rsp>");
                    return FormatStrReturn("0", sb.ToString());
                }
                TopicCommentInfo model = new TopicCommentInfo();
                model.UserId = GetUserId(userId);
                model.TopicSubjectId = Guid.Parse(topicID);
                model.ContentText = content;
                model.Sort = 0;
                model.IsTop = false;
                model.IsDisable = false;
                model.Remark = "";
                model.LastUpdatedDate = DateTime.Now;

                TopicComment bll = new TopicComment();
                int rt = bll.Insert(model);

                if (rt > 0)
                {
                    sb.Append("<IsOk>true</IsOk>");
                    sb.Append("<RtMsg>提交成功</RtMsg>");
                }
                else
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>提交失败</RtMsg>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }
        #endregion

        #region 活动

        public string GetActivityList(int pageIndex, int pageCount)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                ActivitySubject bll = new ActivitySubject();
                ParamsHelper parms = new ParamsHelper();
                string sqlWhere = " and StartDate <= @Date and EndDate > @Date and IsDisable=0";
                SqlParameter parm = new SqlParameter("@Date", SqlDbType.DateTime);
                parm.Value = DateTime.Now;
                parms.Add(parm);
                DataSet ds = bll.GetListOW(pageIndex, pageCount, sqlWhere, parms.ToArray());

                sb.Append("<Rsp>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append("<N>");
                    sb.AppendFormat("<ActivityID><![CDATA[{0}]]></ActivityID>", row["Id"].ToString());
                    sb.AppendFormat("<Title><![CDATA[{0}]]></Title>", row["Title"].ToString());
                    sb.AppendFormat("<StartDate><![CDATA[{0}]]></StartDate>", Convert.ToDateTime(row["StartDate"]).ToString("yyyy-MM-dd HH:mm"));
                    sb.AppendFormat("<EndDate><![CDATA[{0}]]></EndDate>", Convert.ToDateTime(row["EndDate"]).ToString("yyyy-MM-dd HH:mm"));
                    sb.AppendFormat("<ActivityType><![CDATA[{0}]]></ActivityType>", row["ActivityType"].ToString());
                    sb.AppendFormat("<SignUpCount><![CDATA[{0}]]></SignUpCount>", row["UpdateSignUpCount"].ToString());

                    string oriImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string bImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string mImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string sImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);

                    sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                    sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                    sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                    sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);
                    sb.AppendFormat("<InsertTime><![CDATA[{0}]]></InsertTime>", Convert.ToDateTime(row["InsertDate"]).ToString("yyyy-MM-dd HH:mm"));
                    sb.Append("</N>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string GetActivityInfo(string activityID, string userID)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                Guid gUserId = GetUserId(userID);

                ActivitySubject bll = new ActivitySubject();
                DataSet ds = bll.GetModelOW(activityID);

                sb.Append("<Rsp>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.AppendFormat("<ActivityID><![CDATA[{0}]]></ActivityID>", row["Id"].ToString());
                    sb.AppendFormat("<Title><![CDATA[{0}]]></Title>", row["Title"].ToString());
                    sb.AppendFormat("<Content><![CDATA[{0}]]></Content>", row["ContentText"].ToString());
                    sb.AppendFormat("<StartDate><![CDATA[{0}]]></StartDate>", Convert.ToDateTime(row["StartDate"]).ToString("yyyy-MM-dd HH:mm"));
                    sb.AppendFormat("<EndDate><![CDATA[{0}]]></EndDate>", Convert.ToDateTime(row["EndDate"]).ToString("yyyy-MM-dd HH:mm"));
                    sb.AppendFormat("<ActivityType><![CDATA[{0}]]></ActivityType>", row["ActivityType"].ToString());
                    sb.AppendFormat("<SignUpCount><![CDATA[{0}]]></SignUpCount>", row["UpdateSignUpCount"].ToString());

                    if (row["ActivityType"].ToString() == "1")
                    {
                        ActivitySignUp bllAS = new ActivitySignUp();
                        sb.AppendFormat("<IsSignUp><![CDATA[{0}]]></IsSignUp>", bllAS.IsAlreadySignUp(gUserId.ToString(), activityID));
                    }
                    else
                    {
                        sb.Append("<IsSignUp></IsSignUp>");
                    }

                    string oriImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string bImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string mImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string sImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);

                    sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                    sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                    sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                    sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);
                    sb.AppendFormat("<InsertTime><![CDATA[{0}]]></InsertTime>", Convert.ToDateTime(row["InsertDate"]).ToString("yyyy-MM-dd HH:mm"));
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string GetActivityPlayerList(string activityID, int pageIndex, int pageCount)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                ActivityPlayer bll = new ActivityPlayer();
                ParamsHelper parms = new ParamsHelper();
                string sqlWhere = " and ActivityId = @ActivityId and IsDisable=0";
                SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(activityID);
                parms.Add(parm);
                DataSet ds = bll.GetListOW(pageIndex, pageCount, sqlWhere, parms.ToArray());
                sb.Append("<Rsp>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append("<N>");
                    sb.AppendFormat("<PlayerID><![CDATA[{0}]]></PlayerID>", row["Id"].ToString());
                    sb.AppendFormat("<Name><![CDATA[{0}]]></Name>", row["Named"].ToString());
                    sb.AppendFormat("<VoteCount><![CDATA[{0}]]></VoteCount>", row["UpdateVoteCount"].ToString());

                    string oriImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string bImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string mImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string sImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);

                    sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                    sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                    sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                    sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);
                    sb.Append("</N>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string GetActivityPlayerInfo(string playerID, string userID)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                ActivityPlayer bll = new ActivityPlayer();
                DataSet ds = bll.GetModelOW(playerID);
                Guid gUserId = GetUserId(userID);

                sb.Append("<Rsp>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.AppendFormat("<PlayerID><![CDATA[{0}]]></PlayerID>", row["Id"].ToString());
                    sb.AppendFormat("<Name><![CDATA[{0}]]></Name>", row["Named"].ToString());
                    sb.AppendFormat("<DetailInfo><![CDATA[{0}]]></DetailInfo>", row["DetailInformation"].ToString());
                    sb.AppendFormat("<VoteCount><![CDATA[{0}]]></VoteCount>", row["UpdateVoteCount"].ToString());

                    string oriImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string bImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string mImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string sImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);

                    sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                    sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                    sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                    sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);

                    VoteToPlayer bllVP = new VoteToPlayer();
                    ParamsHelper parms = new ParamsHelper();
                    string sqlWhere = " and ActivityId = @ActivityId and UserId = @UserId";
                    SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
                    parm.Value = Guid.Parse(row["ActivityId"].ToString());
                    parms.Add(parm);
                    parm = new SqlParameter("@UserId", SqlDbType.UniqueIdentifier);
                    parm.Value = gUserId;
                    parms.Add(parm);
                    int userVoteCount = bllVP.GetCount(sqlWhere, parms.ToArray());
                    int maxVoteCount = int.Parse(row["MaxVoteCount"].ToString());

                    sb.AppendFormat("<MaxVoteCount><![CDATA[{0}]]></MaxVoteCount>", maxVoteCount);
                    sb.AppendFormat("<UserVoteCount><![CDATA[{0}]]></UserVoteCount>", userVoteCount);
                    sb.AppendFormat("<SurplusVoteCount><![CDATA[{0}]]></SurplusVoteCount>", maxVoteCount - userVoteCount);
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string AddVoteToPlayer(string playerID, string userID, int voteCount)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>登录ID为空</RtMsg>");
                    return FormatStrReturn("0", sb.ToString());
                }

                Guid gUserId = GetUserId(userID);

                VoteToPlayerInfo model = new VoteToPlayerInfo();
                model.UserId = gUserId;
                model.PlayerId = Guid.Parse(playerID);
                model.VoteCount = voteCount;
                model.Remark = "";
                model.LastUpdatedDate = DateTime.Now;

                sb.Append("<Rsp>");

                ActivityPlayer bllAP = new ActivityPlayer();
                DataSet ds = bllAP.GetModelOW(playerID);
                if (ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    VoteToPlayer bll = new VoteToPlayer();
                    ParamsHelper parms = new ParamsHelper();
                    string sqlWhere = " and PlayerID = @PlayerID and UserId = @UserId";
                    SqlParameter parm = new SqlParameter("@PlayerID", SqlDbType.UniqueIdentifier);
                    parm.Value = model.PlayerId;
                    parms.Add(parm);
                    parm = new SqlParameter("@UserId", SqlDbType.UniqueIdentifier);
                    parm.Value = model.UserId;
                    parms.Add(parm);
                    int count = bll.GetCount(sqlWhere, parms.ToArray());

                    int maxVoteCount = int.Parse(ds.Tables[0].Rows[0]["MaxVoteCount"].ToString());
                    int rt = 0;
                    if (count + voteCount <= maxVoteCount)
                    {
                        if (count > 0)
                        {
                            rt = bll.UpdateVoteCount(model);
                        }
                        else
                        {
                            rt = bll.Insert(model);
                        }
                        if (rt > 0)
                        {
                            bllAP.UpdateVoteCount(model.PlayerId, voteCount);
                            sb.Append("<IsOk>true</IsOk>");
                            sb.Append("<RtMsg>投票成功</RtMsg>");
                        }
                        else
                        {
                            sb.Append("<IsOk>false</IsOk>");
                            sb.Append("<RtMsg>投票失败</RtMsg>");
                        }
                    }
                    else
                    {
                        sb.Append("<IsOk>false</IsOk>");
                        sb.Append("<RtMsg>超过用户剩余投票数</RtMsg>");
                    }
                }
                else
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>选手无效</RtMsg>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string AddSignUpPlayer(string LoginID, string ActivityId, string Remark)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (string.IsNullOrEmpty(LoginID))
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>登录ID为空</RtMsg>");
                    return FormatStrReturn("0", sb.ToString());
                }

                Guid gUserId = GetUserId(LoginID);


                //实例化实体
                ActivitySignUpInfo model = new ActivitySignUpInfo();
                model.UserId = gUserId;//Guid.Parse(userID);
                model.ActivityId = Guid.Parse(ActivityId);
                model.Remark = Remark;
                model.LastUpdatedDate = DateTime.Now;

                sb.Append("<Rsp>");

                ActivitySignUp bll = new ActivitySignUp();

                int rt = 0;
                //先判断是否已达到报名总数
                if (bll.IsNotAtFull(ActivityId) && !bll.IsAlreadySignUp(gUserId.ToString(), ActivityId))
                {
                    rt = bll.Insert(model);
                }

                if (rt > 0)
                {
                    ActivitySubject bllSb = new ActivitySubject();
                    bllSb.UpdateSignUpCount(Guid.Parse(ActivityId));
                    sb.Append("<IsOk>true</IsOk>");
                    sb.AppendFormat("<RtMsg><SignUpCount>{0}</SignUpCount></RtMsg>", bll.SignUpCount(ActivityId));
                }
                else
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>报名失败</RtMsg>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string UpdateActivityPlayerPicture(string username, string imageBase64, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<PictureInfo>");
            try
            {
                var userId = GetUserId(username);
                if (userId == null)
                {
                    throw new ArgumentException("用户【" + username + "】不存在，请正确操作");
                }

                string fileEx = Path.GetExtension(fileName);
                if (string.IsNullOrWhiteSpace(imageBase64) || string.IsNullOrWhiteSpace(fileName))
                {
                    throw new ArgumentException("未获取到任何可上传的图片，请正确操作");
                }

                int uploadFileSize = int.Parse(ConfigurationManager.AppSettings["UploadFileSize"]);

                byte[] bytes = Convert.FromBase64String(imageBase64);
                int fileSize = bytes.Length;
                if (fileSize > uploadFileSize)
                {
                    throw new ArgumentException("文件【" + fileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                }

                ImageHelper ih = new ImageHelper();
                if (!ih.IsPictureValidated(bytes))
                {
                    throw new ArgumentException("图片不符合规定，请正确操作");
                }

                EnumData.UploadSaveDir subDirName = EnumData.UploadSaveDir.ActivityPhotoPicture;

                ActivityPlayerPhotoPicture uhpBll = new ActivityPlayerPhotoPicture();
                ActivityPlayerPhotoPictureInfo model = new ActivityPlayerPhotoPictureInfo();

                using (TransactionScope scope = new TransactionScope())
                {
                    using (TransactionScope scope2 = new TransactionScope(TransactionScopeOption.Suppress))
                    {
                        if (uhpBll.IsExist(fileName, fileSize))
                        {
                            scope2.Complete();
                            throw new ArgumentException("文件【" + fileName + "】已存在，请勿重复上传！");
                        }

                        scope2.Complete();
                    }

                    string rootPath = ConfigurationManager.AppSettings["FilesRoot"];
                    string picturePath = rootPath + "\\" + subDirName.ToString() + "";
                    string rndCode = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), GetRandomNumber("app"));
                    string saveDir = string.Format("{0}\\{1}", picturePath, DateTime.Now.ToString("yyyyMM"));
                    if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
                    string saveFilePath = string.Format("{0}\\{1}", saveDir, fileName);

                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        Image img = Image.FromStream(ms);
                        img.Save(saveFilePath);
                    }

                    model.Id = Guid.NewGuid();
                    model.FileName = fileName;
                    model.FileSize = bytes.Length;
                    model.FileExtension = fileEx;
                    model.FileDirectory = saveDir.Replace(rootPath.Substring(0, rootPath.LastIndexOf("\\")), "").Replace("\\", "/") + "/";
                    model.RandomFolder = rndCode;
                    model.LastUpdatedDate = DateTime.Now;
                    uhpBll.InsertByOutput(model);

                    string rndDirFullPath = string.Format("{0}\\{1}", saveDir, rndCode);
                    if (!Directory.Exists(rndDirFullPath))
                    {
                        Directory.CreateDirectory(rndDirFullPath);
                    }
                    File.Copy(saveFilePath, string.Format("{0}\\{1}{2}", rndDirFullPath, rndCode, fileEx), true);

                    string[] platformNames = Enum.GetNames(typeof(EnumData.Platform));
                    foreach (string name in platformNames)
                    {
                        string platformUrlFullPath = string.Format("{0}\\{1}\\{2}", saveDir, rndCode, name);
                        if (!Directory.Exists(platformUrlFullPath))
                        {
                            Directory.CreateDirectory(platformUrlFullPath);
                        }
                        string sizeAppend = ConfigurationManager.AppSettings[name];
                        string[] sizeArr = sizeAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < sizeArr.Length; i++)
                        {
                            string bmsPicUrl = string.Format("{0}\\{1}_{2}{3}", platformUrlFullPath, rndCode, i, fileEx);
                            string[] wh = sizeArr[i].Split('*');

                            ih.CreateThumbnailImage(saveFilePath, bmsPicUrl, int.Parse(wh[0]), int.Parse(wh[1]), "DB", fileEx);
                        }
                    }

                    scope.Complete();
                }

                sb.AppendFormat(@"<IsOk>{0}</IsOk>", true);
                sb.Append(@"<RtMsg></RtMsg>");
                sb.Append(@"<PictureId>" + model.Id + "</PictureId>");

            }
            catch (Exception ex)
            {
                sb.AppendFormat(@"<IsOk>{0}</IsOk>", false);
                sb.AppendFormat(@"<RtMsg><![CDATA[{0}]]></RtMsg>", ex.Message);
                new CustomException(string.Format("string UpdateActivityPlayerPicture(string username, string imageBase64, string fileName)方法异常：{0}", ex.Message), ex);
            }
            sb.Append("</PictureInfo>");

            return sb.ToString();
        }

        #endregion

        #region 活动（新）
        public string GetActivityListNew(int pageIndex, int pageCount)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                ActivitySubjectNew bll = new ActivitySubjectNew();
                ParamsHelper parms = new ParamsHelper();
                string sqlWhere = " and StartDate <= @Date and DATEADD(day,1,EndDate) > @Date and IsDisable=0";
                SqlParameter parm = new SqlParameter("@Date", SqlDbType.DateTime);
                parm.Value = DateTime.Now;
                parms.Add(parm);
                DataSet ds = bll.GetListOW(pageIndex, pageCount, sqlWhere, parms.ToArray());

                sb.Append("<Rsp>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append("<N>");
                    sb.AppendFormat("<ActivityID><![CDATA[{0}]]></ActivityID>", row["Id"].ToString());
                    sb.AppendFormat("<Title><![CDATA[{0}]]></Title>", row["Title"].ToString());
                    sb.AppendFormat("<StartDate><![CDATA[{0}]]></StartDate>", Convert.ToDateTime(row["StartDate"]).ToString("yyyy-MM-dd"));
                    sb.AppendFormat("<EndDate><![CDATA[{0}]]></EndDate>", Convert.ToDateTime(row["EndDate"]).ToString("yyyy-MM-dd"));
                    sb.AppendFormat("<SignUpCount><![CDATA[{0}]]></SignUpCount>", row["VirtualSignUpCount"].ToString());

                    string oriImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string bImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string mImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string sImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);

                    sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                    sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                    sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                    sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);
                    sb.AppendFormat("<IsScratchLotto><![CDATA[{0}]]></IsScratchLotto>", row["IsPrize"].ToString());
                    sb.AppendFormat("<InsertTime><![CDATA[{0}]]></InsertTime>", Convert.ToDateTime(row["InsertDate"]).ToString("yyyy-MM-dd HH:mm"));
                    sb.Append("</N>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string GetActivityInfoNew(string activityID, string userID, int pageIndex, int pageCount)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                Guid gUserId = GetUserId(userID);

                ActivitySubjectNew bll = new ActivitySubjectNew();
                DataSet ds = bll.GetModelOW(activityID);
                Regex r = new Regex("(<img)(.*)src=\"([^\"]*?)\"(.*)/>");

                sb.Append("<Rsp>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    bll.UpdateViewCount(activityID);

                    sb.AppendFormat("<ActivityID><![CDATA[{0}]]></ActivityID>", row["Id"].ToString());
                    sb.AppendFormat("<Title><![CDATA[{0}]]></Title>", row["Title"].ToString());
                    sb.AppendFormat("<Content><![CDATA[{0}]]></Content>", r.Replace(row["ContentText"].ToString(), "$1$2src=\"" + _webSiteHost + "$3\" />"));
                    sb.AppendFormat("<StartDate><![CDATA[{0}]]></StartDate>", Convert.ToDateTime(row["StartDate"]).ToString("yyyy-MM-dd"));
                    sb.AppendFormat("<EndDate><![CDATA[{0}]]></EndDate>", Convert.ToDateTime(row["EndDate"]).ToString("yyyy-MM-dd"));

                    string oriImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string bImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string mImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string sImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);

                    sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                    sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                    sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                    sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);
                    sb.AppendFormat("<SignUpCount><![CDATA[{0}]]></SignUpCount>", row["VirtualSignUpCount"].ToString());
                    sb.AppendFormat("<SignUpRule><![CDATA[{0}]]></SignUpRule>", r.Replace(row["SignUpRule"].ToString(), "$1$2src=\"" + _webSiteHost + "$3\" />"));

                    VoteToPlayerNew bllVP = new VoteToPlayerNew();
                    ParamsHelper parms = new ParamsHelper();
                    string sqlWhere = " and ActivityId = @ActivityId";
                    SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
                    parm.Value = Guid.Parse(activityID);
                    parms.Add(parm);
                    int count = bllVP.GetCount(sqlWhere, parms.ToArray());

                    sb.AppendFormat("<CumulativeVoteCount><![CDATA[{0}]]></CumulativeVoteCount>", count);

                    sb.AppendFormat("<AccessCount><![CDATA[{0}]]></AccessCount>", row["ViewCount"].ToString());

                    ActivityPlayerNew bllAP = new ActivityPlayerNew();
                    ParamsHelper parmsAP = new ParamsHelper();
                    sqlWhere = " and ActivityId = @ActivityId and AP.IsDisable=0";
                    SqlParameter parmAP = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
                    parmAP.Value = Guid.Parse(activityID);
                    parmsAP.Add(parmAP);
                    DataSet dsAP = bllAP.GetListOW(pageIndex, pageCount, sqlWhere, parmsAP.ToArray());
                    sb.Append("<PlayerList>");
                    foreach (DataRow rowAP in dsAP.Tables[0].Rows)
                    {
                        sb.Append("<PlayerInfo>");
                        sb.AppendFormat("<PlayerID><![CDATA[{0}]]></PlayerID>", rowAP["Id"].ToString());
                        sb.AppendFormat("<No><![CDATA[{0}]]></No>", rowAP["No"].ToString());
                        sb.AppendFormat("<Name><![CDATA[{0}]]></Name>", rowAP["Named"].ToString());
                        sb.AppendFormat("<VoteCount><![CDATA[{0}]]></VoteCount>", rowAP["VirtualVoteCount"].ToString());

                        oriImgUrl = rowAP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", rowAP["FileDirectory"], rowAP["RandomFolder"], rowAP["FileExtension"]);
                        bImgUrl = rowAP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", rowAP["FileDirectory"], rowAP["RandomFolder"], rowAP["FileExtension"]);
                        mImgUrl = rowAP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", rowAP["FileDirectory"], rowAP["RandomFolder"], rowAP["FileExtension"]);
                        sImgUrl = rowAP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", rowAP["FileDirectory"], rowAP["RandomFolder"], rowAP["FileExtension"]);

                        sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                        sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                        sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                        sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);
                        sb.Append("</PlayerInfo>");
                    }
                    sb.Append("</PlayerList>");
                    sb.AppendFormat("<IsScratchLotto><![CDATA[{0}]]></IsScratchLotto>", row["IsPrize"].ToString());
                    sb.AppendFormat("<ScratchLottoRule><![CDATA[{0}]]></ScratchLottoRule>", r.Replace(row["PrizeRule"].ToString(), "$1$2src=\"" + _webSiteHost + "$3\" />"));

                    int vpCountByUser = 0;
                    bool isScratch = true;

                    if (row["IsPrize"].ToString().ToUpper() == "TRUE")
                    {
                        #region 获取该用户当天是否投过票
                        ParamsHelper parmsVP = new ParamsHelper();
                        sqlWhere = string.Format(" and ActivityId = @ActivityId and UserFlag = @UserFlag and VP.LastUpdatedDate>='{0}' and VP.LastUpdatedDate<'{1}'",
                            DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                        parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
                        parm.Value = Guid.Parse(activityID);
                        parmsVP.Add(parm);
                        parm = new SqlParameter("@UserFlag", SqlDbType.VarChar, 50);
                        parm.Value = userID;
                        parmsVP.Add(parm);
                        vpCountByUser = bllVP.GetCount(sqlWhere, parmsVP.ToArray());
                        #endregion

                        if (vpCountByUser > 0)
                        {
                            #region 判断该用户当天是否刮过奖
                            WinningRecord bllWR = new WinningRecord();
                            isScratch = bllWR.IsScratch(Guid.Parse(activityID), userID);
                            #endregion
                        }
                    }
                    sb.AppendFormat("<IsHaveScratch><![CDATA[{0}]]></IsHaveScratch>", isScratch ? "False" : "True");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string GetActivityPlayerListNew(string activityID, string key, int pageIndex, int pageCount)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                ActivityPlayerNew bll = new ActivityPlayerNew();
                ParamsHelper parms = new ParamsHelper();
                string sqlWhere = " and ActivityId = @ActivityId and AP.IsDisable=0";
                SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(activityID);
                parms.Add(parm);
                if (!string.IsNullOrEmpty(key))
                {
                    sqlWhere += " and (Named like @Named or No = @No)";
                    parm = new SqlParameter("@Named", SqlDbType.VarChar, 30);
                    parm.Value = "%" + key + "%";
                    parms.Add(parm);
                    parm = new SqlParameter("@No", SqlDbType.Int);

                    int no = 0;
                    int.TryParse(key, out no);

                    parm.Value = no;
                    parms.Add(parm);
                }
                DataSet ds = bll.GetListOW(pageIndex, pageCount, sqlWhere, parms.ToArray());
                sb.Append("<Rsp>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append("<N>");
                    sb.AppendFormat("<PlayerID><![CDATA[{0}]]></PlayerID>", row["Id"].ToString());
                    sb.AppendFormat("<No><![CDATA[{0}]]></No>", row["No"].ToString());
                    sb.AppendFormat("<Name><![CDATA[{0}]]></Name>", row["Named"].ToString());
                    sb.AppendFormat("<VoteCount><![CDATA[{0}]]></VoteCount>", row["VirtualVoteCount"].ToString());

                    string oriImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string bImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string mImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string sImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);

                    sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                    sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                    sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                    sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);
                    sb.Append("</N>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string GetActivityPlayerInfoNew(string playerID, string userID)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                ActivityPlayerNew bll = new ActivityPlayerNew();
                DataSet ds = bll.GetModelOW(playerID);
                Guid gUserId = GetUserId(userID);

                sb.Append("<Rsp>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.AppendFormat("<PlayerID><![CDATA[{0}]]></PlayerID>", row["Id"].ToString());
                    sb.AppendFormat("<No><![CDATA[{0}]]></No>", row["No"].ToString());
                    sb.AppendFormat("<Name><![CDATA[{0}]]></Name>", row["HiddenAttribute"].ToString().Contains("Named") ? "" : row["Named"].ToString());
                    sb.AppendFormat("<Age><![CDATA[{0}]]></Age>", row["HiddenAttribute"].ToString().Contains("Age") ? "" : row["Age"].ToString());
                    sb.AppendFormat("<Occupation><![CDATA[{0}]]></Occupation>", row["HiddenAttribute"].ToString().Contains("Occupation") ? "" : row["Occupation"].ToString());
                    sb.AppendFormat("<Phone><![CDATA[{0}]]></Phone>", row["HiddenAttribute"].ToString().Contains("Phone") ? "" : row["Phone"].ToString());
                    sb.AppendFormat("<Location><![CDATA[{0}]]></Location>", row["HiddenAttribute"].ToString().Contains("Location") ? "" : row["Location"].ToString());
                    sb.AppendFormat("<Professional><![CDATA[{0}]]></Professional>", row["HiddenAttribute"].ToString().Contains("Professional") ? "" : row["Professional"].ToString());
                    sb.AppendFormat("<DetailInfo><![CDATA[{0}]]></DetailInfo>", row["Descr"].ToString());
                    sb.AppendFormat("<VoteCount><![CDATA[{0}]]></VoteCount>", row["VirtualVoteCount"].ToString());

                    //string oriImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    //string bImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    //string mImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    //string sImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);

                    //sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                    //sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                    //sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                    //sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);

                    VoteToPlayerNew bllVP = new VoteToPlayerNew();
                    ParamsHelper parms = new ParamsHelper();
                    string sqlWhere = string.Format(" and ActivityId = @ActivityId and UserFlag = @UserFlag and VP.LastUpdatedDate>='{0}' and VP.LastUpdatedDate<'{1}'",
                        DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                    SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
                    parm.Value = Guid.Parse(row["ActivityId"].ToString());
                    parms.Add(parm);
                    parm = new SqlParameter("@UserFlag", SqlDbType.VarChar, 50);
                    parm.Value = userID;
                    parms.Add(parm);
                    int count = bllVP.GetCount(sqlWhere, parms.ToArray());

                    sb.AppendFormat("<IsVote><![CDATA[{0}]]></IsVote>", (count > 0 || string.IsNullOrEmpty(userID)) ? "false" : "true");

                    PlayerPictureNew bllPP = new PlayerPictureNew();
                    DataSet dsPP = bllPP.GetListOW(playerID);
                    sb.Append("<PictureList>");
                    foreach (DataRow rowPP in dsPP.Tables[0].Rows)
                    {
                        sb.Append("<Picture>");
                        string oriImgUrl = rowPP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", rowPP["FileDirectory"], rowPP["RandomFolder"], rowPP["FileExtension"]);
                        string bImgUrl = rowPP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", rowPP["FileDirectory"], rowPP["RandomFolder"], rowPP["FileExtension"]);
                        string mImgUrl = rowPP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", rowPP["FileDirectory"], rowPP["RandomFolder"], rowPP["FileExtension"]);
                        string sImgUrl = rowPP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", rowPP["FileDirectory"], rowPP["RandomFolder"], rowPP["FileExtension"]);

                        sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                        sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                        sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                        sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);
                        sb.Append("</Picture>");
                    }
                    sb.Append("</PictureList>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string AddSignUpNew(string userID, string activityID, string name, string age, string occupation, string phone, string location, string professional, string detailInfo)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<Rsp>");

                if (string.IsNullOrEmpty(userID))
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>登录ID为空</RtMsg>");
                    sb.Append("</Rsp>");
                    return FormatStrReturn("0", sb.ToString());
                }

                Guid gUserId = GetUserId(userID);


                ActivityPlayerNewInfo model = new ActivityPlayerNewInfo();
                model.LastUpdatedDate = DateTime.Now;
                model.Remark = "";

                model.ActivityId = Guid.Parse(activityID);
                model.UserId = gUserId;
                model.Named = name;
                model.Age = int.Parse(age);
                model.Occupation = occupation;
                model.Phone = phone;
                model.Location = location;
                model.Professional = professional;
                model.Descr = detailInfo;
                model.VoteCount = 0;
                model.VirtualVoteCount = 0;

                ActivitySubjectNew bllAS = new ActivitySubjectNew();
                ActivitySubjectNewInfo info = new ActivitySubjectNewInfo();
                info = bllAS.GetModel(activityID);

                if (info.SignUpCount >= info.MaxVoteCount)
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>报名数已达上限</RtMsg>");
                }
                else
                {
                    ActivityPlayerNew bll = new ActivityPlayerNew();
                    if (bll.IsExist(model.ActivityId, phone))
                    {
                        sb.Append("<IsOk>false</IsOk>");
                        sb.Append("<RtMsg>该选手已存在</RtMsg>");
                    }
                    else
                    {
                        Guid playerId = bll.InsertByOutput(model);
                        if (!playerId.Equals(Guid.Empty))
                        {
                            sb.Append("<IsOk>true</IsOk>");
                            sb.Append("<RtMsg>报名成功</RtMsg>");
                            sb.AppendFormat("<PlayerID>{0}</PlayerID>", playerId);
                            bllAS.UpdateSignUpCount(activityID);
                        }
                        else
                        {
                            sb.Append("<IsOk>false</IsOk>");
                            sb.Append("<RtMsg>报名失败</RtMsg>");
                        }
                    }
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string AddSignUpImg(string playerID, string userID, string fileName, string base64String, int sort)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<Rsp>");

                if (string.IsNullOrEmpty(playerID))
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>选手ID为空</RtMsg>");
                    sb.Append("</Rsp>");
                    return FormatStrReturn("0", sb.ToString());
                }

                fileName = Guid.NewGuid().ToString() + fileName.Substring(fileName.LastIndexOf("."));

                string pictureIdXml = UpdateActivityPlayerPicture(userID, base64String, fileName);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(pictureIdXml);
                XmlNode node = doc.SelectSingleNode("PictureInfo/IsOk");
                if (node.InnerText == "False")
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.AppendFormat("<RtMsg>{0}</RtMsg>", doc.SelectSingleNode("PictureInfo/RtMsg").InnerText);
                    sb.Append("</Rsp>");
                    return FormatStrReturn("0", sb.ToString());
                }
                string sPictureId = doc.SelectSingleNode("PictureInfo/PictureId").InnerText;
                PlayerPictureNew bllPP = new PlayerPictureNew();
                PlayerPictureNewInfo infoPP = new PlayerPictureNewInfo();
                Guid pictureId = Guid.Empty;
                Guid.TryParse(sPictureId, out pictureId);
                infoPP.PlayerId = Guid.Parse(playerID);
                infoPP.PictureId = pictureId;
                infoPP.Sort = sort;
                infoPP.IsHeadImg = (sort == 1);
                int rt = bllPP.Insert(infoPP);

                if (rt > 0)
                {
                    sb.Append("<IsOk>true</IsOk>");
                    sb.Append("<RtMsg>上传成功</RtMsg>");
                }
                else
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>上传失败</RtMsg>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string AddVoteToPlayerNew(string playerID, string userID)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<Rsp>");
                if (string.IsNullOrEmpty(userID))
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>登录ID或设备ID为空</RtMsg>");
                    sb.Append("</Rsp>");
                    return FormatStrReturn("0", sb.ToString());
                }

                Guid gUserId = GetUserId(userID);

                VoteToPlayerNewInfo model = new VoteToPlayerNewInfo();
                model.UserId = gUserId;
                model.UserFlag = userID;
                model.PlayerId = Guid.Parse(playerID);
                model.TotalVoteCount = 1;
                model.Remark = "";
                model.LastUpdatedDate = DateTime.Now;

                ActivityPlayerNew bllAP = new ActivityPlayerNew();
                ActivityPlayerNewInfo infoAP = new ActivityPlayerNewInfo();
                infoAP = bllAP.GetModel(playerID);
                if (infoAP != null)
                {
                    VoteToPlayerNew bllVP = new VoteToPlayerNew();
                    ParamsHelper parms = new ParamsHelper();
                    string sqlWhere = string.Format(" and ActivityId = @ActivityId and UserFlag = @UserFlag and VP.LastUpdatedDate>='{0}' and VP.LastUpdatedDate<'{1}'",
                        DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                    SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
                    parm.Value = infoAP.ActivityId;
                    parms.Add(parm);
                    parm = new SqlParameter("@UserFlag", SqlDbType.VarChar, 50);
                    parm.Value = userID;
                    parms.Add(parm);
                    int count = bllVP.GetCount(sqlWhere, parms.ToArray());

                    ActivitySubjectNew bllAS = new ActivitySubjectNew();
                    ActivitySubjectNewInfo infoAS = new ActivitySubjectNewInfo();
                    infoAS = bllAS.GetModel(infoAP.ActivityId);
                    if (infoAP.VoteCount < infoAS.MaxVoteCount)
                    {
                        if (count <= 0)
                        {
                            int rt = 0;
                            parms = new ParamsHelper();
                            sqlWhere = " and PlayerId = @PlayerId and UserFlag = @UserFlag";
                            parm = new SqlParameter("@PlayerId", SqlDbType.UniqueIdentifier);
                            parm.Value = Guid.Parse(playerID);
                            parms.Add(parm);
                            parm = new SqlParameter("@UserFlag", SqlDbType.VarChar, 50);
                            parm.Value = userID;
                            parms.Add(parm);
                            int voteCount = bllVP.GetCount(sqlWhere, parms.ToArray());
                            if (voteCount > 0)
                            {
                                rt = bllVP.UpdateVoteCount(model);
                            }
                            else
                            {
                                rt = bllVP.Insert(model);
                            }
                            if (rt > 0)
                            {
                                bllAP.UpdateVoteCount(playerID);
                                sb.Append("<IsOk>true</IsOk>");
                                sb.Append("<RtMsg>投票成功</RtMsg>");
                            }
                            else
                            {
                                sb.Append("<IsOk>false</IsOk>");
                                sb.Append("<RtMsg>投票失败</RtMsg>");
                            }
                        }
                        else
                        {
                            sb.Append("<IsOk>false</IsOk>");
                            sb.Append("<RtMsg>每天只能投票一次</RtMsg>");
                        }
                    }
                    else
                    {
                        sb.Append("<IsOk>false</IsOk>");
                        sb.Append("<RtMsg>超过最大投票数</RtMsg>");
                    }
                }
                else
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>选手无效</RtMsg>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string SubmitScratchLotto(string userID, string activityID)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<Rsp>");
                if (string.IsNullOrEmpty(userID))
                {
                    sb.Append("<IsWinning>false</IsWinning>");
                    sb.Append("</Rsp>");
                    return FormatStrReturn("0", sb.ToString());
                }


                Guid gUserId = GetUserId(userID);
                ActivitySubjectNew bllAS = new ActivitySubjectNew();
                ActivityPrize bllAP = new ActivityPrize();
                WinningRecord bllWR = new WinningRecord();

                if (bllWR.IsScratch(Guid.Parse(activityID), userID))
                {
                    sb.Append("<IsWinning>false</IsWinning>");
                    sb.Append("</Rsp>");
                    return FormatStrReturn("0", sb.ToString());
                }

                DataSet dsAS = bllAS.GetModelOW(activityID);
                if (dsAS.Tables[0].Rows != null && dsAS.Tables[0].Rows.Count > 0)
                {
                    DataRow rowAS = dsAS.Tables[0].Rows[0];
                    int winningCount = int.Parse(rowAS["PrizeProbability"].ToString());
                    Random rd = new Random();
                    int number = rd.Next(1, 1001);
                    if (number > 0 && number <= winningCount)
                    {
                        //中奖
                        ParamsHelper parms = new ParamsHelper();
                        string sqlWhere = " and ActivityId = @ActivityId and UpdateWinningTimes > 0 and IsDisable=0";
                        SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
                        parm.Value = Guid.Parse(activityID);
                        parms.Add(parm);
                        DataSet dsAP = bllAP.GetListOW(sqlWhere, parms.ToArray());
                        if (dsAP.Tables[0].Rows != null && dsAP.Tables[0].Rows.Count > 0)
                        {
                            //奖项品类数
                            int prizeCount = dsAP.Tables[0].Rows.Count;
                            //随机奖项
                            int prizeNumber = rd.Next(0, prizeCount);
                            DataRow rowAP = dsAP.Tables[0].Rows[prizeNumber];

                            sb.Append("<IsWinning>true</IsWinning>");
                            sb.Append("<N>");
                            sb.AppendFormat("<PrizeID><![CDATA[{0}]]></PrizeID>", rowAP["ID"].ToString());
                            sb.AppendFormat("<PrizeName><![CDATA[{0}]]></PrizeName>", rowAP["PrizeName"].ToString());
                            sb.AppendFormat("<PrizeCount><![CDATA[{0}]]></PrizeCount>", rowAP["PrizeCount"].ToString());
                            sb.AppendFormat("<PrizeContent><![CDATA[{0}]]></PrizeContent>", rowAP["PrizeContent"].ToString());
                            sb.AppendFormat("<BusinessName><![CDATA[{0}]]></BusinessName>", rowAP["BusinessName"].ToString());
                            sb.AppendFormat("<BusinessPhone><![CDATA[{0}]]></BusinessPhone>", rowAP["BusinessPhone"].ToString());
                            sb.AppendFormat("<BusinessAddress><![CDATA[{0}]]></BusinessAddress>", rowAP["BusinessAddress"].ToString());

                            string oriImgUrl = rowAP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", rowAP["FileDirectory"], rowAP["RandomFolder"], rowAP["FileExtension"]);
                            string bImgUrl = rowAP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", rowAP["FileDirectory"], rowAP["RandomFolder"], rowAP["FileExtension"]);
                            string mImgUrl = rowAP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", rowAP["FileDirectory"], rowAP["RandomFolder"], rowAP["FileExtension"]);
                            string sImgUrl = rowAP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", rowAP["FileDirectory"], rowAP["RandomFolder"], rowAP["FileExtension"]);

                            sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                            sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                            sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                            sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);
                            sb.Append("</N>");
                        }
                        else
                        {
                            //当日奖品已领取完
                            sb.Append("<IsWinning>false</IsWinning>");
                        }
                    }
                    else
                    {
                        //不中奖
                        sb.Append("<IsWinning>false</IsWinning>");
                    }
                }
                else
                {
                    sb.Append("<IsWinning>false</IsWinning>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string SubmitWinningInfo(string userID, string activityID, string prizeID, string mobilePhone)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<Rsp>");
                if (string.IsNullOrEmpty(userID))
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>登录ID或设备ID为空</RtMsg>");
                    sb.Append("</Rsp>");
                    return FormatStrReturn("0", sb.ToString());
                }

                Guid gUserId = GetUserId(userID);

                ActivityPrize bllAP = new ActivityPrize();
                WinningRecord bllWR = new WinningRecord();

                if (bllWR.IsScratch(Guid.Parse(activityID), userID))
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>每日刮奖次数不超过1次</RtMsg>");
                    sb.Append("</Rsp>");
                    return FormatStrReturn("0", sb.ToString());
                }

                UserBase bllUB = new UserBase();
                WinningRecordInfo info = new WinningRecordInfo();
                info.ActivityId = Guid.Parse(activityID);
                info.UserId = gUserId;
                info.UserFlag = userID;
                info.MobilePhone = gUserId.Equals(Guid.Empty) ? mobilePhone : bllUB.GetModel(gUserId).MobilePhone;
                info.Status = 0;
                info.Remark = "";
                info.LastUpdatedDate = DateTime.Now;

                if (string.IsNullOrEmpty(prizeID))
                {
                    info.ActivityPrizeId = Guid.Empty;
                }
                else
                {
                    if (bllAP.UpdateWinningTimes(prizeID) > 0)
                    {
                        info.ActivityPrizeId = Guid.Parse(prizeID);
                    }
                    else
                    {
                        sb.Append("<IsOk>false</IsOk>");
                        sb.Append("<RtMsg>提交失败</RtMsg>");
                        sb.Append("</Rsp>");
                        return FormatStrReturn("0", sb.ToString());
                    }
                }
                if (bllWR.Insert(info) > 0)
                {
                    sb.Append("<IsOk>true</IsOk>");
                    sb.Append("<RtMsg>提交成功</RtMsg>");
                }
                else
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>提交失败</RtMsg>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string GetMyWinningRecordList(string userID, string activityID, int pageIndex, int pageCount)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                Guid gUserId = GetUserId(userID);

                WinningRecord bll = new WinningRecord();
                ParamsHelper parms = new ParamsHelper();
                string sqlWhere = " and WR.UserFlag=@UserFlag and WR.ActivityPrizeId <> '00000000-0000-0000-0000-000000000000' ";
                SqlParameter parm = new SqlParameter("@UserFlag", SqlDbType.VarChar, 50);
                parm.Value = userID;
                parms.Add(parm);
                if (!string.IsNullOrEmpty(activityID))
                {
                    sqlWhere += " and WR.ActivityId = @ActivityId";
                    SqlParameter parm1 = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
                    parm1.Value = Guid.Parse(activityID);
                    parms.Add(parm1);
                }
                DataSet ds = bll.GetListOW(pageIndex, pageCount, sqlWhere, parms.ToArray());
                sb.Append("<Rsp>");
                foreach (DataRow rowAP in ds.Tables[0].Rows)
                {
                    sb.Append("<N>");
                    sb.AppendFormat("<ID>{0}</ID>", rowAP["Id"].ToString());
                    sb.AppendFormat("<MobilePhone>{0}</MobilePhone>", rowAP["MobilePhone"].ToString());
                    sb.AppendFormat("<PrizeName><![CDATA[{0}]]></PrizeName>", rowAP["PrizeName"].ToString());
                    sb.AppendFormat("<PrizeTime>{0}</PrizeTime>", Convert.ToDateTime(rowAP["LastUpdatedDate"]).ToString("yyyy-MM-dd HH:mm:ss"));
                    sb.AppendFormat("<PrizeCount>{0}</PrizeCount>", rowAP["PrizeCount"].ToString());
                    sb.AppendFormat("<PrizeContent><![CDATA[{0}]]></PrizeContent>", rowAP["PrizeContent"].ToString());
                    sb.AppendFormat("<BusinessName><![CDATA[{0}]]></BusinessName>", rowAP["BusinessName"].ToString());
                    sb.AppendFormat("<BusinessPhone><![CDATA[{0}]]></BusinessPhone>", rowAP["BusinessPhone"].ToString());
                    sb.AppendFormat("<BusinessAddress><![CDATA[{0}]]></BusinessAddress>", rowAP["BusinessAddress"].ToString());

                    string oriImgUrl = rowAP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", rowAP["FileDirectory"], rowAP["RandomFolder"], rowAP["FileExtension"]);
                    string bImgUrl = rowAP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", rowAP["FileDirectory"], rowAP["RandomFolder"], rowAP["FileExtension"]);
                    string mImgUrl = rowAP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", rowAP["FileDirectory"], rowAP["RandomFolder"], rowAP["FileExtension"]);
                    string sImgUrl = rowAP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", rowAP["FileDirectory"], rowAP["RandomFolder"], rowAP["FileExtension"]);

                    sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                    sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                    sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                    sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);
                    sb.Append("</N>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string UpdateWinningMobilePhone(string userID, string winningID, string mobilePhone)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<Rsp>");
                if (string.IsNullOrEmpty(userID))
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>登录ID或设备ID为空</RtMsg>");
                    sb.Append("</Rsp>");
                    return FormatStrReturn("0", sb.ToString());
                }

                WinningRecord bllWR = new WinningRecord();

                if (bllWR.UpdateMobilePhone(winningID, mobilePhone) > 0)
                {
                    sb.Append("<IsOk>true</IsOk>");
                    sb.Append("<RtMsg>修改成功</RtMsg>");
                }
                else
                {
                    sb.Append("<IsOk>false</IsOk>");
                    sb.Append("<RtMsg>修改失败</RtMsg>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }
        #endregion

        #region 摇奖

        public string IsExistErnieLatest()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Rsp>");

            try
            {
                Ernie bll = new Ernie();

                sb.AppendFormat("<IsOk>{0}</IsOk><IsExist>{1}</IsExist>", true, bll.IsExistLatest());
                sb.Append("<RtMsg></RtMsg>");
            }
            catch (Exception ex)
            {
                sb.AppendFormat("<IsOk>{0}</IsOk>", false);
                sb.AppendFormat("<RtMsg>{0}</RtMsg>", ex.Message);
                new CustomException(string.Format("方法名：string IsExistErnieLatest()，异常：{0}", ex.Message), ex);
            }

            sb.Append("</Rsp>");

            return sb.ToString();
        }

        #endregion

        #region 用户基本信息

        public string GetUserInfo(string username)
        {
            try
            {
                WebSecurityClient wsClient = new WebSecurityClient();
                var userId = wsClient.GetUserId(username);
                if (userId == null) return "";
                UserBase bll = new UserBase();
                var model = bll.GetModel(userId);
                if (model == null) return "";
                return string.Format(@"<Rsp><Nickname>{0}</Nickname><HeadImg>{1}</HeadImg><Sex>{2}</Sex><MobilePhone>{3}</MobilePhone><TotalGold>{4}</TotalGold><TotalSilver>{5}</TotalSilver><TotalIntegral>{6}</TotalIntegral><SilverLevel>{7}</SilverLevel><ColorLevel>{8}</ColorLevel><IntegralLevel>{9}</IntegralLevel><VIPLevel>{10}</VIPLevel><UserId>{11}</UserId></Rsp>", 
                    model.Nickname, _webSiteHost + model.HeadPicture, model.Sex, model.MobilePhone, model.TotalGold, model.TotalSilver, model.TotalIntegral, model.SilverLevel, model.ColorLevel, model.IntegralLevel, model.VIPLevel, model.UserId);
                
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return "";
            }
        }

        public string UpdateHeadPicture(string username, string imageBase64, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<UploadHeadImg>");
            try
            {
                var userId = GetUserId(username);
                if (userId == null)
                {
                    throw new ArgumentException("用户【" + username + "】不存在，请正确操作");
                }
                UserBase bll = new UserBase();
                var model = bll.GetModel(userId);
                if (model == null)
                {
                    throw new ArgumentException("用户【" + username + "】的基本信息不存在，请检查");
                }
                string fileEx = Path.GetExtension(fileName);
                if (string.IsNullOrWhiteSpace(imageBase64) || string.IsNullOrWhiteSpace(fileName))
                {
                    throw new ArgumentException("未获取到任何可上传的头像，请正确操作");
                }

                int uploadFileSize = int.Parse(ConfigurationManager.AppSettings["UploadFileSize"]);

                byte[] bytes = Convert.FromBase64String(imageBase64);
                int fileSize = bytes.Length;
                if (fileSize > uploadFileSize)
                {
                    throw new ArgumentException("文件【" + fileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                }

                ImageHelper ih = new ImageHelper();
                if (!ih.IsPictureValidated(bytes))
                {
                    throw new ArgumentException("上传的头像不符合规定，请正确操作");
                }

                UserHeadPicture uhpBll = new UserHeadPicture();
                if (uhpBll.IsExist(fileName, fileSize))
                {
                    throw new ArgumentException("文件【" + fileName + "】已存在，请勿重复上传！");
                }

                string rootPath = ConfigurationManager.AppSettings["FilesRoot"];
                string userPicturePath = rootPath + "\\UserHeadPicture";
                string rndCode = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), GetRandomNumber("hp"));
                string saveDir = string.Format("{0}\\{1}", userPicturePath, DateTime.Now.ToString("yyyyMM"));
                if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
                string saveFilePath = string.Format("{0}\\{1}", saveDir, fileName);
                if (File.Exists(saveFilePath))
                {
                    throw new ArgumentException("文件【" + fileName + "】已存在，请勿重复上传！");
                }

                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    Image img = Image.FromStream(ms);
                    img.Save(saveFilePath);
                }

                UserHeadPictureInfo uhpModel = new UserHeadPictureInfo();
                uhpModel.FileName = fileName;
                uhpModel.FileSize = bytes.Length;
                uhpModel.FileExtension = fileEx;
                uhpModel.FileDirectory = saveDir.Replace(rootPath.Substring(0, rootPath.LastIndexOf("\\")), "").Replace("\\", "/") + "/";
                uhpModel.RandomFolder = rndCode;
                uhpModel.LastUpdatedDate = DateTime.Now;
                uhpBll.Insert(uhpModel);

                string rndDirFullPath = string.Format("{0}\\{1}", saveDir, rndCode);
                if (!Directory.Exists(rndDirFullPath))
                {
                    Directory.CreateDirectory(rndDirFullPath);
                }
                File.Copy(saveFilePath, string.Format("{0}\\{1}{2}", rndDirFullPath, rndCode, fileEx), true);

                string[] platformNames = Enum.GetNames(typeof(EnumData.Platform));
                foreach (string name in platformNames)
                {
                    //string platformUrl = string.Format("{0}\\{1}\\{2}", dir, rndCode, name);
                    string platformUrlFullPath = string.Format("{0}\\{1}\\{2}", saveDir, rndCode, name);
                    if (!Directory.Exists(platformUrlFullPath))
                    {
                        Directory.CreateDirectory(platformUrlFullPath);
                    }
                    string sizeAppend = ConfigurationManager.AppSettings[name];
                    string[] sizeArr = sizeAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < sizeArr.Length; i++)
                    {
                        string bmsPicUrl = string.Format("{0}\\{1}_{2}{3}", platformUrlFullPath, rndCode, i, fileEx);
                        string[] wh = sizeArr[i].Split('*');

                        ih.CreateThumbnailImage(saveFilePath, bmsPicUrl, int.Parse(wh[0]), int.Parse(wh[1]), "DB", fileEx);

                        if (name == "Android" && i == 0)
                        {
                            model.HeadPicture = bmsPicUrl.Replace(rootPath.Substring(0, rootPath.LastIndexOf("\\")), "").Replace("\\", "/");
                            if (!model.HeadPicture.StartsWith("/"))
                            {
                                model.HeadPicture = "/" + model.HeadPicture;
                            }
                        }
                    }
                }
                if (bll.Update(model) < 1)
                {
                    throw new ArgumentException("操作失败");
                }
                sb.AppendFormat(@"<IsOk>{0}</IsOk>", true);
                sb.Append(@"<RtMsg></RtMsg>");
                sb.AppendFormat(@"<OriginalPicture> <![CDATA[{0}]]></OriginalPicture>", _webSiteHost + model.HeadPicture);
                sb.AppendFormat(@"<BPicture> <![CDATA[{0}]]></BPicture>", _webSiteHost + model.HeadPicture);
                sb.AppendFormat(@"<MPicture> <![CDATA[{0}]]></MPicture>", _webSiteHost + model.HeadPicture);
                sb.AppendFormat(@"<SPicture> <![CDATA[{0}]]></SPicture>", _webSiteHost + model.HeadPicture);
            }
            catch (Exception ex)
            {
                sb.AppendFormat(@"<IsOk>{0}</IsOk>", false);
                sb.AppendFormat(@"<RtMsg><![CDATA[{0}]]></RtMsg>", ex.Message);
                new CustomException(string.Format("string UpdateHeadPicture(string username, string imageBase64, string fileName)方法异常：{0}", ex.Message), ex);
            }
            sb.Append("</UploadHeadImg>");

            return sb.ToString();
        }

        public string UpdateUserBaseModel(string username, string nickname)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<UpdateMyInfo>");

            try
            {
                WebSecurityClient wsClient = new WebSecurityClient();
                var userId = wsClient.GetUserId(username);
                if (userId == null)
                {
                    throw new ArgumentException("用户【" + username + "】不存在，请正确操作");
                }
                UserBase bll = new UserBase();
                var model = bll.GetModel(userId);
                if (model == null)
                {
                    throw new ArgumentException("用户【" + username + "】的基本信息不存在，请检查");
                }
                model.Nickname = nickname;
                if (bll.Update(model) < 1) throw new ArgumentException("修改昵称失败，原因：在执行更新数据到数据表时发生了异常！");
                sb.AppendFormat(@"<IsOk>{0}</IsOk>", true);
                sb.AppendFormat(@"<RtMsg>{0}</RtMsg>", "操作成功！");
            }
            catch (Exception ex)
            {
                sb.AppendFormat(@"<IsOk>{0}</IsOk>", false);
                sb.AppendFormat(@"<RtMsg><![CDATA[{0}]]></RtMsg>", ex.Message);
                new CustomException(string.Format("string UpdateUserBaseModel(string username, string Nickname)方法异常：{0}", ex.Message), ex);
            }

            sb.Append("</UpdateMyInfo>");
            return sb.ToString();
        }

        #endregion

        #region 格式化返回值

        private string FormatStrReturn(string retCode, string msg)
        {
            switch (retCode)
            {
                case "-1":
                    return string.Format("<ResponseMsg><RetCode>{0}</RetCode><RetMsg>{1}</RetMsg><RetData></RetData></ResponseMsg>", retCode, msg);
                case "0":
                    return string.Format("<ResponseMsg><RetCode>{0}</RetCode><RetMsg>成功</RetMsg><RetData>{1}</RetData></ResponseMsg>", retCode, msg);
                default:
                    return "";
            }
        }

        #endregion

        #region 资讯
        public string GetInformationList(int pageIndex, int pageCount)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                Information bll = new Information();
                ParamsHelper parms = new ParamsHelper();
                string sqlWhere = " and IsDisable=0";
                SqlParameter parm = new SqlParameter("@Date", SqlDbType.DateTime);
                parm.Value = DateTime.Now;
                parms.Add(parm);
                DataSet ds = bll.GetListOW(pageIndex, pageCount, sqlWhere, parms.ToArray());

                sb.Append("<Rsp>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append("<N>");
                    sb.AppendFormat("<InformationID><![CDATA[{0}]]></InformationID>", row["Id"].ToString());
                    sb.AppendFormat("<Title><![CDATA[{0}]]></Title>", row["Title"].ToString());
                    sb.AppendFormat("<Summary><![CDATA[{0}]]></Summary>", row["Summary"].ToString());
                    sb.AppendFormat("<ViewCount><![CDATA[{0}]]></ViewCount>", row["ViewCount"].ToString());
                    sb.AppendFormat("<Sort><![CDATA[{0}]]></Sort>", row["Sort"].ToString());
                    sb.AppendFormat("<ViewType><![CDATA[{0}]]></ViewType>", row["ViewType"].ToString());
                    sb.AppendFormat("<LastUpdatedDate><![CDATA[{0}]]></LastUpdatedDate>", Convert.ToDateTime(row["LastUpdatedDate"]).ToString("yyyy-MM-dd HH:mm"));

                    InformationPicture bllPP = new InformationPicture();
                    DataSet dsPP = bllPP.GetListOW(row["Id"].ToString());
                    sb.Append("<PictureList>");
                    foreach (DataRow rowPP in dsPP.Tables[0].Rows)
                    {
                        sb.Append("<Picture>");
                        string oriImgUrl = rowPP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", rowPP["FileDirectory"], rowPP["RandomFolder"], rowPP["FileExtension"]);
                        string bImgUrl = rowPP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", rowPP["FileDirectory"], rowPP["RandomFolder"], rowPP["FileExtension"]);
                        string mImgUrl = rowPP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", rowPP["FileDirectory"], rowPP["RandomFolder"], rowPP["FileExtension"]);
                        string sImgUrl = rowPP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", rowPP["FileDirectory"], rowPP["RandomFolder"], rowPP["FileExtension"]);

                        sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                        sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                        sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                        sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);
                        sb.Append("</Picture>");
                    }
                    sb.Append("</PictureList>");

                    InformationAd bllIA = new InformationAd();

                    ParamsHelper parmsAd = new ParamsHelper();
                    string nowDate = DateTime.Now.ToString("yyyy-MM-dd");
                    string sqlWhereAd = string.Format(" and InformationId = @Id and StartDate <= '{0}' and EndDate >= '{1}' order by Sort asc ", nowDate, nowDate);
                    SqlParameter parmAd = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                    parmAd.Value = Guid.Parse(row["Id"].ToString());
                    parmsAd.Add(parmAd);

                    DataSet dsIA = bllIA.GetValidInforAdListOW(sqlWhereAd, parmsAd.ToArray());

                    sb.Append("<AdList>");
                    foreach (DataRow rowIA in dsIA.Tables[0].Rows)
                    {
                        sb.Append("<Ad>");
                        sb.AppendFormat("<AdID><![CDATA[{0}]]></AdID>", rowIA["InformationAdId"].ToString());
                        sb.AppendFormat("<Title><![CDATA[{0}]]></Title>", rowIA["Title"].ToString());
                        sb.AppendFormat("<Descr><![CDATA[{0}]]></Descr>", rowIA["Descr"].ToString());
                        sb.AppendFormat("<Url><![CDATA[{0}]]></Url>", rowIA["Url"].ToString());
                        sb.AppendFormat("<ViewType><![CDATA[{0}]]></ViewType>", rowIA["ViewType"].ToString());
                        sb.AppendFormat("<Sort><![CDATA[{0}]]></Sort>", rowIA["Sort"].ToString());
                        InformationAdPicture bllIAPP = new InformationAdPicture();
                        DataSet dsIAPP = bllIAPP.GetListOW(rowIA["InformationAdId"].ToString());
                        sb.Append("<AdPictureList>");
                        foreach (DataRow rowIAPP in dsIAPP.Tables[0].Rows)
                        {
                            sb.Append("<Picture>");
                            string oriImgUrl = rowIAPP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", rowIAPP["FileDirectory"], rowIAPP["RandomFolder"], rowIAPP["FileExtension"]);
                            string bImgUrl = rowIAPP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", rowIAPP["FileDirectory"], rowIAPP["RandomFolder"], rowIAPP["FileExtension"]);
                            string mImgUrl = rowIAPP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", rowIAPP["FileDirectory"], rowIAPP["RandomFolder"], rowIAPP["FileExtension"]);
                            string sImgUrl = rowIAPP["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", rowIAPP["FileDirectory"], rowIAPP["RandomFolder"], rowIAPP["FileExtension"]);

                            sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                            sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                            sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                            sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);
                            sb.Append("</Picture>");
                        }
                        sb.Append("</AdPictureList>");
                        sb.Append("</Ad>");
                    }
                    sb.Append("</AdList>");

                    sb.Append("</N>");
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string GetInformationInfo(string informationID)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                Information bll = new Information();
                DataSet ds = bll.GetModelOW(informationID);
                Regex r = new Regex("(<img)(.*)src=\"([^\"]*?)\"(.*)/>");

                sb.Append("<Rsp>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    bll.UpdateViewCount(informationID);

                    sb.AppendFormat("<InformationID><![CDATA[{0}]]></InformationID>", row["Id"].ToString());
                    sb.AppendFormat("<Title><![CDATA[{0}]]></Title>", row["Title"].ToString());
                    sb.AppendFormat("<Summary><![CDATA[{0}]]></Summary>", row["Summary"].ToString());
                    sb.AppendFormat("<ViewCount><![CDATA[{0}]]></ViewCount>", row["ViewCount"].ToString());
                    sb.AppendFormat("<Sort><![CDATA[{0}]]></Sort>", row["Sort"].ToString());
                    sb.AppendFormat("<ViewType><![CDATA[{0}]]></ViewType>", row["ViewType"].ToString());
                    sb.AppendFormat("<LastUpdatedDate><![CDATA[{0}]]></LastUpdatedDate>", Convert.ToDateTime(row["LastUpdatedDate"]).ToString("yyyy-MM-dd HH:mm"));
                    sb.AppendFormat("<Source><![CDATA[{0}]]></Source>", row["Source"].ToString());
                    //sb.AppendFormat("<ContentText><![CDATA[{0}]]></ContentText>", row["ContentText"].ToString());
                    sb.AppendFormat("<ContentText><![CDATA[{0}]]></ContentText>", r.Replace(row["ContentText"].ToString(), "$1$2src=\"" + _webSiteHost + "$3\" />"));
                    sb.AppendFormat("<Remark><![CDATA[{0}]]></Remark>", row["Remark"].ToString());
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }

        public string GetInformationAdInfo(string informationAdID)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                InformationAd bll = new InformationAd();
                DataSet ds = bll.GetModelOW(informationAdID);
                Regex r = new Regex("(<img)(.*)src=\"([^\"]*?)\"(.*)/>");

                sb.Append("<Rsp>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.AppendFormat("<InformationAdID><![CDATA[{0}]]></InformationAdID>", row["Id"].ToString());
                    sb.AppendFormat("<Title><![CDATA[{0}]]></Title>", row["Title"].ToString());
                    sb.AppendFormat("<Descr><![CDATA[{0}]]></Descr>", row["Descr"].ToString());
                    //sb.AppendFormat("<ContentText><![CDATA[{0}]]></ContentText>", row["ContentText"].ToString());
                    sb.AppendFormat("<ContentText><![CDATA[{0}]]></ContentText>", r.Replace(row["ContentText"].ToString(), "$1$2src=\"" + _webSiteHost + "$3\" />"));
                    sb.AppendFormat("<LastUpdatedDate><![CDATA[{0}]]></LastUpdatedDate>", Convert.ToDateTime(row["LastUpdatedDate"]).ToString("yyyy-MM-dd HH:mm"));
                   
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }
        #endregion

        #region 广告
        public string GetAdvertInfo()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                AdvertSubject bll = new AdvertSubject();
                ParamsHelper parms = new ParamsHelper();
                string sqlWhere = " and IsDisable=0";
                DataSet ds = bll.GetListOW(sqlWhere, null);

                sb.Append("<Rsp>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.AppendFormat("<ID>{0}</ID>", row["Id"].ToString());
                    sb.AppendFormat("<Title><![CDATA[{0}]]></Title>", row["Title"].ToString());
                    sb.AppendFormat("<PlayTime>{0}</PlayTime>", row["PlayTime"].ToString());

                    string oriImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/{1}{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string bImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_0{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string mImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_1{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                    string sImgUrl = row["PictureId"] is DBNull ? "" : _webSiteHost + string.Format("{0}{1}/Android/{1}_2{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);

                    sb.AppendFormat("<OriginalPicture><![CDATA[{0}]]></OriginalPicture>", oriImgUrl);
                    sb.AppendFormat("<BPicture><![CDATA[{0}]]></BPicture>", bImgUrl);
                    sb.AppendFormat("<MPicture><![CDATA[{0}]]></MPicture>", mImgUrl);
                    sb.AppendFormat("<SPicture><![CDATA[{0}]]></SPicture>", sImgUrl);
                }
                sb.Append("</Rsp>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return FormatStrReturn("-1", ex.Message);
            }
            return FormatStrReturn("0", sb.ToString());
        }
        #endregion

        #region 私有

        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        private Guid GetUserId(string loginId)
        {
            WebSecurityClient wsClient = new WebSecurityClient();
            var userId = wsClient.GetUserId(loginId);
            return userId == null ? Guid.Empty : Guid.Parse(userId.ToString());
        }

        /// <summary>
        /// 获取唯一随机数
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        private string GetRandomNumber(string prefix)
        {
            WebSecurityClient wsClient = new WebSecurityClient();
            return wsClient.GetRandomNumber(prefix);
        }

        #endregion

    }
}
