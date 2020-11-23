namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Upload settings.
    /// </summary>
    [DataContract]
    public class UploadSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether only relevant when using unsigned presets this setting prevents specifying public_id as one of the extra
        /// parameters for upload.
        /// </summary>
        [DataMember(Name = "disallow_public_id")]
        public bool DisallowPublicId { get; set; }

        /// <summary>
        /// Gets or sets indication if Cloudinary should backup the uploaded image. Overrides the default backup settings of your
        /// account.
        /// </summary>
        [DataMember(Name = "backup")]
        public bool? Backup { get; set; }

        /// <summary>
        /// Gets or sets privacy mode of the image. Valid values: 'private', 'upload' and 'authenticated'. Default: 'upload'.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of tag names to assign to the uploaded image for later group reference.
        /// </summary>
        [DataMember(Name = "tags")]
        public JToken Tags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether to invalidate CDN cache copies of a previously uploaded image that shares the same public ID.
        /// Default: false.
        /// </summary>
        [DataMember(Name = "invalidate")]
        public bool Invalidate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether to use the original file name of the uploaded image if available for the public ID. The file name
        /// is normalized and random characters are appended to ensure uniqueness. Default: false.
        /// </summary>
        [DataMember(Name = "use_filename")]
        public bool UseFilename { get; set; }

        /// <summary>
        /// Gets or sets value that indicates file uniqueness.
        /// Only relevant if <see cref="UseFilename"/> is True. When set to false, should not add random characters at
        /// the end of the filename to guarantee uniqueness.
        /// </summary>
        [DataMember(Name = "unique_filename")]
        public bool? UniqueFilename { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to discard the name of the original uploaded file. Relevant when delivering images as attachments
        /// (setting the 'flags' transformation parameter to 'attachment'). Default: false.
        /// </summary>
        [DataMember(Name = "discard_original_filename")]
        public bool DiscardOriginalFilename { get; set; }

        /// <summary>
        /// Gets or sets an HTTP URL to send notification to (a webhook) when the upload is completed.
        /// </summary>
        [DataMember(Name = "notification_url")]
        public string NotificationUrl { get; set; }

        /// <summary>
        /// Gets or sets proxy to use when Cloudinary accesses remote folders.
        /// </summary>
        [DataMember(Name = "proxy")]
        public string Proxy { get; set; }

        /// <summary>
        /// Gets or sets base Folder to use when building the Cloudinary public ID.
        /// </summary>
        [DataMember(Name = "folder")]
        public string Folder { get; set; }

        /// <summary>
        /// Gets or sets whether to overwrite existing resources with the same public ID.
        /// </summary>
        [DataMember(Name = "overwrite")]
        public bool? Overwrite { get; set; }

        /// <summary>
        /// Gets or sets conversion mode.
        /// If set to "aspose", Cloudinary will automatically convert Office documents to PDF files and other image
        /// formats using the Aspose Document Conversion add-on.
        /// </summary>
        [DataMember(Name = "raw_convert")]
        public string RawConvert { get; set; }

        /// <summary>
        /// Gets or sets a set of key-value pairs together with resource.
        /// </summary>
        [DataMember(Name = "context")]
        public JToken Context { get; set; }

        /// <summary>
        /// Gets or sets a set of allowed formats.
        /// </summary>
        [DataMember(Name = "allowed_formats")]
        public JToken AllowedFormats { get; set; }

        /// <summary>
        /// Gets or sets whether to add the uploaded image to a queue of pending moderation images. Set to "webpurify" to
        /// automatically moderate the uploaded image using the WebPurify Image Moderation add-on.
        /// </summary>
        [DataMember(Name = "moderation")]
        public string Moderation { get; set; }

        /// <summary>
        /// Gets or sets an optional format to convert the uploaded image to before saving in the cloud. For example: "jpg".
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets a transformation to run on the uploaded image before saving it in the cloud.
        /// </summary>
        [DataMember(Name = "transformation")]
        public JToken Transformation { get; set; }

        /// <summary>
        /// Gets or sets a list of transformations to create for the uploaded image during the upload process, instead of lazily
        /// creating them when being accessed by your site's visitors.
        /// </summary>
        [DataMember(Name = "eager")]
        public JToken EagerTransforms { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether to retrieve the Exif metadata of the uploaded photo. Default: false.
        /// </summary>
        [DataMember(Name = "exif")]
        public bool Exif { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to retrieve predominant colors and color histogram of the uploaded image. Default: false.
        /// </summary>
        [DataMember(Name = "colors")]
        public bool Colors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to retrieve a list of coordinates of automatically detected faces in the uploaded photo.
        /// Default: false.
        /// </summary>
        [DataMember(Name = "faces")]
        public bool Faces { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to retrieve the quality analysis of the image. Default: false.
        /// </summary>
        [DataMember(Name = "quality_analysis")]
        public bool QualityAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the face coordinates. Use plain string (x,y,w,h|x,y,w,h) or <see cref="FaceCoordinates" /> object.
        /// </summary>
        [DataMember(Name = "face_coordinates")]
        public JToken FaceCoordinates { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to retrieve IPTC and detailed Exif metadata of the uploaded photo. Default: false.
        /// </summary>
        [Obsolete("Property Metadata is deprecated, please use ImageMetadata instead")]
        public bool Metadata
        {
            get { return ImageMetadata; }
            set { ImageMetadata = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to retrieve IPTC and detailed Exif metadata of the uploaded photo. Default: false.
        /// </summary>
        [DataMember(Name = "image_metadata")]
        public bool ImageMetadata { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate the eager transformations asynchronously in the background after the upload request is
        /// completed, rather than online as part of the upload call. Default: false.
        /// </summary>
        [DataMember(Name = "eager_async")]
        public bool EagerAsync { get; set; }

        /// <summary>
        /// Gets or sets an HTTP URL to send notification to (a webhook) when the generation of eager transformations is completed.
        /// </summary>
        [DataMember(Name = "eager_notification_url")]
        public string EagerNotificationUrl { get; set; }

        /// <summary>
        /// Gets or sets set to "rekognition_scene" to automatically detect scene categories of photos using the ReKognition Scene
        /// Categorization add-on.
        /// </summary>
        [DataMember(Name = "categorization")]
        public string Categorization { get; set; }

        /// <summary>
        /// Gets or sets AutoTagging parameter. If set, uploaded images are automatically assigned tags based on the
        /// detected scene categories. The value of the AutoTagging parameter is the minimum score of a detected
        /// category that should be automatically used as an assigned tag.
        /// See also http://cloudinary.com/documentation/rekognition_scene_categorization_addon#automatic_image_tagging.
        /// </summary>
        [DataMember(Name = "auto_tagging")]
        public float? AutoTagging { get; set; }

        /// <summary>
        /// Gets or sets used detection add-on.
        /// </summary>
        [DataMember(Name = "detection")]
        public string Detection { get; set; }

        /// <summary>
        /// Gets or sets used similarity search add-on.
        /// </summary>
        [DataMember(Name = "similarity_search")]
        public string SimilaritySearch { get; set; }

        /// <summary>
        /// Gets or sets used ocr add-on.
        /// </summary>
        [DataMember(Name = "ocr")]
        public string Ocr { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether indicates whether the upload preset is used for live streaming. Default: false.
        /// </summary>
        [JsonConverter(typeof(SafeBooleanConverter))]
        [DataMember(Name = "live")]
        public bool Live { get; set; }
    }
}
