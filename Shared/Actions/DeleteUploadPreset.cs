namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Results of DeleteUploadPreset operation.
    /// </summary>
    [DataContract]
    public class DeleteUploadPresetResult : BaseResult
    {
        /// <summary>
        /// API message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; protected set; }
    }
}
