using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace TygaSoft.WcfWS
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ProjectInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.ServiceName = "Hnztc.Team.Service";
            service.Description = "海南直通车（团队），接口等服务。技术支持：天涯孤岸，QQ283335746";
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
