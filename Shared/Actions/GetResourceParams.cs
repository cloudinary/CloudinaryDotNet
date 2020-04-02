namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Parameters of the request of resource as well as its derived resources.
    /// </summary>
    public class GetResourceParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetResourceParams"/> class.
        /// </summary>
        /// <param name="publicId">The public ID of the resource.</param>
        public GetResourceParams(string publicId)
        {
            PublicId = publicId;
            Type = "upload";
            Exif = false;
            Colors = false;
            Faces = false;
            Pages = false;
        }

        /// <summary>
        /// Gets or sets public id assigned to the requested resource.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets the type of file. Optional (String, default: image).
        /// Possible values: image, raw, video.
        /// Note: Use the video resource type for all video resources as well as for audio files, such as .mp3.
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Gets or sets optional (String, default: upload). The storage type: upload, private, authenticated, facebook, twitter,
        /// gplus, instagram_name, gravatar, youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Exif is used. Optional (Boolean, default: false). Deprecated. Use <see cref="Metadata"/> instead.
        /// </summary>
        public bool Exif { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include color information: predominant colors and histogram. Optional (Boolean, default: false).
        /// of 32 leading colors.
        /// </summary>
        public bool Colors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include a list of coordinates of detected faces. Optional (Boolean, default: false).
        /// </summary>
        public bool Faces { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to return a quality analysis value for the image
        /// between 0 and 1, where 0 means the image is blurry and out of focus and 1 means the image is sharp
        /// and in focus. Optional (Boolean, default: false).
        /// </summary>
        public bool QualityAnalysis { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include IPTC, XMP, and detailed Exif metadata.
        /// Supported for images, video, and audio. Optional (Boolean, default: false).
        /// </summary>
        public bool Metadata { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include previously specified custom cropping coordinates and
        /// faces coordinates. Optional (Boolean, default: false).
        /// </summary>
        public bool Coordinates { get; set; }

        /// <summary>
        /// Gets or sets the number of derived images to return. Default=10. Maximum=100. Optional.
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the perceptual hash (pHash) of the uploaded photo for
        /// image similarity detection. Optional (Boolean, default: false).
        /// </summary>
        public bool Phash { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a number of pages of a multi-page document.
        /// </summary>
        public bool Pages { get; set; }

        /// <summary>
        /// Gets or sets if there are more derived images than max_results, this is returned as part of the response.
        /// You can then specify this value as the derived_next_cursor parameter of the following listing request.
        /// </summary>
        public string DerivedNextCursor { get; set; }

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
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (MaxResults > 0)
            {
                AddParam(dict, "max_results", MaxResults.ToString(CultureInfo.InvariantCulture));
            }

            AddParam(dict, "exif", Exif);
            AddParam(dict, "colors", Colors);
            AddParam(dict, "faces", Faces);
            AddParam(dict, "quality_analysis", QualityAnalysis);
            AddParam(dict, "image_metadata", Metadata);
            AddParam(dict, "phash", Phash);
            AddParam(dict, "coordinates", Coordinates);
            AddParam(dict, "pages", Pages);
            AddParam(dict, "derived_next_cursor", DerivedNextCursor);

            return dict;
        }
    }
}
