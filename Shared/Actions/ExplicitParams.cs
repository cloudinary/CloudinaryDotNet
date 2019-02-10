using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters for the explicit method to apply actions to already uploaded assets.
    /// </summary>
    public class ExplicitParams : BaseParams
    {
        /// <summary>
        /// Instantiates the <see cref="ExplicitParams"/> object with public id.
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
        /// (Optional) A list of transformations to create for the uploaded image during the upload process, instead
        /// of lazily creating them when being accessed by your site's visitors.
        /// </summary>
        public List<Transformation> EagerTransforms { get; set; }

        /// <summary>
        /// Determines whether to generate the eager transformations asynchronously in the background. Default: false.
        /// </summary>
        public bool? EagerAsync { get; set; }

        /// <summary>
        /// (Optional) An HTTP or HTTPS URL to notify your application (a webhook) when the generation of eager
        /// transformations is completed.
        /// </summary>
        public string EagerNotificationUrl { get; set; }

        /// <summary>
        /// The specific type of asset. 
        /// Valid values for uploaded images and videos: upload, private, or authenticated. 
        /// Valid values for remotely fetched images: fetch, facebook, twitter, gplus, instagram_name, gravatar, 
        /// youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Set to "adv_ocr" to extract all text elements in an image as well as the bounding box coordinates of each
        /// detected element using the OCR text detection and extraction add-on.
        /// Relevant for images only.
        /// </summary>
        public string Ocr { get; set; }
        
        /// <summary>
        /// The type of resource.
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// The identifier of the uploaded asset or the URL of the remote asset. 
        /// Note: The public ID value for images and videos should not include a file extension.Include the file
        /// extension for raw files only.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// An HTTP header or a list of headers lines for returning as response HTTP headers when delivering the
        /// uploaded image to your users. Supported headers: 'Link', 'X-Robots-Tag'. 
        /// For example 'X-Robots-Tag: noindex'.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// A comma-separated list of tag names to assign to an asset that replaces any current tags assigned to 
        /// the asset (if any).
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Sets the coordinates of faces contained in an uploaded image and overrides the automatically detected 
        /// faces.
        /// Use plain string (x,y,w,h|x,y,w,h) or <see cref="Core.Rectangle"/>.
        /// </summary>
        public object FaceCoordinates { get; set; }

        /// <summary>
        /// Sets the coordinates of a region contained in an uploaded image that is subsequently used for cropping
        /// uploaded images using the custom gravity mode. The region is specified by the X and Y coordinates of the top
        /// left corner and the width and height of the region, as a comma-separated list. 
        /// For example: 85,120,220,310. Relevant for images only.
        /// </summary>
        public object CustomCoordinates { get; set; }

        /// <summary>
        /// A list of the key-value pairs of general textual context metadata to attach to an uploaded asset. 
        /// The context values of uploaded files are available for fetching using the Admin API. 
        /// For example: alt=My image, caption=Profile image.
        /// </summary>
        public StringDictionary Context { get; set; }

        /// <summary>
        /// Requests that Cloudinary automatically find the best breakpoints from the array of breakpoint request
        /// settings.
        /// </summary>
        public List<ResponsiveBreakpoint> ResponsiveBreakpoints { get; set; }

        /// <summary>
        /// Optional. Pass a list of AccessControlRule parameters.
        /// </summary>
        public List<AccessControlRule> AccessControl { get; set; }

        /// <summary>
        /// Whether to invalidate the asset (and all its transformed versions) on the CDN. Default: false.
        /// </summary>
        /// <value>
        ///   <c>true</c> to invalidate; otherwise, <c>false</c>.
        /// </value>
        public bool Invalidate { get; set; }

        /// <summary>
        /// Tells Cloudinary whether to perform the request in the background (asynchronously). Default: false.
        /// </summary>
        public bool? Async { get; set; }

        /// <summary>
        /// Whether to return a quality analysis value for the image between 0 and 1, where 0 means the image is blurry
        /// and out of focus and 1 means the image is sharp and in focus. Default: false. Relevant for images only.
        /// </summary>
        public bool QualityAnalysis { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(PublicId))
                throw new ArgumentException("PublicId must be set!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "public_id", PublicId);
            AddParam(dict, "tags", Tags);
            AddParam(dict, "type", Type);
            AddParam(dict, "ocr", Ocr);
            AddParam(dict, "eager_async", EagerAsync);
            AddParam(dict, "eager_notification_url", EagerNotificationUrl);
            AddParam(dict, "invalidate", Invalidate);
            AddParam(dict, "async", Async);
            AddParam(dict, "quality_analysis", QualityAnalysis);

            AddCoordinates(dict, "face_coordinates", FaceCoordinates);
            AddCoordinates(dict, "custom_coordinates", CustomCoordinates);

            if (EagerTransforms != null)
            {
                AddParam(dict, "eager",
                    string.Join("|", EagerTransforms.Select(t => t.Generate()).ToArray()));
            }

            if (Context != null && Context.Count > 0)
            {
                AddParam(dict, Constants.CONTEXT_PARAM_NAME, Utils.SafeJoin("|", Context.SafePairs));
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
                    sb.AppendFormat("{0}: {1}\n", item.Key, item.Value);
                }

                dict.Add("headers", sb.ToString());
            }

            return dict;
        }
    }
}
