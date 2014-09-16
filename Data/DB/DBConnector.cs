using MWS.Helper;

namespace MWS.Data
{
    public class DBConnector : DBWrapper
    {
        // private constructor required for the singleton generic pattern
        private DBConnector() : base() {

            m_eProvider = PROVIDER_TYPE.PROVIDER_SQLCLIENT;
        }

        public static DBConnector GetConnector()
        {
            return Singleton<DBConnector>.Instance;
        }
    }
}
