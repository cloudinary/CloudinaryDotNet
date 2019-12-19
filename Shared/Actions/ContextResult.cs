namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Results of context management.
    /// </summary>
    [DataContract]
    public class ContextResult : BaseResult
    {
        /// <summary>
        /// Public IDs of affected assets.
        /// </summary>
        [DataMember(Name = "public_ids")]
        public string[] PublicIds { get; protected set; }

        /// <inheritdoc/>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            this.PublicIds = source.ReadValueAsSnakeCase<string[]>(nameof(PublicIds));
        }
    }
}
