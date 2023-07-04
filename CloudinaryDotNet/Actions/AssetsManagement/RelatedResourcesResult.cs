namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed result of RelatedResources.
    /// </summary>
    [DataContract]
    public abstract class RelatedResourcesResult : BaseResult
    {
        /// <summary>
        /// Gets or sets the list of successful assets.
        /// </summary>
        [DataMember(Name = "success")]
        public List<RelatedResource> Success { get; set; }

        /// <summary>
        /// Gets or sets the list of failed assets.
        /// </summary>
        [DataMember(Name = "failed")]
        public List<RelatedResource> Failed { get; set; }
    }
}
