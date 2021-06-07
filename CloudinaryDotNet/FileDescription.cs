namespace CloudinaryDotNet
{
    using System.IO;

    /// <summary>
    /// Represents a file for uploading to cloudinary.
    /// </summary>
    public class FileDescription
    {
        /// <summary>
        /// Maximum size of a single chunk of data to be uploaded.
        /// </summary>
        internal int BufferLength = int.MaxValue;

        /// <summary>
        /// Byte sent.
        /// </summary>
        internal long BytesSent;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDescription"/> class.
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
        /// Initializes a new instance of the <see cref="FileDescription"/> class.
        /// Constructor to upload file by path specifying explicit filename.
        /// </summary>
        /// <param name="name">Resource name.</param>
        /// <param name="filePath">Either URL (http/https/s3/data) or local path to file.</param>
        public FileDescription(string name, string filePath)
        {
            IsRemote = Utils.IsRemoteFile(filePath);
            FilePath = filePath;
            FileName = IsRemote ? filePath : name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDescription"/> class.
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
        /// Gets stream to upload.
        /// </summary>
        public Stream Stream { get; }

        /// <summary>
        /// Gets or sets name of the file to upload.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets filesystem path to the file to upload.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Gets a value indicating whether it is remote (by URL) or local file.
        /// </summary>
        public bool IsRemote { get; }

        /// <summary>
        /// Gets a value indicating whether the pointer is at the end of file.
        /// </summary>
        internal bool Eof => BytesSent == GetFileLength();

        /// <summary>
        /// Get file length.
        /// </summary>
        /// <returns>The length of file.</returns>
        internal long GetFileLength()
        {
            return Stream?.Length ?? new FileInfo(FilePath).Length;
        }

        /// <summary>
        /// Reset stream buffer length and bytes sent values.
        /// </summary>
        /// <param name="bufferSize">(Optional) Size of the buffer.</param>
        internal void Reset(int bufferSize = int.MaxValue)
        {
            BufferLength = bufferSize;
            BytesSent = 0;
        }
    }
}
