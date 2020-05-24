namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed response after a call of Explicit method.
    /// </summary>
    [DataContract]
    public class ExplicitResult : RawUploadResult
    {
        /// <summary>
        /// The specific type of asset.
        /// </summary>
        /// <summary>
        /// The derived images generated as per the requested eager transformations of the method call.
        /// </summary>
        [DataMember(Name = "eager")]
        public Eager[] Eager { get; protected set; }

        /// <summary>
        /// List of responsive breakpoint settings of the asset.
        /// </summary>
        public List<ResponsiveBreakpointList> ResponsiveBreakpoints { get; set; }

        /// <summary>
        /// Status is returned when passing 'Async' argument set to 'true' to the server.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; protected set; }

        /// <summary>
        /// Any requested information from executing one of the Cloudinary Add-ons on the media asset.
        /// </summary>
        [DataMember(Name = "info")]
        public Info Info { get; protected set; }

        /// <summary>
        /// Details of the quality analysis.
        /// </summary>
        [DataMember(Name = "quality_analysis")]
        public QualityAnalysis QualityAnalysis { get; protected set; }

        /// <summary>
        /// Color information: predominant colors and histogram of 32 leading colors.
        /// </summary>
        [DataMember(Name = "colors")]
        public string[][] Colors { get; protected set; }

        /// <summary>
        /// Gets or sets details of cinemagraph analysis for the resource.
        /// </summary>
        [DataMember(Name = "cinemagraph_analysis")]
        public CinemagraphAnalysis CinemagraphAnalysis { get; protected set; }

        /// <summary>
        /// IPTC, XMP, and detailed Exif metadata. Supported for images, video, and audio.
        /// </summary>
        [Obsolete("Property Metadata is deprecated, please use ImageMetadata instead")]
        public Dictionary<string, string> Metadata
        {
            get { return ImageMetadata; }
            set { ImageMetadata = value; }
        }

        /// <summary>
        /// IPTC, XMP, and detailed Exif metadata. Supported for images, video, and audio.
        /// </summary>
        [DataMember(Name = "image_metadata")]
        public Dictionary<string, string> ImageMetadata { get; protected set; }

        /// <summary>
        /// A perceptual hash (pHash) of the uploaded resource for image similarity detection.
        /// </summary>
        [DataMember(Name = "phash")]
        public string Phash { get; protected set; }

        /// <summary>
        /// A list of coordinates of detected faces.
        /// </summary>
        [DataMember(Name = "faces")]
        public int[][] Faces { get; protected set; }

        /// <summary>
        /// Parameter "width" of the resource. Not relevant for raw files.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Parameter "height" of the resource. Not relevant for raw files.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// A slot token.
        /// </summary>
        [DataMember(Name = "slot_token")]
        public string SlotToken { get; protected set; }

        /// <summary>
        /// The number of pages in the asset: included if the asset has multiple pages (e.g., PDF or animated GIF).
        /// </summary>
        [DataMember(Name = "pages")]
        public int Pages { get; protected set; }

        /// <summary>
        /// The likelihood that the image is an illustration as opposed to a photograph.
        /// A value between 0 (photo) and 1.0 (illustration).
        /// </summary>
        [DataMember(Name = "illustration_score")]
        public float IllustrationScore { get; protected set; }

        /// <summary>
        /// The predominant colors in the image according to both a Google palette and a Cloudinary palette.
        /// </summary>
        [DataMember(Name = "predominant")]
        public Predominant Predominant { get; protected set; }

        /// <summary>
        /// The predominant colors in the image according to both a Google palette and a Cloudinary palette.
        /// </summary>
        [DataMember(Name = "profiling_data")]
        public ProfilingData[] ProfilingData { get; protected set; }

        /// <summary>
        /// The color ambiguity score that indicate how good\bad an image is for colorblind people.
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
            var responsiveBreakpoints = source["responsive_breakpoints"];
            if (responsiveBreakpoints != null)
            {
                ResponsiveBreakpoints = responsiveBreakpoints.ToObject<List<ResponsiveBreakpointList>>();
            }
        }
    }

    /// <summary>
    /// Details of the derived image generated as per the requested eager transformation applied.
    /// </summary>
    [DataContract]
    public class Eager
    {
        /// <summary>
        /// The URL for accessing the uploaded asset.
        /// </summary>
        [Obsolete("Property Uri is deprecated, please use Url instead")]
        public Uri Uri
        {
            get { return Url; }
            set { Url = value; }
        }

        /// <summary>
        /// The URL for accessing the uploaded asset.
        /// </summary>
        [DataMember(Name = "url")]
        public Uri Url { get; protected set; }

        /// <summary>
        /// The HTTPS URL for securely accessing the uploaded asset.
        /// </summary>
        [Obsolete("Property SecureUri is deprecated, please use SecureUrl instead")]
        public Uri SecureUri
        {
            get { return SecureUrl; }
            set { SecureUrl = value; }
        }

        /// <summary>
        /// The HTTPS URL for securely accessing the uploaded asset.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public Uri SecureUrl { get; protected set; }

        /// <summary>
        /// The transformation applied to the asset.
        /// </summary>
        [DataMember(Name = "transformation")]
        public string Transformation { get; protected set; }

        /// <summary>
        /// Parameter "width" of the resource. Not relevant for raw files.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Parameter "height" of the resource. Not relevant for raw files.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// The size of the media asset in bytes.
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Bytes { get; protected set; }

        /// <summary>
        /// Asset format.
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; protected set; }
    }

    /// <summary>
    /// Represents processing statistics data.
    /// </summary>
    [DataContract]
    public class ProfilingData
    {
        /// <summary>
        /// Cpu usage metrics.
        /// </summary>
        [DataMember(Name = "cpu")]
        public long Cpu { get; protected set; }

        /// <summary>
        /// Real metrics.
        /// </summary>
        [DataMember(Name = "real")]
        public long Real { get; protected set; }

        /// <summary>
        /// Represents processing statistics data about actions on resource.
        /// </summary>
        [DataMember(Name = "action")]
        public ProfilingDataAction Action { get; protected set; }
    }

    /// <summary>
    /// Represents processing statistics data about actions on resource.
    /// </summary>
    [DataContract]
    public class ProfilingDataAction
    {
        /// <summary>
        /// Represent action name applied to resource asset.
        /// </summary>
        [DataMember(Name = "action")]
        public string Action { get; protected set; }

        /// <summary>
        /// Represent action parameter applied to resource asset.
        /// </summary>
        [DataMember(Name = "parameter")]
        public string Parameter { get; protected set; }

        /// <summary>
        /// Size before applied action.
        /// </summary>
        [DataMember(Name = "presize")]
        public long[] Presize { get; protected set; }

        /// <summary>
        /// Size after applied action.
        /// </summary>
        [DataMember(Name = "postsize")]
        public long[] Postsize { get; protected set; }
    }
}
