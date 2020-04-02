namespace CloudinaryDotNet
{
    /// <summary>
    /// Cloudinary account.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// Default constructor.
        /// </summary>
        public Account()
        {
            Cloud = CloudinaryConfiguration.CloudName;
            ApiKey = CloudinaryConfiguration.ApiKey;
            ApiSecret = CloudinaryConfiguration.ApiSecret;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// Parameterized constructor.
        /// </summary>
        /// <param name="cloud">Cloud name.</param>
        /// <param name="apiKey">API key.</param>
        /// <param name="apiSecret">API secret.</param>
        public Account(string cloud, string apiKey, string apiSecret)
        {
            Cloud = cloud;
            ApiKey = apiKey;
            ApiSecret = apiSecret;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// Parameterized constructor.
        /// </summary>
        /// <param name="cloud">Cloud name.</param>
        public Account(string cloud)
        {
            Cloud = cloud;
        }

        /// <summary>
        /// Gets or sets cloud name.
        /// </summary>
        public string Cloud { get; set; }

        /// <summary>
        /// Gets or sets API key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets API secret.
        /// </summary>
        public string ApiSecret { get; set; }
    }
}
