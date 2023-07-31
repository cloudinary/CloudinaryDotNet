namespace CloudinaryDotNet
{
    /// <summary>
    /// Advanced search provider. Allows you to retrieve information on all the assets in your account with the help of
    /// query expressions in a Lucene-like query language.
    /// </summary>
    public class Search : SearchFluent<Search>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Search"/> class.
        /// </summary>
        /// <param name="api">Provider of the API calls.</param>
        public Search(ApiShared api)
            : base(api)
        {
        }
    }
}
