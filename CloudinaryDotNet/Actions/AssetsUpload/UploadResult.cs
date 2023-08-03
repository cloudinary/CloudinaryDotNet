namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Results of uploading the asset.
    /// </summary>
    [DataContract]
    public abstract class UploadResult : BaseResult
    {
        /// <summary>
        /// Gets or sets the ID of uploaded asset.
        /// </summary>
        [DataMember(Name = "asset_id")]
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets public ID of uploaded asset.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets the folder where the asset is stored in the Media Library.
        /// This value does not impact the asset’s Public ID.
        /// </summary>
        [DataMember(Name = "asset_folder")]
        public string AssetFolder { get; set; }

        /// <summary>
        /// Gets or sets the name that is displayed for the asset in the Media Library.
        /// This value does not impact the asset’s Public ID.
        /// </summary>
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets version of uploaded asset.
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the URL for accessing the uploaded asset.
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
        /// Gets or sets the HTTPS URL for securely accessing the uploaded asset.
        /// </summary>
        [Obsolete("Property SecureUri is deprecated, please use SecureUrl instead")]
        public Uri SecureUri
        {
            get { return SecureUrl; }
            set { SecureUrl = value; }
        }

        /// <summary>
        /// Gets or sets the HTTPS URL for securely accessing the uploaded asset.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public Uri SecureUrl { get; set; }

        /// <summary>
        /// Gets or sets resource length in bytes.
        /// </summary>
        [Obsolete("Property Length is deprecated, please use Bytes instead")]
        public long Length
        {
            get { return Bytes; }
            set { Bytes = value; }
        }

        /// <summary>
        /// Gets or sets resource length in bytes..
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Bytes { get; set; }

        /// <summary>
        /// Gets or sets asset format.
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets a key-value pairs of custom metadata fields associated with the resource.
        /// </summary>
        [DataMember(Name = "metadata")]
        public JToken MetadataFields { get; set; }

        /// <summary>
        /// Gets or sets upload hook execution status.
        /// </summary>
        [DataMember(Name = "hook_execution")]
        public JToken HookExecution { get; set; }
    }
}
