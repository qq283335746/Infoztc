using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.ServiceModel.Activation;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using TygaSoft.SysHelper;

namespace TygaSoft.WcfService
{
    public class AlipayService : IAlipay
    {
        static readonly string payConfigPath = ConfigHelper.GetAppSetting("PayXml");

        #region 私有

        string appCode = "Hnztc";

        private string GetSign(Dictionary<string, string> dic)
        {
            Dictionary<string, string> newDic = new Dictionary<string, string>();

            foreach (var kvp in dic)
            {
                if (string.IsNullOrWhiteSpace(kvp.Key) || string.IsNullOrWhiteSpace(kvp.Value)) continue;
                newDic.Add(kvp.Key, kvp.Value);
            }

            var keys = newDic.Keys.ToArray();
            Array.Sort(keys);

            StringBuilder sb = new StringBuilder();
            int index = 0;
            foreach (var key in keys)
            {
                if (key == "sign" || key == "sign_type") continue;
                if (index > 0) sb.Append("&");
                sb.AppendFormat("{0}={1}", key, newDic[key]);
                index++;
            }
            //sb.AppendFormat("&key={0}", GetSignKey());

            return FormsAuthentication.HashPasswordForStoringInConfigFile(sb.ToString(), "MD5").ToUpper();
        }

        private string[] GetAlipayRsaKey()
        {
            var xelRoot = GetRootElement();
            var xelRsa = xelRoot.Descendants("Rsa").FirstOrDefault();
            if(xelRsa == null) throw new ArgumentException("未找到配置节点Rsa");
            var items = new string[2];
            items[0] = xelRsa.Element("PrivateKey").Value.Trim();
            items[1] = xelRsa.Element("PublicKey").Value.Trim();

            return items;
        }

        private XElement GetRootElement()
        {
            var xel = XElement.Load(payConfigPath);
            var q = xel.Descendants("Data").FirstOrDefault(x => x.Attribute("AppCode").Value == appCode && x.Descendants().Any(a=>a.Attribute("IsDefault").Value == "1"));

            if (q == null) throw new ArgumentException("未找到相关的支付配置启用节点");

            return q;
        }

        #endregion
    }
}
