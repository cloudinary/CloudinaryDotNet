namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed response after a call of Explode method.
    /// </summary>
    [DataContract]
    public class ExplodeResult : BaseResult
    {
        /// <summary>
        /// Status is returned when passing 'Async' argument set to 'true' to the server.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; protected set; }

        /// <summary>
        /// Id of the batch.
        /// </summary>
        [DataMember(Name = "batch_id")]
        public string BatchId { get; protected set; }

        /// <inheritdoc/>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            Status = source.ReadValueAsSnakeCase<string>(nameof(Status));
            BatchId = source.ReadValueAsSnakeCase<string>(nameof(BatchId));
        }
    }
}
