using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of uploading a file to cloudinary
    /// </summary>
    public class RawUploadParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawUploadParams"/> class.
        /// </summary>
        public RawUploadParams()
        {
            Overwrite = true;
            UniqueFilename = true;
            Context = new StringDictionary();
        }

        /// <summary>
        /// Either the actual data of the image or an HTTP URL of a public image on the Internet.
        /// </summary>
        public FileDescription File { get; set; }

        /// <summary>
        /// The identifier that is used for accessing the uploaded resource. A randomly generated ID is assigned if not specified.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// A comma-separated list of tag names to assign to the uploaded image for later group reference.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets privacy mode of the file. Valid values: 'upload' and 'authenticated'. Default: 'upload'.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Whether to invalidate CDN cache copies of a previously uploaded image that shares the same public ID. Default: false.
        /// </summary>
        /// <value>
        ///   <c>true</c> to invalidate; otherwise, <c>false</c>.
        /// </value>
        public bool Invalidate { get; set; }

        /// <summary>
        /// An HTTP header or a list of headers lines for returning as response HTTP headers when delivering the uploaded image to your users. Supported headers: 'Link', 'X-Robots-Tag'. For example 'X-Robots-Tag: noindex'.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Whether to use the original file name of the uploaded image if available for the public ID. The file name is normalized and random characters are appended to ensure uniqueness. Default: false.
        /// </summary>
        public bool UseFilename { get; set; }

        /// <summary>
        /// Only relevant if <see cref="UseFilename"/> is True. When set to false, should not add random characters at the end of the filename that guarantee its uniqueness.
        /// </summary>
        public bool UniqueFilename { get; set; }

        /// <summary>
        /// Whether to discard the name of the original uploaded file. Relevant when delivering images as attachments (setting the 'flags' transformation parameter to 'attachment'). Default: false.
        /// </summary>
        public bool DiscardOriginalFilename { get; set; }

        /// <summary>
        /// An HTTP URL to send notification to (a webhook) when the upload is completed.
        /// </summary>
        public string NotificationUrl { get; set; }

        /// <summary>
        /// Proxy to use when Cloudinary accesses remote folders
        /// </summary>
        public string Proxy { get; set; }

        /// <summary>
        /// Base Folder to use when building the Cloudinary public_id
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// Whether to overwrite existing resources with the same public ID.
        /// </summary>
        public bool Overwrite { get; set; }

        /// <summary>
        /// Allows to store a set of key-value pairs together with resource.
        /// </summary>
        public StringDictionary Context { get; set; }

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
            AddParam(dict, "tags", Tags);
            AddParam(dict, "type", Type);
            AddParam(dict, "use_filename", UseFilename);

            if (UseFilename)
                AddParam(dict, "unique_filename", UniqueFilename);

            AddParam(dict, "invalidate", Invalidate);
            AddParam(dict, "discard_original_filename", DiscardOriginalFilename);
            AddParam(dict, "notification_url", NotificationUrl);
            AddParam(dict, "proxy", Proxy);
            AddParam(dict, "folder", Folder);
            AddParam(dict, "overwrite", Overwrite);

            if (Context != null && Context.Count > 0)
            {
                AddParam(dict, "context", String.Join("|", Context.Pairs));
            }

            if (Headers != null && Headers.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in Headers)
                {
                    sb.AppendFormat("{0}: {1}\n", item.Key, item.Value);
                }

                dict.Add("headers", sb.ToString());
            }

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
            m_isRemote = Regex.IsMatch(filePath, "^https?:.*|s3:.*|data:[^;]*;base64,([a-zA-Z0-9/+\n=]+)");
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
