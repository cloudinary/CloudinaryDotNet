using System.ComponentModel;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// The format for the generated archive
    /// </summary>
    public enum ArchiveFormat
    {
        /// <summary>
        /// Specifies ZIP format for an archive
        /// </summary>
        [EnumMember(Value = "zip")]
        Zip
    }
}
