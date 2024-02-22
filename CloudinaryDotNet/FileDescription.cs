namespace CloudinaryDotNet
{
    using System.IO;

    /// <summary>
    /// Represents a file for uploading to cloudinary.
    /// </summary>
    public class FileDescription
    {
        /// <summary>
        /// Indicates whether the file/stream content represents the last chunk of a large file.
        /// </summary>
        public bool LastChunk;

        /// <summary>
        /// Indicates whether the file/stream content represents a chunk of a large file.
        /// </summary>
        internal bool Chunked;

        /// <summary>
        /// Maximum size of a single chunk of data to be uploaded.
        /// </summary>
        internal int BufferSize = int.MaxValue;

        /// <summary>
        /// Current position (offset) in file (full file).
        /// </summary>
        internal long CurrPos;

        /// <summary>
        /// Current position (offset) in the current chunk.
        /// </summary>
        internal long CurrChunkPos;

        /// <summary>
        /// Current chunk size.
        /// </summary>
        internal long CurrChunkSize;

        private bool isEof;

        private Stream stream;

        private string filePath;

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
        /// Gets or sets stream to upload.
        /// </summary>
        public Stream Stream
        {
            get => stream;
            set
            {
                stream = value;
                CurrChunkPos = 0;
            }
        }

        /// <summary>
        /// Gets or sets name of the file to upload.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets filesystem path to the file to upload.
        /// </summary>
        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                CurrChunkPos = 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether it is remote (by URL) or local file.
        /// </summary>
        public bool IsRemote { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the pointer is at the end of file.
        /// </summary>
        internal bool Eof
        {
            get => isEof ? isEof : GetFileLength() != -1 && CurrPos == GetFileLength();
            set => isEof = value;
        }

        /// <summary>
        /// Get file length.
        /// </summary>
        /// <returns>The length of file.</returns>
        internal long GetFileLength()
        {
            if (Chunked)
            {
                return -1; // unknown length
            }

            if (Stream == null)
            {
                return new FileInfo(FilePath).Length;
            }

            if (Stream?.CanSeek ?? false)
            {
                return Stream?.Length ?? -1;
            }

            return -1; // unknown length
        }

        /// <summary>
        /// Reset stream buffer length and bytes sent values.
        /// </summary>
        /// <param name="bufferSize">(Optional) Size of the buffer.</param>
        internal void Reset(int bufferSize = int.MaxValue)
        {
            BufferSize = bufferSize;
            CurrPos = 0;
            CurrChunkPos = 0;
            LastChunk = false;
        }

        /// <summary>
        /// Shift current position by an offset value.
        /// </summary>
        /// <param name="offset">The offset to apply.</param>
        internal void ShiftCurrPos(long offset)
        {
            CurrPos += offset;
            CurrChunkPos += offset;
        }
    }
}
