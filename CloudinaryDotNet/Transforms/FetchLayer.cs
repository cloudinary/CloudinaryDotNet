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
            m_resourceType = Constants.RESOURCE_TYPE_FETCH;
        }

        /// <summary>
        /// The URL to fetch an image for.
        /// </summary>
        /// <param name="url">The image URL.</param>
        /// <returns>The layer with set parameter.</returns>
        public FetchLayer Url(string url)
        {
            this.m_url = UrlEncode(url);
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

            List<string> components = new List<string>();
            if (!string.IsNullOrEmpty(m_url))
            {
                components.Add(string.Format(CultureInfo.InvariantCulture, "fetch:{0}", m_url));
            }

            return string.Join(":", components.ToArray());
        }

        /// <summary>
        /// Get this layer represented as string.
        /// </summary>
        /// <returns>A string that represents the layer.</returns>
        public override string ToString()
        {
            return AdditionalParams();
        }

        /// <summary>
        /// Prepare text for Overlay.
        /// </summary>
        private static string UrlEncode(string url)
        {
            // Microsoft.IdentityModel.Tokens
            // return Base64UrlEncoder.Encode(StringToEncode);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(url);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
