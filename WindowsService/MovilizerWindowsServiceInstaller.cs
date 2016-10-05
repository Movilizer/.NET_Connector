using System.Configuration.Install;
using System.ServiceProcess; 

namespace MWS.WindowsService
{
    public class MovilizerWindowsServiceInstaller : Installer
    {
        public MovilizerWindowsServiceInstaller(string serviceId = "MWS", string serviceName = "MWS (Movilizer MWS)", string serviceDescription = "Movilzer .Net Connector ")
        {
            ServiceInstaller installer = new ServiceInstaller();
            installer.ServiceName = serviceId;
            installer.DisplayName = serviceName;
            installer.Description = serviceDescription;
            base.Installers.Add(installer);


            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;
            processInstaller.Password = null;
            processInstaller.Username = null;
            base.Installers.Add(processInstaller);
        }
    }
}