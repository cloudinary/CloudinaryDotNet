namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Details of the quality analysis.
    /// </summary>
    [DataContract]
    public class QualityAnalysis
    {
        /// <summary>
        /// Gets or sets jpeg quality value.
        /// </summary>
        [DataMember(Name = "jpeg_quality")]
        public double JpegQuality { get; set; }

        /// <summary>
        /// Gets or sets jpeg chroma value.
        /// </summary>
        [DataMember(Name = "jpeg_chroma")]
        public double JpegChroma { get; set; }

        /// <summary>
        /// Gets or sets focus value.
        /// </summary>
        [DataMember(Name = "focus")]
        public double Focus { get; set; }

        /// <summary>
        /// Gets or sets noise value.
        /// </summary>
        [DataMember(Name = "noise")]
        public double Noise { get; set; }

        /// <summary>
        /// Gets or sets contrast value.
        /// </summary>
        [DataMember(Name = "contrast")]
        public double Contrast { get; set; }

        /// <summary>
        /// Gets or sets exposure value.
        /// </summary>
        [DataMember(Name = "exposure")]
        public double Exposure { get; set; }

        /// <summary>
        /// Gets or sets saturation value.
        /// </summary>
        [DataMember(Name = "saturation")]
        public double Saturation { get; set; }

        /// <summary>
        /// Gets or sets lighting value.
        /// </summary>
        [DataMember(Name = "lighting")]
        public double Lighting { get; set; }

        /// <summary>
        /// Gets or sets pixel score value.
        /// </summary>
        [DataMember(Name = "pixel_score")]
        public double PixelScore { get; set; }

        /// <summary>
        /// Gets or sets color score value.
        /// </summary>
        [DataMember(Name = "color_score")]
        public double ColorScore { get; set; }

        /// <summary>
        /// Gets or sets DCT value.
        /// </summary>
        [DataMember(Name = "dct")]
        public double Dct { get; set; }

        /// <summary>
        /// Gets or sets blockiness value.
        /// </summary>
        [DataMember(Name = "blockiness")]
        public double Blockiness { get; set; }

        /// <summary>
        /// Gets or sets chroma subsampling value.
        /// </summary>
        [DataMember(Name = "chroma_subsampling")]
        public double ChromaSubsampling { get; set; }

        /// <summary>
        /// Gets or sets resolution value.
        /// </summary>
        [DataMember(Name = "resolution")]
        public double Resolution { get; set; }
    }
}