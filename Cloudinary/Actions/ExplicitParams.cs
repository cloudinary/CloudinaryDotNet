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
            Type = String.Empty;
            Tags = String.Empty;
        }

        /// <summary>
        /// A list of transformations to create for the uploaded image during the upload process, instead of lazily creating them when being accessed by your site's visitors.
        /// </summary>
        public List<Transformation> EagerTransforms { get; set; }

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
        /// Sets the face coordinates. Use plain string (x,y,w,h|x,y,w,h) or <see cref="FaceCoordinates"> object</see>/>.
        /// </summary>
        public object FaceCoordinates { get; set; }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            if (String.IsNullOrEmpty(PublicId))
                throw new ArgumentException("PublicId must be set!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = new SortedDictionary<string, object>();

            AddParam(dict, "public_id", PublicId);
            AddParam(dict, "tags", Tags);
            AddParam(dict, "type", Type);

            if (FaceCoordinates != null)
            {
                AddParam(dict, "face_coordinates", FaceCoordinates.ToString());
            }

            if (EagerTransforms != null)
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
