using CloudinaryDotNet.Actions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CloudinaryDotNet
{
    public partial class Cloudinary
    {
        /// <summary>
        /// Create a new streaming profile asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of streaming profile creating.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        public Task<StreamingProfileResult> CreateStreamingProfileAsync(StreamingProfileCreateParams parameters, CancellationToken? cancellationToken = null) => 
            CallStreamingProfileApiAsync(HttpMethod.POST, parameters, cancellationToken);

        /// <summary>
        /// Create a new streaming profile
        /// </summary>
        /// <param name="parameters">Parameters of streaming profile creating.</param>
        public StreamingProfileResult CreateStreamingProfile(StreamingProfileCreateParams parameters) =>
            CallStreamingProfileApi(HttpMethod.POST, parameters);

        /// <summary>
        /// Update streaming profile asynchronously.
        /// </summary>
        /// <param name="name">Name to be assigned to a streaming profile.</param>
        /// <param name="parameters">Parameters of streaming profile updating.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <exception cref="ArgumentNullException">parameters can't be null</exception>
        /// <exception cref="ArgumentException">name can't be null or empty</exception>
        public Task<StreamingProfileResult> UpdateStreamingProfileAsync(
            string name,
            StreamingProfileUpdateParams parameters, 
            CancellationToken? cancellationToken = null)
        {
            ValidateCallStreamingProfileApiParameters(name, parameters);
            return CallStreamingProfileApiAsync(HttpMethod.PUT, parameters, cancellationToken);
        }

        /// <summary>
        /// Update streaming profile
        /// </summary>
        /// <param name="name">Name to be assigned to a streaming profile.</param>
        /// <param name="parameters">Parameters of streaming profile updating.</param>
        /// <exception cref="ArgumentNullException">both arguments can't be null</exception>
        public StreamingProfileResult UpdateStreamingProfile(string name, StreamingProfileUpdateParams parameters)
        {
            ValidateCallStreamingProfileApiParameters(name, parameters);
            return CallStreamingProfileApi(HttpMethod.PUT, parameters, name);
        }

        /// <summary>
        /// Delete streaming profile asynchronously.
        /// </summary>
        /// <param name="name">The Name of streaming profile.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <exception cref="ArgumentException">name can't be null</exception>
        public Task<StreamingProfileResult> DeleteStreamingProfileAsync(string name, CancellationToken? cancellationToken = null)
        {
            ValidateNameForCallStreamingProfileApiParameters(name);
            return CallStreamingProfileApiAsync(HttpMethod.DELETE, null, cancellationToken, name);
        }

        /// <summary>
        /// Delete streaming profile
        /// </summary>
        /// <exception cref="ArgumentNullException">name can't be null</exception>
        public StreamingProfileResult DeleteStreamingProfile(string name)
        {
            ValidateNameForCallStreamingProfileApiParameters(name);
            return CallStreamingProfileApi(HttpMethod.DELETE, null, name);
        }

        /// <summary>
        /// Retrieve the details of a single streaming profile by name asynchronously.
        /// </summary>
        /// <param name="name">The Name of streaming profile.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <exception cref="ArgumentException">name can't be null</exception>
        public Task<StreamingProfileResult> GetStreamingProfileAsync(string name, CancellationToken? cancellationToken = null)
        {
            ValidateNameForCallStreamingProfileApiParameters(name);
            return CallStreamingProfileApiAsync(HttpMethod.GET, null, cancellationToken, name);
        }

        /// <summary>
        /// Retrieve the details of a single streaming profile by name
        /// </summary>
        /// <exception cref="ArgumentNullException">name can't be null</exception>
        public StreamingProfileResult GetStreamingProfile(string name)
        {
            ValidateNameForCallStreamingProfileApiParameters(name);
            return CallStreamingProfileApi(HttpMethod.GET, null, name);
        }

        /// <summary>
        /// Retrieve the list of streaming profiles, including built-in and custom profiles asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        public Task<StreamingProfileListResult> ListStreamingProfilesAsync(CancellationToken? cancellationToken = null)
        {
            return m_api.CallApiAsync<StreamingProfileListResult>(
                HttpMethod.GET,
                m_api.ApiUrlStreamingProfileV.BuildUrl(),
                null,
                null, 
                null, 
                cancellationToken);
        }

        /// <summary>
        /// Retrieve the list of streaming profiles, including built-in and custom profiles
        /// </summary>
        public StreamingProfileListResult ListStreamingProfiles()
        {
            return m_api.CallApi<StreamingProfileListResult>(
                HttpMethod.GET, m_api.ApiUrlStreamingProfileV.BuildUrl(), null, null);
        }

        private Task<StreamingProfileResult> CallStreamingProfileApiAsync(
            HttpMethod httpMethod,
            BaseParams parameters, 
            CancellationToken? cancellationToken,
            string name = null)
        {
            return m_api.CallApiAsync<StreamingProfileResult>(
                httpMethod,
                m_api.ApiUrlStreamingProfileV.Add(name).BuildUrl(),
                parameters,
                null, 
                null, 
                cancellationToken);
        }

        private StreamingProfileResult CallStreamingProfileApi(HttpMethod httpMethod, BaseParams parameters, string name = null)
        {
            return m_api.CallApi<StreamingProfileResult>(
                httpMethod,
                m_api.ApiUrlStreamingProfileV.Add(name).BuildUrl(),
                parameters,
                null);
        }

        private static void ValidateCallStreamingProfileApiParameters(string name, StreamingProfileUpdateParams parameters)
        {
            ValidateNameForCallStreamingProfileApiParameters(name);
            ValidateStreamingProfileUpdateParams(parameters);
        }

        private static void ValidateStreamingProfileUpdateParams(StreamingProfileUpdateParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
        }

        private static void ValidateNameForCallStreamingProfileApiParameters(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name parameter should be defined", nameof(name));
        }
    }
}
