namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed response after upload of the image resource.
    /// </summary>
    [DataContract]
    public class ImageUploadResult : RawUploadResult
    {
        /// <summary>
        /// Gets or sets parameter "width" of the image.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets parameter "height" of the image.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets exif metadata for the image.
        /// </summary>
        [DataMember(Name = "exif")]
        public Dictionary<string, string> Exif { get; set; }

        /// <summary>
        /// Gets or sets returned metadata for the image. Includes: PixelsPerUnitX, PixelsPerUnitY, PixelUnits, Colorspace, and DPI.
        /// </summary>
        [Obsolete("Property Metadata is deprecated, please use ImageMetadata instead")]
        public Dictionary<string, string> Metadata
        {
            get { return ImageMetadata; }
            set { ImageMetadata = value; }
        }

        /// <summary>
        /// Gets or sets the returned metadata for the image.
        /// Includes: PixelsPerUnitX, PixelsPerUnitY, PixelUnits, Colorspace, and DPI.
        /// </summary>
        [DataMember(Name = "image_metadata")]
        public Dictionary<string, string> ImageMetadata { get; set; }

        /// <summary>
        /// Gets or sets the coordinates of faces contained in an uploaded image.
        /// </summary>
        [DataMember(Name = "faces")]
        public int[][] Faces { get; set; }

        /// <summary>
        /// Gets or sets quality analysis value for the image between 0 and 1, where 0 means the image is blurry and out of focus
        /// and 1 means the image is sharp and in focus.
        /// </summary>
        [DataMember(Name = "quality_analysis")]
        public QualityAnalysis QualityAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the quality score.
        /// </summary>
        [DataMember(Name = "quality_score")]
        public double QualityScore { get; set; }

        /// <summary>
        /// Gets or sets the predominant colors and color histogram of the uploaded image.
        /// Note: If all returned colors are opaque, then 6-digit RGB hex values are returned.
        /// If one or more colors contain an alpha channel, then 8-digit RGBA hex quadruplet values are returned.
        /// </summary>
        [DataMember(Name = "colors")]
        public string[][] Colors { get; set; }

        /// <summary>
        /// Gets or sets the perceptual hash (pHash) of the uploaded photo for image similarity detection.
        /// </summary>
        [DataMember(Name = "phash")]
        public string Phash { get; set; }

        /// <summary>
        /// Gets or sets the deletion token for the image. The token can be used to delete the uploaded asset within 10 minutes
        /// using an unauthenticated API request.
        /// </summary>
        [DataMember(Name = "delete_token")]
        public string DeleteToken { get; set; }

        /// <summary>
        /// Gets or sets the detailed info about the image.
        /// </summary>
        [DataMember(Name = "info")]
        public Info Info { get; set; }

        /// <summary>
        /// Gets or sets the number of page(s) or layers in a multi-page or multi-layer file (PDF, animated GIF, TIFF, PSD).
        /// </summary>
        [DataMember(Name = "pages")]
        public int Pages { get; set; }

        /// <summary>
        /// Gets or sets list of responsive breakpoints for the image.
        /// </summary>
        public List<ResponsiveBreakpointList> ResponsiveBreakpoints { get; set; }

        /// <summary>
        /// Gets or sets a key-value pairs of context associated with the resource.
        /// </summary>
        [DataMember(Name = "context")]
        public JToken Context { get; set; }

        /// <summary>
        /// Gets or sets the likelihood that the image is an illustration as opposed to a photograph.
        /// A value between 0 (photo) and 1.0 (illustration).
        /// </summary>
        [DataMember(Name = "illustration_score")]
        public float IllustrationScore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if the image has an alpha (transparency) channel.
        /// </summary>
        [DataMember(Name = "semi_transparent")]
        public bool SemiTransparent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the image only contains a single grayscale channel.
        /// </summary>
        [DataMember(Name = "grayscale")]
        public bool Grayscale { get; set; }

        /// <summary>
        /// Gets or sets the specific type of asset.
        /// </summary>
        /// <summary>
        /// The derived images generated as per the requested eager transformations of the method call.
        /// </summary>
        [DataMember(Name = "eager")]
        public Eager[] Eager { get; set; }

        /// <summary>
        /// Gets or sets the predominant colors in the image according to both a Google palette and a Cloudinary palette.
        /// </summary>
        [DataMember(Name = "predominant")]
        public Predominant Predominant { get; set; }

        /// <summary>
        /// Gets or sets details of cinemagraph analysis for the image.
        /// </summary>
        [DataMember(Name = "cinemagraph_analysis")]
        public CinemagraphAnalysis CinemagraphAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the color ambiguity score that indicate how good\bad an image is for colorblind people.
        /// Will they be able to differentiate between different elements in the image.
        /// </summary>
        [DataMember(Name = "accessibility_analysis")]
        public AccessibilityAnalysis AccessibilityAnalysis { get; set; }

        /// <summary>
        /// Overrides corresponding method of <see cref="BaseResult"/> class.
        /// Populates additional token fields.
        /// </summary>
        /// <param name="source">JSON token received from the server.</param>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            var responsiveBreakpoints = source["responsive_breakpoints"];
            if (responsiveBreakpoints != null)
            {
                ResponsiveBreakpoints = responsiveBreakpoints.ToObject<List<ResponsiveBreakpointList>>();
            }
        }
    }
}
