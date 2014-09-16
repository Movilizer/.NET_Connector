using System;

namespace MWS.Log
{
    public class ConsoleLog : LogInterface
    {
        private ConsoleLog()
        {
            // private constructor
        }

        private void WriteEntry(DateTime timestamp, string message)
        {
            Console.WriteLine(String.Format("{0}: {1}", timestamp, message));
        }

        public void WriteInfo(string message)
        {
            WriteEntry(DateTime.Now, message);
        }

        public void WriteWarning(string message)
        {
            WriteEntry(DateTime.Now, message);
        }

        public void WriteError(string message)
        {
            WriteEntry(DateTime.Now, message);
        }
    }
}
