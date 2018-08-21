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
            service.ServiceName = "Hnztc.Service";
            service.Description = "海南直通车，系统、电子商城、队列等服务。技术支持：天涯孤岸，QQ283335746";
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
