using System;
using System.Diagnostics;

namespace MWS.Log
{
    public class WindowsLog : EventLog, LogInterface
    {
        private static string LOG_SOURCE_OLD = @"MovilizerLogSource";  // depricated
        private static string LOG_SOURCE = @"MovilizerWindowsService";
        private static string LOG_NAME = @"MovilizerLog";


        private WindowsLog() 
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

        public void WriteEntry(MovilizerStatusMessage statusMessage)
        {
            EventLogEntryType type;

            switch (statusMessage.severity)
            {
                case 2: // warning=2
                    type = EventLogEntryType.Warning;
                    break;
                case 3: case 4: // error=3, fatal=4
                    type = EventLogEntryType.Error;
                    break;
                default: // debug=-1, info=1
                    type = EventLogEntryType.Information;
                    break;
            }

            WriteEntry(type, statusMessage.timestamp, statusMessage.message);
        }

        public void WriteEntry(MovilizerMoveletError moveletError)
        {
            WriteEntry(EventLogEntryType.Error, moveletError.timestamp, moveletError.message);
        }

        public void WriteInfo(string message)
        {
            WriteEntry(EventLogEntryType.Information, DateTime.Now, message);
        }

        public void WriteWarning(string message)
        {
            WriteEntry(EventLogEntryType.Warning, DateTime.Now, message);
        }

        public void WriteError(string message)
        {
            WriteEntry(EventLogEntryType.Error, DateTime.Now, message);
        }

        new public void WriteEntry(string message)
        {
            throw new InvalidOperationException("Please use following methods instead: WriteInfo, WriteWarning, WriteError");
        }
    }
}
