namespace CloudinaryDotNet
{
    /// <summary>
    /// Cloudinary Search API Interface.
    /// </summary>
    public interface ICloudinarySearchApi
    {
        /// <summary>
        /// Gets the advanced search provider used by the Cloudinary instance.
        /// </summary>
        /// <returns>Instance of the <see cref="Cloudinary.Search"/> class.</returns>
        Search Search();

        /// <summary>
        /// Gets the advanced search folders provider used by the Cloudinary instance.
        /// </summary>
        /// <returns>Instance of the <see cref="Cloudinary.SearchFolders"/> class.</returns>
        SearchFolders SearchFolders();
    }
}
