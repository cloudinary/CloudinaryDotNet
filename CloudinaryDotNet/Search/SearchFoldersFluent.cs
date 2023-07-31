namespace CloudinaryDotNet
{
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Advanced search provider. Allows you to retrieve information on all the folder in your account with the help of
    /// query expressions in a Lucene-like query language.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    public class SearchFoldersFluent<T> : SearchBaseFluent<T>
        where T : SearchFoldersFluent<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchFoldersFluent{T}"/> class.
        /// </summary>
        /// <param name="api">Provider of the API calls.</param>
        public SearchFoldersFluent(ApiShared api)
            : base(api)
        {
        }

        private Url SearchFoldersUrl => api?.ApiUrlV?
            .Add("folders")
            .Add("search");

        /// <summary>
        /// Execute search request.
        /// </summary>
        /// <returns>Search response with information about the assets matching the search criteria.</returns>
        public SearchFoldersResult Execute()
        {
            return api.CallAndParse<SearchFoldersResult>(
                HttpMethod.POST,
                SearchFoldersUrl.BuildUrl(),
                PrepareSearchParams(),
                null,
                Utils.PrepareJsonHeaders());
        }

        /// <summary>
        /// Execute search request asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Search response with information about the assets matching the search criteria.</returns>
        public Task<SearchFoldersResult> ExecuteAsync(CancellationToken? cancellationToken = null)
        {
            return api.CallAndParseAsync<SearchFoldersResult>(
                HttpMethod.POST,
                SearchFoldersUrl.BuildUrl(),
                PrepareSearchParams(),
                null,
                Utils.PrepareJsonHeaders(),
                cancellationToken);
        }
    }
}
