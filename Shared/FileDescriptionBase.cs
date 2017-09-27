using System;
using System.IO;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Represents a file for uploading to cloudinary
    /// </summary>
    public abstract class FileDescriptionBase
    {
        string m_name;
        string m_path;
        Stream m_stream;
        bool m_isRemote;

        internal int BufferLength = Int32.MaxValue;
        internal bool EOF = false;
        internal int BytesSent = 0;

        internal long GetFileLength()
        {
            long len = 0;

            if (m_stream != null)
                len = m_stream.Length;
            else
                len = new FileInfo(m_path).Length;
            return len;
        }

        internal bool IsLastPart()
        {
            return GetFileLength() - BytesSent <= BufferLength;
        }

        /// <summary>
        /// Constructor to upload file from stream
        /// </summary>
        /// <param name="name">Resource name</param>
        /// <param name="stream">Stream to read from (will be disposed with this object)</param>
        public FileDescriptionBase(string name, Stream stream)
        {
            m_name = name;
            m_stream = stream;
        }

        protected virtual Stream GetStream(string url)
        {
            throw new Exception("Please call overridden method.");
        }
        
        /// <summary>
        /// Constructor to upload file by path
        /// </summary>
        /// <param name="filePath">Either URL (http/https/s3/data) or local path to file</param>
        public FileDescriptionBase(string filePath)
        {
            m_isRemote = Regex.IsMatch(filePath, "^ftp:.*|https?:.*|s3:.*|data:[^;]*;base64,([a-zA-Z0-9/+\n=]+)");
            bool isBase64 = Regex.IsMatch(filePath, "data:[^;]*;base64,([a-zA-Z0-9/+\n=]+)");
            m_path = filePath;

            if (!m_isRemote)
            {
                m_name = Path.GetFileName(m_path);
            }
            else
            {
                m_name = m_path;
                m_stream = isBase64 ? null : GetStream(m_name);
            }
        }

        /// <summary>
        /// Stream to upload
        /// </summary>
        public Stream Stream
        { get { return m_stream; } }

        /// <summary>
        /// Name of the file to upload
        /// </summary>
        public string FileName
        { get { return m_name; } }

        /// <summary>
        /// Filesystem path to the file to upload
        /// </summary>
        public string FilePath
        { get { return m_path; } }

        /// <summary>
        /// Whether it is remote (by URL) or local file
        /// </summary>
        public bool IsRemote
        { get { return m_isRemote; } }
    }
}
