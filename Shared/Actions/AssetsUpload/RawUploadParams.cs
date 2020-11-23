namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// Extended Parameters of file uploading.
    /// </summary>
    public class RawUploadParams : BasicRawUploadParams
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
        /// Gets or sets a comma-separated list of tag names to assign to the uploaded image for later group reference.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets whether to invalidate CDN cache copies of a previously uploaded image that shares the same public ID.
        /// Default: false.
        /// </summary>
        /// <value>
        ///   <c>true</c> to invalidate; otherwise, <c>false</c>.
        /// </value>
        public bool? Invalidate { get; set; }

        /// <summary>
        /// Gets or sets an HTTP header or a list of headers lines for returning as response HTTP headers when delivering the
        /// uploaded image to your users. Supported headers: 'Link', 'X-Robots-Tag'.
        /// For example 'X-Robots-Tag: noindex'.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Gets or sets whether to use the original file name of the uploaded image if available for the public ID. The file name
        /// is normalized and random characters are appended to ensure uniqueness. Default: false.
        /// </summary>
        public bool? UseFilename { get; set; }

        /// <summary>
        /// Gets or sets unique file name usage setting. Only relevant if <see cref="UseFilename"/> is True.
        /// When set to false, should not add random characters at the end of the filename to guarantee its uniqueness.
        /// </summary>
        public bool? UniqueFilename { get; set; }

        /// <summary>
        /// Gets or sets whether to discard the name of the original uploaded file. Relevant when delivering images as attachments
        /// (setting the 'flags' transformation parameter to 'attachment'). Default: false.
        /// </summary>
        public bool? DiscardOriginalFilename { get; set; }

        /// <summary>
        /// Gets or sets an HTTP or HTTPS URL to receive the upload response (a webhook) when the upload is completed or a
        /// notification when any requested asynchronous action is completed.
        /// </summary>
        public string NotificationUrl { get; set; }

        /// <summary>
        /// Gets or sets a possibility for the resource to behave as if it's of the authenticated 'type' while still using the default 'upload'
        /// type in delivery URLs.
        /// </summary>
        public string AccessMode { get; set; }

        /// <summary>
        /// Gets or sets proxy to use when Cloudinary accesses remote folders.
        /// </summary>
        public string Proxy { get; set; }

        /// <summary>
        /// Gets or sets base Folder to use when building the Cloudinary public_id.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// Gets or sets whether to overwrite existing resources with the same public ID.
        /// </summary>
        public bool? Overwrite { get; set; }

        /// <summary>
        /// Gets or sets conversion mode. Set to "aspose" to automatically convert Office documents to PDF files
        /// and other image formats using the Aspose Document Conversion add-on.
        /// </summary>
        public string RawConvert { get; set; }

        /// <summary>
        /// Gets or sets a possibility to store a set of key-value pairs together with resource.
        /// </summary>
        public StringDictionary Context { get; set; }

        /// <summary>
        /// Gets or sets allows to store a set of custom metadata fields (by external_id) and the values to assign to each of them.
        /// </summary>
        public StringDictionary MetadataFields { get; set; }

        /// <summary>
        /// Gets or sets a set of allowed formats.
        /// </summary>
        public string[] AllowedFormats { get; set; }

        /// <summary>
        /// Gets or sets moderation mode. Set to "manual" to add the uploaded image to a queue of pending moderation images.
        /// Set to "webpurify" to automatically moderate the uploaded image using the WebPurify Image Moderation add-on.
        /// </summary>
        public string Moderation { get; set; }

        /// <summary>
        /// Gets or sets whether to perform the upload request in the background (asynchronously).
        /// </summary>
        public string Async { get; set; }

        /// <summary>
        /// Gets or sets a list of AccessControlRule parameters. Optional.
        /// </summary>
        public List<AccessControlRule> AccessControl { get; set; }

        /// <summary>
        /// Gets or sets JavaScript code expression to be evaluated.
        /// </summary>
        public string Eval { get; set; }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "tags", Tags);
            AddParam(dict, "use_filename", UseFilename);
            AddParam(dict, "moderation", Moderation);

            if (UseFilename.HasValue && UseFilename.Value)
            {
                AddParam(dict, "unique_filename", UniqueFilename);
            }

            if (AllowedFormats != null)
            {
                AddParam(dict, "allowed_formats", string.Join(",", AllowedFormats));
            }

            AddParam(dict, "invalidate", Invalidate);
            AddParam(dict, "discard_original_filename", DiscardOriginalFilename);
            AddParam(dict, "notification_url", NotificationUrl);
            AddParam(dict, "access_mode", AccessMode);
            AddParam(dict, "proxy", Proxy);
            AddParam(dict, "folder", Folder);
            AddParam(dict, "raw_convert", RawConvert);
            AddParam(dict, "overwrite", Overwrite);
            AddParam(dict, "async", Async);
            AddParam(dict, "eval", Eval);

            if (Context != null && Context.Count > 0)
            {
                AddParam(dict, Constants.CONTEXT_PARAM_NAME, Utils.SafeJoin("|", Context.SafePairs));
            }

            if (MetadataFields != null && MetadataFields.Count > 0)
            {
                AddParam(dict, Constants.METADATA_PARAM_NAME, Utils.SafeJoin("|", MetadataFields.SafePairs));
            }

            if (Headers != null && Headers.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in Headers)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}: {1}\n", item.Key, item.Value);
                }

                dict.Add("headers", sb.ToString());
            }

            if (AccessControl != null && AccessControl.Count > 0)
            {
                AddParam(dict, "access_control", JsonConvert.SerializeObject(AccessControl));
            }

            return dict;
        }
    }
}
