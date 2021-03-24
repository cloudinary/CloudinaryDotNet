namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed response after manipulation of upload presets.
    /// </summary>
    [DataContract]
    public class UploadPresetResult : BaseResult
    {
        /// <summary>
        /// Gets or sets response message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets name assigned to the upload preset.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}