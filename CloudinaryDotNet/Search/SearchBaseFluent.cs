namespace CloudinaryDotNet
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Advanced search provider. Allows you to retrieve information on all the assets in your account with the help of
    /// query expressions in a Lucene-like query language.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    public abstract class SearchBaseFluent<T>
        where T : SearchBaseFluent<T>
    {
        /// <summary>
        /// The API provider.
        /// </summary>
        protected ApiShared api;

        private List<Dictionary<string, object>> sortByParam;
        private List<string> aggregateParam;
        private List<string> withFieldParam;
        private Dictionary<string, object> searchParams;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBaseFluent{T}"/> class.
        /// </summary>
        /// <param name="api">Provider of the API calls.</param>
        public SearchBaseFluent(ApiShared api)
        {
            this.api = api;
            searchParams = new Dictionary<string, object>();
            sortByParam = new List<Dictionary<string, object>>();
            aggregateParam = new List<string>();
            withFieldParam = new List<string>();
        }

        /// <summary>
        /// The (Lucene-like) string expression specifying the search query. If this parameter is not provided then all
        /// resources are listed (up to max_results).
        /// </summary>
        /// <param name="value">Search query expression.</param>
        /// <returns>The search provider with search query defined.</returns>
        public T Expression(string value)
        {
            searchParams.Add("expression", value);
            return (T)this;
        }

        /// <summary>
        /// The maximum number of results to return. Default 50. Maximum 500.
        /// </summary>
        /// <param name="value">Number of results to return.</param>
        /// <returns>The search provider with maximum number of results defined.</returns>
        public T MaxResults(int value)
        {
            searchParams.Add("max_results", value);
            return (T)this;
        }

        /// <summary>
        /// Set value of NextCursor.
        /// </summary>
        /// <param name="value">The value of NextCursor.</param>
        /// <returns>The search provider with next cursor defined.</returns>
        public T NextCursor(string value)
        {
            searchParams.Add("next_cursor", value);
            return (T)this;
        }

        /// <summary>
        /// Set value of Direction.
        /// </summary>
        /// <param name="value">The value of Direction.</param>
        /// <returns>The search provider with direction defined.</returns>
        public T Direction(string value)
        {
            searchParams.Add("direction", value);
            return (T)this;
        }

        /// <summary>
        /// Set name of field (attribute) for which aggregation counts should be calculated and returned in the
        /// response. Supported parameters: resource_type, type, pixels, duration, format, and bytes.
        /// </summary>
        /// <param name="field">The name of field.</param>
        /// <returns>The search provider with aggregation field defined.</returns>
        public T Aggregate(string field)
        {
            aggregateParam.Add(field);
            return (T)this;
        }

        /// <summary>
        /// The name of an additional asset attribute to include for each asset in the response.
        /// Possible value: context, tags, image_metadata and image_analysis.
        /// </summary>
        /// <param name="field">The name of field.</param>
        /// <returns>The search provider with additional asset attribute defined.</returns>
        public T WithField(string field)
        {
            withFieldParam.Add(field);
            return (T)this;
        }

        /// <summary>
        /// Set sort parameter. If this parameter is not provided then the results are sorted by descending
        /// creation date. Valid sort directions are 'asc' or 'desc'.
        /// </summary>
        /// <param name="field">The field to sort by.</param>
        /// <param name="dir">The direction.</param>
        /// <returns>The search provider with sort parameter defined.</returns>
        public T SortBy(string field, string dir)
        {
            var sortBucket = new Dictionary<string, object> { { field, dir } };
            sortByParam.Add(sortBucket);

            return (T)this;
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
                queryParams.Add("with_field", withFieldParam.Distinct());
            }

            if (sortByParam.Count > 0)
            {
                queryParams.Add("sort_by", sortByParam.GroupBy(d => d.Keys.First()).Select(l => l.Last()));
            }

            if (aggregateParam.Count > 0)
            {
                queryParams.Add("aggregate", aggregateParam.Distinct());
            }

            return queryParams;
        }

        /// <summary>
        /// Prepares search params.
        /// </summary>
        /// <returns>Updated search params.</returns>
        protected SortedDictionary<string, object> PrepareSearchParams()
        {
            var sParams = new SortedDictionary<string, object>(ToQuery())
            {
                { "unsigned", string.Empty },
                { "removeUnsignedParam", string.Empty },
            };

            return sParams;
        }
    }
}
