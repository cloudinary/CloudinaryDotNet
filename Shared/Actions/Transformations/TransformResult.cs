namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed response after transformation manipulation.
    /// </summary>
    [DataContract]
    public class TransformResult : BaseResult
    {
        /// <summary>
        /// Gets or sets result message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
