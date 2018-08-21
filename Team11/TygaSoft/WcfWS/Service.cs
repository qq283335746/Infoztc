using System.Configuration;
using System.Messaging;
using System.ServiceModel;
using System.ServiceProcess;
using TygaSoft.WcfService;

namespace TygaSoft.WcfWS
{
    public class Service : ServiceBase
    {
        ServiceHost hnztcHost = null;
        ServiceHost ecshopHost = null;
        ServiceHost securityHost = null;
        ServiceHost sysHost = null;
        ServiceHost queueHost = null;

        public Service()
        {
            ServiceName = "Hnztc.Service";
        }

        public static void Main()
        {
            ServiceBase.Run(new Service());
        }

        protected override void OnStart(string[] args)
        {
            if (hnztcHost != null)
            {
                hnztcHost.Close();
            }
            if (ecshopHost != null)
            {
                ecshopHost.Close();
            }
            if (securityHost != null)
            {
                securityHost.Close();
            }
            if (sysHost != null)
            {
                sysHost.Close();
            }
            if (queueHost != null)
            {
                queueHost.Close();
            }

            #region 检查并创建 microsoft 消息队列

            string sysQueue = ConfigurationManager.AppSettings["SysQueue"];
            if (!MessageQueue.Exists(sysQueue))
                MessageQueue.Create(sysQueue, true);
            string userBaseQueue = ConfigurationManager.AppSettings["UserBaseQueue"];
            if (!MessageQueue.Exists(userBaseQueue))
                MessageQueue.Create(userBaseQueue, true);
            string accessStatisticQueue = ConfigurationManager.AppSettings["AccessStatisticQueue"];
            if (!MessageQueue.Exists(accessStatisticQueue))
                MessageQueue.Create(accessStatisticQueue, true);

            #endregion

            hnztcHost = new ServiceHost(typeof(HnztcService));
            ecshopHost = new ServiceHost(typeof(ECShopService));
            securityHost = new ServiceHost(typeof(WebSecurityService));
            sysHost = new ServiceHost(typeof(HnztcSysService));
            queueHost = new ServiceHost(typeof(HnztcQueueService));

            hnztcHost.Open();
            ecshopHost.Open();
            securityHost.Open();
            sysHost.Open();
            queueHost.Open();
        }

        protected override void OnStop()
        {
            if (hnztcHost != null)
            {
                hnztcHost.Close();
                hnztcHost = null;
            }
            if (ecshopHost != null)
            {
                ecshopHost.Close();
                ecshopHost = null;
            }
            if (securityHost != null)
            {
                securityHost.Close();
                securityHost = null;
            }
            if (sysHost != null)
            {
                sysHost.Close();
                sysHost = null;
            }
            if (queueHost != null)
            {
                queueHost.Close();
                queueHost = null;
            }
        }
    }
}
