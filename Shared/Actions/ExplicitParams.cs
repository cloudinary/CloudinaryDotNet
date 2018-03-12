using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudinaryDotNet.Actions
{
    public class ExplicitParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExplicitParams"/> class.
        /// </summary>
        /// <param name="publicId">The identifier that is used for accessing the uploaded resource.</param>
        public ExplicitParams(string publicId)
        {
            PublicId = publicId;
            Type = string.Empty;
            Tags = string.Empty;
        }

        /// <summary>
        /// A list of transformations to create for the uploaded image during the upload process, instead of lazily creating them when being accessed by your site's visitors.
        /// </summary>
        public List<Transformation> EagerTransforms { get; set; }

        /// <summary>
        /// Whether to generate the eager transformations asynchronously in the background after the upload request is completed rather than online as part of the upload call. Default: false.
        /// </summary>
        public bool? EagerAsync { get; set; }

        /// <summary>
        /// An HTTP URL to send notification to (a webhook) when the generation of eager transformations is completed.
        /// </summary>
        public string EagerNotificationUrl { get; set; }

        public string Type { get; set; }

        /// <summary>
        /// The identifier that is used for accessing the uploaded resource. A randomly generated ID is assigned if not specified.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// An HTTP header or a list of headers lines for returning as response HTTP headers when delivering the uploaded image to your users. Supported headers: 'Link', 'X-Robots-Tag'. For example 'X-Robots-Tag: noindex'.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// A comma-separated list of tag names to assign to the uploaded image for later group reference.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Sets the face coordinates. Use plain string (x,y,w,h|x,y,w,h) or "Rectangle" />.
        /// </summary>
        public object FaceCoordinates { get; set; }

        /// <summary>
        /// Coordinates of an interesting region contained in an uploaded image. The given coordinates are used for cropping uploaded images using the custom gravity mode. The region is specified by the X and Y coordinates of the top left corner and the width and height of the region. For example: "85,120,220,310". Otherwise, one can use "Rectangle" structure.
        /// </summary>
        public object CustomCoordinates { get; set; }

        /// <summary>
        /// Allows to store a set of key-value pairs together with resource.
        /// </summary>
        public StringDictionary Context { get; set; }

        /// <summary>
        /// Allows generate breakpoints for an already uploaded image
        /// </summary>
        public List<ResponsiveBreakpoint> ResponsiveBreakpoints { get; set; }

        /// <summary>
        /// Optional. Pass a list of AccessControlRule parameters
        /// </summary>
        public List<AccessControlRule> AccessControl { get; set; }
        
        /// <summary>
        /// Whether to invalidate CDN cache copies of a previously uploaded image that shares the same public ID. Default: false.
        /// </summary>
        /// <value>
        ///   <c>true</c> to invalidate; otherwise, <c>false</c>.
        /// </value>
        public bool Invalidate { get; set; }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(PublicId))
                throw new ArgumentException("PublicId must be set!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "public_id", PublicId);
            AddParam(dict, "tags", Tags);
            AddParam(dict, "type", Type);
            AddParam(dict, "eager_async", EagerAsync);
            AddParam(dict, "eager_notification_url", EagerNotificationUrl);
            AddParam(dict, "invalidate", Invalidate);

            AddCoordinates(dict, "face_coordinates", FaceCoordinates);
            AddCoordinates(dict, "custom_coordinates", CustomCoordinates);

            if (EagerTransforms != null)
            {
                AddParam(dict, "eager",
                    string.Join("|", EagerTransforms.Select(t => t.Generate()).ToArray()));
            }

            if (Context != null && Context.Count > 0)
            {
                AddParam(dict, "context", string.Join("|", Context.Pairs));
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
