namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed response with detailed information about the created animated GIF.
    /// </summary>
    [DataContract]
    public class MultiResult : BaseResult
    {
        /// <summary>
        /// Gets or sets the URL for accessing the created animated GIF.
        /// </summary>
        [DataMember(Name = "url")]
        public Uri Uri { get; protected set; }

        /// <summary>
        /// Gets or sets the HTTPS URL for securely accessing the created animated GIF.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public Uri SecureUri { get; protected set; }

        /// <summary>
        /// Gets or sets public ID assigned to the created GIF.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        /// <summary>
        /// Gets or sets version of the created GIF.
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; protected set; }
    }
}
