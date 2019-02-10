using System;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// The report on the status of your Cloudinary account usage details. Note that numbers are updated periodically.
    /// </summary>
    [DataContract]
    public class UsageResult : BaseResult
    {
        /// <summary>
        /// Your current plan.
        /// </summary>
        [DataMember(Name = "plan")]
        public string Plan { get; protected set; }

        /// <summary>
        /// Date of the last report update.
        /// </summary>
        [DataMember(Name = "last_updated")]
        public DateTime LastUpdated { get; protected set; }

        /// <summary>
        /// Information about the objects in your account.
        /// </summary>
        [DataMember(Name = "objects")]
        public Usage Objects { get; protected set; }

        /// <summary>
        /// Information about usage of bandwith in your account.
        /// </summary>
        [DataMember(Name = "bandwidth")]
        public Usage Bandwidth { get; protected set; }

        /// <summary>
        /// Information about usage of storage in your account.
        /// </summary>
        [DataMember(Name = "storage")]
        public Usage Storage { get; protected set; }

        /// <summary>
        /// Count of requests used.
        /// </summary>
        [DataMember(Name = "requests")]
        public int Requests { get; protected set; }

        /// <summary>
        /// Count of resources in your account.
        /// </summary>
        [DataMember(Name = "resources")]
        public int Resources { get; protected set; }

        /// <summary>
        /// Count of derived resources.
        /// </summary>
        [DataMember(Name = "derived_resources")]
        public int DerivedResources { get; protected set; }
        
    }

    /// <summary>
    /// Usage information about the objects in account.
    /// </summary>
    [DataContract]
    public class Usage
    {
        /// <summary>
        /// A number of objects in your account.
        /// </summary>
        [DataMember(Name = "usage")]
        public long Used { get; protected set; }

        /// <summary>
        /// Current limit of objects for account.
        /// </summary>
        [DataMember(Name = "limit")]
        public long Limit { get; protected set; }

        /// <summary>
        /// Current usage percentage.
        /// </summary>
        [DataMember(Name = "used_percent")]
        public float UsedPercent { get; protected set; }
    }
}
