namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using CloudinaryDotNet.Core;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parameters of upload preset, which enable you to centrally define a set of image upload options instead of
    /// specifying them in each upload call.
    /// </summary>
    public class UploadPresetParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UploadPresetParams"/> class.
        /// Default parameterless constructor.
        /// </summary>
        public UploadPresetParams()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadPresetParams"/> class with result object.
        /// </summary>
        /// <param name="preset">The preset returned from API.</param>
        public UploadPresetParams(GetUploadPresetResult preset)
        {
            Name = preset.Name;
            Unsigned = preset.Unsigned;

            if (preset.Settings == null)
            {
                return;
            }

            DisallowPublicId = preset.Settings.DisallowPublicId;
            Backup = preset.Settings.Backup;
            Type = preset.Settings.Type;

            if (preset.Settings.Tags != null)
            {
                if (preset.Settings.Tags.Type == JTokenType.String)
                {
                    Tags = preset.Settings.Tags.ToString();
                }
                else if (preset.Settings.Tags.Type == JTokenType.Array)
                {
                    Tags = string.Join(",", preset.Settings.Tags.Values<string>().ToArray());
                }
            }

            Invalidate = preset.Settings.Invalidate;
            UseFilename = preset.Settings.UseFilename;
            UniqueFilename = preset.Settings.UniqueFilename;
            DiscardOriginalFilename = preset.Settings.DiscardOriginalFilename;
            NotificationUrl = preset.Settings.NotificationUrl;
            Proxy = preset.Settings.Proxy;
            Folder = preset.Settings.Folder;
            Overwrite = preset.Settings.Overwrite;
            RawConvert = preset.Settings.RawConvert;

            if (preset.Settings.Context != null)
            {
                Context = new StringDictionary();
                foreach (JProperty prop in preset.Settings.Context)
                {
                    Context.Add(prop.Name, prop.Value.ToString());
                }
            }

            if (preset.Settings.AllowedFormats != null)
            {
                if (preset.Settings.AllowedFormats.Type == JTokenType.String)
                {
                    AllowedFormats = preset.Settings.AllowedFormats.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                }
                else if (preset.Settings.AllowedFormats.Type == JTokenType.Array)
                {
                    AllowedFormats = preset.Settings.AllowedFormats.Select(t => t.ToString()).ToArray();
                }
            }

            Moderation = preset.Settings.Moderation;
            Format = preset.Settings.Format;

            if (preset.Settings.Transformation != null)
            {
                if (preset.Settings.Transformation.Type == JTokenType.String)
                {
                    Transformation = preset.Settings.Transformation.ToString();
                }
                else if (preset.Settings.Transformation.Type == JTokenType.Array)
                {
                    var dict = new Dictionary<string, object>();
                    foreach (JObject obj in preset.Settings.Transformation)
                    {
                        foreach (var prop in obj)
                        {
                            dict.Add(prop.Key, prop.Value.ToString());
                        }
                    }

                    Transformation = new Transformation(dict);
                }
            }

            if (preset.Settings.EagerTransforms != null)
            {
                EagerTransforms = new List<object>();
                foreach (JToken token in preset.Settings.EagerTransforms)
                {
                    if (token.Type == JTokenType.String)
                    {
                        EagerTransforms.Add(token.ToString());
                    }
                    else if (token.Type == JTokenType.Array)
                    {
                        var dict = new Dictionary<string, object>();
                        foreach (JObject obj in token)
                        {
                            foreach (var prop in obj)
                            {
                                dict.Add(prop.Key, prop.Value.ToString());
                            }
                        }

                        EagerTransforms.Add(new Transformation(dict));
                    }
                }
            }

            Exif = preset.Settings.Exif;
            Colors = preset.Settings.Colors;
            Faces = preset.Settings.Faces;
            QualityAnalysis = preset.Settings.QualityAnalysis;

            if (preset.Settings.FaceCoordinates != null)
            {
                if (preset.Settings.FaceCoordinates.Type == JTokenType.String)
                {
                    FaceCoordinates = preset.Settings.FaceCoordinates.ToString();
                }
                else if (preset.Settings.FaceCoordinates.Type == JTokenType.Array)
                {
                    var fc = new List<Rectangle>();
                    foreach (JToken token in preset.Settings.FaceCoordinates)
                    {
                        fc.Add(new Rectangle(token[0].Value<int>(), token[1].Value<int>(), token[2].Value<int>(), token[3].Value<int>()));
                    }
                }
            }

            ImageMetadata = preset.Settings.ImageMetadata;
            EagerAsync = preset.Settings.EagerAsync;
            EagerNotificationUrl = preset.Settings.EagerNotificationUrl;
            Categorization = preset.Settings.Categorization;
            AutoTagging = preset.Settings.AutoTagging;
            Detection = preset.Settings.Detection;
            SimilaritySearch = preset.Settings.SimilaritySearch;
            Ocr = preset.Settings.Ocr;
            Live = preset.Settings.Live;
        }

        /// <summary>
        /// Gets or sets this unique preset name is specified as the upload_preset parameter when calling the upload API.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable unsigned uploading to Cloudinary with this upload preset.
        /// </summary>
        public bool Unsigned { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to prevent specifying public_id as one of the extra
        /// parameters for upload. Only relevant when using unsigned presets.
        /// </summary>
        public bool DisallowPublicId { get; set; }

        /// <summary>
        /// Gets or sets whether to backup the uploaded image.
        /// Overrides the default backup settings of your account.
        /// </summary>
        public bool? Backup { get; set; }

        /// <summary>
        /// Gets or sets privacy mode of the image. Valid values: 'private', 'upload' and 'authenticated'.
        /// Default: 'upload'.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of tag names to assign to the uploaded image for later group reference.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to invalidate CDN cache copies of a previously uploaded image that shares the same public ID.
        /// Default: false.
        /// </summary>
        /// <value>
        ///   <c>true</c> to invalidate; otherwise, <c>false</c>.
        /// </value>
        public bool Invalidate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the original file name of the uploaded image if available for the public ID. The file name
        /// is normalized and random characters are appended to ensure uniqueness. Default: false.
        /// </summary>
        public bool UseFilename { get; set; }

        /// <summary>
        /// Gets or sets whether to add random characters at the end of the filename to guarantee uniqueness.
        /// Only relevant if <see cref="UseFilename"/> is True.
        /// </summary>
        public bool? UniqueFilename { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to discard the name of the original uploaded file. Relevant when delivering images as attachments
        /// (setting the 'flags' transformation parameter to 'attachment'). Default: false.
        /// </summary>
        public bool DiscardOriginalFilename { get; set; }

        /// <summary>
        /// Gets or sets an HTTP URL to send notification to (a webhook) when the upload is completed.
        /// </summary>
        public string NotificationUrl { get; set; }

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
        /// Gets or sets a possibility to use "aspose" to automatically convert Office documents to PDF files and other image formats using the
        /// Aspose Document Conversion add-on.
        /// </summary>
        public string RawConvert { get; set; }

        /// <summary>
        /// Gets or sets a possibility to store a set of key-value pairs together with resource.
        /// </summary>
        public StringDictionary Context { get; set; }

        /// <summary>
        /// Gets or sets a set of allowed formats.
        /// </summary>
        public string[] AllowedFormats { get; set; }

        /// <summary>
        /// Gets or sets a possibility to use "manual" to add the uploaded image to a queue of pending moderation images. Set to "webpurify" to
        /// automatically moderate the uploaded image using the WebPurify Image Moderation add-on.
        /// </summary>
        public string Moderation { get; set; }

        /// <summary>
        /// Gets or sets an optional format to convert the uploaded image to before saving in the cloud. For example: "jpg".
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets a transformation to run on the uploaded image before saving it in the cloud. For example: limit the
        /// dimension of the uploaded image to 512x512 pixels.
        /// One may use string representation or <see cref="Transformation"/> class.
        /// </summary>
        public object Transformation { get; set; }

        /// <summary>
        /// Gets or sets a list of transformations to create for the uploaded image during the upload process, instead of lazily
        /// creating them when accessed by your site's visitors.
        /// One may use string representation or <see cref="Transformation"/> class.
        /// </summary>
        public ICollection<object> EagerTransforms { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to retrieve the Exif metadata of the uploaded photo. Default: false.
        /// </summary>
        public bool Exif { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to retrieve predominant colors and color histogram of the uploaded image. Default: false.
        /// </summary>
        public bool Colors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to retrieve a list of coordinates of automatically detected faces in the uploaded photo.
        /// Default: false.
        /// </summary>
        public bool Faces { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to retrieve the quality analysis of the image. Default: false.
        /// </summary>
        public bool QualityAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the face coordinates. Use plain string (x,y,w,h|x,y,w,h) or <see cref="FaceCoordinates"> object</see>.
        /// </summary>
        public object FaceCoordinates { get; set; }

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
        /// Gets or sets a value indicating whether to retrieve IPTC and detailed Exif metadata of the uploaded photo.
        /// Default: false.
        /// </summary>
        public bool ImageMetadata { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate the eager transformations asynchronously in the background after the upload request is
        /// completed rather than online as part of the upload call. Default: false.
        /// </summary>
        public bool EagerAsync { get; set; }

        /// <summary>
        /// Gets or sets an HTTP URL to send notification to (a webhook) when the generation of eager transformations is completed.
        /// </summary>
        public string EagerNotificationUrl { get; set; }

        /// <summary>
        /// Gets or sets a possibility to use "rekognition_scene" to automatically detect scene categories of photos using the ReKognition Scene
        /// Categorization add-on.
        /// </summary>
        public string Categorization { get; set; }

        /// <summary>
        /// Gets or sets a possibility for uploaded images to be automatically assigned tags based on the
        /// detected scene categories. The value of the AutoTagging parameter is the minimum score of a detected
        /// category that should be automatically used as an assigned tag.
        /// See http://cloudinary.com/documentation/rekognition_scene_categorization_addon#automatic_image_tagging
        /// for comments.
        /// </summary>
        public float? AutoTagging { get; set; }

        /// <summary>
        /// Gets or sets a possibility to use "rekognition_face" to automatically extract advanced face attributes of photos using the ReKognition
        /// Detect Face Attributes add-on.
        /// </summary>
        public string Detection { get; set; }

        /// <summary>
        /// Gets or sets a possibility to use "tineye" to use the TinEye add-on.
        /// </summary>
        public string SimilaritySearch { get; set; }

        /// <summary>
        /// Gets or sets the following: if you are subscribed to the OCR Text Detection and Extraction add-on and you set this parameter
        /// to adv_ocr, the response includes the content and bounding box coordinates of all detected text on the
        /// image as well as the content and bounding box of each individual detected text element. Optional.
        /// </summary>
        public string Ocr { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the upload preset for live streaming. Default: false.
        /// </summary>
        public bool Live { get; set; }

        /// <summary>
        /// Gets or sets JavaScript code expression to be evaluated.
        /// </summary>
        public string Eval { get; set; }

        /// <summary>
        /// Gets or sets accessibility analysis information.
        /// Optional (Boolean, default: false).
        /// </summary>
        public bool? AccessibilityAnalysis { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (Overwrite.HasValue && Overwrite.Value && Unsigned)
            {
                throw new ArgumentException("Don't set both Overwrite and Unsigned to true!");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>
        /// Sorted dictionary of parameters.
        /// </returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "name", Name);
            AddParam(dict, "unsigned", Unsigned);
            AddParam(dict, "disallow_public_id", DisallowPublicId);
            AddParam(dict, "type", Type);
            AddParam(dict, "tags", Tags);
            AddParam(dict, "use_filename", UseFilename);
            AddParam(dict, "moderation", Moderation);
            AddParam(dict, "format", Format);
            AddParam(dict, "exif", Exif);
            AddParam(dict, "faces", Faces);
            AddParam(dict, "quality_analysis", QualityAnalysis);
            AddParam(dict, "colors", Colors);
            AddParam(dict, "image_metadata", ImageMetadata);
            AddParam(dict, "eager_async", EagerAsync);
            AddParam(dict, "eager_notification_url", EagerNotificationUrl);
            AddParam(dict, "categorization", Categorization);
            AddParam(dict, "detection", Detection);
            AddParam(dict, "ocr", Ocr);
            AddParam(dict, "similarity_search", SimilaritySearch);
            AddParam(dict, "invalidate", Invalidate);
            AddParam(dict, "discard_original_filename", DiscardOriginalFilename);
            AddParam(dict, "notification_url", NotificationUrl);
            AddParam(dict, "proxy", Proxy);
            AddParam(dict, "folder", Folder);
            AddParam(dict, "raw_convert", RawConvert);
            AddParam(dict, "backup", Backup);
            AddParam(dict, "overwrite", Overwrite);
            AddParam(dict, "unique_filename", UniqueFilename);
            AddParam(dict, "live", Live);
            AddParam(dict, "eval", Eval);
            AddParam(dict, "accessibility_analysis", AccessibilityAnalysis);

            AddParam(dict, "transformation", GetTransformation(Transformation));

            if (AutoTagging.HasValue)
            {
                AddParam(dict, "auto_tagging", AutoTagging.Value);
            }

            if (FaceCoordinates != null)
            {
                AddParam(dict, "face_coordinates", FaceCoordinates.ToString());
            }

            if (EagerTransforms != null && EagerTransforms.Count > 0)
            {
                AddParam(dict, "eager", string.Join("|", EagerTransforms.Select(GetTransformation).ToArray()));
            }

            if (AllowedFormats != null)
            {
                AddParam(dict, "allowed_formats", string.Join(",", AllowedFormats));
            }

            if (Context != null && Context.Count > 0)
            {
                AddParam(dict, Constants.CONTEXT_PARAM_NAME, Utils.SafeJoin("|", Context.SafePairs));
            }

            return dict;
        }

        private string GetTransformation(object o)
        {
            if (o == null)
            {
                return null;
            }

            if (o is string)
            {
                return (string)o;
            }
            else if (o is Transformation)
            {
                return ((Transformation)o).Generate();
            }
            else
            {
                throw new NotSupportedException(
                    string.Format(CultureInfo.InvariantCulture, "Instance of type {0} is not supported as Transformation!", o.GetType()));
            }
        }
    }
}
