namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed response after transformation manipulation.
    /// </summary>
    [DataContract]
    public class TransformResult : BaseResult
    {
        /// <summary>
        /// Result message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; protected set; }

        /// <inheritdoc/>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            Message = source.ReadValueAsSnakeCase<string>(nameof(Message));
        }
    }
}
