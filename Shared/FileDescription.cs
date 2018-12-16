using System.IO;

namespace CloudinaryDotNet
{
    public class FileDescription : FileDescriptionBase
    {
        /// <inheritdoc />
        /// <summary>
        /// Constructor to upload file from stream
        /// </summary>
        /// <param name="name">Resource name</param>
        /// <param name="stream">Stream to read from (will be disposed with this object)</param>
        public FileDescription(string name, Stream stream) : base(name, stream)
        {}

        /// <inheritdoc />
        /// <summary>
        /// Constructor to upload file by path
        /// </summary>
        /// <param name="filePath">Either URL (http/https/s3/data) or local path to file</param>
        public FileDescription(string filePath) : base(filePath)
        {}
    }
}
