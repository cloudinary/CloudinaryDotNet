namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Core;

    /// <summary>
    /// Represents a file for uploading to cloudinary.
    /// </summary>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Reviewed.")]
    public class FileDescription : IDisposable
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
        internal int BufferSize = UnlimitedBuffer;

        /// <summary>
        /// Current chunk size.
        /// </summary>
        internal long CurrChunkSize;

        /// <summary>
        /// Represents chunks of file for upload.
        /// </summary>
        protected BlockingCollection<ChunkData> chunks;

        private const int UnlimitedBuffer = int.MaxValue;

        private readonly Mutex mutex = new ();

        private readonly object chunkLock = new ();

        private readonly object streamLock = new ();

        private Stream fileStream;

        private string filePath;

        private long currPos;

        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDescription"/> class with default values.
        /// </summary>
        public FileDescription()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDescription"/> class.
        /// </summary>
        /// <param name="original">The original object to copy from.</param>
        public FileDescription(FileDescription original)
        {
            original.CopyPropertiesTo(this);
        }

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
            FilePath = filePath;
            FileName = IsRemote ? filePath : name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDescription"/> class.
        /// Constructor to upload file by path specifying explicit filename.
        /// </summary>
        /// <param name="name">Resource name.</param>
        /// <param name="chunked">Indicates whether we want to use chunked upload and chunks will be passed.</param>
        public FileDescription(string name, bool chunked = true)
        {
            FileName = name;
            Chunked = chunked;
            chunks = new BlockingCollection<ChunkData>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDescription"/> class.
        /// Constructor to upload file by path.
        /// </summary>
        /// <param name="filePath">Either URL (http/https/s3/data) or local path to file.</param>
        public FileDescription(string filePath)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// Gets or sets stream to upload.
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// Gets or sets name of the file to upload.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets current position (offset) in file (full file).
        /// </summary>
        public long CurrPos
        {
            get => currPos;
            set => Interlocked.Exchange(ref currPos, value);
        }

        /// <summary>
        /// Gets or sets filesystem path to the file to upload.
        /// </summary>
        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                IsRemote = Utils.IsRemoteFile(filePath);
                if (IsRemote)
                {
                    FileName = filePath;
                }
                else
                {
                    FileName = Path.GetFileName(filePath);
                    FileSize = GetFileLength();
                }

                Reset();
            }
        }

        /// <summary>
        /// Gets or sets file size.
        /// </summary>
        public long FileSize { get; set; } = -1;

        /// <summary>
        /// Gets a value indicating whether it is remote (by URL) or local file.
        /// </summary>
        public bool IsRemote { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the pointer is at the end of file.
        /// </summary>
        internal bool Eof { get; private set; }

        /// <summary>
        /// Gets or creates a file stream( if applicable).
        /// </summary>
        /// <returns>File stream or null for remote or uploads from single chunks.</returns>
        public Stream GetFileStream()
        {
            if (Stream != null)
            {
                return Stream;
            }
            #if NETSTANDARD1_3
            return fileStream ??= IsRemote || string.IsNullOrEmpty(FilePath) ? null : File.OpenRead(FilePath);
            #else
            return fileStream ??= IsRemote || string.IsNullOrEmpty(FilePath) ? null : Stream.Synchronized(File.OpenRead(FilePath));
            #endif
        }

        /// <summary>
        /// Adds a single chunk.
        /// </summary>
        /// <param name="chunkStream">The chunk content.</param>
        /// <param name="last"> Indicates whether the chunk represents the last chunk of a large file.</param>
        public void AddChunk(Stream chunkStream, bool last = false)
        {
            if (!chunkStream.CanSeek)
            {
                throw new NotSupportedException("Cannot add non-seekable streams without specifying their size. Use a different overload method.");
            }

            AddChunk(chunkStream, currPos, chunkStream.Length, last);
        }

        /// <summary>
        /// Adds a single chunk.
        /// </summary>
        /// <param name="chunkPath">The chunk file path.</param>
        /// <param name="last"> Indicates whether the chunk represents the last chunk of a large file.</param>
        public void AddChunk(string chunkPath, bool last = false)
        {
            var chunkStream = File.Open(chunkPath, FileMode.Open);

            AddChunk(chunkStream, last);
        }

        /// <summary>
        /// Adds a single chunk.
        /// </summary>
        /// <param name="chunkPaths">A List of the file chunks paths.</param>
        public void AddChunks(List<string> chunkPaths)
        {
            foreach (var chunkPath in chunkPaths)
            {
                AddChunk(chunkPath, chunkPaths.IndexOf(chunkPath) == chunkPaths.Count - 1);
            }
        }

        /// <summary>
        /// Adds a single chunk.
        /// </summary>
        /// <param name="chunkStreams">A List of the file chunks streams.</param>
        public void AddChunks(List<Stream> chunkStreams)
        {
            foreach (var chunkStream in chunkStreams)
            {
                AddChunk(chunkStream, chunkStreams.IndexOf(chunkStream) == chunkStreams.Count - 1);
            }
        }

        /// <summary>
        /// Adds a single chunk.
        /// </summary>
        /// <param name="chunkStream">The chunk content.</param>
        /// <param name="startByte">Chunk start offset.</param>
        /// <param name="chunkSize">Chunk size.</param>
        /// <param name="last"> Indicates whether the chunk represents the last chunk of a large file.</param>
        public void AddChunk(Stream chunkStream, long startByte, long chunkSize, bool last = false)
        {
            lock (chunkLock)
            {
                chunks ??= new BlockingCollection<ChunkData>();
            }

            CurrPos = startByte + chunkSize;

            if (last)
            {
                FileSize = CurrPos;
            }

            var limitedStream = new LimitedStream(chunkStream, 0, chunkSize, streamLock);
            var chunk = new ChunkData(limitedStream, startByte, CurrPos - 1, FileSize)
            {
                LastChunk = last,
            };

            chunks.Add(chunk);

            if (last)
            {
                chunks.CompleteAdding();
            }
        }

        /// <summary>
        /// Gets a range from file asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>ChunkData class representing a single chunk.</returns>
        public async Task<ChunkData> GetNextChunkAsync(CancellationToken? cancellationToken = null)
        {
            // lock this section, so we don't send the same chunk multiple times.
            mutex.WaitOne();
            try
            {
                Stream resultingStream;

                var chunkStream = GetFileStream();

                if (chunkStream == null)
                {
                    return TakeNextChunk();
                }

                if (chunkStream.CanSeek)
                {
                    // Create a limited stream over the stream and return it.
                    CurrChunkSize = Math.Min(BufferSize, chunkStream.Length - CurrPos);
                    resultingStream = new LimitedStream(chunkStream, CurrPos, CurrChunkSize, streamLock);
                    if ((CurrChunkSize < BufferSize && BufferSize != UnlimitedBuffer) || CurrPos + CurrChunkSize == FileSize)
                    {
                        LastChunk = true;
                    }
                }
                else
                {
                    // We actually need to read data, otherwise we will not know the size of the chunk.
                    resultingStream = new MemoryStream();
                    using var writer = new StreamWriter(resultingStream, Encoding.ASCII, 1024, leaveOpen: true);
                    writer.AutoFlush = true;

                    CurrChunkSize = await ReadBytesAsync(chunkStream, writer, BufferSize, cancellationToken).ConfigureAwait(false);

                    resultingStream.Seek(0, SeekOrigin.Begin);
                }

                var chunk = new ChunkData(resultingStream, CurrPos, CurrPos + CurrChunkSize - 1, FileSize);

                CurrPos += CurrChunkSize;

                if ((BufferSize != UnlimitedBuffer && CurrChunkSize < BufferSize) || LastChunk)
                {
                    Eof = true;
                    chunk.TotalBytes = FileSize = CurrPos;
                }

                return chunk;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Returns number of chunks in a large file.
        /// </summary>
        /// <returns>Number of file chunks.</returns>
        public int GetNumOfChunks()
        {
            if (chunks != null && chunks.Count != 0)
            {
                return chunks.Count;
            }

            if (BufferSize is UnlimitedBuffer or 0)
            {
                return 1;
            }

            var fileLength = GetFileLength();

            if (fileLength > 0)
            {
                return (int)Math.Ceiling((double)fileLength / BufferSize);
            }

            return -1;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Reset stream buffer length and bytes sent values.
        /// </summary>
        /// <param name="bufferSize">(Optional) Size of the buffer.</param>
        internal void Reset(int bufferSize = UnlimitedBuffer)
        {
            BufferSize = bufferSize;
            CurrPos = 0;
            LastChunk = false;
            fileStream?.Dispose();
            fileStream = null;
        }

        /// <summary>
        /// Gets file length.
        /// </summary>
        /// <returns>The length of file.</returns>
        internal long GetFileLength()
        {
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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                fileStream?.Dispose();
                fileStream = null;

                chunks?.Dispose();
                chunks = null;

                mutex.Dispose();
            }

            disposedValue = true;
        }

        private static async Task<int> ReadBytesAsync(Stream stream, StreamWriter writer, int bytes, CancellationToken? cancellationToken = null)
        {
            var bytesSent = 0;
            var buf = new byte[65000];
            int toSend;
            int cnt;
            var token = cancellationToken ?? CancellationToken.None;
            while ((toSend = bytes - bytesSent) > 0
                   && (cnt = await stream.ReadAsync(buf, 0, toSend > buf.Length ? buf.Length : toSend, token).ConfigureAwait(false)) > 0)
            {
                await writer.BaseStream.WriteAsync(buf, 0, cnt, token).ConfigureAwait(false);
                bytesSent += cnt;
            }

            return bytesSent;
        }

        /// <summary>
        /// Takes a single chunk.
        /// </summary>
        private ChunkData TakeNextChunk()
        {
            if (!Chunked)
            {
                return null;
            }

            try
            {
                return chunks.Take();
            }
            catch (InvalidOperationException)
            {
                // An InvalidOperationException means that Take() was called on a completed collection
                return null;
            }
        }
    }
}
