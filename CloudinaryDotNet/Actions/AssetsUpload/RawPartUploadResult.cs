namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Results of a file's chunk uploading.
    /// </summary>
    [DataContract]
    public class RawPartUploadResult : RawUploadResult
    {
        /// <summary>
        /// Gets or sets id of the uploaded chunk of the asset.
        /// </summary>
        [DataMember(Name = "upload_id")]
        public string UploadId { get; set; }
    }
}