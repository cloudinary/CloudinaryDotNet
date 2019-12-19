namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed result of asset deletion.
    /// </summary>
    [DataContract]
    public class DeletionResult : BaseResult
    {
        /// <summary>
        /// Result description.
        /// </summary>
        [DataMember(Name = "result")]
        public string Result { get; protected set; }

        /// <inheritdoc/>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            Result = source.ReadValueAsSnakeCase<string>(nameof(Result));
        }
    }
}
