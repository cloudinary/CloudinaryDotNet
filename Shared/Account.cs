using Cloudinary.NetCoreShared;
using System;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Cloudinary account
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Account()
        {
            Cloud = CloudinaryConfiguration.CloudName;
            ApiKey = CloudinaryConfiguration.ApiKey;
            ApiSecret = CloudinaryConfiguration.ApiSecret;
        }

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="cloud">Cloud name</param>
        /// <param name="apiKey">API key</param>
        /// <param name="apiSecret">API secret</param>
        public Account(string cloud, string apiKey, string apiSecret)
        {
            Cloud = cloud;
            ApiKey = apiKey;
            ApiSecret = apiSecret;
        }

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="cloud">Cloud name</param>
        public Account(string cloud)
        {
            Cloud = cloud;
        }

        /// <summary>
        /// Cloud name
        /// </summary>
        public string Cloud { get; set; }

        /// <summary>
        /// API key
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// API secret
        /// </summary>
        public string ApiSecret { get; set; }
    }
}
