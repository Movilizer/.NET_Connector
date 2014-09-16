using System;
using System.Security.Cryptography;

using MWS.Helper;

namespace MWS.WindowsService
{
    public static class Configuration
    {
        private const string REGKEY_CONFIG = @"SOFTWARE\Movilitas\Movilizer\Service";


        #region Database Specific Settings

        private const string REGVAL_DB_DRIVER = @"Database Driver";
        private const string REGVAL_DB_PATH = @"Database Path";
        private const string REGVAL_DB_USER = @"Database User";
        private const string REGVAL_DB_PASSWORD = @"Database Password";
        private const string REGVAL_DB_PARAMETERS = @"Database Parameters";


        private static string _databaseDriver;
        private static string _databasePath;
        private static string _databaseUser;
        private static string _databasePassword;
        private static string _databaseParameters;


        public static string GetDatabaseDriver()
        {
            return _databaseDriver;
        }

        public static string GetDatabasePath()
        {
            return _databasePath;
        }

        public static string GetDatabaseUser()
        {
            return _databaseUser;
        }

        public static string GetDatabasePassword()
        {
            return _databasePassword;
        }

        public static string GetDatabaseParameters()
        {
            return _databaseParameters;
        }

        #endregion


        #region Movilizer Web Service Specific Settings

        private const string REGVAL_SYSTEM_ID = @"System ID";
        private const string REGVAL_SYSTEM_PASSWORD = @"System Password";
        private const string REGVAL_ENCRYPTION_ALGORITHM = @"Encryption Algorithm";
        private const string REGVAL_ENCRYPTION_PASSWORD = @"Encryption Password";
        private const string REGVAL_WEB_SERVICE_HOST = @"Web Service Host";
        private const string REGVAL_WEB_SERVICE_PROXY = @"Web Service Proxy";


        private static long _systemId;
        private static string _systemPassword;
        private static string _encryptionAlgorithm;
        private static string _encryptionPassword;
        private static string _webServiceHost;
        private static string _webServiceProxy;


        public static long GetSystemId()
        {
            return _systemId;
        }

        public static string GetSystemPassword()
        {
            return _systemPassword;
        }

        public static string GetEncryptionAlgorithm()
        {
            return _encryptionAlgorithm;
        }

        public static string GetEncryptionPassword()
        {
            return _encryptionPassword;
        }

        public static string GetWebServiceHost()
        {
            return _webServiceHost;
        }

        public static string GetWebServiceProxy()
        {
            return _webServiceProxy;
        }

        #endregion


        #region Windows Server Specific Settings

        private const string REGVAL_TIME_INTERVAL = @"Service Time Interval";        
        private const string REGVAL_DEBUG_OUTPUT = @"Debug Output Path";


        private static int _serviceTimeInterval;        
        private static string _debugOutputPath;


        public static int GetServiceTimeInterval()
        {
            return _serviceTimeInterval;
        }

        public static string GetDebugOutputPath()
        {
            return _debugOutputPath;
        }

        #endregion


        static Configuration()
        {
            ReadConfigurationFromRegistry();
        }

        public static void ReadConfigurationFromRegistry()
        {
            try
            {
                // database specific settings
                _databaseDriver         = ReadString(REGVAL_DB_DRIVER);
                _databasePath           = ReadString(REGVAL_DB_PATH);
                _databaseUser           = ReadString(REGVAL_DB_USER, "");
                _databasePassword       = ReadString(REGVAL_DB_PASSWORD, _databaseUser);
                _databaseParameters     = ReadString(REGVAL_DB_PARAMETERS, "");

                // movilizer web service specific settings
                _systemId               = ReadInt(REGVAL_SYSTEM_ID);
                _systemPassword         = ReadString(REGVAL_SYSTEM_PASSWORD, "" + _systemId);

                // encryption parameters
                _encryptionAlgorithm    = ReadString(REGVAL_ENCRYPTION_ALGORITHM);
                _encryptionPassword     = ReadString(REGVAL_ENCRYPTION_PASSWORD, _encryptionAlgorithm);

                // connection settings
                _webServiceHost         = ReadString(REGVAL_WEB_SERVICE_HOST);
                _webServiceProxy        = ReadString(REGVAL_WEB_SERVICE_PROXY);

                // windows service settings
                _serviceTimeInterval    = ReadInt(REGVAL_TIME_INTERVAL, "60000");                
                _debugOutputPath        = ReadString(REGVAL_DEBUG_OUTPUT);
            }
            catch (Exception e)
            {
                throw e;
            }         
        }

        #region Local Registry Wrapper Methods

        private static string ReadString(string name)
        {
            return RegistryHelper.ReadString(REGKEY_CONFIG, name);
        }

        private static string ReadProtectedString(string name, string entropy)
        {
            return RegistryHelper.ReadProtectedString(REGKEY_CONFIG, name, entropy);
        }

        private static string ReadProtectedString(string name, string defaultValue, string entropy)
        {
            return RegistryHelper.ReadProtectedString(REGKEY_CONFIG, name, defaultValue, entropy);
        }

        private static string ReadString(string name, string defaultValue)
        {
            return RegistryHelper.ReadString(REGKEY_CONFIG, name, defaultValue);
        }

        private static int ReadInt(string name)
        {
            return RegistryHelper.ReadInt(REGKEY_CONFIG, name);
        }

        private static int ReadInt(string name, string defaultValue)
        {
            return RegistryHelper.ReadInt(REGKEY_CONFIG, name, defaultValue);
        }

        #endregion
    }
}
