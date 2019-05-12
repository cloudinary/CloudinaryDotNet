using System;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Represents the video parameter (l_video: in URLs) to specify the name of another 
    /// uploaded video to be added as an overlay.   
    /// </summary>
    public class VideoLayer : BaseLayer<VideoLayer>
    {
        /// <summary>
        /// Instantiates the <see cref="VideoLayer"/> object.
        /// </summary>
        public VideoLayer()
        {
            m_resourceType = "video";
        }

        /// <summary>
        /// Instantiates the <see cref="VideoLayer"/> object with public ID.
        /// </summary>
        /// <param name="publicId">Public ID of a previously uploaded PNG image.</param>
        public VideoLayer(string publicId) : this()
        {
            PublicId(publicId);
        }

        /// <summary>
        /// ResourceType for video layers. Not allowed to modify.
        /// </summary>
        public new VideoLayer ResourceType(string resourceType)
        {
            throw new InvalidOperationException("Cannot modify resourceType for video layers");
        }

        /// <summary>
        /// Type for video layer. Not allowed to modify.
        /// </summary>
        public new VideoLayer Type(string type)
        {
            throw new InvalidOperationException("Cannot modify type for video layers");
        }

        /// <summary>
        /// Format for video layer. Not allowed to modify.
        /// </summary>
        public new VideoLayer Format(string format)
        {
            throw new InvalidOperationException("Cannot modify format for video layers");
        }

        /// <summary>
        /// Get the string representation of the video layer parameters.
        /// </summary>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(m_publicId))
            {
                throw new ArgumentException("Must supply publicId.");
            }

            return base.ToString();
        }
    }
}
