namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Usage information about the objects in account.
    /// </summary>
    [DataContract]
    public class Usage
    {
        /// <summary>
        /// Gets or sets a number of objects in your account.
        /// </summary>
        [DataMember(Name = "usage")]
        public long Used { get; set; }

        /// <summary>
        /// Gets or sets current limit of objects for account.
        /// </summary>
        [DataMember(Name = "limit")]
        public long Limit { get; set; }

        /// <summary>
        /// Gets or sets current usage percentage.
        /// </summary>
        [DataMember(Name = "used_percent")]
        public float UsedPercent { get; set; }

        /// <summary>
        /// Gets or sets current usage of credits.
        /// </summary>
        [DataMember(Name = "credits_usage")]
        public float CreditsUsage { get; set; }
    }
}