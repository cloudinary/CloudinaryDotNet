namespace CloudinaryDotNet
{
    using System;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Provides methods for adaptive streaming profiles management.
    /// </summary>
    public partial class Cloudinary
    {
        /// <summary>
        /// Create a new streaming profile.
        /// </summary>
        /// <param name="parameters">Parameters that define streaming profile.</param>
        public StreamingProfileResult CreateStreamingProfile(StreamingProfileCreateParams parameters)
        {
            return m_api.CallApi<StreamingProfileResult>(HttpMethod.POST, m_api.ApiUrlStreamingProfileV.BuildUrl(), parameters, null);
        }

        /// <summary>
        /// Update streaming profile.
        /// </summary>
        /// <param name="name">Name of the streaming profile.</param>
        /// <param name="parameters">Parameters that define streaming profile update request.</param>
        /// <exception cref="ArgumentNullException">both arguments can't be null.</exception>
        public StreamingProfileResult UpdateStreamingProfile(string name, StreamingProfileUpdateParams parameters)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            return m_api.CallApi<StreamingProfileResult>(
                HttpMethod.PUT,
                m_api.ApiUrlStreamingProfileV.Add(name).BuildUrl(),
                parameters,
                null);
        }

        /// <summary>
        /// Delete streaming profile.
        /// </summary>
        /// <param name="name">Streaming profile name to delete.</param>
        /// <exception cref="ArgumentNullException">name can't be null.</exception>
        public StreamingProfileResult DeleteStreamingProfile(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return m_api.CallApi<StreamingProfileResult>(
                HttpMethod.DELETE, m_api.ApiUrlStreamingProfileV.Add(name).BuildUrl(), null, null);
        }

        /// <summary>
        /// Retrieve the details of a single streaming profile by name.
        /// </summary>
        /// <param name="name">Streaming profile name.</param>
        /// <exception cref="ArgumentNullException">name can't be null.</exception>
        public StreamingProfileResult GetStreamingProfile(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return m_api.CallApi<StreamingProfileResult>(
                HttpMethod.GET, m_api.ApiUrlStreamingProfileV.Add(name).BuildUrl(), null, null);
        }

        /// <summary>
        /// Retrieve the list of streaming profiles, including built-in and custom profiles.
        /// </summary>
        public StreamingProfileListResult ListStreamingProfiles()
        {
            return m_api.CallApi<StreamingProfileListResult>(
                HttpMethod.GET, m_api.ApiUrlStreamingProfileV.BuildUrl(), null, null);
        }
    }
}
