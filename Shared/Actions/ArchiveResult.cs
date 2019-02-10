using System;
using System.Net;
using Newtonsoft.Json.Linq;

namespace CloudinaryDotNet.Actions
{
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
