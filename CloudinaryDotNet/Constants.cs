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

        /// <summary> API URL pattern for metadata fields.</summary>
        public const string METADATA_FIELDS_API_URL = "metadata_fields";

        /// <summary> API URL action name for metadata operations.</summary>
        public const string METADATA = "metadata";

        /// <summary> API URL action name for access mode update.</summary>
        public const string UPDATE_ACESS_MODE = "update_access_mode";

        /// <summary> API URL action name for tags management.</summary>
        public const string TAGS_MANGMENT = "tags";

        /// <summary> API URL action name for context management.</summary>
        public const string CONTEXT_MANAGMENT = "context";

        /// <summary> API URL action name for data source management.</summary>
        public const string DATASOURCE_MANAGMENT = "datasource";

        /// <summary> Parameter name for tag.</summary>
        public const string TAG_PARAM_NAME = "tag";

        /// <summary> Parameter name for context.</summary>
        public const string CONTEXT_PARAM_NAME = "context";

        /// <summary> Parameter name for metadata.</summary>
        public const string METADATA_PARAM_NAME = "metadata";

        /// <summary> Parameter name for prefix.</summary>
        public const string PREFIX_PARAM_NAME = "prefix";

        /// <summary> Parameter name for public IDs.</summary>
        public const string PUBLIC_IDS = "public_ids";

        /// <summary> Parameter name for tag.</summary>
        public const string TYPE_PARAM_NAME = "type";

        /// <summary> Parameter name for command.</summary>
        public const string COMMAND = "command";

        /// <summary> Resource type name for fetch.</summary>
        public const string RESOURCE_TYPE_FETCH = "fetch";

        /// <summary> Resource type name for image asset.</summary>
        public const string RESOURCE_TYPE_IMAGE = "image";

        /// <summary> Resource type name for video asset.</summary>
        public const string RESOURCE_TYPE_VIDEO = "video";

        /// <summary> Resource type name for all asset types.</summary>
        public const string RESOURCE_TYPE_ALL = "all";

        /// <summary> Parameter name for asset resource type.</summary>
        public const string RESOURCE_TYPE = "resource_type";

        /// <summary> Action name for upload.</summary>
        public const string ACTION_NAME_UPLOAD = "upload";

        /// <summary> Action name for fetch.</summary>
        public const string ACTION_NAME_FETCH = "fetch";

        /// <summary> Action name for multi.</summary>
        public const string ACTION_NAME_MULTI = "multi";

        /// <summary> Action name for multi.</summary>
        public const string ACTION_NAME_SPRITE = "sprite";

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

        /// <summary> Part of general API URL pattern for account api.</summary>
        public const string PROVISIONING = "provisioning";

        /// <summary> Part of general API URL pattern for account api.</summary>
        public const string ACCOUNTS = "accounts";

        /// <summary> Sub-accounts resource of account api.</summary>
        public const string SUB_ACCOUNTS = "sub_accounts";

        /// <summary> Users resource of account api.</summary>
        public const string USERS = "users";

        /// <summary> User groups resource of account api.</summary>
        public const string USER_GROUPS = "user_groups";
    }
}
