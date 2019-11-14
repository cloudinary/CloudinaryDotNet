using System;
using System.IO;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Represents a file for uploading to cloudinary.
    /// </summary>
    public class FileDescription
    {
        /// <summary>
        /// Maximum size of a single chunk of data to be uploaded
        /// </summary>
        internal int BufferLength = Int32.MaxValue;

        /// <summary>
        /// End of file flag
        /// </summary>
        internal bool Eof => BytesSent == GetFileLength();

        /// <summary>
        /// Byte sent
        /// </summary>
        internal int BytesSent;

        /// <summary>
        /// Get file length
        /// </summary>
        /// <returns></returns>
        internal long GetFileLength()
        {
            return Stream?.Length ?? new FileInfo(FilePath).Length;
        }

        /// <summary>
        /// Reset stream buffer length and bytes sent values.
        /// </summary>
        /// <param name="bufferSize"></param>
        internal void Reset(int bufferSize = int.MaxValue)
        {
            BufferLength = bufferSize;
            BytesSent = 0;
        }

        /// <summary>
        /// Constructor to upload file from stream.
        /// </summary>
        /// <param name="name">Resource name.</param>
        /// <param name="stream">Stream to read from (will be disposed with this object).</param>
        public FileDescription(string name, Stream stream)
        {
            FileName = name;
            Stream = stream;
        }
        /// <summary>
        /// Constructor to upload file by path specifying explicit filename
        /// </summary>
        /// <param name="name">Resource name</param>
        /// <param name="filePath">Either URL (http/https/s3/data) or local path to file</param>
        public FileDescription(string name, string filePath)
        {
            IsRemote = Utils.IsRemoteFile(filePath);
            FilePath = filePath;
            FileName = IsRemote ? filePath : name;
        }
        /// <inheritdoc />
        /// <summary>
        /// Constructor to upload file by path.
        /// </summary>
        /// <param name="filePath">Either URL (http/https/s3/data) or local path to file.</param>
        public FileDescription(string filePath)
        {
            IsRemote = Utils.IsRemoteFile(filePath);
            FilePath = filePath;
            FileName = IsRemote ? filePath : Path.GetFileName(filePath);
        }

        /// <summary>
        /// Stream to upload
        /// </summary>
        public Stream Stream { get; }

        /// <summary>
        /// Name of the file to upload
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Filesystem path to the file to upload
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Whether it is remote (by URL) or local file
        /// </summary>
        public bool IsRemote { get; }
    }
}
