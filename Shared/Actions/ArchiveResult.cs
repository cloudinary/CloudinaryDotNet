namespace CloudinaryDotNet.Actions
{
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed result of creating the archive.
    /// </summary>
    public class ArchiveResult : BaseResult
    {
        /// <summary>
        /// Gets the URL for accessing the created archive.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Gets the HTTPS URL for securely accessing the created archive.
        /// </summary>
        public string SecureUrl { get; private set; }

        /// <summary>
        /// Gets publicId of the created archive.
        /// </summary>
        public string PublicId { get; private set; }

        /// <summary>
        /// Gets size of the created archive (in bytes).
        /// </summary>
        public long Bytes { get; private set; }

        /// <summary>
        /// Gets count of files in the archive.
        /// </summary>
        public int FileCount { get; private set; }

        /// <summary>
        /// Overrides corresponding method of <see cref="BaseResult"/> class.
        /// Populates additional token fields.
        /// </summary>
        /// <param name="source">JSON token received from the server.</param>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            Url = source.Value<string>("url");
            SecureUrl = source.Value<string>("secure_url");
            PublicId = source.Value<string>("public_id");
            Bytes = source.Value<long>("bytes");
            FileCount = source.Value<int>("file_count");
        }
    }
}
