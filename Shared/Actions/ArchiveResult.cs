using System;
using System.Net;
using Newtonsoft.Json.Linq;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Result of creating the archive
    /// </summary>
    public class ArchiveResult : BaseResult
    {
        /// <summary>
        /// Url of created archive
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Secure Url of created archive
        /// </summary>
        public string SecureUrl { get; private set; }

        /// <summary>
        /// PublicId of generated archive
        /// </summary>
        public string PublicId { get; private set; }

        /// <summary>
        /// Size of generated archive (bytes)
        /// </summary>
        public long Bytes { get; private set; }

        /// <summary>
        /// Count of files in archive
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
