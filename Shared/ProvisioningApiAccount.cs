namespace CloudinaryDotNet
{
    using System;

    /// <summary>
    /// Cloudinary account.
    /// </summary>
    public class ProvisioningApiAccount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProvisioningApiAccount"/> class.
        /// Parameterized constructor.
        /// </summary>
        public ProvisioningApiAccount()
        {
            var accountUrl = Environment.GetEnvironmentVariable("CLOUDINARY_ACCOUNT_URL");
            if (accountUrl == null)
            {
                return;
            }

            var accountUri = new Uri(accountUrl);
            AccountId = accountUri.Host;
            var credentials = accountUri.UserInfo.Split(':');
            ApiKey = credentials[0];
            ApiSecret = credentials[1];
        }

        /// <summary>
        /// Gets or sets cloud name.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// Gets or sets aPI key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets aPI secret.
        /// </summary>
        public string ApiSecret { get; set; }
    }
}