namespace CloudinaryDotNet.Provisioning
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <inheritdoc />
    public class ProvisioningApi : ApiShared
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProvisioningApi"/> class.
        /// </summary>
        public ProvisioningApi()
        {
            ProvisioningApiAccount = new ProvisioningApiAccount();
        }

        /// <summary>
        /// Gets cloudinary account API credentials.
        /// </summary>
        public ProvisioningApiAccount ProvisioningApiAccount { get; private set; }

        /// <summary>
        /// Gets cloudinary account API URL.
        /// </summary>
        public Url AccountApiUrlV =>
            new Url(Constants.PROVISIONING)
                .CloudinaryAddr(m_apiAddr)
                .ApiVersion(API_VERSION)
                .Add(Constants.ACCOUNTS)
                .Add(ProvisioningApiAccount.AccountId);

        /// <summary>
        /// Call account api asynchronous and return response of specified type asynchronously.
        /// </summary>
        /// <param name="method">HTTP method.</param>
        /// <param name="url">Url for api call.</param>
        /// <param name="parameters">Parameters for api call.</param>
        /// <param name="file">File to upload (must be null for non-uploading actions).</param>
        /// <param name="extraHeaders">Extra headers.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Return response of specified type.</returns>
        /// <typeparam name="T">Type of the parsed response.</typeparam>
        internal Task<T> CallAccountApiAsync<T>(
            HttpMethod method,
            string url,
            BaseParams parameters,
            FileDescription file,
            Dictionary<string, string> extraHeaders = null,
            CancellationToken? cancellationToken = null)
            where T : BaseResult, new()
        {
            ValidateAccountApiCredentials();
            parameters?.Check();

            var callParams = (method == HttpMethod.PUT || method == HttpMethod.POST)
                ? parameters?.ToParamsDictionary()
                : null;

            return CallAndParseAsync<T>(
                method,
                url,
                callParams,
                file,
                extraHeaders,
                cancellationToken);
        }

        /// <summary>
        /// Call account api synchronous and return response of specified type.
        /// </summary>
        /// <param name="method">HTTP method.</param>
        /// <param name="url">Url for api call.</param>
        /// <param name="parameters">Parameters for api call.</param>
        /// <param name="file">File to upload (must be null for non-uploading actions).</param>
        /// <param name="extraHeaders">Extra headers.</param>
        /// <returns>Return response of specified type.</returns>
        /// <typeparam name="T">Type of the parsed response.</typeparam>
        internal T CallAccountApi<T>(HttpMethod method, string url, BaseParams parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
            where T : BaseResult, new()
        {
            ValidateAccountApiCredentials();
            parameters?.Check();

            return CallAndParse<T>(
                method,
                url,
                (method == HttpMethod.PUT || method == HttpMethod.POST) ? parameters?.ToParamsDictionary() : null,
                file,
                extraHeaders);
        }

        /// <summary>
        /// Gets authentication credentials for provisioning api.
        /// </summary>
        /// <returns>Credentials string for authentication.</returns>
        protected override string GetApiCredentials()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}:{1}",
                ProvisioningApiAccount.ProvisioningApiKey,
                ProvisioningApiAccount.ProvisioningApiSecret);
        }

        private void ValidateAccountApiCredentials()
        {
            const string message = "for account provisioning API cannot be null";
            Utils.ShouldNotBeEmpty(() => ProvisioningApiAccount.ProvisioningApiKey, message);
            Utils.ShouldNotBeEmpty(() => ProvisioningApiAccount.ProvisioningApiSecret, message);
        }
    }
}
