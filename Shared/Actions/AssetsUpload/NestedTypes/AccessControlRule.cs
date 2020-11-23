namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Access type for the asset.
    /// </summary>
    public enum AccessType
    {
        /// <summary>
        /// Allows public access to the asset. The anonymous access type can optionally include start and/or end
        /// dates (in ISO 8601 format) that define when the asset is publically available.
        /// </summary>
        [EnumMember(Value = "anonymous")]
        Anonymous,

        /// <summary>
        /// Requires either the token-based authentication or the cookie-based authentication for accessing the asset.
        /// </summary>
        [EnumMember(Value = "token")]
        Token,
    }

    /// <summary>
    /// Stores parameters for the asset access management.
    /// </summary>
    public class AccessControlRule
    {
        /// <summary>
        /// Gets or sets access type for the asset.
        /// </summary>
        [JsonProperty(PropertyName = "access_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AccessType AccessType { get; set; }

        /// <summary>
        /// Gets or sets start date that defines when the asset is publicly available.
        /// </summary>
        [JsonProperty(PropertyName = "start", NullValueHandling=NullValueHandling.Ignore)]
        public DateTime? Start { get; set; }

        /// <summary>
        /// Gets or sets end date that defines when the asset is publicly available.
        /// </summary>
        [JsonProperty(PropertyName = "end", NullValueHandling=NullValueHandling.Ignore)]
        public DateTime? End { get; set; }
    }
}
