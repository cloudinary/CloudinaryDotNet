namespace CloudinaryDotNet
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Group of the most widely used API constants.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    internal static class Constants
    {
        /// <summary> API URL pattern for resources.</summary>
        public const string RESOURCES_API_URL = "resources";

        /// <summary> API URL pattern for tags.</summary>
        public const string TAG_API_URL = "tag";

        /// <summary> API URL pattern for streaming profiles.</summary>
        public const string STREAMING_PROFILE_API_URL = "streaming_profiles";

        /// <summary> API URL action name for access mode update.</summary>
        public const string UPDATE_ACESS_MODE = "update_access_mode";

        /// <summary> API URL action name for tags management.</summary>
        public const string TAGS_MANGMENT = "tags";

        /// <summary> API URL action name for context management.</summary>
        public const string CONTEXT_MANAGMENT = "context";

        /// <summary> Parameter name for tag.</summary>
        public const string TAG_PARAM_NAME = "tag";

        /// <summary> Parameter name for context.</summary>
        public const string CONTEXT_PARAM_NAME = "context";

        /// <summary> Parameter name for prefix.</summary>
        public const string PREFIX_PARAM_NAME = "prefix";

        /// <summary> Parameter name for public IDs.</summary>
        public const string PUBLIC_IDS = "public_ids";

        /// <summary> Parameter name for command.</summary>
        public const string COMMAND = "command";

        /// <summary> Resource type name for fetch.</summary>
        public const string RESOURCE_TYPE_FETCH = "fetch";

        /// <summary> Resource type name for image asset.</summary>
        public const string RESOURCE_TYPE_IMAGE = "image";

        /// <summary> Resource type name for video asset.</summary>
        public const string RESOURCE_TYPE_VIDEO = "video";

        /// <summary> Action name for upload.</summary>
        public const string ACTION_NAME_UPLOAD = "upload";

        /// <summary> Action name for fetch.</summary>
        public const string ACTION_NAME_FETCH = "fetch";

        /// <summary> URL for shared CDN.</summary>
        public const string CF_SHARED_CDN = "d3jpl91pxevbkh.cloudfront.net";

        /// <summary> URL for old Akamai shared CDN.</summary>
        public const string OLD_AKAMAI_SHARED_CDN = "cloudinary-a.akamaihd.net";

        /// <summary> URL for Akamai shared CDN.</summary>
        public const string AKAMAI_SHARED_CDN = "res.cloudinary.com";

        /// <summary> Alias for Akamai shared CDN.</summary>
        public const string SHARED_CDN = AKAMAI_SHARED_CDN;

        /// <summary> HTTP protocol Content-Type header name.</summary>
        public const string HEADER_CONTENT_TYPE = "Content-Type";

        /// <summary> HTTP protocol Content-Type header value for application/json.</summary>
        public const string CONTENT_TYPE_APPLICATION_JSON = "application/json";
    }
}
