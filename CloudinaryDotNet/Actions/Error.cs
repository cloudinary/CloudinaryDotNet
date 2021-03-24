namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a server-side error.
    /// </summary>
    [DataContract]
    public class Error
    {
        /// <summary>
        /// Gets or sets error description.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}