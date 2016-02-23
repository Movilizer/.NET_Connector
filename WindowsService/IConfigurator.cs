namespace MWS.WindowsService
{
    /// <summary>
    /// Description of interface <see cref="IConfigurator"/>
    /// </summary>
    public interface IConfigurator
    {
        #region Database Specific Settings
        string DatabaseDriver { get; }
        string DatabasePath { get; }
        string DatabaseUser { get; }
        string DatabasePassword { get; }
        string DatabaseParameters { get; }
        #endregion

        #region Movilizer Web Service Specific Settings
        long SystemId { get; }
        string SystemPassword { get; }
        string WebServiceHost { get; }
        string WebServiceProtocol { get; }
        string WebServiceProxy { get; }
        #endregion

        #region Windows Server Specific Settings
        int ServiceTimeInterval { get; }
        string DebugOutputPath { get; }
        bool ForceRequeingOnError { get; }
        #endregion
    }
}