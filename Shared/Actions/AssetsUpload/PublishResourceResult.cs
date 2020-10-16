namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Result of resource publishing.
    /// </summary>
    [DataContract]
    public class PublishResourceResult : BaseResult
    {
        /// <summary>
        /// Gets or sets list of details of published resources.
        /// </summary>
        [DataMember(Name = "published")]
        public List<object> Published { get; set; }

        /// <summary>
        /// Gets or sets list of details of the resources with failed publish.
        /// </summary>
        [DataMember(Name = "failed")]
        public List<object> Failed { get; set; }
    }
}
