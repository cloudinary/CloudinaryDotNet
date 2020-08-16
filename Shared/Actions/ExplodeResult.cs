namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed response after a call of Explode method.
    /// </summary>
    [DataContract]
    public class ExplodeResult : BaseResult
    {
        /// <summary>
        /// Gets or sets status that is returned when passing 'Async' argument set to 'true' to the server.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets id of the batch.
        /// </summary>
        [DataMember(Name = "batch_id")]
        public string BatchId { get; set; }
    }
}
