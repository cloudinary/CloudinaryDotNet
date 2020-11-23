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
        /// Gets or sets the storage type: upload, private, authenticated, facebook, twitter,
        /// gplus, instagram_name, gravatar, youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Exif is used. Optional (Boolean, default: false). Deprecated. Use <see cref="ImageMetadata"/> instead.
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
        [Obsolete("Property Metadata is deprecated, please use ImageMetadata instead")]
        public bool? Metadata
        {
            get { return ImageMetadata; }
            set { ImageMetadata = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include IPTC, XMP, and detailed Exif metadata.
        /// Supported for images, video, and audio.
        /// </summary>
        public bool? ImageMetadata { get; set; }

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
        /// Gets or sets find all assets with a public ID that starts with the given prefix.
        /// The assets are sorted by public ID in the response.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        ///  Gets or sets when a request has more results to return than max_results,
        /// the next_cursor value is returned as part of the response.
        /// You can then specify this value as the next_cursor parameter of a following request.
        /// </summary>
        public string NextCursor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the animation/video is cinemagraph. Optional (Boolean, default: false).
        /// If true, includes a cinemagraph analysis value for the animation/video between 0 and 1, where 0 means the video/animation
        /// is NOT a cinemagraph and 1 means the GIF/video IS a cinemagraph.
        /// Running cinemagraph analysis on static images returns 0.
        /// </summary>
        public bool? CinemagraphAnalysis { get; set; }

        /// <summary>
        /// Gets or sets get assets that were created since the given timestamp.
        /// Supported unless prefix or public_ids were specified.
        /// </summary>
        public string StartAt { get; set; }

        /// <summary>
        /// Gets or sets the order of returned assets, according to the created_at date.
        /// Note: if a prefix is specified, this parameter is ignored and the results
        /// are sorted by public ID. Possible values: desc or -1 (default), asc or 1.
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// Gets or sets whether to include the list of tag names assigned to each asset. Default: false.
        /// </summary>
        public bool? Tags { get; set; }

        /// <summary>
        /// Gets or sets whether to include key-value pairs of context associated with each asset. Default: false.
        /// </summary>
        public bool? Context { get; set; }

        /// <summary>
        /// Gets or sets whether to optionally include the image moderation status of each asset. Default: false.
        /// </summary>
        public bool? Moderation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include accessibility analysis information.
        /// Optional (Boolean, default: false).
        /// </summary>
        public bool? AccessibilityAnalysis { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include asset version information. Default: false.
        /// </summary>
        public bool? Versions { get; set; }

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

            if (MaxResults > 0)
            {
                AddParam(dict, "max_results", MaxResults.ToString(CultureInfo.InvariantCulture));
            }

            AddParam(dict, "exif", Exif);
            AddParam(dict, "colors", Colors);
            AddParam(dict, "faces", Faces);
            AddParam(dict, "quality_analysis", QualityAnalysis);
            AddParam(dict, "image_metadata", ImageMetadata);
            AddParam(dict, "phash", Phash);
            AddParam(dict, "coordinates", Coordinates);
            AddParam(dict, "pages", Pages);
            AddParam(dict, "derived_next_cursor", DerivedNextCursor);
            AddParam(dict, "cinemagraph_analysis", CinemagraphAnalysis);
            AddParam(dict, "tags", Tags);
            AddParam(dict, "context", Context);
            AddParam(dict, "moderation", Moderation);
            AddParam(dict, "prefix", Prefix);
            AddParam(dict, "next_cursor", NextCursor);
            AddParam(dict, "start_at", StartAt);
            AddParam(dict, "direction", Direction);
            AddParam(dict, "accessibility_analysis", AccessibilityAnalysis);
            AddParam(dict, "versions", Versions);

            return dict;
        }
    }
}
