namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Results of the resource access mode update.
    /// </summary>
    [DataContract]
    public class UpdateResourceAccessModeResult : BaseResult
    {
        /// <summary>
        /// Gets or sets list of successfully updated results.
        /// </summary>
        [DataMember(Name = "updated")]
        public List<object> Updated { get; set; }

        /// <summary>
        /// Gets or sets list of failed results.
        /// </summary>
        [DataMember(Name = "failed")]
        public List<object> Failed { get; set; }
    }
}
