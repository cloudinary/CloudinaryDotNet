using CloudinaryDotNet.Actions;
using CloudinaryShared.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coudinary.NetCoreShared
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
            this.m_api = api;
            this.searchParams = new Dictionary<string, object>();
            this.sortByParam = new List<Dictionary<string, object>>();
            this.aggregateParam = new List<string>();
            this.withFieldParam = new List<string>();
        }

        public Search Expression(string value)
        {
            this.searchParams.Add("expression", value);
            return this;
        }

        public Search MaxResults(int value)
        {
            this.searchParams.Add("max_results", value);
            return this;
        }

        public Search NextCursor(string value)
        {
            this.searchParams.Add("next_cursor", value);
            return this;
        }

        public Search Direction(string value)
        {
            this.searchParams.Add("direction", value);
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
            this.sortByParam.Add(sortBucket);

            return this;
        }

        public Dictionary<string, object> ToQuery()
        {
            Dictionary<string, object> queryParams = new Dictionary<string, object>(this.searchParams);

            if (withFieldParam.Count > 0)
                queryParams.Add("with_field", withFieldParam);

            if(sortByParam.Count > 0)
                queryParams.Add("sort_by", sortByParam);

            if(aggregateParam.Count > 0)
                queryParams.Add("aggregate", aggregateParam);

            return queryParams;
        }

        public SearchResult Execute()
        {
            Url url = m_api.ApiUrlV.Add("resources").Add("search");

            SortedDictionary<string, object> sParams = new SortedDictionary<string, object>(this.ToQuery());
            sParams.Add("unsigned", string.Empty);
            sParams.Add("removeUnsignedParam", string.Empty);

            var response = m_api.InternalCall(HttpMethod.POST, url.BuildUrl(), sParams, null);

            return SearchResult.Parse(response);
        }
    }
}
