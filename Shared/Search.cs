using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudinaryDotNet
{
    public class Search
    {
        private List<Dictionary<string, object>> sortByParam;
        private List<string> aggregateParam;
        private List<string> withFieldParam;
        private Dictionary<string, object> searchParams;
        private ApiShared m_api;

        public Search(ApiShared api)
        {
            m_api = api;
            searchParams = new Dictionary<string, object>();
            sortByParam = new List<Dictionary<string, object>>();
            aggregateParam = new List<string>();
            withFieldParam = new List<string>();
        }

        public Search Expression(string value)
        {
            searchParams.Add("expression", value);
            return this;
        }

        public Search MaxResults(int value)
        {
            searchParams.Add("max_results", value);
            return this;
        }

        public Search NextCursor(string value)
        {
            searchParams.Add("next_cursor", value);
            return this;
        }

        public Search Direction(string value)
        {
            searchParams.Add("direction", value);
            return this;
        }

        public Search Aggregate(string field)
        {
            aggregateParam.Add(field);
            return this;
        }

        public Search WithField(string field)
        {
            withFieldParam.Add(field);
            return this;
        }

        public Search SortBy(string field, string dir)
        {
            Dictionary<string, object> sortBucket = new Dictionary<string, object>();
            sortBucket.Add(field, dir);
            sortByParam.Add(sortBucket);

            return this;
        }

        public Dictionary<string, object> ToQuery()
        {
            Dictionary<string, object> queryParams = new Dictionary<string, object>(searchParams);

            if (withFieldParam.Count > 0)
                queryParams.Add("with_field", withFieldParam);

            if(sortByParam.Count > 0)
                queryParams.Add("sort_by", sortByParam);

            if(aggregateParam.Count > 0)
                queryParams.Add("aggregate", aggregateParam);

            return queryParams;
        }

        private SortedDictionary<string, object> PrepareSearchParams()
        {
            SortedDictionary<string, object> sParams = new SortedDictionary<string, object>(ToQuery());
            sParams.Add("unsigned", string.Empty);
            sParams.Add("removeUnsignedParam", string.Empty);

            return sParams;
        }

        private Dictionary<string, string> PrepareHeaders()
        {
            Dictionary<string, string> extraHeaders = new Dictionary<string, string>();
            extraHeaders.Add("Content-Type", "application/json");

            return extraHeaders;
        }

        public SearchResult Execute()
        {
            Url url = m_api.ApiUrlV.Add("resources").Add("search");
            
            return m_api.CallAndParse<SearchResult>(HttpMethod.POST, url.BuildUrl(), PrepareSearchParams(), null, PrepareHeaders());
        }
    }
}
