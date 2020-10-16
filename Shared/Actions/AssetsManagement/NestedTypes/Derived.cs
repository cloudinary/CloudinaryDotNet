namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The list of derived assets generated (and cached) from the original media asset, including the transformation
    /// applied, size and URL for accessing the derived media asset.
    /// </summary>
    [DataContract]
    public class Derived
    {
        /// <summary>
        /// Gets or sets the transformation applied to the asset.
        /// </summary>
        [DataMember(Name = "transformation")]
        public string Transformation { get; set; }

        /// <summary>
        /// Gets or sets format of the derived asset.
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets size of the derived asset.
        /// </summary>
        [Obsolete("Property Length is deprecated, please use Bytes instead")]
        public long Length
        {
            get { return Bytes; }
            set { Bytes = value; }
        }

        /// <summary>
        /// Gets or sets size of the derived asset.
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Bytes { get; set; }

        /// <summary>
        /// Gets or sets id of the derived resource.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets uRL for accessing the derived media asset.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the HTTPS URL for securely accessing the derived media asset.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; set; }
    }
}