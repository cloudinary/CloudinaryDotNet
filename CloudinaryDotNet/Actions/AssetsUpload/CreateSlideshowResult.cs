namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Result of the create slideshow operation.
    /// </summary>
    [DataContract]
    public class CreateSlideshowResult : BaseResult
    {
        /// <summary>
        /// Gets or sets the status of the create slideshow operation.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the public ID assigned to the asset.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the batch.
        /// </summary>
        [DataMember(Name = "batch_id")]
        public string BatchId { get; set; }
    }
}
