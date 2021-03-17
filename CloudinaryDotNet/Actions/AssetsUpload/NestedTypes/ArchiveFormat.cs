namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The format for the generated archive.
    /// </summary>
    public enum ArchiveFormat
    {
        /// <summary>
        /// Specifies ZIP format for an archive
        /// </summary>
        [EnumMember(Value = "zip")]
        Zip,
    }
}
