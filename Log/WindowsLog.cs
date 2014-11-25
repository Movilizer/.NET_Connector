using System;
using System.Diagnostics;

namespace MWS.Log
{
    public class WindowsLog : EventLog, LogInterface
    {
        private static string LOG_SOURCE_OLD = @"MovilizerLogSource";  // depricated
        private static string LOG_SOURCE = @"MovilizerWindowsService";
        private static string LOG_NAME = @"MovilizerLog";


        public WindowsLog() 
        {
            if (SourceExists(LOG_SOURCE_OLD))
            {
                Source = LOG_SOURCE_OLD;
            }
            else if (SourceExists(LOG_SOURCE))
            {
                Source = LOG_SOURCE;
            }
            else
            {
                CreateEventSource(LOG_SOURCE, LOG_NAME);
                Source = LOG_SOURCE;
            }

            Log = LOG_NAME;
        }

        private void WriteEntry(EventLogEntryType logEntryType, Nullable<System.DateTime> timestamp, string message)
        {
            Console.WriteLine(String.Format("{0}: {1}", timestamp, message));
            WriteEntry(String.Format("{0}: {1}", timestamp, message), logEntryType);
        }

        #region LogInterface Members

        public void WriteInfo(DateTime date, string message)
        {
            this.WriteEntry(EventLogEntryType.Information, date, message);
        }

        public void WriteWarning(DateTime date, string message)
        {
            this.WriteEntry(EventLogEntryType.Warning, date, message);
        }

        public void WriteError(DateTime date, string message)
        {
            this.WriteEntry(EventLogEntryType.Error, date, message);
        }

        #endregion
    }
}
