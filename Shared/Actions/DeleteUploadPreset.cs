using System.Net.Http;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
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
