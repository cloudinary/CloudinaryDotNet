namespace CloudinaryDotNet
{
#if NETSTANDARD2_0
    using System;
    using System.Net;
#endif
    using System.Net.Http;

    /// <summary>
    /// Provider for the API calls.
    /// </summary>
    public partial class ApiShared
    {
        /// <summary>
        /// Sends HTTP requests and receives HTTP responses.
        /// </summary>
        public HttpClient Client = new HttpClient();

#if NETSTANDARD2_0
        private string m_apiProxy = string.Empty;

        /// <summary>
        /// Gets or sets address of the proxy server that will be used for API calls.
        /// </summary>
        public string ApiProxy
        {
            get => m_apiProxy;
            set
            {
                string newValue = value ?? string.Empty;

                if (newValue != m_apiProxy)
                {
                    m_apiProxy = newValue;
                    RecreateClient();
                }
            }
        }

        private void RecreateClient()
        {
            Client.Dispose();
            Client = null;

            if (string.IsNullOrEmpty(ApiProxy))
            {
                Client = new HttpClient();
            }
            else
            {
                var proxyAddress = new Uri(ApiProxy);
                var webProxy = new WebProxy(proxyAddress, false);
                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = webProxy,
                    UseProxy = true,
                };

                Client = new HttpClient(httpClientHandler);
            }
        }
#endif
    }
}
