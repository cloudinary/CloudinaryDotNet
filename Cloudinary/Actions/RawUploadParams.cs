using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of uploading a file to cloudinary
    /// </summary>
    public class RawUploadParams : BaseParams
    {
        /// <summary>
        /// Either the actual data of the image or an HTTP URL of a public image on the Internet.
        /// </summary>
        public FileDescription File { get; set; }

        /// <summary>
        /// The identifier that is used for accessing the uploaded resource. A randomly generated ID is assigned if not specified.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            if (File == null)
                throw new ArgumentException("File must be specified in UploadParams!");

            if (!File.IsRemote && File.Stream == null || !File.Stream.CanRead)
                throw new ArgumentException("File is not ready!");

            if (String.IsNullOrEmpty(File.FileName))
                throw new ArgumentException("File name must be specified in UploadParams!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = new SortedDictionary<string, object>();

            AddParam(dict, "public_id", PublicId);

            return dict;
        }
    }

    /// <summary>
    /// Represents a file to upload to cloudinary
    /// </summary>
    public class FileDescription
    {
        string m_name;
        string m_path;
        Stream m_stream;
        bool m_isRemote;

        /// <summary>
        /// Constructor to upload file from stream
        /// </summary>
        /// <param name="name">Resource name</param>
        /// <param name="stream">Stream to read from (will be disposed with this object)</param>
        public FileDescription(string name, Stream stream)
        {
            m_name = name;
            m_stream = stream;
        }

        /// <summary>
        /// Constructor to upload file by path
        /// </summary>
        /// <param name="filePath">Either URL (http/https/s3/data) or local path to file</param>
        public FileDescription(string filePath)
        {
            m_isRemote = Regex.IsMatch(filePath, "^https?:.*|s3:.*|data:image/\\w*;base64,([a-zA-Z0-9/+\n=]+)");
            m_path = filePath;

            if (!m_isRemote)
                m_name = Path.GetFileNameWithoutExtension(m_path);
            else
                m_name = m_path;
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
