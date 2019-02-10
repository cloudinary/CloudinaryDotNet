using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of the request of resource as well as its derived resources.
    /// </summary>
    public class GetResourceParams : BaseParams
    {
        /// <summary>
        /// Instantiates the <see cref="GetResourceParams"/> object.
        /// </summary>
        /// <param name="publicId"></param>
        public GetResourceParams(string publicId)
        {
            PublicId = publicId;
            Type = "upload";
            Exif = false;
            Colors = false;
            Faces = false;
        }

        /// <summary>
        /// Public id assigned to the requested resource.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// Optional (String, default: image). The type of file. 
        /// Possible values: image, raw, video. 
        /// Note: Use the video resource type for all video resources as well as for audio files, such as .mp3. 
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Optional (String, default: upload). The storage type: upload, private, authenticated, facebook, twitter,
        /// gplus, instagram_name, gravatar, youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion. 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Optional (Boolean, default: false). Deprecated. Use <see cref="Metadata"/> instead.
        /// </summary>
        public bool Exif { get; set; }

        /// <summary>
        /// Optional (Boolean, default: false). If true, include color information: predominant colors and histogram
        /// of 32 leading colors. 
        /// </summary>
        public bool Colors { get; set; }

        /// <summary>
        /// Optional (Boolean, default: false). If true, include a list of coordinates of detected faces. 
        /// </summary>
        public bool Faces { get; set; }

        /// <summary>
        /// Optional (Boolean, default: false). If true, returns a quality analysis value for the image 
        /// between 0 and 1, where 0 means the image is blurry and out of focus and 1 means the image is sharp 
        /// and in focus. 
        /// </summary>
        public bool QualityAnalysis { get; set; }

        /// <summary>
        /// Optional (Boolean, default: false). If true, include IPTC, XMP, and detailed Exif metadata.
        /// Supported for images, video, and audio. 
        /// </summary>
        public bool Metadata { get; set; }

        /// <summary>
        /// Optional (Boolean, default: false). If true, include previously specified custom cropping coordinates and
        /// faces coordinates.
        /// </summary>
        public bool Coordinates { get; set; }

        /// <summary>
        /// Optional. The number of derived images to return. Default=10. Maximum=100. 
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Optional (Boolean, default: false). If true, include the perceptual hash (pHash) of the uploaded photo for
        /// image similarity detection. 
        /// </summary>
        public bool Phash { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (String.IsNullOrEmpty(PublicId))
                throw new ArgumentException("PublicId must be set!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (MaxResults > 0)
                AddParam(dict, "max_results", MaxResults.ToString());

            AddParam(dict, "exif", Exif);
            AddParam(dict, "colors", Colors);
            AddParam(dict, "faces", Faces);
            AddParam(dict, "quality_analysis", QualityAnalysis);
            AddParam(dict, "image_metadata", Metadata);
            AddParam(dict, "phash", Phash);
            AddParam(dict, "coordinates", Coordinates);

            return dict;
        }
    }
}
