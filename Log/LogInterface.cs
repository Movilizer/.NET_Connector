
namespace MWS.Log
{
    using System;

    public interface LogInterface
    {
        void WriteInfo(DateTime date,string message);
        void WriteWarning(DateTime date, string message);
        void WriteError(DateTime date, string message);
    }
}

