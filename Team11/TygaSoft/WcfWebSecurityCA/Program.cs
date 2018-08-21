using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TygaSoft.WcfSecurityService;

namespace TygaSoft.WcfSecurityServiceCA
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost selfHost = new ServiceHost(typeof(WebSecurityService));

            try
            {
                selfHost.Open();
                Console.WriteLine("海南直通车安全服务（http://127.0.0.1:18881/Services/HnztcSecurityService）已启动就绪！");
                Console.WriteLine("终止服务请按任意键！");
                Console.WriteLine();
                Console.ReadLine();

                selfHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("海南直通车安全服务发生异常: {0}", ce.Message);
                selfHost.Abort();
            }
        }
    }
}
