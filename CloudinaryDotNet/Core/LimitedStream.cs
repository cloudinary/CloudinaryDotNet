namespace CloudinaryDotNet.Core
{
    using System;
    using System.IO;

    /// <summary>
    /// Helper class for creating a limited view of the stream.
    /// <inheritdoc />
    /// </summary>
    internal class LimitedStream : Stream
    {
        private readonly object streamLock;
        private readonly Stream originalStream;
        private long remainingBytes;
        private long startOffset;
        private long currOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedStream"/> class.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="offset">The offset from which to start reading in the underlying stream.
        ///                      We ignore it for non-seekable streams.</param>
        /// <param name="maxBytes">Maximum bytes to read.</param>
        /// <param name="streamLockObj">Stream lock object.</param>
        public LimitedStream(Stream stream, long offset, long maxBytes, object streamLockObj = null)
        {
            originalStream = stream ?? throw new ArgumentNullException(nameof(stream));
            remainingBytes = maxBytes;
            startOffset = currOffset = offset;
            streamLock = streamLockObj;

            if (!stream.CanSeek)
            {
                return;
            }

            if (startOffset < 0 || startOffset >= originalStream.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startOffset), "Offset is out of range.");
            }

            remainingBytes = Math.Min(maxBytes, originalStream.Length - startOffset);

            originalStream.Seek(startOffset, SeekOrigin.Begin);
        }

        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override bool CanSeek => false;

        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override long Length => throw new NotSupportedException();

        /// <inheritdoc/>
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (streamLock)
            {
                // make sure stream is not moved around.
                originalStream.Seek(currOffset, SeekOrigin.Begin);
                var bytesRead = originalStream.Read(buffer, offset, (int)Math.Min(count, remainingBytes));
                currOffset += bytesRead;
                remainingBytes -= bytesRead;

                return bytesRead;
            }
        }

        /// <inheritdoc/>
        public override void Flush() => throw new NotSupportedException();

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        /// <inheritdoc/>
        public override void SetLength(long value) => throw new NotSupportedException();

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
}
