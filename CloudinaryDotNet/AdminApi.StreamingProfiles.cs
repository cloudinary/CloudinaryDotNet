namespace CloudinaryDotNet
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Part of Cloudinary .NET API main class, responsible for streaming profiles management.
    /// </summary>
    internal partial class AdminApi
    {
        /// <inheritdoc/>
        public Task<StreamingProfileResult> CreateStreamingProfileAsync(StreamingProfileCreateParams parameters, CancellationToken? cancellationToken = null) =>
            CallStreamingProfileApiAsync(HttpMethod.POST, parameters, cancellationToken);

        /// <inheritdoc/>
        public StreamingProfileResult CreateStreamingProfile(StreamingProfileCreateParams parameters) =>
            CallStreamingProfileApi(HttpMethod.POST, parameters);

        /// <inheritdoc/>
        public Task<StreamingProfileResult> UpdateStreamingProfileAsync(
            string name,
            StreamingProfileUpdateParams parameters,
            CancellationToken? cancellationToken = null)
        {
            ValidateCallStreamingProfileApiParameters(name, parameters);
            return CallStreamingProfileApiAsync(HttpMethod.PUT, parameters, cancellationToken);
        }

        /// <inheritdoc/>
        public StreamingProfileResult UpdateStreamingProfile(string name, StreamingProfileUpdateParams parameters)
        {
            ValidateCallStreamingProfileApiParameters(name, parameters);
            return CallStreamingProfileApi(HttpMethod.PUT, parameters, name);
        }

        /// <inheritdoc/>
        public Task<StreamingProfileResult> DeleteStreamingProfileAsync(string name, CancellationToken? cancellationToken = null)
        {
            ValidateNameForCallStreamingProfileApiParameters(name);
            return CallStreamingProfileApiAsync(HttpMethod.DELETE, null, cancellationToken, name);
        }

        /// <inheritdoc/>
        public StreamingProfileResult DeleteStreamingProfile(string name)
        {
            ValidateNameForCallStreamingProfileApiParameters(name);
            return CallStreamingProfileApi(HttpMethod.DELETE, null, name);
        }

        /// <inheritdoc/>
        public Task<StreamingProfileResult> GetStreamingProfileAsync(string name, CancellationToken? cancellationToken = null)
        {
            ValidateNameForCallStreamingProfileApiParameters(name);
            return CallStreamingProfileApiAsync(HttpMethod.GET, null, cancellationToken, name);
        }

        /// <inheritdoc/>
        public StreamingProfileResult GetStreamingProfile(string name)
        {
            ValidateNameForCallStreamingProfileApiParameters(name);
            return CallStreamingProfileApi(HttpMethod.GET, null, name);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public StreamingProfileListResult ListStreamingProfiles()
        {
            return m_api.CallApi<StreamingProfileListResult>(
                HttpMethod.GET, m_api.ApiUrlStreamingProfileV.BuildUrl(), null, null);
        }

        private static void ValidateCallStreamingProfileApiParameters(string name, StreamingProfileUpdateParams parameters)
        {
            ValidateNameForCallStreamingProfileApiParameters(name);
            ValidateStreamingProfileUpdateParams(parameters);
        }

        private static void ValidateStreamingProfileUpdateParams(StreamingProfileUpdateParams parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }

        private static void ValidateNameForCallStreamingProfileApiParameters(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name parameter should be defined", nameof(name));
            }
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
    }
}
