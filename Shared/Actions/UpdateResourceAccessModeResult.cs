namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Results of the resource access mode update.
    /// </summary>
    [DataContract]
    public class UpdateResourceAccessModeResult : BaseResult
    {
        /// <summary>
        /// List of successfully updated results.
        /// </summary>
        [DataMember(Name = "updated")]
        public List<object> Updated { get; protected set; }

        /// <summary>
        /// List of failed results.
        /// </summary>
        [DataMember(Name = "failed")]
        public List<object> Failed { get; protected set; }

        /// <inheritdoc/>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            Updated = source.ReadValueAsSnakeCase<List<object>>(nameof(Updated));
            Failed = source.ReadValueAsSnakeCase<List<object>>(nameof(Failed));
        }
    }
}
