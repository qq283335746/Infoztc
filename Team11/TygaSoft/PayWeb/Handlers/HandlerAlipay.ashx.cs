using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using TygaSoft.CustomExceptions;
using TygaSoft.SysHelper;
using TygaSoft.PayWebHelper;
using System.Collections.Specialized;

namespace TygaSoft.PayWeb.Handlers
{
    /// <summary>
    /// HandlerAlipay 的摘要说明
    /// </summary>
    public class HandlerAlipay : IHttpHandler
    {
        HttpContext context;

        //static string publicKey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";

        public void ProcessRequest(HttpContext context)
        {
            this.context = context;

            try
            {
                //var ssContent = @"discount=0.00&payment_type=1&subject=测试的商品&trade_no=2016032821001004820214603427&buyer_email=hdydf0714@sina.com&gmt_create=2016-03-28 18:48:23¬ify_type=trade_status_sync&quantity=1&out_trade_no=DD16-00113&seller_id=2088911994568903¬ify_time=2016-03-28 20:12:10&body=该测试商品的详细描述&trade_status=TRADE_SUCCESS&is_total_fee_adjust=N&total_fee=0.01&gmt_payment=2016-03-28 18:48:24&seller_email=ztc0898@163.com&price=0.01&buyer_id=2088202998437820¬ify_id=817f4f1c419e216f9a4fa235877da91mbu&use_coupon=N";
                //var ssArr = ssContent.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                //var sslen = ssArr.Length;
                //Array.Sort(ssArr);

                //var sContent = @"body=该测试商品的详细描述&buyer_email=hdydf0714@sina.com&buyer_id=2088202998437820&discount=0.00&gmt_create=2016-03-28 18:48:23&gmt_payment=2016-03-28 18:48:24&is_total_fee_adjust=N&notify_id=817f4f1c419e216f9a4fa235877da91mbu&notify_time=2016-03-28 20:12:10&notify_type=trade_status_sync&out_trade_no=DD16-00113&payment_type=1&price=0.01&quantity=1&seller_email=ztc0898@163.com&seller_id=2088911994568903&subject=测试的商品&total_fee=0.01&trade_no=2016032821001004820214603427&trade_status=TRADE_SUCCESS&use_coupon=N";
                //var arr = sContent.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                //var len = arr.Length;

                ////var sssSign1 = RSAFromPkcs8.Sign(sContent, privateKey, "");

                //var sssSign = @"RC/BSb2XR/o9ZJ+VxImK8i/1t2wTuvbvwKQIfEQzWic4pxjleiHn3L99fcHU+OpeOIsbgI9g6SLJvKZF/7DTd+TQ5jWqWRmPR+6roIO0b1/h/criVrThPnb3oGMx53qJWr7rhkvJYZgtu5+WR5iC53CCqFDkgkC/mgSvwagbk/o=";
                //var isV = RSAFromPkcs8.verify(sContent, sssSign, publicKey, "");
                //////var isp = sssSign1 == sssSign;

                //return;

                if (context.Request.HttpMethod.ToUpper() != "POST") return;

                SaveRequest();
 
                #region 支付宝交易通知：服务器异步通知 商户通知参数合法性验证：1、验证签名 2、验证是否是支付宝发来的通知

                SortedDictionary<string, string> sPara = GetRequestPost();

                if (sPara.Count == 0)
                {
                    return;
                }

                Notify aliNotify = new Notify();
                bool isVerify = aliNotify.Verify(sPara, context.Request.Form["notify_id"], context.Request.Form["sign"]);

                if (!isVerify)
                {
                    context.Response.Write("fail");
                    return;
                }

                //if (!RSAFromPkcs8.verify(content.ToString().Trim('&'), sSign, publicKey, ""))
                //{
                //    logInfo.Append("，验证签名不通过");
                //    new CustomException(logInfo.ToString());
                //    return;
                //}

                var statusCode = -1;
                var result = string.Empty;

                //var isFromAlipayUrl = "https://mapi.alipay.com/gateway.do?service=notify_verify";
                //HttpHelper.DoHttpPost(isFromAlipayUrl, string.Format(@"partner={0}&notify_id={1}", sellerId, notifyId),out statusCode,out result);
                //if (statusCode != 200)
                //{
                //    logInfo.AppendFormat(@"来自支付宝支付页记录信息：{0}", "”验证是否是支付宝发来的通知“请求失败，原因：" + result + "");
                //    new CustomException(logInfo.ToString());
                //    return;
                //}
                //if (result.ToLower() != "true")
                //{
                //    logInfo.AppendFormat(@"来自支付宝支付页记录信息：{0}", "验证是否是支付宝发来的通知未通过，原因：" + result + "");
                //    new CustomException(logInfo.ToString());
                //    return;
                //}

                #endregion

                var tradeStatus = context.Request.Form["trade_status"].ToUpper();
                if (tradeStatus == "TRADE_SUCCESS" || tradeStatus == "TRADE_FINISHED")
                {
                    var orderCode = context.Request.Form["out_trade_no"];
                    var payPrice = context.Request.Form["total_fee"];
                    var payMode = 1;

                    //var orderCode = "DD16-00112";
                    //var payPrice = decimal.Parse("0.01");
                    //var payMode = 1;

                    var siteUrl = string.Format(@"http://112.74.86.148:8080/changhe/XBM_Service.bsp?Page&BizzID=2&PageID=43", orderCode, payPrice, payMode);
                    var postContent = string.Format(@"OrderID={0}&PaySum={1}&PayStyleID={2}", orderCode, payPrice, payMode);

                    HttpHelper.DoHttpPost(siteUrl, postContent, out statusCode, out result);
                    if (statusCode != 200)
                    {
                        new CustomException(string.Format(@"来自支付宝支付页记录信息：在调用商户URL时请求失败，原因：{0}", result));
                    }
                    if (result.ToLower() != "success")
                    {
                        new CustomException(string.Format(@"来自支付宝支付页记录信息：在调用商户URL时返回结果：{0}", result));
                    }
                }

                context.Response.Write("success");
            }
            catch (Exception ex)
            {
                new CustomException(string.Format(@"来自支付宝支付页异常信息：{0}",ex.Message),ex);
                ResResult(false, ex.Message, "");
            }
        }

        #region 私有

        private void SaveRequest()
        {
            var sb = new StringBuilder(2000);
            using (StreamReader sr = new StreamReader(context.Request.InputStream))
            {
                while (sr.Peek() >= 0)
                {
                    sb.Append(sr.ReadLine());
                }
            }

            ReqHelper rh = new ReqHelper();
            var sClient = rh.GetHttpClientInfo(context);

            var sReqText = sb.ToString();
            if (string.IsNullOrWhiteSpace(sReqText))
            {
                return;
            }
            new CustomException(string.Format(@"来自支付宝支付页记录信息：请求方式：{0}，请求数据：{1}，关于客户端信息：{2}",
                context.Request.HttpMethod.ToUpper(), HttpUtility.UrlDecode(sReqText), sClient));
        }

        private void ResResult(bool isOk, string msg, object data)
        {
            ResResult rr = new ResResult();
            context.Response.ContentType = "text/plain";
            context.Response.Write(rr.ResJsonString(isOk, msg, data));
        }

        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection nvc = context.Request.Form;

            string[] items = nvc.AllKeys;

            for (i = 0; i < items.Length; i++)
            {
                sArray.Add(items[i], nvc[items[i]]);
            }

            return sArray;
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