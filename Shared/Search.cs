namespace CloudinaryDotNet
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Advanced search provider. Allows you to retrieve information on all the assets in your account with the help of
    /// query expressions in a Lucene-like query language.
    /// </summary>
    public class Search
    {
        private List<Dictionary<string, object>> sortByParam;
        private List<string> aggregateParam;
        private List<string> withFieldParam;
        private Dictionary<string, object> searchParams;
        private ApiShared m_api;

        /// <summary>
        /// Initializes a new instance of the <see cref="Search"/> class.
        /// </summary>
        /// <param name="api">Provider of the API calls.</param>
        public Search(ApiShared api)
        {
            m_api = api;
            searchParams = new Dictionary<string, object>();
            sortByParam = new List<Dictionary<string, object>>();
            aggregateParam = new List<string>();
            withFieldParam = new List<string>();
        }

        private Url SearchResourcesUrl => m_api?.ApiUrlV?
            .Add("resources")
            .Add("search");

        /// <summary>
        /// The (Lucene-like) string expression specifying the search query. If this parameter is not provided then all
        /// resources are listed (up to max_results).
        /// </summary>
        /// <param name="value">Search query expression.</param>
        /// <returns>The search provider with search query defined.</returns>
        public Search Expression(string value)
        {
            searchParams.Add("expression", value);
            return this;
        }

        /// <summary>
        /// The maximum number of results to return. Default 50. Maximum 500.
        /// </summary>
        /// <param name="value">Number of results to return.</param>
        /// <returns>The search provider with maximum number of results defined.</returns>
        public Search MaxResults(int value)
        {
            searchParams.Add("max_results", value);
            return this;
        }

        /// <summary>
        /// Set value of NextCursor.
        /// </summary>
        /// <param name="value">The value of NextCursor.</param>
        /// <returns>The search provider with next cursor defined.</returns>
        public Search NextCursor(string value)
        {
            searchParams.Add("next_cursor", value);
            return this;
        }

        /// <summary>
        /// Set value of Direction.
        /// </summary>
        /// <param name="value">The value of Direction.</param>
        /// <returns>The search provider with direction defined.</returns>
        public Search Direction(string value)
        {
            searchParams.Add("direction", value);
            return this;
        }

        /// <summary>
        /// Set name of field (attribute) for which aggregation counts should be calculated and returned in the
        /// response. Supported parameters: resource_type, type, pixels, duration, format, and bytes.
        /// </summary>
        /// <param name="field">The name of field.</param>
        /// <returns>The search provider with aggregation field defined.</returns>
        public Search Aggregate(string field)
        {
            aggregateParam.Add(field);
            return this;
        }

        /// <summary>
        /// The name of an additional asset attribute to include for each asset in the response.
        /// Possible value: context, tags, image_metadata and image_analysis.
        /// </summary>
        /// <param name="field">The name of field.</param>
        /// <returns>The search provider with additional asset attribute defined.</returns>
        public Search WithField(string field)
        {
            withFieldParam.Add(field);
            return this;
        }

        /// <summary>
        /// Set sort parameter. If this parameter is not provided then the results are sorted by descending
        /// creation date. Valid sort directions are 'asc' or 'desc'.
        /// </summary>
        /// <param name="field">The field to sort by.</param>
        /// <param name="dir">The direction.</param>
        /// <returns>The search provider with sort parameter defined.</returns>
        public Search SortBy(string field, string dir)
        {
            Dictionary<string, object> sortBucket = new Dictionary<string, object>();
            sortBucket.Add(field, dir);
            sortByParam.Add(sortBucket);

            return this;
        }

        /// <summary>
        /// Collect all search parameters to a dictionary.
        /// </summary>
        /// <returns>Search parameters as dictionary.</returns>
        public Dictionary<string, object> ToQuery()
        {
            Dictionary<string, object> queryParams = new Dictionary<string, object>(searchParams);

            if (withFieldParam.Count > 0)
            {
                queryParams.Add("with_field", withFieldParam);
            }

            if (sortByParam.Count > 0)
            {
                queryParams.Add("sort_by", sortByParam);
            }

            if (aggregateParam.Count > 0)
            {
                queryParams.Add("aggregate", aggregateParam);
            }

            return queryParams;
        }

        /// <summary>
        /// Execute search request.
        /// </summary>
        /// <returns>Search response with information about the assets matching the search criteria.</returns>
        public SearchResult Execute()
        {
            return m_api.CallAndParse<SearchResult>(
                HttpMethod.POST,
                SearchResourcesUrl.BuildUrl(),
                PrepareSearchParams(),
                null,
                Utils.PrepareJsonHeaders());
        }

        /// <summary>
        /// Execute search request asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Search response with information about the assets matching the search criteria.</returns>
        public Task<SearchResult> ExecuteAsync(CancellationToken? cancellationToken = null)
        {
            return m_api.CallAndParseAsync<SearchResult>(
                HttpMethod.POST,
                SearchResourcesUrl.BuildUrl(),
                PrepareSearchParams(),
                null,
                Utils.PrepareJsonHeaders(),
                cancellationToken);
        }

        private SortedDictionary<string, object> PrepareSearchParams()
        {
            SortedDictionary<string, object> sParams = new SortedDictionary<string, object>(ToQuery());
            sParams.Add("unsigned", string.Empty);
            sParams.Add("removeUnsignedParam", string.Empty);

            return sParams;
        }
    }
}
