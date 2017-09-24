using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    public class GetResourceParams : BaseParams
    {
        public GetResourceParams(string publicId)
        {
            PublicId = publicId;
            Type = "upload";
            Exif = false;
            Colors = false;
            Faces = false;
        }

        public string PublicId { get; set; }

        public ResourceType ResourceType { get; set; }

        public string Type { get; set; }

        /// <summary>
        /// Whether to include EXIF info in result.
        /// </summary>
        public bool Exif { get; set; }

        /// <summary>
        /// Whether to include colors info in result.
        /// </summary>
        public bool Colors { get; set; }

        /// <summary>
        /// Whether to include faces coordinates in result.
        /// </summary>
        public bool Faces { get; set; }

        /// <summary>
        /// Whether to include metadata in result.
        /// </summary>
        public bool Metadata { get; set; }

        /// <summary>
        /// Whether to include custom coordinates in result.
        /// </summary>
        public bool Coordinates { get; set; }

        public int MaxResults { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to return the phash value.
        /// </summary>
        public bool Phash { get; set; }

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
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (MaxResults > 0)
                AddParam(dict, "max_results", MaxResults.ToString());

            AddParam(dict, "exif", Exif);
            AddParam(dict, "colors", Colors);
            AddParam(dict, "faces", Faces);
            AddParam(dict, "image_metadata", Metadata);
            AddParam(dict, "phash", Phash);
            AddParam(dict, "coordinates", Coordinates);

            return dict;
        }
    }
}
