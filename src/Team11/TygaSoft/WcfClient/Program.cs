using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.WcfClient
{
    class Program
    {
        static void Main(string[] args)
        {
            int totalRecords = 0;

            #region 电子商务

            //ECShopClient shopClient = new ECShopClient();

            //string sGetCategoryTreeJson = shopClient.GetCategoryTreeJson();

            //string sGetBrandTreeJson = shopClient.GetBrandTreeJson();
            //string sGetBrandListByParentId = shopClient.GetBrandListByParentId("5FFF281C-BFAB-429D-88E2-F901DBDBE813");
            //string sGetBrandListByCategoryId = shopClient.GetBrandListByCategoryId("F3312568-F286-47DE-B575-B5385812D5FE");
            //string sGetProductDetail = shopClient.GetProductDetail(Guid.Parse("c3f0c0fb-b54f-4c84-bf50-2a32bf380132"));

            #region 商品相关

            //string sGetProductListByPage = shopClient.GetProductListByPage(out totalRecords,1,10);
            //string sGetProductListByCategory = shopClient.GetProductListByCategory(out totalRecords,1,10,Guid.Empty);
            //string GetProductListByBrand = shopClient.GetProductListByBrand(out totalRecords, 1, 10, Guid.Empty);
            //string GetProductListByMenu = shopClient.GetProductListByMenu(out totalRecords, 1, 10, Guid.Parse("6ba12a33-a572-424d-99f0-cd6316764898"));     //3B6A37FE-225E-49BD-A89F-07FD1109CC06

            #endregion

            //Console.WriteLine("sGetProductListByPage--" + sGetProductListByPage);

            #endregion

            #region 海南直通车

            HnztcClient hnztcClient = new HnztcClient();

            //Console.WriteLine("公告相关接口------------------------------------------");
            //var sGetAnnouncementList = hnztcClient.GetAnnouncementList(1, 10);
            //var sGetAnnouncementModel = hnztcClient.GetAnnouncementModel(Guid.Parse("22194a75-0d05-4b45-9fde-dc13f3da895a"));
            //Console.WriteLine("公告相关接口------------------------------------------");

            //Console.WriteLine("广告相关接口------------------------------------------");
            //var sGetSiteFunList = hnztcClient.GetSiteFunList();    //获取广告区列表 
            //var sGetAdvertisementList = hnztcClient.GetAdvertisementList(1, 10, Guid.Parse("6cc0b6e9-308a-4292-8af2-038b71613794"));    //获取当前广告区的所有广告
            //var sGetAdvertisementModel = hnztcClient.GetAdvertisementModel(Guid.Parse("11464a42-219b-4fa7-825c-5ccf49b240bc")); 

            //Console.WriteLine("公告相关接口------------------------------------------");

            //Console.WriteLine("服务相关接口------------------------------------------");
            //string sGetServiceList = hnztcClient.GetServiceList("283335746", 1, 10);
            //string sGetServiceListByServiceItemId = hnztcClient.GetServiceListByServiceItemId("283335746", 1, 10, Guid.Parse("DFE52420-DD38-4427-AB44-106F067CC83D"));

            //string sGetServiceVoteById = hnztcClient.GetServiceVoteById(Guid.Parse("3d295a81-f5f9-4f29-be12-e672156b1da9"));
            //string sGetServiceContentById = hnztcClient.GetServiceContentById(Guid.Parse("266386c6-1ee9-422d-8842-ea2d16a89312"));
            //string sGetServiceLinkById = hnztcClient.GetServiceLinkById(Guid.Parse("0cf233f8-d40f-499e-b3f0-194e522dca70"));
            //string sSaveServiceUserPraise = hnztcClient.SaveServiceUserPraise("283335746", Guid.Parse("5360AC13-8399-4DD7-909F-E0F9193F25A2"));

            //Console.WriteLine("商城菜单相关接口------------------------------------------");
            //string sGetShopMenuListByRoot = hnztcClient.GetShopMenuListByRoot(); //6ba12a33-a572-424d-99f0-cd6316764898
            //string sGetContentTypeChildListByParentId = hnztcClient.GetContentTypeChildListByParentId(Guid.Parse("53A15F04-B5DA-4A71-A1F9-1053CE830B2D"));

            //Console.WriteLine("用户基本信息相关接口------------------------------------------");

            ////头像上传接口
            //string fileName = @"E:\上传文件测试用\20150510222831_6719.jpg";
            //string imgBase64 = "";
            //using (var fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            //{
            //    var buffer = new byte[fs.Length];
            //    fs.Read(buffer, 0, (int)fs.Length);
            //    imgBase64 = Convert.ToBase64String(buffer);
            //}

            //imgBase64 = @"";

            //var sUpdateHeadPicture = hnztcClient.UpdateHeadPicture("13647544790", imgBase64, "20150510222831_6719.jpg");

            //修改昵称
            //var sUpdateUserBaseModel = hnztcClient.UpdateUserBaseModel("13647544790", "陈老大");

            Console.WriteLine("用户基本信息相关接口------------------------------------------");

            #endregion

            #region 安全服务

            WebSecurityClient wsClient = new WebSecurityClient();
            //string sRegister = wsClient.Register("User3", "123456","张三");
            string sLogin = wsClient.Login("13687590736", "999999");
            //var userId = wsClient.GetUserId("Manager");
            //string sGetUserInfo = wsClient.GetUserInfo("13647544790");

            //string sGetRandomNumber = wsClient.GetRandomNumber();

            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < 10000; i++)
            //{
            //    string s = wsClient.GetRandomNumber();
            //    sb.Append(s);
            //}
            //Console.WriteLine(sb.ToString());

            //string sUpdatePassword = wsClient.UpdatePassword("13687590736", "123456");
            //string sChangePassword = wsClient.ChangePassword("13687590736", "999999", "123456");

            //Console.WriteLine(sChangePassword);
            
            #endregion

            #region 系统日志服务

            HnztcSysClient sysClient = new HnztcSysClient();
            //TygaSoft.Services.HnztcSysService.SyslogInfo sysLogInfo = new Services.HnztcSysService.SyslogInfo();
            //sysLogInfo.AppName = "海南直通车系统日志服务";
            //sysLogInfo.MethodName = "TygaSoft.WcfClient.Main";
            //sysLogInfo.Message = "首次测试";
            //sysLogInfo.LastUpdatedDate = DateTime.Now;
            //sysClient.InsertSysLog(sysLogInfo);

            #endregion

            #region 消息队列服务

            UserBaseQueueClient ubQueueClient = new UserBaseQueueClient();
            ////金币、元宝、颜色、等级
            TygaSoft.Services.HnztcQueueService.UserLevelInfo userLevelInfo = new Services.HnztcQueueService.UserLevelInfo();
            userLevelInfo.UserId = Guid.Parse("60286733-AC08-4C0D-B800-B53E1A7DF01A");
            SysHelper.EnumHelper eh = new SysHelper.EnumHelper();
            var funCode = eh.GetValue(typeof(SysHelper.EnumData.FunCode), "fw", 0);
            userLevelInfo.FunCode = funCode;
            userLevelInfo.EnumSource = 0;
            userLevelInfo.IsReduce = false;
            userLevelInfo.TotalGold = 4;
            userLevelInfo.TotalSilver = 5;
            userLevelInfo.TotalIntegral = 2;
            ubQueueClient.SaveUserLevel(userLevelInfo);

            #endregion 

            Console.WriteLine("客户端调用执行完毕！");

            Console.WriteLine("按任意键结束程序");
            Console.ReadLine();
        }
    }
}
