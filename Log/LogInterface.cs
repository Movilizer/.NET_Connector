
namespace MWS.Log
{
    public interface LogInterface
    {
        void WriteInfo(string message);
        void WriteWarning(string message);
        void WriteError(string message);
    }
}

