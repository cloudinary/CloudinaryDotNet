namespace CloudinaryDotNet.Core
{
    using System;
    using System.IO;

    /// <summary>
    /// Represents a chunk of data along with its start and end bytes, and total bytes.
    /// </summary>
    public class ChunkData
    {
        private bool lastChunk;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChunkData"/> class.
        /// </summary>
        /// <param name="chunk">The chunk of data as a stream.</param>
        /// <param name="startByte">The index of the start byte in the original stream.</param>
        /// <param name="endByte">The index of the end byte in the original stream.</param>
        /// <param name="totalBytes">The total number of bytes in the original stream.</param>
        public ChunkData(Stream chunk, long startByte, long endByte, long totalBytes)
        {
            Chunk = chunk ?? throw new ArgumentNullException(nameof(chunk));
            StartByte = startByte;
            EndByte = endByte;
            TotalBytes = totalBytes;
        }

        /// <summary>
        /// Gets or sets the chunk of data as a stream.
        /// </summary>
        public Stream Chunk { get; set; }

        /// <summary>
        /// Gets or sets the index of the start byte in the original stream.
        /// </summary>
        public long StartByte { get; set; }

        /// <summary>
        /// Gets or sets the index of the end byte in the original stream.
        /// </summary>
        public long EndByte { get; set; }

        /// <summary>
        /// Gets or sets the total number of bytes in the original stream.
        /// </summary>
        public long TotalBytes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether current chunk is the last one.
        /// </summary>
        public bool LastChunk
        {
            get => lastChunk ? lastChunk : TotalBytes != -1 && TotalBytes == EndByte + 1;
            set => lastChunk = value;
        }
    }
}
