using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MWS.Helper;

namespace MWS.WindowsService
{
    /// <summary>
    /// Description of <see cref="RegistryConfigurator"/>
    /// </summary>
    public class RegistryConfigurator : IConfigurator
    {
        #region IConfigurator Membres
        private const string REGKEY_CONFIG = @"SOFTWARE\Movilitas\Movilizer\Service";

        private const string REGVAL_DB_DRIVER = @"Database Driver";
        private const string REGVAL_DB_PATH = @"Database Path";
        private const string REGVAL_DB_USER = @"Database User";
        private const string REGVAL_DB_PASSWORD = @"Database Password";
        private const string REGVAL_DB_PARAMETERS = @"Database Parameters";

        public string DatabaseDriver
        {
            get { return RegistryHelper.ReadString(REGKEY_CONFIG, REGVAL_DB_DRIVER); }
        }

        public string DatabasePath
        {
            get { return RegistryHelper.ReadString(REGKEY_CONFIG, REGVAL_DB_PATH); }
        }

        public string DatabaseUser
        {
            get { return RegistryHelper.ReadString(REGKEY_CONFIG, REGVAL_DB_USER, ""); }
        }

        public string DatabasePassword
        {
            get { return RegistryHelper.ReadString(REGKEY_CONFIG, REGVAL_DB_PASSWORD, ""); }
        }

        public string DatabaseParameters
        {
            get { return RegistryHelper.ReadString(REGKEY_CONFIG, REGVAL_DB_PARAMETERS, ""); }
        }

        private const string REGVAL_SYSTEM_ID = @"System ID";
        private const string REGVAL_SYSTEM_PASSWORD = @"System Password";
        private const string REGVAL_WEB_SERVICE_PROTOCOL = @"Web Service Protocol";
        private const string REGVAL_WEB_SERVICE_HOST = @"Web Service Host";
        private const string REGVAL_WEB_SERVICE_PROXY = @"Web Service Proxy";

        public long SystemId
        {
            get { return RegistryHelper.ReadInt(REGKEY_CONFIG, REGVAL_SYSTEM_ID); }
        }

        public string SystemPassword
        {
            get { return RegistryHelper.ReadString(REGKEY_CONFIG, REGVAL_SYSTEM_PASSWORD); }
        }

        public string WebServiceHost
        {
            get { return RegistryHelper.ReadString(REGKEY_CONFIG, REGVAL_WEB_SERVICE_HOST); }
        }

        public string WebServiceProtocol
        {
            get { return RegistryHelper.ReadString(REGKEY_CONFIG, REGVAL_WEB_SERVICE_PROTOCOL); }
        }

        public string WebServiceProxy
        {
            get { return RegistryHelper.ReadString(REGKEY_CONFIG, REGVAL_WEB_SERVICE_PROXY); }
        }

        private const string REGVAL_TIME_INTERVAL = @"Service Time Interval";
        private const string REGVAL_DEBUG_OUTPUT = @"Debug Output Path";

        public int ServiceTimeInterval
        {
            get { return RegistryHelper.ReadInt(REGKEY_CONFIG, REGVAL_TIME_INTERVAL, "60000"); }
        }

        public string DebugOutputPath
        {
            get { return RegistryHelper.ReadString(REGKEY_CONFIG, REGVAL_DEBUG_OUTPUT); }
        }

        public bool ForceRequeingOnError
        {
            get { return false; }
        }

        #endregion
    }
}
