using System.Collections;

using MWS.Helper;

namespace MWS.Log
{
    public static class LogFactory
    {
        private static ArrayList _logs;

        // public logs
        public static WindowsLog Log;
        public static LogInterface Console;


        static LogFactory()
        {
            Initialize();
        }

        static void Initialize()
        {
            // service logs
            Log = Singleton<WindowsLog>.Instance;
            Console = Singleton<ConsoleLog>.Instance; 

            // internal logs
            _logs = new ArrayList();

            // register logs
            // RegisterLog(Log);
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
            foreach (LogInterface log in _logs)
            {
                log.WriteInfo(message);
            }
        }

        public static void WriteWarning(string message)
        {
            foreach (LogInterface log in _logs)
            {
                log.WriteWarning(message);
            }
        }

        public static void WriteError(string message)
        {
            foreach (LogInterface log in _logs)
            {
                log.WriteError(message);
            }
        }
    }
}
