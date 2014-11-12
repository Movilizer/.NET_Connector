using System;

namespace MWS.Log
{
    public class ConsoleLog : LogInterface
    {
        public ConsoleLog()
        {
            // private constructor
        }

        private void WriteEntry(DateTime timestamp, string message)
        {
            Console.WriteLine(String.Format("{0}: {1}", timestamp, message));
        }

        public void WriteInfo(DateTime date, string message)
        {
            this.WriteEntry(date, message);
        }

        public void WriteWarning(DateTime date, string message)
        {
            this.WriteEntry(date, message);
        }

        public void WriteError(DateTime date, string message)
        {
            this.WriteEntry(date, message);
        }
    }
}
