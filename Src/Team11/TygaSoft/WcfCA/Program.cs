using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Configuration;
using System.ServiceModel;
using TygaSoft.WcfService;

namespace TygaSoft.WcfCA
{
    class Program
    {
        static void Main(string[] args)
        {
            string sysQueue = ConfigurationManager.AppSettings["SysQueue"];
            if (!MessageQueue.Exists(sysQueue))
                MessageQueue.Create(sysQueue, true);
            string userBaseQueue = ConfigurationManager.AppSettings["UserBaseQueue"];
            if (!MessageQueue.Exists(userBaseQueue))
                MessageQueue.Create(userBaseQueue, true);
            string accessStatisticQueue = ConfigurationManager.AppSettings["AccessStatisticQueue"];
            if (!MessageQueue.Exists(accessStatisticQueue))
                MessageQueue.Create(accessStatisticQueue, true);

            ServiceHost selfHost = new ServiceHost(typeof(HnztcService));
            ServiceHost shopSelfHost = new ServiceHost(typeof(ECShopService));
            ServiceHost securitySelfHost = new ServiceHost(typeof(WebSecurityService));
            ServiceHost syslogSelfHost = new ServiceHost(typeof(HnztcSysService));
            ServiceHost queueSelfHost = new ServiceHost(typeof(HnztcQueueService));

            try
            {
                selfHost.Open();
                shopSelfHost.Open();
                securitySelfHost.Open();
                syslogSelfHost.Open();
                queueSelfHost.Open();
                Console.WriteLine("海南直通车数据服务（直通车功能模块、商城、安全、系统日志、队列等）已启动就绪！");
                Console.WriteLine("http://x:18881/Services/ECShopService/");
                Console.WriteLine("http://x:18881/Services/HnztcService/");
                Console.WriteLine("http://x:18881/Services/HnztcSecurityService/");
                Console.WriteLine("http://x:18881/Services/HnztcSysService/");
                Console.WriteLine("http://x:18881/Services/HnztcQueueService/");
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("终止服务请按任意键！");
                Console.ReadLine();

                selfHost.Close();
                shopSelfHost.Close();
                securitySelfHost.Close();
                syslogSelfHost.Close();
                queueSelfHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("海南直通车数据服务（直通车功能模块、商城、系统日志、消息队列等）发生异常: {0}", ce.Message);
                selfHost.Abort();
                shopSelfHost.Abort();
                securitySelfHost.Abort();
                syslogSelfHost.Abort();
                queueSelfHost.Abort();
                Console.ReadLine();
            }
        }
    }
}
