namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

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
    }
}
