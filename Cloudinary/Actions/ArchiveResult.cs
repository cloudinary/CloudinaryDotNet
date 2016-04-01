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

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static ArchiveResult Parse(HttpWebResponse response)
        {
            ArchiveResult result = Parse<ArchiveResult>(response);
            result.Url = result.JsonObj.Value<string>("url");
            result.SecureUrl = result.JsonObj.Value<string>("secure_url");
            result.PublicId = result.JsonObj.Value<string>("public_id");
            result.Bytes = result.JsonObj.Value<long>("bytes");
            result.FileCount = result.JsonObj.Value<int>("file_count");
            return result;
        }
    }
}
