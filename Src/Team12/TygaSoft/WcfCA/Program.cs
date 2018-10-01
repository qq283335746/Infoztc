using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TygaSoft.WcfService;

namespace TygaSoft.WcfCA
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost teamSelfHost = new ServiceHost(typeof(HnztcTeamService));

            try
            {
                teamSelfHost.Open();
                Console.WriteLine("海南直通车数据服务（团队版）已启动就绪！");
                Console.WriteLine("http://xxx:18881/Services/HnztcTeamService/");
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("终止服务请按任意键！");
                Console.WriteLine();
                Console.ReadLine();

                teamSelfHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("海南直通车数据服务（团队版）发生异常: {0}", ce.Message);
                teamSelfHost.Abort();
            }
        }
    }
}
