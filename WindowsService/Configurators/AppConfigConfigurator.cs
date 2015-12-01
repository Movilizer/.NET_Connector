using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using MWS.Helper;

namespace MWS.WindowsService
{
    /// <summary>
    /// Description of <see cref="AppConfigConfigurator"/>
    /// </summary>
    public class AppConfigConfigurator : IConfigurator
    {
        #region IConfigurator Membres

        public string DatabaseDriver
        {
            get { return ConfigurationManager.AppSettings["Database Driver"]; }
        }

        public string DatabasePath
        {
            get { return ConfigurationManager.AppSettings["Database Path"]; }
        }

        public string DatabaseUser
        {
            get { return ConfigurationManager.AppSettings["Database User"]; }
        }

        public string DatabasePassword
        {
            get { return ConfigurationManager.AppSettings["Database Password"]; }
        }

        public string DatabaseParameters
        {
            get { return ConfigurationManager.AppSettings["Database Parameters"]; }
        }

        public long SystemId
        {
            get { return long.Parse(ConfigurationManager.AppSettings["System ID"]); }
        }

        public string SystemPassword
        {
            get { return ConfigurationManager.AppSettings["System Password"]; }
        }

        public string WebServiceHost
        {
            get { return ConfigurationManager.AppSettings["Web Service Host"]; }
        }

        public string WebServiceProtocol
        {
            get { return ConfigurationManager.AppSettings["Web Service Protocol"]; }
        }

        public string WebServiceProxy
        {
            get { return ConfigurationManager.AppSettings["Database Driver"]; }
        }

        public int ServiceTimeInterval
        {
            get { return int.Parse(ConfigurationManager.AppSettings["Service Time Interval"]); }
        }

        public string DebugOutputPath
        {
            get { return ConfigurationManager.AppSettings["Debug Output Path"]; }
        }

        public bool ForceRequeingOnError
        {
            get { return false; }
        }

        #endregion
    }
}
