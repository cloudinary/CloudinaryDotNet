namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Details of a related resource.
    /// </summary>
    [DataContract]
    public class RelatedResource
    {
        /// <summary>
        /// Gets or sets an API message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the API code.
        /// </summary>
        [DataMember(Name = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the asset ID.
        /// </summary>
        [DataMember(Name = "asset")]
        public string Asset { get; set; }

        /// <summary>
        /// Gets or sets an API status.
        /// </summary>
        [DataMember(Name = "status")]
        public int Status { get; set; }
    }
}
