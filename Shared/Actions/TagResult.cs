namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Results of tags management.
    /// </summary>
    [DataContract]
    public class TagResult : BaseResult
    {
        /// <summary>
        /// A list of public IDs (up to 1000) of affected assets.
        /// </summary>
        [DataMember(Name = "public_ids")]
        public string[] PublicIds { get; protected set; }

        /// <inheritdoc/>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            PublicIds = source.ReadValueAsSnakeCase<string[]>(nameof(PublicIds));
        }
    }
}
