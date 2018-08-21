using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WcfClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //HnztcTeamClient teamClient = new HnztcTeamClient();

            #region 知识竞猜测试

            //string sGetQuestionList = teamClient.GetQuestionList("Manager");
            //string sGetQXCLotteryInfo = teamClient.GetQXCLotteryInfo("Manager");

            //继续添加你要测试的回调方法
            //string sGetTopicList = teamClient.GetTopicInfo("D3F0FE03-3507-4033-9D5A-1A8732CEB86E");

            #endregion

            #region 彩票

            #endregion

            #region 摇奖

            //string sIsExistErnieLatest = teamClient.IsExistErnieLatest();

            #endregion

            #region 用户信息

            Console.WriteLine("用户基本信息相关接口------------------------------------------");

            /*

            //头像上传接口
            string filePath = @"D:\2.jpg";
            string fileName = Path.GetFileName(filePath);
            string imgBase64 = "";
            using (var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                imgBase64 = Convert.ToBase64String(buffer);
            }

            //imgBase64 = @"";

            var sUpdateHeadPicture = teamClient.UpdateHeadPicture("13647544790", imgBase64, fileName);

            //修改昵称
            //var sUpdateUserBaseModel = teamClient.UpdateUserBaseModel("13647544790", "陈老大");

             */

            Console.WriteLine("用户基本信息相关接口------------------------------------------");

            #endregion 

            #region 安全服务

            /*

            WebSecurityClient wsClient = new WebSecurityClient();
            string sGetRandomNumber = wsClient.GetRandomNumber();

             */

            #endregion

            #region 消息队列服务

            UserBaseQueueClient ubQueueClient = new UserBaseQueueClient();
            //金币、元宝、颜色、等级
            TygaSoft.Services.HnztcQueueService.UserLevelInfo userLevelInfo = new TygaSoft.Services.HnztcQueueService.UserLevelInfo();
            userLevelInfo.UserId = Guid.Parse("60286733-AC08-4C0D-B800-B53E1A7DF01A");
            userLevelInfo.IsReduce = false;
            userLevelInfo.TotalGold = 4;
            userLevelInfo.TotalSilver = 5;
            userLevelInfo.TotalIntegral = 2;
            ubQueueClient.SaveUserLevel(userLevelInfo);

            #endregion 
        }
    }
}
