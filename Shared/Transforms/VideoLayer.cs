namespace CloudinaryDotNet
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents the video parameter (l_video: in URLs) to specify the name of another
    /// uploaded video to be added as an overlay.
    /// </summary>
    [SuppressMessage("Performance", "CA1822:MarkMembersAsStatic", Justification = "Reviewed.")]
    public class VideoLayer : BaseLayer<VideoLayer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoLayer"/> class.
        /// </summary>
        public VideoLayer()
        {
            m_resourceType = "video";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoLayer"/> class with public ID.
        /// </summary>
        /// <param name="publicId">Public ID of a previously uploaded PNG image.</param>
        public VideoLayer(string publicId)
            : this()
        {
            PublicId(publicId);
        }

        /// <summary>
        /// ResourceType for video layers. Not allowed to modify.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <returns>The layer with parameter applied.</returns>
        public new VideoLayer ResourceType(string resourceType)
        {
            throw new InvalidOperationException($"Cannot modify resourceType {resourceType} for video layers");
        }

        /// <summary>
        /// Type for video layer. Not allowed to modify.
        /// </summary>
        /// <param name="type">Type of the asset.</param>
        /// <returns>The layer with parameter applied.</returns>
        public new VideoLayer Type(string type)
        {
            throw new InvalidOperationException($"Cannot modify type {type} for video layers");
        }

        /// <summary>
        /// Format for video layer. Not allowed to modify.
        /// </summary>
        /// <param name="format">Format of the asset.</param>
        /// <returns>The layer with parameter applied.</returns>
        public new VideoLayer Format(string format)
        {
            throw new InvalidOperationException($"Cannot modify format {format} for video layers");
        }

        /// <summary>
        /// Get an additional parameters for the text layer.
        /// </summary>
        /// <returns>A string that represents additional parameters.</returns>
        public override string AdditionalParams()
        {
            if (string.IsNullOrEmpty(m_publicId))
            {
                throw new ArgumentException("Must supply publicId.");
            }

            return base.AdditionalParams();
        }
    }
}
