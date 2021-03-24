namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Possible roles of a user.
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// Has access to all elements of the Cloudinary console, including user
        /// and account management, billing details and purchase/upgrade options,
        /// and full permissions to use all Cloudinary functionality.
        /// </summary>
        [EnumMember(Value = "master_admin")]
        MaserAdmin,

        /// <summary>
        /// Same as a master admin, except that they do not have access to
        /// account management, billing details and purchase/upgrade options.
        /// </summary>
        [EnumMember(Value = "admin")]
        Admin,

        /// <summary>
        /// Can access only billing-related areas of the Cloudinary management console,
        /// including the Billing tab (for paid accounts), usage reports, and purchase/upgrade options.
        /// </summary>
        [EnumMember(Value = "billing")]
        Billing,

        /// <summary>
        /// Same as an Admin, except they do not have access to the List of users area of the User Settings.
        /// </summary>
        [EnumMember(Value = "technical_admin")]
        TechnicalAdmin,

        /// <summary>
        /// Can access only reporting details in the Cloudinary console,
        /// including those in the Dashboard and in the Reports tabs.
        /// </summary>
        [EnumMember(Value = "reports")]
        Reports,

        /// <summary>
        /// Full read-write access to all areas of the Cloudinary console that are related to asset management.
        /// </summary>
        [EnumMember(Value = "media_library_admin")]
        MediaLibraryAdmin,

        /// <summary>
        /// Can access only the Media Library area of the console. The specific read, write,
        /// and other access permissions that a user has within the Media Library are controlled
        /// by the user groups that the user belongs to and the folders that are shared with those user groups.
        /// </summary>
        [EnumMember(Value = "media_library_user")]
        MediaLibraryUser,
    }
}
