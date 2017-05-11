using System.Net;

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

        protected override void OnParse()
        {
            Url = JsonObj.Value<string>("url");
            SecureUrl = JsonObj.Value<string>("secure_url");
            PublicId = JsonObj.Value<string>("public_id");
            Bytes = JsonObj.Value<long>("bytes");
            FileCount = JsonObj.Value<int>("file_count");
        }
        

    }
}
