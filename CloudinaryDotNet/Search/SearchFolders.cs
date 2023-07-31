namespace CloudinaryDotNet
{
    /// <summary>
    /// Advanced search provider. Allows you to retrieve information on all the folders in your account with the help of
    /// query expressions in a Lucene-like query language.
    /// </summary>
    public class SearchFolders : SearchFoldersFluent<SearchFolders>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchFolders"/> class.
        /// </summary>
        /// <param name="api">Provider of the API calls.</param>
        public SearchFolders(ApiShared api)
            : base(api)
        {
        }
    }
}
