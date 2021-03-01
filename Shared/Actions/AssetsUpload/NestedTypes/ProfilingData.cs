namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents processing statistics data.
    /// </summary>
    [DataContract]
    public class ProfilingData
    {
        /// <summary>
        /// Gets or sets cpu usage metrics.
        /// </summary>
        [DataMember(Name = "cpu")]
        public long Cpu { get; set; }

        /// <summary>
        /// Gets or sets real metrics.
        /// </summary>
        [DataMember(Name = "real")]
        public long Real { get; set; }

        /// <summary>
        /// Gets or sets processing statistics data about actions on resource.
        /// </summary>
        [DataMember(Name = "action")]
        public ProfilingDataAction Action { get; set; }
    }
}