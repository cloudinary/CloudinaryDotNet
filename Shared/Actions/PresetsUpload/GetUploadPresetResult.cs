namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Upload preset details.
    /// </summary>
    [DataContract]
    public class GetUploadPresetResult : BaseResult
    {
        /// <summary>
        /// Gets or sets name of upload preset.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether true enables unsigned uploading to Cloudinary with this upload preset.
        /// </summary>
        [DataMember(Name = "unsigned")]
        public bool Unsigned { get; set; }

        /// <summary>
        /// Gets or sets other preset settings.
        /// </summary>
        [DataMember(Name = "settings")]
        public UploadSettings Settings { get; set; }

        /// <summary>
        /// Gets or sets JavaScript code expression to be evaluated.
        /// </summary>
        [DataMember(Name = "eval")]
        public string Eval { get; set; }
    }
}
