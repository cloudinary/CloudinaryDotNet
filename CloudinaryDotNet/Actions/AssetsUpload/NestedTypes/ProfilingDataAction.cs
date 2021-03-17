namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents processing statistics data about actions on resource.
    /// </summary>
    [DataContract]
    public class ProfilingDataAction
    {
        /// <summary>
        /// Gets or sets action name applied to resource asset.
        /// </summary>
        [DataMember(Name = "action")]
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets action parameter applied to resource asset.
        /// </summary>
        [DataMember(Name = "parameter")]
        public string Parameter { get; set; }

        /// <summary>
        /// Gets or sets size before applied action.
        /// </summary>
        [DataMember(Name = "presize")]
        public long[] Presize { get; set; }

        /// <summary>
        /// Gets or sets size after applied action.
        /// </summary>
        [DataMember(Name = "postsize")]
        public long[] Postsize { get; set; }
    }
}