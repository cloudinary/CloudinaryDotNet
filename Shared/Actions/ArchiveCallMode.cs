namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Either return a URL to the generated archive file ('download') or store it as a raw asset
    /// in your Cloudinary account('create').
    /// </summary>
    public enum ArchiveCallMode
    {
        /// <summary>
        /// Generates a signed URL that expires after 1 hour (by default). The URL can be accessed to dynamically
        /// create and then download an archive file based on the given parameter values. The resulting archive file
        /// is not cached or stored in your Cloudinary account.
        /// </summary>
        [EnumMember(Value = "download")]
        Download,

        /// <summary>
        /// Generates an archive file based on the given parameter values (default target_format = zip), uploads the
        /// file to your Cloudinary account, returns a JSON response with the URLs for accessing the archive file,
        /// and can then be delivered like any other raw file uploaded to Cloudinary.
        /// </summary>
        [EnumMember(Value = "create")]
        Create,
    }
}
