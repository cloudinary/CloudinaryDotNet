namespace CloudinaryDotNet
{
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Advanced search provider. Allows you to retrieve information on all the assets in your account with the help of
    /// query expressions in a Lucene-like query language.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    public class SearchFluent<T> : SearchBaseFluent<T>
        where T : SearchFluent<T>
    {
        private int urlTtl = 300;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchFluent{T}"/> class.
        /// </summary>
        /// <param name="api">Provider of the API calls.</param>
        public SearchFluent(ApiShared api)
            : base(api)
        {
        }

        private Url SearchResourcesUrl => api?.ApiUrlV?
            .Add("resources")
            .Add("search");

        /// <summary>
        /// Sets the time to live of the search URL.
        /// </summary>
        /// <param name="ttl">The time to live in seconds.</param>
        /// <returns>The search provider with TTL defined.</returns>
        public T Ttl(int ttl)
        {
            urlTtl = ttl;

            return (T)this;
        }

        /// <summary>
        /// Execute search request.
        /// </summary>
        /// <returns>Search response with information about the assets matching the search criteria.</returns>
        public SearchResult Execute()
        {
            return api.CallAndParse<SearchResult>(
                HttpMethod.POST,
                SearchResourcesUrl.BuildUrl(),
                PrepareSearchParams(),
                Utils.PrepareJsonHeaders());
        }

        /// <summary>
        /// Execute search request asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Search response with information about the assets matching the search criteria.</returns>
        public Task<SearchResult> ExecuteAsync(CancellationToken? cancellationToken = null)
        {
            return api.CallAndParseAsync<SearchResult>(
                HttpMethod.POST,
                SearchResourcesUrl.BuildUrl(),
                PrepareSearchParams(),
                Utils.PrepareJsonHeaders(),
                cancellationToken);
        }

        /// <summary>
        /// Creates a signed Search URL that can be used on the client side.
        /// </summary>
        /// <param name="ttl">The time to live in seconds.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <returns>The resulting search URL.</returns>
        public string ToUrl(int? ttl = null, string nextCursor = null)
        {
            if (ttl == null)
            {
                ttl = urlTtl;
            }

            return api.Url.BuildSearchUrl(ToQuery(), (int)ttl, nextCursor);
        }
    }
}
