using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Upload preset details.
    /// </summary>
    [DataContract]
    public class GetUploadPresetResult : BaseResult
    {
        /// <summary>
        /// Name of upload preset.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; protected set; }

        /// <summary>
        /// True enables unsigned uploading to Cloudinary with this upload preset.
        /// </summary>
        [DataMember(Name = "unsigned")]
        public bool Unsigned { get; protected set; }

        /// <summary>
        /// Other preset settings.
        /// </summary>
        [DataMember(Name = "settings")]
        public UploadSettings Settings { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static GetUploadPresetResult Parse(Object response)
        {
            return Api.Parse<GetUploadPresetResult>(response);
        }
    }

    /// <summary>
    /// Upload settings.
    /// </summary>
    [DataContract]
    public class UploadSettings
    {
        /// <summary>
        /// Only relevant when using unsigned presets this setting prevents specifying public_id as one of the extra
        /// parameters for upload.
        /// </summary>
        [DataMember(Name = "disallow_public_id")]
        public bool DisallowPublicId { get; protected set; }

        /// <summary>
        /// Tell Cloudinary whether to backup the uploaded image. Overrides the default backup settings of your
        /// account.
        /// </summary>
        [DataMember(Name = "backup")]
        public bool? Backup { get; protected set; }

        /// <summary>
        /// Gets privacy mode of the image. Valid values: 'private', 'upload' and 'authenticated'. Default: 'upload'.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; protected set; }

        /// <summary>
        /// A comma-separated list of tag names to assign to the uploaded image for later group reference.
        /// </summary>
        [DataMember(Name = "tags")]
        public JToken Tags { get; protected set; }

        /// <summary>
        /// Whether to invalidate CDN cache copies of a previously uploaded image that shares the same public ID.
        /// Default: false.
        /// </summary>
        [DataMember(Name = "invalidate")]
        public bool Invalidate { get; protected set; }

        /// <summary>
        /// Whether to use the original file name of the uploaded image if available for the public ID. The file name
        /// is normalized and random characters are appended to ensure uniqueness. Default: false.
        /// </summary>
        [DataMember(Name = "use_filename")]
        public bool UseFilename { get; protected set; }

        /// <summary>
        /// Only relevant if <see cref="UseFilename"/> is True. When set to false, should not add random characters at
        /// the end of the filename that guarantee its uniqueness.
        /// </summary>
        [DataMember(Name = "unique_filename")]
        public bool? UniqueFilename { get; protected set; }

        /// <summary>
        /// Whether to discard the name of the original uploaded file. Relevant when delivering images as attachments
        /// (setting the 'flags' transformation parameter to 'attachment'). Default: false.
        /// </summary>
        [DataMember(Name = "discard_original_filename")]
        public bool DiscardOriginalFilename { get; protected set; }

        /// <summary>
        /// An HTTP URL to send notification to (a webhook) when the upload is completed.
        /// </summary>
        [DataMember(Name = "notification_url")]
        public string NotificationUrl { get; protected set; }

        /// <summary>
        /// Proxy to use when Cloudinary accesses remote folders.
        /// </summary>
        [DataMember(Name = "proxy")]
        public string Proxy { get; protected set; }

        /// <summary>
        /// Base Folder to use when building the Cloudinary public ID.
        /// </summary>
        [DataMember(Name = "folder")]
        public string Folder { get; protected set; }

        /// <summary>
        /// Whether to overwrite existing resources with the same public ID.
        /// </summary>
        [DataMember(Name = "overwrite")]
        public bool? Overwrite { get; protected set; }

        /// <summary>
        /// If set to "aspose" Cloudinary will automatically convert Office documents to PDF files and other image
        /// formats using the Aspose Document Conversion add-on.
        /// </summary>
        [DataMember(Name = "raw_convert")]
        public string RawConvert { get; protected set; }

        /// <summary>
        /// Gets a set of key-value pairs together with resource.
        /// </summary>
        [DataMember(Name = "context")]
        public JToken Context { get; protected set; }

        /// <summary>
        /// Gets a set of allowed formats.
        /// </summary>
        [DataMember(Name = "allowed_formats")]
        public JToken AllowedFormats { get; protected set; }

        /// <summary>
        /// Whether to add the uploaded image to a queue of pending moderation images. Set to "webpurify" to
        /// automatically moderate the uploaded image using the WebPurify Image Moderation add-on.
        /// </summary>
        [DataMember(Name = "moderation")]
        public string Moderation { get; protected set; }

        /// <summary>
        /// An optional format to convert the uploaded image to before saving in the cloud. For example: "jpg".
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        /// <summary>
        /// A transformation to run on the uploaded image before saving it in the cloud.
        /// </summary>
        [DataMember(Name = "transformation")]
        public JToken Transformation { get; protected set; }

        /// <summary>
        /// A list of transformations to create for the uploaded image during the upload process, instead of lazily 
        /// creating them when being accessed by your site's visitors.
        /// </summary>
        [DataMember(Name = "eager")]
        public JToken EagerTransforms { get; protected set; }

        /// <summary>
        /// Whether to retrieve the Exif metadata of the uploaded photo. Default: false.
        /// </summary>
        [DataMember(Name = "exif")]
        public bool Exif { get; protected set; }

        /// <summary>
        /// Whether to retrieve predominant colors and color histogram of the uploaded image. Default: false.
        /// </summary>
        [DataMember(Name = "colors")]
        public bool Colors { get; protected set; }

        /// <summary>
        /// Whether to retrieve a list of coordinates of automatically detected faces in the uploaded photo. 
        /// Default: false.
        /// </summary>
        [DataMember(Name = "faces")]
        public bool Faces { get; protected set; }
        
        /// <summary>
        /// Whether to retrieve the quality analysis of the image. Default: false.
        /// </summary>
        [DataMember(Name = "quality_analysis")]
        public bool QualityAnalysis { get; protected set; }

        /// <summary>
        /// Sets the face coordinates. Use plain string (x,y,w,h|x,y,w,h) or <see cref="FaceCoordinates" /> object.
        /// </summary>
        [DataMember(Name = "face_coordinates")]
        public JToken FaceCoordinates { get; protected set; }

        /// <summary>
        /// Whether to retrieve IPTC and detailed Exif metadata of the uploaded photo. Default: false.
        /// </summary>
        [DataMember(Name = "image_metadata")]
        public bool Metadata { get; protected set; }

        /// <summary>
        /// Whether to generate the eager transformations asynchronously in the background after the upload request is 
        /// completed rather than online as part of the upload call. Default: false.
        /// </summary>
        [DataMember(Name = "eager_async")]
        public bool EagerAsync { get; protected set; }

        /// <summary>
        /// An HTTP URL to send notification to (a webhook) when the generation of eager transformations is completed.
        /// </summary>
        [DataMember(Name = "eager_notification_url")]
        public string EagerNotificationUrl { get; protected set; }

        /// <summary>
        /// Set to "rekognition_scene" to automatically detect scene categories of photos using the ReKognition Scene 
        /// Categorization add-on.
        /// </summary>
        [DataMember(Name = "categorization")]
        public string Categorization { get; protected set; }

        /// <summary>
        /// By providing the AutoTagging parameter, uploaded images are automatically assigned tags based on the
        /// detected scene categories. The value of the AutoTagging parameter is the minimum score of a detected
        /// category that should be automatically used as an assigned tag. 
        /// See also http://cloudinary.com/documentation/rekognition_scene_categorization_addon#automatic_image_tagging.
        /// </summary>
        [DataMember(Name = "auto_tagging")]
        public float? AutoTagging { get; protected set; }

        /// <summary>
        /// Used detection add-on.
        /// </summary>
        [DataMember(Name = "detection")]
        public string Detection { get; protected set; }

        [DataMember(Name = "similarity_search")]
        public string SimilaritySearch { get; protected set; }

        /// <summary>
        /// Used ocr add-on.
        /// </summary>
        [DataMember(Name = "ocr")]
        public string Ocr { get; protected set; }
    }
}
