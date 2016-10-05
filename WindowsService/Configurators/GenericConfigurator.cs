namespace MWS.WindowsService
{
    /// <summary>
    /// Description of <see cref="GenericConfigurator"/>
    /// </summary>
    public class GenericConfigurator : IConfigurator
    {
        public string DatabaseDriver { get; set; }

        public string DatabasePath { get; set; }

        public string DatabaseUser { get; set; }

        public string DatabasePassword { get; set; }

        public string DatabaseParameters { get; set; }

        public long SystemId { get; set; }

        public string SystemPassword { get; set; }

        public string WebServiceHost { get; set; }

        public string WebServiceProtocol { get; set; } = "https";

        public string WebServiceProxy { get; set; }

        public int ServiceTimeInterval { get; set; } = 5000;

        public string DebugOutputPath { get; set; }

        public bool ForceRequeingOnError
        {
            get { return false; }
        }
    }
}
