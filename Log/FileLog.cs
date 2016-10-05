using System;
using System.IO;

namespace MWS.Log
{
    public class FileLog : LogInterface
    {
        private string _path { get; set; }
        public FileLog(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            _path = path;
        }

        private void WriteEntry(DateTime timestamp, string message, string severity)
        {
            File.WriteAllText(getPath(), String.Format("{0} [{1}]: {2}", timestamp, severity, message));
        }

        public void WriteInfo(DateTime date, string message)
        {
            this.WriteEntry(date, message, "INFO");
        }

        public void WriteWarning(DateTime date, string message)
        {
            this.WriteEntry(date, message, "WARNING");
        }

        public void WriteError(DateTime date, string message)
        {
            this.WriteEntry(date, message, "ERROR");
        }

        private string getPath()
        {
            if (_path.EndsWith("/"))
                return String.Concat(_path, "Log_", DateTime.Now.ToString("yyyy-MM-dd"), ".movLog");
            else
                return String.Concat(_path, "/Log_", DateTime.Now.ToString("yyyy-MM-dd"), ".movLog");
        }
    }
}
