using System.Configuration;
using System.ServiceModel;
using System.ServiceProcess;
using TygaSoft.WcfService;

namespace TygaSoft.WcfWS
{
    public class Service : ServiceBase
    {
        ServiceHost teamHost = null;

        public Service()
        {
            ServiceName = "Hnztc.Team.Service";
        }

        public static void Main()
        {
            ServiceBase.Run(new Service());
        }

        protected override void OnStart(string[] args)
        {
            if (teamHost != null)
            {
                teamHost.Close();
            }

            teamHost = new ServiceHost(typeof(HnztcTeamService));

            teamHost.Open();
        }

        protected override void OnStop()
        {
            if (teamHost != null)
            {
                teamHost.Close();
                teamHost = null;
            }
           
        }
    }
}
