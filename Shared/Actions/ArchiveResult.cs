namespace CloudinaryDotNet.Actions
{
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed result of creating the archive.
    /// </summary>
    public class ArchiveResult : BaseResult
    {
        /// <summary>
        /// The URL for accessing the created archive.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// The HTTPS URL for securely accessing the created archive.
        /// </summary>
        public string SecureUrl { get; private set; }

        /// <summary>
        /// PublicId of the created archive.
        /// </summary>
        public string PublicId { get; private set; }

        /// <summary>
        /// Size of the created archive (in bytes).
        /// </summary>
        public long Bytes { get; private set; }

        /// <summary>
        /// Count of files in the archive.
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
            Url = source.ReadValueAsSnakeCase<string>(nameof(Url));
            SecureUrl = source.ReadValueAsSnakeCase<string>(nameof(SecureUrl));
            PublicId = source.ReadValueAsSnakeCase<string>(nameof(PublicId));
            Bytes = source.ReadValueAsSnakeCase<long>(nameof(Bytes));
            FileCount = source.ReadValueAsSnakeCase<int>(nameof(FileCount));
        }
    }
}
