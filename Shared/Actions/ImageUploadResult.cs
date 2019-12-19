namespace CloudinaryDotNet.Actions
{
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
        /// Parameter "width" of the image.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Parameter "height" of the image.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// Exif metadata for the image.
        /// </summary>
        [DataMember(Name = "exif")]
        public Dictionary<string, string> Exif { get; protected set; }

        /// <summary>
        /// Returned metadata for the image. Includes: PixelsPerUnitX, PixelsPerUnitY, PixelUnits, Colorspace, and DPI.
        /// </summary>
        [DataMember(Name = "image_metadata")]
        public Dictionary<string, string> Metadata { get; protected set; }

        /// <summary>
        /// The coordinates of faces contained in an uploaded image.
        /// </summary>
        [DataMember(Name = "faces")]
        public int[][] Faces { get; protected set; }

        /// <summary>
        /// Quality analysis value for the image between 0 and 1, where 0 means the image is blurry and out of focus
        /// and 1 means the image is sharp and in focus.
        /// </summary>
        [DataMember(Name = "quality_analysis")]
        public QualityAnalysis QualityAnalysis { get; protected set; }

        /// <summary>
        /// The predominant colors and color histogram of the uploaded image.
        /// Note: If all returned colors are opaque, then 6-digit RGB hex values are returned.
        /// If one or more colors contain an alpha channel, then 8-digit RGBA hex quadruplet values are returned.
        /// </summary>
        [DataMember(Name = "colors")]
        public string[][] Colors { get; protected set; }

        /// <summary>
        /// The perceptual hash (pHash) of the uploaded photo for image similarity detection.
        /// </summary>
        [DataMember(Name = "phash")]
        public string Phash { get; protected set; }

        /// <summary>
        /// The deletion token for the image. The token can be used to delete the uploaded asset within 10 minutes
        /// using an unauthenticated API request.
        /// </summary>
        [DataMember(Name = "delete_token")]
        public string DeleteToken { get; protected set; }

        /// <summary>
        /// The detailed info about the image.
        /// </summary>
        [DataMember(Name = "info")]
        public Info Info { get; protected set; }

        /// <summary>
        /// The number of page(s) or layers in a multi-page or multi-layer file (PDF, animated GIF, TIFF, PSD).
        /// </summary>
        [DataMember(Name = "pages")]
        public int Pages { get; protected set; }

        /// <summary>
        /// List of responsive breakpoints for the image.
        /// </summary>
        public List<ResponsiveBreakpointList> ResponsiveBreakpoints { get; set; }

        /// <summary>
        /// Overrides corresponding method of <see cref="BaseResult"/> class.
        /// Populates additional token fields.
        /// </summary>
        /// <param name="source">JSON token received from the server.</param>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            Width = source.ReadValueAsSnakeCase<int>(nameof(Width));
            Height = source.ReadValueAsSnakeCase<int>(nameof(Height));
            Exif = source.ReadValueAsSnakeCase<Dictionary<string, string>>(nameof(Exif));
            Metadata = source.ReadValue<Dictionary<string, string>>("image_metadata");
            QualityAnalysis = source.ReadObject(nameof(QualityAnalysis).ToSnakeCase(), _ => new QualityAnalysis(_));
            Phash = source.ReadValueAsSnakeCase<string>(nameof(Phash));
            DeleteToken = source.ReadValueAsSnakeCase<string>(nameof(DeleteToken));
            Info = source.ReadValueAsSnakeCase<Info>(nameof(Info));
            Pages = source.ReadValueAsSnakeCase<int>(nameof(Pages));
            Colors = source.ReadValueAsSnakeCase<string[][]>(nameof(Colors));
            Faces = source.ReadValueAsSnakeCase<int[][]>(nameof(Faces));
            ResponsiveBreakpoints = source.ReadValueAsSnakeCase<List<ResponsiveBreakpointList>>(nameof(ResponsiveBreakpoints));
        }
    }
}
