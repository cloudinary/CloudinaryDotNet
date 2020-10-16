namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Details of the derived image generated as per the requested eager transformation applied.
    /// </summary>
    [DataContract]
    public class Eager
    {
        /// <summary>
        /// Gets or sets URL for accessing the asset.
        /// </summary>
        [Obsolete("Property Uri is deprecated, please use Url instead")]
        public Uri Uri
        {
            get { return Url; }
            set { Url = value; }
        }

        /// <summary>
        /// Gets or sets the URL for accessing the uploaded asset.
        /// </summary>
        [DataMember(Name = "url")]
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets the HTTPS URL for securely accessing the asset.
        /// </summary>
        [Obsolete("Property SecureUri is deprecated, please use SecureUrl instead")]
        public Uri SecureUri
        {
            get { return SecureUrl; }
            set { SecureUrl = value; }
        }

        /// <summary>
        /// Gets or sets the HTTPS URL for securely accessing the asset.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public Uri SecureUrl { get; set; }

        /// <summary>
        /// Gets or sets the transformation applied to the asset.
        /// </summary>
        [DataMember(Name = "transformation")]
        public string Transformation { get; set; }

        /// <summary>
        /// Gets or sets parameter "width" of the resource. Not relevant for raw files.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets parameter "height" of the resource. Not relevant for raw files.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the size of the media asset in bytes.
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Bytes { get; set; }

        /// <summary>
        /// Gets or sets asset format.
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; set; }
    }
}