namespace CloudinaryDotNet.Provisioning
{
    using System;

    /// <summary>
    /// Cloudinary provisioning API account.
    /// </summary>
    public class ProvisioningApiAccount
    {
        private const string CloudinaryAccountUrl = "CLOUDINARY_ACCOUNT_URL";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvisioningApiAccount"/> class.
        /// </summary>
        public ProvisioningApiAccount()
            : this(Environment.GetEnvironmentVariable(CloudinaryAccountUrl))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvisioningApiAccount"/> class.
        /// Parameterized constructor.
        /// </summary>
        /// <param name="accountUrl">The Cloudinary Account Url.</param>
        public ProvisioningApiAccount(string accountUrl)
        {
            if (string.IsNullOrEmpty(accountUrl))
            {
                return;
            }

            var accountUri = new Uri(accountUrl);
            AccountId = accountUri.Host;

            var credentials = accountUri.UserInfo.Split(':');
            ProvisioningApiKey = credentials[0];
            ProvisioningApiSecret = credentials[1];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvisioningApiAccount"/> class.
        /// Parameterized constructor.
        /// </summary>
        /// <param name="accountId">Account id.</param>
        /// <param name="provisioningApiKey">Provisioning API key.</param>
        /// <param name="provisioningApiSecret">Provisioning API secret.</param>
        public ProvisioningApiAccount(string accountId, string provisioningApiKey, string provisioningApiSecret)
        {
            AccountId = accountId;
            ProvisioningApiKey = provisioningApiKey;
            ProvisioningApiSecret = provisioningApiSecret;
        }

        /// <summary>
        /// Gets or sets the account id.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// Gets or sets the provisioning API key.
        /// </summary>
        public string ProvisioningApiKey { get; set; }

        /// <summary>
        /// Gets or sets the provisioning API secret.
        /// </summary>
        public string ProvisioningApiSecret { get; set; }
    }
}
