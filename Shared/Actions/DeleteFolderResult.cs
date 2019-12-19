namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed result of folder deletion.
    /// </summary>
    [DataContract]
    public class DeleteFolderResult : BaseResult
    {
        /// <summary>
        /// The list of media assets requested for deletion, with the status of each asset (deleted unless there was an issue).
        /// </summary>
        [DataMember(Name = "deleted")]
        public List<string> Deleted { get; protected set; }

        /// <inheritdoc/>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            Deleted = source.ReadValueAsSnakeCase<List<string>>(nameof(Deleted));
        }
    }
}
