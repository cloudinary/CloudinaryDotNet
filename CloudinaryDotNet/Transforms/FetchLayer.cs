namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Represents property of the overlay parameter to specify the Url on another image to be added as an overlay.
    /// </summary>
    public class FetchLayer : BaseLayer<FetchLayer>
    {
        /// <summary>
        /// The URL to fetch an image for.
        /// </summary>
        protected string m_url;

        /// <summary>
        /// Initializes a new instance of the <see cref="FetchLayer"/> class.
        /// Default parameterless constructor.
        /// </summary>
        public FetchLayer()
        {
            m_resourceType = Constants.RESOURCE_TYPE_IMAGE;

            m_type = Constants.RESOURCE_TYPE_FETCH;
        }

        /// <summary>
        /// The URL to fetch an image for.
        /// </summary>
        /// <param name="url">The image URL.</param>
        /// <returns>The layer with set parameter.</returns>
        public FetchLayer Url(string url)
        {
            this.m_url = url;
            return this;
        }

        /// <summary>
        /// Get an additional parameters for the fetch layer.
        /// </summary>
        /// <returns>A string that represents additional parameters.</returns>
        public override string AdditionalParams()
        {
            if (string.IsNullOrEmpty(m_url))
            {
                throw new ArgumentException("Must supply url.");
            }

            return Utils.EncodeUrlSafe(m_url);
        }
    }
}
