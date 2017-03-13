using System.ComponentModel;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    public enum ArchiveCallMode
    {
        /// <summary>
        ///  Indicates to return the generated archive file
        /// </summary>
        [EnumMember(Value = "download")]
        Download,
        /// <summary>
        /// Indicates to store the generated archive file as a raw resource in your Cloudinary account and return a JSON with the URLs for accessing it
        /// </summary>
        [EnumMember(Value = "create")]
        Create
    }
}
