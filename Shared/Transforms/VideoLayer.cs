﻿namespace CloudinaryDotNet
{
    using System;

    /// <summary>
    /// Represents the video parameter (l_video: in URLs) to specify the name of another
    /// uploaded video to be added as an overlay.
    /// </summary>
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
            throw new InvalidOperationException("Cannot modify resourceType for video layers");
        }

        /// <summary>
        /// Type for video layer. Not allowed to modify.
        /// </summary>
        /// <param name="type">Type of the asset.</param>
        /// <returns>The layer with parameter applied.</returns>
        public new VideoLayer Type(string type)
        {
            throw new InvalidOperationException("Cannot modify type for video layers");
        }

        /// <summary>
        /// Format for video layer. Not allowed to modify.
        /// </summary>
        /// <param name="format">Format of the asset.</param>
        /// <returns>The layer with parameter applied.</returns>
        public new VideoLayer Format(string format)
        {
            throw new InvalidOperationException("Cannot modify format for video layers");
        }

        /// <summary>
        /// Get the string representation of the video layer parameters.
        /// </summary>
        /// <returns>A string that represents the layer.</returns>
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
