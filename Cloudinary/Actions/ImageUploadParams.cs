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
        /// A transformation to run on the uploaded image before saving it in the cloud. For example: limit the dimension of the uploaded image to 512x512 pixels.
        /// </summary>
        public Transformation Transformation { get; set; }

        /// <summary>
        /// A list of transformations to create for the uploaded image during the upload process, instead of lazily creating them when being accessed by your site's visitors.
        /// </summary>
        public List<Transformation> EagerTransforms { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "tags", Tags);
            AddParam(dict, "format", Format);

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
