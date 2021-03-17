namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a custom constructor for uniform resource identifiers (URIs) and modifies URIs
    /// for the <see cref="Url"/> class.
    /// </summary>
    public class UrlBuilder : UriBuilder
    {
        private StringDictionary queryString;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// Default parameterless constructor.
        /// </summary>
        public UrlBuilder()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class with the specified URI.
        /// </summary>
        /// <param name="uri">A URI string.</param>
        public UrlBuilder(string uri)
            : base(uri)
        {
            PopulateQueryString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class
        /// with the specified URI and dictionary with cloudinary parameters.
        /// </summary>
        /// <param name="uri">A URI string.</param>
        /// <param name="params">Cloudinary parameters.</param>
        public UrlBuilder(string uri, IDictionary<string, object> @params)
            : base(uri)
        {
            PopulateQueryString();
            SetParameters(@params);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class with the specified <see cref="Uri"/> instance.
        /// </summary>
        /// <param name="uri">An instance of the <see cref="Uri"/> class.</param>
        public UrlBuilder(Uri uri)
            : base(uri)
        {
            PopulateQueryString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class with the specified scheme and host.
        /// </summary>
        /// <param name="schemeName">An Internet access protocol.</param>
        /// <param name="hostName">A DNS-style domain name or IP address.</param>
        public UrlBuilder(string schemeName, string hostName)
            : base(schemeName, hostName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class with the specified scheme, host, and port.
        /// </summary>
        /// <param name="scheme">An Internet access protocol.</param>
        /// <param name="host">A DNS-style domain name or IP address.</param>
        /// <param name="portNumber">An IP port number for the service.</param>
        public UrlBuilder(string scheme, string host, int portNumber)
            : base(scheme, host, portNumber)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class  with the specified scheme, host, port number, and path.
        /// </summary>
        /// <param name="scheme">An Internet access protocol.</param>
        /// <param name="host">A DNS-style domain name or IP address.</param>
        /// <param name="port">An IP port number for the service.</param>
        /// <param name="pathValue">The path to the Internet resource.</param>
        public UrlBuilder(string scheme, string host, int port, string pathValue)
            : base(scheme, host, port, pathValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class
        /// with the specified scheme, host, port number, path and query string or fragment identifier.
        /// </summary>
        /// <param name="scheme">An Internet access protocol.</param>
        /// <param name="host">A DNS-style domain name or IP address.</param>
        /// <param name="port">An IP port number for the service.</param>
        /// <param name="path">The path to the Internet resource.</param>
        /// <param name="extraValue">A query string or fragment identifier.</param>
        public UrlBuilder(string scheme, string host, int port, string path, string extraValue)
            : base(scheme, host, port, path, extraValue)
        {
        }

        /// <summary>
        /// Gets the query information included in the Url.
        /// </summary>
        public StringDictionary QueryString
        {
            get
            {
                if (queryString == null)
                {
                    queryString = new StringDictionary();
                }

                return queryString;
            }
        }

        /// <summary>
        /// Gets or sets a path to the resource referenced by the Url.
        /// </summary>
        public string PageName
        {
            get
            {
                string path = Path;
                return path.Substring(path.LastIndexOf("/", StringComparison.Ordinal) + 1);
            }

            set
            {
                string path = Path;
                path = path.Substring(0, path.LastIndexOf("/", StringComparison.Ordinal));
                Path = string.Concat(path, "/", value);
            }
        }

        /// <summary>
        /// Set parameters for the Url to be added as query string.
        /// </summary>
        /// <param name="params">Cloudinary parameters.</param>
        public void SetParameters(IDictionary<string, object> @params)
        {
            foreach (var param in @params)
            {
                if (param.Value is IEnumerable<string>)
                {
                    foreach (var s in (IEnumerable<string>)param.Value)
                    {
                        QueryString.Add(param.Key + "[]", s);
                    }
                }
                else
                {
                    QueryString[param.Key] = param.Value.ToString();
                }
            }
        }

        /// <summary>
        /// Returns a string that represents the current Url.
        /// </summary>
        /// <returns>A string that represents the URL.</returns>
        public new string ToString()
        {
            BuildQueryString();

            return Uri.AbsoluteUri;
        }

        private void PopulateQueryString()
        {
            string query = Query;

            if (string.IsNullOrEmpty(query))
            {
                return;
            }

            if (queryString == null)
            {
                queryString = new StringDictionary();
            }

            queryString.Clear();

            query = query.Substring(1); // remove the ?

            string[] pairs = query.Split(new char[] { '&' });
            foreach (string s in pairs)
            {
                string[] pair = s.Split(new char[] { '=' });

                queryString[pair[0]] = (pair.Length > 1) ? pair[1] : string.Empty;
            }
        }

        private void BuildQueryString()
        {
            if (queryString == null)
            {
                return;
            }

            int count = queryString.Count;

            if (count == 0)
            {
                Query = string.Empty;
                return;
            }

            string[] keys = new string[count];
            string[] values = new string[count];
            string[] pairs = new string[count];

            queryString.Keys.CopyTo(keys, 0);
            queryString.Values.CopyTo(values, 0);

            for (int i = 0; i < count; i++)
            {
                pairs[i] = string.Concat(keys[i], "=", values[i]);
            }

            Query = string.Join("&", pairs);
        }
    }
}