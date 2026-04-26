namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Result of POST /video/combine_remux. The job runs asynchronously — the final upload result
    /// is delivered through the request's notification URL.
    /// </summary>
    [DataContract]
    public class CombineRemuxResult : BaseResult
    {
        /// <summary>
        /// Gets or sets the status of the combine/remux job. Always 'processing' on success.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the identifier that ties together all chunked-upload parts the server posts back to /video/upload_chunked for this job.
        /// </summary>
        [DataMember(Name = "unique_upload_id")]
        public string UniqueUploadId { get; set; }

        /// <summary>
        /// Gets or sets the public ID assigned to the combined asset, if one was supplied in the request.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets the resource type. Always 'video' — combine/remux produces an MP4.
        /// </summary>
        [DataMember(Name = "resource_type")]
        public string ResourceType { get; set; }
    }
}
