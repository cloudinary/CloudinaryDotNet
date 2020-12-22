namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CloudinaryDotNet.Core;
    using Newtonsoft.Json;

    /// <summary>
    /// Parameters of image file uploading.
    /// </summary>
    public class ImageUploadParams : RawUploadParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageUploadParams"/> class.
        /// </summary>
        public ImageUploadParams()
        {
            Overwrite = null;
            UniqueFilename = null;
        }

        /// <summary>
        /// Gets or sets an optional format to convert the uploaded image to before saving in the cloud. For example: "jpg".
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets a transformation to run on the uploaded image before saving it in the cloud. For example: limit the
        /// dimension of the uploaded image to 512x512 pixels.
        /// </summary>
        public Transformation Transformation { get; set; }

        /// <summary>
        /// Gets or sets a list of transformations to create for the uploaded image during the upload process, instead of lazily
        /// creating them when accessed by your site's visitors.
        /// </summary>
        public List<Transformation> EagerTransforms { get; set; }

        /// <summary>
        /// Gets or sets privacy mode of the image. Valid values: 'private', 'upload' and 'authenticated'.
        /// Default: 'upload'.
        /// </summary>
        public new string Type
        {
            get { return base.Type; }
            set { base.Type = value; }
        }

        /// <summary>
        ///  Gets the type of image asset you are uploading.
        /// </summary>
        public override ResourceType ResourceType
        {
            get { return Actions.ResourceType.Image; }
        }

        /// <summary>
        /// Gets or sets whether to retrieve the Exif metadata of the uploaded photo. Default: false.
        /// </summary>
        public bool? Exif { get; set; }

        /// <summary>
        /// Gets or sets whether to retrieve predominant colors and color histogram of the uploaded image. Default: false.
        /// </summary>
        public bool? Colors { get; set; }

        /// <summary>
        /// Gets or sets whether to retrieve a list of coordinates of automatically detected faces in the uploaded photo.
        /// Default: false.
        /// </summary>
        public bool? Faces { get; set; }

        /// <summary>
        /// Gets or sets whether to retrieve the quality analysis of the image. Default: false.
        /// </summary>
        public bool? QualityAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the coordinates of faces contained in an uploaded image and overrides the automatically detected
        /// faces. Format: x,y,w,h|x,y,w,h. For example: string "10,20,150,130❘213,345,82,61" or <see cref="Rectangle"/>.
        /// Relevant for images only.
        /// </summary>
        public object FaceCoordinates { get; set; }

        /// <summary>
        /// Gets or sets coordinates of an interesting region contained in an uploaded image. The given coordinates are used for
        /// cropping uploaded images using the custom gravity mode. The region is specified by the X and Y coordinates
        /// of the top left corner and the width and height of the region. For example: "85,120,220,310". Otherwise,
        /// one can use <see cref="Rectangle"/> structure.
        /// </summary>
        public object CustomCoordinates { get; set; }

        /// <summary>
        /// Gets or sets whether to retrieve IPTC and detailed Exif metadata of the uploaded photo. Default: false.
        /// </summary>
        [Obsolete("Property Metadata is deprecated, please use ImageMetadata instead")]
        public bool? Metadata
        {
            get { return ImageMetadata; }
            set { ImageMetadata = value; }
        }

        /// <summary>
        /// Gets or sets whether to retrieve IPTC and detailed Exif metadata of the uploaded photo. Default: false.
        /// </summary>
        public bool? ImageMetadata { get; set; }

        /// <summary>
        /// Gets or sets whether to generate the eager transformations asynchronously in the background after the upload request is
        /// completed rather than online as part of the upload call. Default: false.
        /// </summary>
        public bool? EagerAsync { get; set; }

        /// <summary>
        /// Gets or sets an HTTP URL to send notification to (a webhook) when the generation of eager transformations is completed.
        /// </summary>
        public string EagerNotificationUrl { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of the categorization add-ons to run on the asset. Set to google_tagging,
        /// google_video_tagging, imagga_tagging and/or aws_rek_tagging to automatically classify the scenes of the
        /// uploaded asset.
        /// </summary>
        public string Categorization { get; set; }

        /// <summary>
        /// Gets or sets background removal.
        /// Set to remove_the_background to automatically clear the background of an image using the
        /// Remove-The-Background Editing add-on.
        /// Relevant for images only.
        /// </summary>
        public string BackgroundRemoval { get; set; }

        /// <summary>
        /// Gets or sets whether to assign tags to an asset according to detected scene categories with a confidence score higher
        /// than the given value (between 0.0 and 1.0).
        /// </summary>
        public float? AutoTagging { get; set; }

        /// <summary>
        /// Gets or sets recognition parameter.
        /// Set to adv_face or aws_rek_face to extract an extensive list of face attributes from an image using the
        /// Advanced Facial Attribute Detection or Amazon Rekognition Celebrity Detection add-ons.
        /// Relevant for images only.
        /// </summary>
        public string Detection { get; set; }

        /// <summary>
        /// Gets or sets TinEye add-on usage.
        /// Set to "tineye" to use the TinEye add-on.
        /// </summary>
        public string SimilaritySearch { get; set; }

        /// <summary>
        /// Gets or sets Ocr parameter.
        /// Set to "adv_ocr" to extract all text elements in an image as well as the bounding box coordinates of each
        /// detected element using the OCR text detection and extraction add-on.
        /// Relevant for images only.
        /// </summary>
        public string Ocr { get; set; }

        /// <summary>
        /// Gets or sets whether to return a deletion token in the upload response. The token can be used to delete the uploaded
        /// asset within 10 minutes using an unauthenticated API request. Default: false.
        /// </summary>
        public bool? ReturnDeleteToken { get; set; }

        /// <summary>
        /// Gets or sets whether it is allowed to use an upload preset for setting parameters of this upload (optional).
        /// </summary>
        public string UploadPreset { get; set; }

        /// <summary>
        /// Gets or sets whether it is allowed to send unsigned request. Requires setting appropriate <see cref="UploadPreset"/>.
        /// </summary>
        public bool? Unsigned { get; set; }

        /// <summary>
        /// Gets or sets the perceptual hash usage (Boolean, default: false).
        /// If true, include the perceptual hash (pHash) of the uploaded photo for
        /// image similarity detection. Optional.
        /// </summary>
        public bool? Phash { get; set; }

        /// <summary>
        /// Gets or sets whether it is allowed to pass a list of ResponsiveBreakpoints parameters to request Cloudinary to automatically
        /// find the best breakpoints.
        /// Relevant for images only. Optional.
        /// </summary>
        public List<ResponsiveBreakpoint> ResponsiveBreakpoints { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the animation/video is cinemagraph. Optional (Boolean, default: false).
        /// If true, returns a cinemagraph analysis value for the animation/video between 0 and 1, where 0 means the video/animation
        /// is NOT a cinemagraph and 1 means the GIF/video IS a cinemagraph.
        /// Running cinemagraph analysis on static images returns 0.
        /// </summary>
        public bool? CinemagraphAnalysis { get; set; }

        /// <summary>
        /// Gets or sets whether to include accessibility analysis information.
        /// Optional (Boolean, default: false).
        /// </summary>
        public bool? AccessibilityAnalysis { get; set; }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();

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
            AddParam(dict, "upload_preset", UploadPreset);
            AddParam(dict, "unsigned", Unsigned);
            AddParam(dict, "phash", Phash);
            AddParam(dict, "background_removal", BackgroundRemoval);
            AddParam(dict, "return_delete_token", ReturnDeleteToken);
            AddParam(dict, "cinemagraph_analysis", CinemagraphAnalysis);
            AddParam(dict, "accessibility_analysis", AccessibilityAnalysis);

            if (AutoTagging.HasValue)
            {
                AddParam(dict, "auto_tagging", AutoTagging.Value);
            }

            AddCoordinates(dict, "face_coordinates", FaceCoordinates);
            AddCoordinates(dict, "custom_coordinates", CustomCoordinates);

            if (Transformation != null)
            {
                AddParam(dict, "transformation", Transformation.Generate());
            }

            if (EagerTransforms != null && EagerTransforms.Count > 0)
            {
                AddParam(
                    dict,
                    "eager",
                    string.Join("|", EagerTransforms.Select(t => t.Generate()).ToArray()));
            }

            if (ResponsiveBreakpoints != null && ResponsiveBreakpoints.Count > 0)
            {
                AddParam(dict, "responsive_breakpoints", JsonConvert.SerializeObject(ResponsiveBreakpoints));
            }

            return dict;
        }
    }
}
