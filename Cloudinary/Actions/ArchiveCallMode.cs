using System.ComponentModel;

namespace CloudinaryDotNet.Actions
{
    public enum ArchiveCallMode
    {
        /// <summary>
        ///  Indicates to return the generated archive file
        /// </summary>
        [Description("download")]
        Download,
        /// <summary>
        /// Indicates to store the generated archive file as a raw resource in your Cloudinary account and return a JSON with the URLs for accessing it
        /// </summary>
        [Description("create")]
        Create
    }
}
