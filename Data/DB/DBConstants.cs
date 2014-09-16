using System;

using MWS.WindowsService;

namespace MWS.Data
{
    public static class DBConstants
    {
        // generic database constants
        private const string DB_DRIVER   = @"{0}";
        private const string DB_SOURCE   = @"{0}";
        private const string DB_USER_ID  = @"{0}";
        private const string DB_PASSWORD = @"{0}";        


        private static string GetDriver()
        {
            return String.Format(DB_DRIVER, Configuration.GetDatabaseDriver());
        }

        private static string GetSource()
        {
            return String.Format(DB_SOURCE, Configuration.GetDatabasePath());
        }

        private static string GetUserId()
        {
            return String.Format(DB_USER_ID, Configuration.GetDatabaseUser());
        }

        private static string GetPassword()
        {
            return String.Format(DB_PASSWORD, Configuration.GetDatabasePassword());
        }

        private static string GetParameters()
        {
            return Configuration.GetDatabaseParameters();
        }

        public static string GetDatabaseUrl()
        {
            return String.Join(";",
                new string[] {
                    GetDriver(),
                    GetSource(),
                    GetUserId(),
                    GetPassword(),
                    GetParameters()
                });
        }
    }
}
