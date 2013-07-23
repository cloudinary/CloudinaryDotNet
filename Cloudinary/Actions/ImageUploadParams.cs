using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters for uploading image to cloudinary
    /// </summary>
    public class ImageUploadParams : RawUploadParams
    {
        /// <summary>
        /// A comma-separated list of tag names to assign to the uploaded image for later group reference.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// An optional format to convert the uploaded image to before saving in the cloud. For example: "jpg".
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Allows uploading images as 'private' or 'authenticated'. Valid values: 'upload', 'private' and 'authenticated'. Default: 'upload'.
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
        /// A transformation to run on the uploaded image before saving it in the cloud. For example: limit the dimension of the uploaded image to 512x512 pixels.
        /// </summary>
        public Transformation Transformation { get; set; }

        /// <summary>
        /// A list of transformations to create for the uploaded image during the upload process, instead of lazily creating them when being accessed by your site's visitors.
        /// </summary>
        public List<Transformation> EagerTransforms { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Whether to include EXIF in the response
        /// </summary>
        public bool Exif { get; set; }

        public bool Colors { get; set; }

        public bool Faces { get; set; }

        /// <summary>
        /// Whether to include metadata in the response
        /// </summary>
        public bool Metadata { get; set; }

        /// <summary>
        /// Whether to use file name as ID
        /// </summary>
        public bool UseFilename { get; set; }

        public bool EagerAsync { get; set; }

        public string NotificationUrl { get; set; }

        public string EagerNotificationUrl { get; set; }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "tags", Tags);
            AddParam(dict, "format", Format);

            AddParam(dict, "type", Type);
            AddParam(dict, "exif", Exif);
            AddParam(dict, "faces", Faces);
            AddParam(dict, "colors", Colors);
            AddParam(dict, "image_metadata", Metadata);
            AddParam(dict, "use_filename", UseFilename);
            AddParam(dict, "eager_async", EagerAsync);
            AddParam(dict, "invalidate", Invalidate);

            AddParam(dict, "notification_url", NotificationUrl);
            AddParam(dict, "eager_notification_url", EagerNotificationUrl);

            if (Transformation != null)
                AddParam(dict, "transformation", Transformation.Generate());

            if (EagerTransforms != null && EagerTransforms.Count > 0)
            {
                AddParam(dict, "eager",
                    String.Join("|", EagerTransforms.Select(t => t.Generate()).ToArray()));
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
