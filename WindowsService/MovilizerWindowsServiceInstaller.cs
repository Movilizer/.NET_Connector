using System.Configuration.Install;
using System.ServiceProcess; 


namespace MWS.WindowsService
{
    public class MovilizerWindowsServiceInstaller : Installer
    {
        public MovilizerWindowsServiceInstaller()
        {
            ServiceInstaller installer = new ServiceInstaller();
            installer.ServiceName = GetServiceName();
            installer.DisplayName = GetDisplayName();
            base.Installers.Add(installer);


            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;
            processInstaller.Password = null;
            processInstaller.Username = null;
            base.Installers.Add(processInstaller);
        }

        protected string GetServiceName()
        {
            return "MWS";
        }

        protected string GetDisplayName()
        {
            return "MWS (Movilizer Windows Service)";
        }
    }
}
