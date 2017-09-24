using System;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Represents the video parameter (l_video: in URLs) to specify the name of another 
    /// uploaded video to be added as an overlay
    /// </summary>
    public class VideoLayer : BaseLayer<VideoLayer>
    {
        public VideoLayer()
        {
            m_resourceType = "video";
        }

        public VideoLayer(string publicId) : this()
        {
            PublicId(publicId);
        }

        public new VideoLayer ResourceType(string resourceType)
        {
            throw new InvalidOperationException("Cannot modify resourceType for video layers");
        }

        public new VideoLayer Type(string type)
        {
            throw new InvalidOperationException("Cannot modify type for video layers");
        }

        public new VideoLayer Format(string format)
        {
            throw new InvalidOperationException("Cannot modify format for video layers");
        }

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
