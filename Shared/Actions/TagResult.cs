namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Results of tags management.
    /// </summary>
    [DataContract]
    public class TagResult : BaseResult
    {
        /// <summary>
        /// Gets or sets a list of public IDs (up to 1000) of affected assets.
        /// </summary>
        [DataMember(Name = "public_ids")]
        public string[] PublicIds { get; set; }
    }
}
