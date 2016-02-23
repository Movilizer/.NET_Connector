using System;
using System.Net;
using MWS.Helper;
using MWS.WindowsService;

namespace MWS.WebService
{
    public static class MovilizerWebServiceConstants
    {
        // movilizer specific constants
        private const string WS_PROTOCOL = @"https";
        private const string WS_RELATIVE_PATH = @"/MovilizerDistributionService/WebService/";
        public  const string WS_NAMESPACE = "http://movilitas.com/movilizer/v14";

        private static string _webServiceProxyAddress;
        private static WebProxy _webServiceProxy;

        // customer specific settings
        public static long GetSystemId()
        {
            return Configuration.GetSystemId();
        }

        public static string GetSystemPassword()
        {
            return Configuration.GetSystemPassword();
        }

        public static string GetWebServiceUrl()
        {
            string webServiceProtocol = Configuration.GetWebServiceProtocol() ?? WS_PROTOCOL;
            string webServiceHost = Configuration.GetWebServiceHost();
            return String.Format("{0}://{1}{2}", webServiceProtocol, webServiceHost, WS_RELATIVE_PATH);
        }

        public static WebProxy GetWebServiceProxy()
        {
            string address = Configuration.GetWebServiceProxy();
            if (WebServiceAddressChanged(address))
            {
                if (StringHelper.IsEmpty(address))
                {
                    WebProxy defaultProxy = WebProxy.GetDefaultProxy();
                    if (defaultProxy != null)
                    {
                        defaultProxy.Credentials = CredentialCache.DefaultCredentials;
                    }

                    _webServiceProxy = defaultProxy;
                    _webServiceProxyAddress = null;
                }
                else
                {
                    _webServiceProxy = new WebProxy(address, true);
                    _webServiceProxy.UseDefaultCredentials = true;
                    //_webServiceProxy.Credentials = new NetworkCredential()
                    _webServiceProxyAddress = address;
                }
            }

            return _webServiceProxy;
        }

        private static bool WebServiceAddressChanged(string newWebServiceProxyAddress)
        {
            if(_webServiceProxyAddress == newWebServiceProxyAddress)
            {
                return false;
            }

            if (_webServiceProxyAddress != null)
            {
                return !_webServiceProxyAddress.Equals(newWebServiceProxyAddress);
            }
            return !newWebServiceProxyAddress.Equals(_webServiceProxyAddress);
        } 
    }
}
