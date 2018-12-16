using System;
using System.IO;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Represents a file for uploading to cloudinary
    /// </summary>
    public abstract class FileDescriptionBase
    {
        internal int BufferLength = Int32.MaxValue;
        internal bool Eof;
        internal int BytesSent;

        internal long GetFileLength()
        {
            return Stream?.Length ?? new FileInfo(FilePath).Length;
        }
        
        internal void Reset(int bufferSize = int.MaxValue)
        {
            BufferLength = bufferSize;
            Eof = false;
            BytesSent = 0;
        }

        /// <summary>
        /// Constructor to upload file from stream
        /// </summary>
        /// <param name="name">Resource name</param>
        /// <param name="stream">Stream to read from (will be disposed with this object)</param>
        public FileDescriptionBase(string name, Stream stream)
        {
            FileName = name;
            Stream = stream;
        }

        /// <summary>
        /// Constructor to upload file by path
        /// </summary>
        /// <param name="filePath">Either URL (http/https/s3/data) or local path to file</param>
        public FileDescriptionBase(string filePath)
        {
            IsRemote = Utils.IsRemoteFile(filePath);
            FilePath = filePath;
            FileName = IsRemote ? filePath : Path.GetFileName(FilePath);
        }

        /// <summary>
        /// Stream to upload
        /// </summary>
        public Stream Stream { get; }

        /// <summary>
        /// Name of the file to upload
        /// </summary>
        public string FileName { get; }

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
