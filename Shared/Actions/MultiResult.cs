using System;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parsed response with detailed information about the created animated GIF.
    /// </summary>
    [DataContract]
    public class MultiResult : BaseResult
    {
        /// <summary>
        /// The URL for accessing the created animated GIF.
        /// </summary>
        [DataMember(Name = "url")]
        public Uri Uri { get; protected set; }

        /// <summary>
        /// The HTTPS URL for securely accessing the created animated GIF.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public Uri SecureUri { get; protected set; }

        /// <summary>
        /// Public ID assigned to the created GIF. 
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        /// <summary>
        /// Version of the created GIF.
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; protected set; }
        
    }
}
