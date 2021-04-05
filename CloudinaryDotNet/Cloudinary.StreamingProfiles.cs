namespace CloudinaryDotNet
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Part of Cloudinary .NET API main class, responsible for streaming profiles management.
    /// </summary>
    public partial class Cloudinary
    {
        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.CreateStreamingProfileAsync method instead.")]
        public Task<StreamingProfileResult> CreateStreamingProfileAsync(StreamingProfileCreateParams parameters, CancellationToken? cancellationToken = null) =>
            AdminApi.CreateStreamingProfileAsync(parameters, cancellationToken);

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.CreateStreamingProfile method instead.")]
        public StreamingProfileResult CreateStreamingProfile(StreamingProfileCreateParams parameters) =>
            AdminApi.CreateStreamingProfile(parameters);

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateStreamingProfileAsync method instead.")]
        public Task<StreamingProfileResult> UpdateStreamingProfileAsync(
            string name,
            StreamingProfileUpdateParams parameters,
            CancellationToken? cancellationToken = null)
        {
            return AdminApi.UpdateStreamingProfileAsync(name, parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateStreamingProfile method instead.")]
        public StreamingProfileResult UpdateStreamingProfile(string name, StreamingProfileUpdateParams parameters)
        {
            return AdminApi.UpdateStreamingProfile(name, parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteStreamingProfileAsync method instead.")]
        public Task<StreamingProfileResult> DeleteStreamingProfileAsync(string name, CancellationToken? cancellationToken = null)
        {
            return AdminApi.DeleteStreamingProfileAsync(name, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteStreamingProfile method instead.")]
        public StreamingProfileResult DeleteStreamingProfile(string name)
        {
            return AdminApi.DeleteStreamingProfile(name);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetStreamingProfileAsync method instead.")]
        public Task<StreamingProfileResult> GetStreamingProfileAsync(string name, CancellationToken? cancellationToken = null)
        {
            return AdminApi.GetStreamingProfileAsync(name, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetStreamingProfile method instead.")]
        public StreamingProfileResult GetStreamingProfile(string name)
        {
            return AdminApi.GetStreamingProfile(name);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListStreamingProfilesAsync method instead.")]
        public Task<StreamingProfileListResult> ListStreamingProfilesAsync(CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListStreamingProfilesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListStreamingProfiles method instead.")]
        public StreamingProfileListResult ListStreamingProfiles()
        {
            return AdminApi.ListStreamingProfiles();
        }
    }
}
