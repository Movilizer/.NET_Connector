using System;
using System.Collections;

using MWS.Helper;
using MWS.Movilizer;

namespace MWS.Log
{
    public static class LogFactory
    {
        private static ArrayList _logs;

        static LogFactory()
        {
            Initialize();
        }

        static void Initialize()
        {
            // internal logs
            _logs = new ArrayList();
        }

        public static void RegisterLog(LogInterface log)
        {
            if (!_logs.Contains(log))
            {
                _logs.Add(log);
            }
        }

        public static void WriteInfo(string message)
        {
            WriteInfo(DateTime.Now, message);
        }

        private static void WriteInfo(DateTime date, string message)
        {
            foreach (LogInterface log in _logs)
            {
                log.WriteInfo(date, message);
            }
        }

        public static void WriteWarning(string message)
        {
            WriteWarning(DateTime.Now, message);
        }

        private static void WriteWarning(DateTime date, string message)
        {
            foreach (LogInterface log in _logs)
            {
                log.WriteWarning(date, message);
            }
        }

        public static void WriteError(string message)
        {
            WriteError(DateTime.Now, message);
        }

        private static void WriteError(DateTime date, string message)
        {
            foreach (LogInterface log in _logs)
            {
                log.WriteError(DateTime.Now, message);
            }
        }

        public static void WriteEntry(MovilizerStatusMessage statusMessage)
        {
            switch (statusMessage.severity)
            {
                case 2: // warning=2
                    WriteWarning(statusMessage.timestamp, statusMessage.message);
                    break;
                case 3:
                case 4: // error=3, fatal=4
                    WriteError(statusMessage.timestamp, statusMessage.message);
                    break;
                default: // debug=-1, info=1
                    WriteInfo(statusMessage.timestamp, statusMessage.message);
                    break;
            }
        }

        public static void WriteEntry(MovilizerMoveletError moveletError)
        {
            WriteError(moveletError.timestamp, moveletError.message);
        }
    }
}
