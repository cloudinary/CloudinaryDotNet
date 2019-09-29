﻿namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;

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
        public FetchLayer Url(string url)
        {
            this.m_url = UrlEncode(url);
            return this;
        }

        /// <summary>
        /// Get an additional parameters for the fetch layer.
        /// </summary>
        public override string AdditionalParams()
        {
            List<string> components = new List<string>();
            if (!string.IsNullOrEmpty(m_url))
            {
                components.Add(string.Format("fetch:{0}", m_url));
            }

            return string.Join(":", components.ToArray());
        }

        /// <summary>
        /// Get this layer represented as string.
        /// </summary>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(m_url))
            {
                throw new ArgumentException("Must supply url.");
            }

            return AdditionalParams();
        }

        /// <summary>
        /// Prepare text for Overlay.
        /// </summary>
        private string UrlEncode(string url)
        {
            // Microsoft.IdentityModel.Tokens
            // return Base64UrlEncoder.Encode(StringToEncode);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(url);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
