namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// Parameters for the explicit method to apply actions to already uploaded assets.
    /// </summary>
    public class ExplicitParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExplicitParams"/> class with public id.
        /// </summary>
        /// <param name="publicId">The identifier that is used for accessing the uploaded resource.</param>
        public ExplicitParams(string publicId)
        {
            PublicId = publicId;
            ResourceType = ResourceType.Image;
            Type = string.Empty;
            Tags = string.Empty;
        }

        /// <summary>
        /// Gets or sets a list of transformations to create for the uploaded image during the upload process, instead
        /// of lazily creating them when being accessed by your site's visitors. Optional.
        /// </summary>
        public List<Transformation> EagerTransforms { get; set; }

        /// <summary>
        /// Gets or sets whether to generate the eager transformations asynchronously in the background. Default: false.
        /// </summary>
        public bool? EagerAsync { get; set; }

        /// <summary>
        /// Gets or sets an HTTP or HTTPS URL to notify your application (a webhook) when the generation of eager
        /// transformations is completed. Optional.
        /// </summary>
        public string EagerNotificationUrl { get; set; }

        /// <summary>
        /// Gets or sets the specific type of asset.
        /// Valid values for uploaded images and videos: upload, private, or authenticated.
        /// Valid values for remotely fetched images: fetch, facebook, twitter, gplus, instagram_name, gravatar,
        /// youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets OCR value. Set to "adv_ocr" to extract all text elements in an image as well as the bounding
        /// box coordinates of each detected element using the OCR text detection and extraction add-on.
        /// Relevant for images only.
        /// </summary>
        public string Ocr { get; set; }

        /// <summary>
        /// Gets or sets the type of resource.
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the uploaded asset or the URL of the remote asset.
        /// Note: The public ID value for images and videos should not include a file extension.Include the file
        /// extension for raw files only.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets an HTTP header or a list of headers lines for returning as response HTTP headers when delivering the
        /// uploaded image to your users. Supported headers: 'Link', 'X-Robots-Tag'.
        /// For example 'X-Robots-Tag: noindex'.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of tag names to assign to an asset that replaces any current tags assigned to
        /// the asset (if any).
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets the coordinates of faces contained in an uploaded image and overrides the automatically detected
        /// faces.
        /// Use plain string (x,y,w,h|x,y,w,h) or <see cref="Core.Rectangle"/>.
        /// </summary>
        public object FaceCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the coordinates of a region contained in an uploaded image that is subsequently used for cropping
        /// uploaded images using the custom gravity mode. The region is specified by the X and Y coordinates of the top
        /// left corner and the width and height of the region, as a comma-separated list.
        /// For example: 85,120,220,310. Relevant for images only.
        /// </summary>
        public object CustomCoordinates { get; set; }

        /// <summary>
        /// Gets or sets a list of the key-value pairs of general textual context metadata to attach to an uploaded asset.
        /// The context values of uploaded files are available for fetching using the Admin API.
        /// For example: alt=My image, caption=Profile image.
        /// </summary>
        public StringDictionary Context { get; set; }

        /// <summary>
        /// Gets or sets a list of custom metadata fields (by external_id) and the values to assign to each of them.
        /// </summary>
        public StringDictionary Metadata { get; set; }

        /// <summary>
        /// Gets or sets requests that Cloudinary automatically find the best breakpoints from the array of breakpoint request
        /// settings.
        /// </summary>
        public List<ResponsiveBreakpoint> ResponsiveBreakpoints { get; set; }

        /// <summary>
        /// Gets or sets a list of AccessControlRule parameters. Optional.
        /// </summary>
        public List<AccessControlRule> AccessControl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether to invalidate the asset (and all its transformed versions) on the CDN. Default: false.
        /// </summary>
        /// <value>
        ///   <c>true</c> to invalidate; otherwise, <c>false</c>.
        /// </value>
        public bool Invalidate { get; set; }

        /// <summary>
        /// Gets or sets whether to perform the request in the background (asynchronously). Default: false.
        /// </summary>
        public bool? Async { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether to return a quality analysis value for the image between 0 and 1, where 0 means the image is blurry
        /// and out of focus and 1 means the image is sharp and in focus. Default: false. Relevant for images only.
        /// </summary>
        public bool QualityAnalysis { get; set; }

        /// <summary>
        /// Gets or sets overwrite.
        ///
        /// Optional. When applying eager for already existing video transformations, this
        /// setting indicates whether to force the existing derived video resources to be regenerated.
        /// Default for videos: false. Note that when specifying existing eager transformations for images,
        /// corresponding derived images are always regenerated.
        /// </summary>
        public bool? Overwrite { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the animation/video is cinemagraph. Optional (Boolean, default: false).
        /// If true, returns a cinemagraph analysis value for the animation/video between 0 and 1, where 0 means the video/animation
        /// is NOT a cinemagraph and 1 means the GIF/video IS a cinemagraph.
        /// Running cinemagraph analysis on static images returns 0.
        /// </summary>
        public bool? CinemagraphAnalysis { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include IPTC, XMP, and detailed Exif metadata.
        /// Supported for images, video, and audio.
        /// </summary>
        public bool? ImageMetadata { get; set; }

        /// <summary>
        /// Gets or sets an HTTP URL to send notification to (a webhook) when the operation or any additional
        /// requested asynchronous action is completed. If not specified,
        /// the response is sent to the global Notification URL (if defined)
        /// in the Upload settings of your account console.
        /// </summary>
        public string NotificationUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to retrieve predominant colors and color histogram of the uploaded
        /// image. If one or more colors contain an alpha channel, then 8-digit RGBA hex quadruplet values are returned.
        /// Default: false. Relevant for images only.
        /// </summary>
        public bool? Colors { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to return the perceptual hash (pHash) on the uploaded image.
        /// The pHash acts as a fingerprint that allows checking image similarity.
        /// Default: false. Relevant for images only.
        /// </summary>
        public bool? Phash { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to return the coordinates of faces contained in an uploaded image
        /// (automatically detected or manually defined).
        /// </summary>
        public bool? Faces { get; set; }

        /// <summary>
        /// Gets or sets a quality value to override the value used when the image is encoded
        /// with Cloudinary's automatic content-aware quality algorithm.
        /// </summary>
        public string QualityOverride { get; set; }

        /// <summary>
        /// Gets or sets Moderation.
        /// For all asset types: Set to manual to add the asset to a queue of pending assets that can be moderated
        /// using the Admin API or the Cloudinary Management Console, or set to metascan to automatically moderate
        /// the uploaded asset using the MetaDefender Anti-malware Protection add-on.
        /// For images only: Set to webpurify or aws_rek to automatically moderate the image
        /// using the WebPurify Image Moderation add-on or the Amazon Rekognition AI Moderation add-on respectively.
        /// Note: Rejected assets are automatically invalidated on the CDN within approximately ten minutes.
        /// </summary>
        public string Moderation { get; set; }

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
            if (string.IsNullOrEmpty(PublicId))
            {
                throw new ArgumentException("PublicId must be set!");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();

            AddParam(dict, "public_id", PublicId);
            AddParam(dict, "tags", Tags);
            AddParam(dict, "type", Type);
            AddParam(dict, "ocr", Ocr);
            AddParam(dict, "eager_async", EagerAsync);
            AddParam(dict, "eager_notification_url", EagerNotificationUrl);
            AddParam(dict, "invalidate", Invalidate);
            AddParam(dict, "async", Async);
            AddParam(dict, "quality_analysis", QualityAnalysis);
            AddParam(dict, "cinemagraph_analysis", CinemagraphAnalysis);
            AddParam(dict, "overwrite", Overwrite);
            AddParam(dict, "image_metadata", ImageMetadata);
            AddParam(dict, "notification_url", NotificationUrl);
            AddParam(dict, "quality_override", QualityOverride);
            AddParam(dict, "moderation", Moderation);
            AddParam(dict, "accessibility_analysis", AccessibilityAnalysis);

            if (ResourceType == ResourceType.Image)
            {
                AddParam(dict, "colors", Colors);
                AddParam(dict, "phash", Phash);
                AddParam(dict, "faces", Faces);
            }

            AddCoordinates(dict, "face_coordinates", FaceCoordinates);
            AddCoordinates(dict, "custom_coordinates", CustomCoordinates);

            if (EagerTransforms != null)
            {
                AddParam(
                    dict,
                    "eager",
                    string.Join("|", EagerTransforms.Select(t => t.Generate()).ToArray()));
            }

            if (Context != null && Context.Count > 0)
            {
                AddParam(dict, Constants.CONTEXT_PARAM_NAME, Utils.SafeJoin("|", Context.SafePairs));
            }

            if (Metadata != null && Metadata.Count > 0)
            {
                AddParam(dict, Constants.METADATA_PARAM_NAME, Utils.SafeJoin("|", Metadata.SafePairs));
            }

            if (ResponsiveBreakpoints != null && ResponsiveBreakpoints.Count > 0)
            {
                AddParam(dict, "responsive_breakpoints", JsonConvert.SerializeObject(ResponsiveBreakpoints));
            }

            if (AccessControl != null && AccessControl.Count > 0)
            {
                AddParam(dict, "access_control", JsonConvert.SerializeObject(AccessControl));
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

            return dict;
        }
    }
}
