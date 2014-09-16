using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MWS.Log
{
    public class DatabaseLog : LogInterface
    {
        private DatabaseLog()
        {
            // private constructor
        }

        /*private bool WriteEntry(System.DateTime date, string message, char type)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("INSERT INTO [t_ptime_logging] ([date],[message],[type]) ");
            sb.Append(" VALUES (@p1, @p2, @p3)");

            SqlCommand command = this.CreateCommand(sb.ToString());
            command.Parameters.AddWithValue("@p1", date);
            command.Parameters.AddWithValue("@p2", message);
            command.Parameters.AddWithValue("@p3", type);
            return (Insert(command));
        }*/

        public void WriteInfo(string message)
        {
            // todo
        }

        public void WriteWarning(string message)
        {
            // todo
        }

        public void WriteError(string message)
        {
            // todo
        }
    }
}
