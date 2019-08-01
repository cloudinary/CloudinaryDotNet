using CloudinaryDotNet.Actions;
using System;
using System.Threading.Tasks;

namespace CloudinaryDotNet
{
    public partial class Cloudinary
    {
        /// <summary>
        /// Create a new streaming profile.
        /// </summary>
        /// <param name="parameters">Parameters of straming profile creating.</param>
        public Task<StreamingProfileResult> CreateStreamingProfileAsync(StreamingProfileCreateParams parameters)
        {
            return m_api.CallApiAsync<StreamingProfileResult>(
                HttpMethod.POST,
                m_api.ApiUrlStreamingProfileV.BuildUrl(),
                parameters,
                null);
        }
        /// <summary>
        /// Create a new streaming profile
        /// </summary>
        public StreamingProfileResult CreateStreamingProfile(StreamingProfileCreateParams parameters)
        {
            return m_api.CallApi<StreamingProfileResult>(
                HttpMethod.POST,
                m_api.ApiUrlStreamingProfileV.BuildUrl(),
                parameters,
                null);
        }

        /// <summary>
        /// Update streaming profile.
        /// </summary>
        /// <param name="name">Name to be assigned to a streaming profile.</param>
        /// <param name="parameters">Parameters of straming profile updating.</param>
        /// <exception cref="ArgumentNullException">parameters can't be null</exception>
        /// <exception cref="ArgumentException">name can't be null or empty</exception>
        public Task<StreamingProfileResult> UpdateStreamingProfileAsync(
            string name,
            StreamingProfileUpdateParams parameters)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name parameter should be defined", nameof(name));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            return m_api.CallApiAsync<StreamingProfileResult>(
                HttpMethod.PUT,
                m_api.ApiUrlStreamingProfileV.Add(name).BuildUrl(),
                parameters,
                null);
        }

        /// <summary>
        /// Update streaming profile
        /// </summary>
        /// <exception cref="ArgumentNullException">both arguments can't be null</exception>
        public StreamingProfileResult UpdateStreamingProfile(string name, StreamingProfileUpdateParams parameters)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            return m_api.CallApi<StreamingProfileResult>(
                HttpMethod.PUT,
                m_api.ApiUrlStreamingProfileV.Add(name).BuildUrl(),
                parameters,
                null);
        }

        /// <summary>
        /// Delete streaming profile.
        /// </summary>
        /// <param name="name">The Name of streaming profile.</param>
        /// <exception cref="ArgumentException">name can't be null</exception>
        public Task<StreamingProfileResult> DeleteStreamingProfileAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name parameter should be defined", nameof(name));

            return m_api.CallApiAsync<StreamingProfileResult>(
                HttpMethod.DELETE,
                m_api.ApiUrlStreamingProfileV.Add(name).BuildUrl(),
                null,
                null);
        }

        /// <summary>
        /// Delete streaming profile
        /// </summary>
        /// <exception cref="ArgumentNullException">name can't be null</exception>
        public StreamingProfileResult DeleteStreamingProfile(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return m_api.CallApi<StreamingProfileResult>(
                HttpMethod.DELETE, m_api.ApiUrlStreamingProfileV.Add(name).BuildUrl(), null, null);
        }

        /// <summary>
        /// Retrieve the details of a single streaming profile by name.
        /// </summary>
        /// <param name="name">The Name of streaming profile.</param>
        /// <exception cref="ArgumentException">name can't be null</exception>
        public Task<StreamingProfileResult> GetStreamingProfileAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name parameter should be defined", nameof(name));

            return m_api.CallApiAsync<StreamingProfileResult>(
                HttpMethod.GET,
                m_api.ApiUrlStreamingProfileV.Add(name).BuildUrl(),
                null,
                null);
        }

        /// <summary>
        /// Retrieve the details of a single streaming profile by name
        /// </summary>
        /// <exception cref="ArgumentNullException">name can't be null</exception>
        public StreamingProfileResult GetStreamingProfile(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name parameter should be defined", nameof(name));

            return m_api.CallApi<StreamingProfileResult>(
                HttpMethod.GET, m_api.ApiUrlStreamingProfileV.Add(name).BuildUrl(), null, null);
        }

        /// <summary>
        /// Retrieve the list of streaming profiles, including built-in and custom profiles.
        /// </summary>
        public Task<StreamingProfileListResult> ListStreamingProfilesAsync()
        {
            return m_api.CallApiAsync<StreamingProfileListResult>(
                HttpMethod.GET,
                m_api.ApiUrlStreamingProfileV.BuildUrl(),
                null,
                null);
        }

        /// <summary>
        /// Retrieve the list of streaming profiles, including built-in and custom profiles
        /// </summary>
        public StreamingProfileListResult ListStreamingProfiles()
        {
            return m_api.CallApi<StreamingProfileListResult>(
                HttpMethod.GET, m_api.ApiUrlStreamingProfileV.BuildUrl(), null, null);
        }
    }
}
