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

        /// <summary>
        /// Parses HTTP response and creates new instance of this class.
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <returns>New instance of this class.</returns>
        internal static DeleteUploadPresetResult Parse(object response)
        {
            return Api.Parse<DeleteUploadPresetResult>(response);
        }
    }
}
