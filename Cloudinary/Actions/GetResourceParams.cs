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

        public bool Exif { get; set; }

        public bool Colors { get; set; }

        public bool Faces { get; set; }

        public bool Metadata { get; set; }

        public int MaxResults { get; set; }

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

            if (MaxResults > 0)
                AddParam(dict, "max_results", MaxResults.ToString());

            AddParam(dict, "exif", Exif);
            AddParam(dict, "colors", Colors);
            AddParam(dict, "faces", Faces);
            AddParam(dict, "image_metadata", Metadata);

            return dict;
        }
    }
}
