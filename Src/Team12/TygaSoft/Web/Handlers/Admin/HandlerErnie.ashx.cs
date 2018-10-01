using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;
using System.Threading;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Handlers.Admin
{
    /// <summary>
    /// HandlerErnie 的摘要说明
    /// </summary>
    public class HandlerErnie : IHttpHandler
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
                        reqName = context.Request.QueryString["reqName"].Trim();
                        break;
                    case "POST":
                        reqName = context.Request.Form["reqName"].Trim();
                        break;
                    default:
                        break;
                }

                switch (reqName)
                {
                    case "SaveErnie":
                        SaveErnie(context);
                        break;
                    case "SaveErnieItem":
                        SaveErnieItem(context);
                        break;
                    case "GetErnieItemForDatagrid":
                        GetErnieItemForDatagrid(context);
                        break;
                    case "GetLatestForBet":
                        GetLatestForBet(context);
                        break;
                    case "GetBetResult":
                        GetBetResult(context);
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
                context.Response.ContentType = "text/plain";
                context.Response.Write("{\"success\": false,\"message\": \"" + msg + "\"}");
            }
        }

        private void SaveErnie(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            Guid Id = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(context.Request.Form["Id"])) Guid.TryParse(context.Request.Form["Id"], out Id);

            if (string.IsNullOrWhiteSpace(context.Request.Form["startTime"]))
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                return;
            }
            DateTime startTime = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(context.Request.Form["startTime"]))
            {
                DateTime.TryParse(context.Request.Form["startTime"], out startTime);
            }
            if (startTime == DateTime.MinValue)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                return;
            }
            DateTime endTime = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(context.Request.Form["endTime"]))
            {
                DateTime.TryParse(context.Request.Form["endTime"], out endTime);
            }
            if (endTime == DateTime.MinValue)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                return;
            }

            if (startTime >= endTime)
            {
                context.Response.Write("{\"success\": false,\"message\": \"结束时间必须大于开始时间，请正确操作\"}");
                return;
            }

            int userBetMaxCount = 0;
            if (!string.IsNullOrWhiteSpace(context.Request.Form["userBetMaxCount"]))
            {
                int.TryParse(context.Request.Form["userBetMaxCount"], out userBetMaxCount);
            }

            bool isDisable = false;
            bool.TryParse(context.Request.Form["isDisable"], out isDisable);

            ErnieInfo model = new ErnieInfo();
            model.Id = Id;
            model.LastUpdatedDate = DateTime.Now;
            model.StartTime = startTime;
            model.UserBetMaxCount = userBetMaxCount;
            model.EndTime = endTime;
            model.IsDisable = isDisable;

            Ernie bll = new Ernie();
            int effect = -1;

            if (!Id.Equals(Guid.Empty))
            {
                var oldModel = bll.GetModel(Id);
                if (oldModel == null)
                {
                    throw new ArgumentException(MessageContent.Submit_Data_NotExists);
                }
                model.IsOver = oldModel.IsOver;
                effect = bll.Update(model);
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (bll.IsExistLatest())
                    {
                        throw new ArgumentException("已存在摇奖设置且未结束，无法再添加摇奖设置！");
                    }
                    else
                    {
                        model.IsOver = false;
                        Id = Guid.NewGuid();
                        model.Id = Id;
                        effect = bll.Insert(model);
                    }

                    scope.Complete();
                }
            }

            context.Response.Write("{\"success\": true,\"message\": \"" + Id + "\"}");
        }

        private void SaveErnieItem(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            Guid Id = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(context.Request.Form["Id"])) Guid.TryParse(context.Request.Form["Id"], out Id);
            Guid ernieId = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(context.Request.Form["ErnieId"])) Guid.TryParse(context.Request.Form["ErnieId"], out ernieId);
            if (ernieId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"success\": false,\"message\": \"请先完成基本信息再执行此操作！\"}");
                return;
            }

            string numType = string.IsNullOrWhiteSpace(context.Request.Form["NumType"]) ? "" : context.Request.Form["NumType"].Trim();
            if (string.IsNullOrWhiteSpace(numType))
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                return;
            }
            string num = string.IsNullOrWhiteSpace(context.Request.Form["Num"]) ? "" : context.Request.Form["Num"].Trim().Trim(',');
            if (string.IsNullOrWhiteSpace(num))
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                return;
            }
            float appearRatio = 0;
            if (!string.IsNullOrWhiteSpace(context.Request.Form["AppearRatio"]))
            {
                float.TryParse(context.Request.Form["AppearRatio"], out appearRatio);
            }
            
            ErnieItemInfo model = new ErnieItemInfo();
            model.Id = Id;
            model.ErnieId = ernieId;
            model.NumType = numType;
            model.Num = num;
            model.AppearRatio = appearRatio;

            ErnieItem bll = new ErnieItem();
            int effect = -1;

            if (!Id.Equals(Guid.Empty))
            {
                effect = bll.Update(model);
            }
            else
            {
                effect = bll.Insert(model);
            }

            if (effect < 1)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_InvalidError + "\"}");
                return;
            }

            context.Response.Write("{\"success\": true,\"message\": \"" + Id + "\"}");
        }

        private void GetErnieItemForDatagrid(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            Guid ernieId = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(context.Request.QueryString["ernieId"]))
            {
                Guid.TryParse(context.Request.QueryString["ernieId"], out ernieId);
            }
            if (ernieId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }

            int totalRecords = 0;
            int pageIndex = 1;
            int pageSize = 10;
            if (!string.IsNullOrWhiteSpace(context.Request.Form["page"]))
            {
                int.TryParse(context.Request.Form["page"], out pageIndex);
            }
            if (!string.IsNullOrWhiteSpace(context.Request.Form["rows"]))
            {
                int.TryParse(context.Request.Form["rows"], out pageSize);
            }
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 10) pageSize = 10;

            ErnieItem bll = new ErnieItem();

            string sqlWhere = "and ErnieId = @ErnieId ";
            SqlParameter parm = new SqlParameter("@ErnieId", SqlDbType.UniqueIdentifier);
            parm.Value = ernieId;
            var list = bll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parm);
            if (list == null || list.Count == 0)
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }

            StringBuilder sb = new StringBuilder(1000);
            foreach (var model in list)
            {
                sb.Append("{\"Id\":\"" + model.Id + "\",\"ErnieId\":\"" + model.ErnieId + "\",\"NumType\":\"" + model.NumType + "\",\"Num\":\"" + model.Num + "\",\"AppearRatio\":\"" + Math.Round((double)model.AppearRatio,2) + "\"},");
            }
            context.Response.Write("{\"total\":" + totalRecords + ",\"rows\":[" + sb.ToString().Trim(',') + "]}");
        }

        private void GetLatestForBet(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            var list = ErnieDataProxy.GetLatest();
            if (list != null && list.Count > 0)
            {
                var model = list[0];
                if ((DateTime.Now >= model.StartTime) && (DateTime.Now < model.EndTime))
                {
                    context.Response.Write("{\"success\": true,\"message\": \"\",\"startTime\": \"" + model.StartTime.ToString("yyyy-MM-dd HH:mm") + "\",\"endTime\": \"" + model.EndTime.ToString("yyyy-MM-dd HH:mm") + "\",\"totalMls\": \"0\",\"status\": \"1\"}");
                    return;
                }
                if (DateTime.Now < model.StartTime)
                {
                    context.Response.Write("{\"success\": true,\"message\": \"\",\"startTime\": \"" + model.StartTime.ToString("yyyy-MM-dd HH:mm") + "\",\"endTime\": \"" + model.EndTime.ToString("yyyy-MM-dd HH:mm") + "\",\"totalMls\": \"" + (model.StartTime - DateTime.Now).TotalMilliseconds + "\",\"status\": \"0\"}");
                    return;
                }
                if (DateTime.Now > model.EndTime)
                {
                    context.Response.Write("{\"success\": true,\"message\": \"\",\"startTime\": \"" + model.StartTime.ToString("yyyy-MM-dd HH:mm") + "\",\"endTime\": \"" + model.EndTime.ToString("yyyy-MM-dd HH:mm") + "\",\"totalMls\": \"0\",\"status\": \"100\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"\",\"startTime\": \"" + model.StartTime.ToString("yyyy-MM-dd HH:mm") + "\",\"endTime\": \"" + model.EndTime.ToString("yyyy-MM-dd HH:mm") + "\",\"totalMls\": \"0\",\"status\": \"100\"}");
                return;
            }
            else
            {
                context.Response.Write("{\"success\": true,\"message\": \"\",\"startTime\": \"\",\"endTime\": \"\",\"totalMls\": \"-1\",\"status\": \"100\"}");
            }
        }

        private void GetBetResult(HttpContext context)
        {
            var list = ErnieDataProxy.GetLatest();
            if (list == null || list.Count == 0)
            {
                int index = 0;
                while (true)
                {
                    Thread.Sleep(5000);
                    list = ErnieDataProxy.GetLatest();
                    if (list.Count > 0) break;
                    index++;
                    if (index > 5) break;
                }
            }
            if (list != null && list.Count > 0)
            {
                var ernieModel = list[0];
                if (!((DateTime.Now >= ernieModel.StartTime) && (DateTime.Now <= ernieModel.EndTime)))
                {
                    context.Response.Write("{\"success\": true,\"message\": \"\",\"gold\": \"0\",\"silver\": \"0\",\"times\": \"0\"}");
                    return;
                }

                var userId = WebCommon.GetUserId();
                if (userId.Equals(Guid.Empty))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"请先登录\",\"gold\": \"0\",\"silver\": \"0\",\"times\": \"0\"}");
                    return;
                }

                UserErnie ueBll = new UserErnie();
                Dictionary<string, string> dic = new Dictionary<string, string>();

                var listT = list.ToList();
                var g = listT.GroupBy(m => m.NumType);
                foreach (var gk in g)
                {
                    var keyList = listT.FindAll(m => m.NumType == gk.Key);
                    var ga = keyList.GroupBy(m => m.AppearRatio);

                    GLBfb[] arrGLBfb = new GLBfb[ga.Count()];
                    int i = 0;
                    foreach(var gak in ga)
                    {
                        arrGLBfb[i] = new GLBfb();
                        arrGLBfb[i].Bfb = (int)(gak.Key * 100);
                        var currList = keyList.FindAll(m => m.AppearRatio == gak.Key);
                        foreach (var model in currList)
                        {
                            var numArr = model.Num.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var num in numArr)
                            {
                                arrGLBfb[i].SjsList.Add(num);
                            }
                        }
                        i++;
                    }

                    RandomForWeight rdfw = new RandomForWeight(arrGLBfb);

                    dic.Add(gk.Key, rdfw.GetGLNumber());
                    
                }

                string gold = "0";
                string silver = "0";
                string times = "0";
                foreach (KeyValuePair<string, string> kvp in dic)
                {
                    switch(kvp.Key)
                    {
                        case "倍数":
                            times = kvp.Value;
                            break;
                        case "金币":
                            gold = kvp.Value;
                            break;
                        case "元宝":
                            silver = kvp.Value;
                            break;
                        default:
                            break;
                    }
                }

                int remainTimes = 0;
                using(TransactionScope scope = new TransactionScope())
                {
                    var totalBetCount = ueBll.GetTotalBetCount(userId, ernieModel.ErnieId);
                    remainTimes = ernieModel.UserBetMaxCount - totalBetCount;
                    if(remainTimes < 0) totalBetCount = 0;
                    if (remainTimes < 1)
                    {
                        context.Response.Write("{\"success\": false,\"message\": \"摇奖机会还剩 " + 0 + " 次\",\"gold\": \"0\",\"silver\": \"0\",\"times\": \"0\",\"remainTimes\":\"0\"}");
                        return;
                    }

                    UserErnieInfo ueModel = new UserErnieInfo();
                    ueModel.UserId = userId;
                    ueModel.ErnieId = ernieModel.ErnieId;
                    ueModel.LastUpdatedDate = DateTime.Now;
                    ueModel.WinGold = int.Parse(gold) * int.Parse(times);
                    ueModel.WinSilver = int.Parse(silver) * int.Parse(times);

                    ueBll.Insert(ueModel);

                    UserBaseQueueClient ubqClient = new UserBaseQueueClient();
                    TygaSoft.Services.HnztcQueueService.UserLevelInfo userLevelInfo = new TygaSoft.Services.HnztcQueueService.UserLevelInfo();
                    userLevelInfo.UserId = userId;
                    userLevelInfo.IsReduce = false;
                    userLevelInfo.TotalGold = ueModel.WinGold;
                    userLevelInfo.TotalSilver = ueModel.WinSilver;
                    userLevelInfo.TotalIntegral = 0;
                    ubqClient.SaveUserLevel(userLevelInfo);

                    scope.Complete();

                    remainTimes = remainTimes - 1;
                }

                context.Response.Write("{\"success\": true,\"message\": \"摇奖机会还剩 " + remainTimes + " 次\",\"gold\": \"" + gold + "\",\"silver\": \"" + silver + "\",\"times\": \"" + times + "\",\"remainTimes\":\"" + remainTimes + "\"}");
            }
            else
            {
                context.Response.Write("{\"success\": true,\"message\": \"\",\"gold\": \"0\",\"silver\": \"0\",\"times\": \"0\"}");
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