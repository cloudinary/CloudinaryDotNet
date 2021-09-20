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
        /// <inheritdoc />
        public Task<StreamingProfileResult> CreateStreamingProfileAsync(StreamingProfileCreateParams parameters, CancellationToken? cancellationToken = null) =>
            cloudinaryAdmin.CreateStreamingProfileAsync(parameters, cancellationToken);

        /// <inheritdoc />
        public StreamingProfileResult CreateStreamingProfile(StreamingProfileCreateParams parameters) =>
            cloudinaryAdmin.CreateStreamingProfile(parameters);

        /// <inheritdoc />
        public Task<StreamingProfileResult> UpdateStreamingProfileAsync(
            string name,
            StreamingProfileUpdateParams parameters,
            CancellationToken? cancellationToken = null) =>
                cloudinaryAdmin.UpdateStreamingProfileAsync(name, parameters, cancellationToken);

        /// <inheritdoc />
        public StreamingProfileResult UpdateStreamingProfile(string name, StreamingProfileUpdateParams parameters) =>
            cloudinaryAdmin.UpdateStreamingProfile(name, parameters);

        /// <inheritdoc />
        public Task<StreamingProfileResult> DeleteStreamingProfileAsync(string name, CancellationToken? cancellationToken = null) =>
            cloudinaryAdmin.DeleteStreamingProfileAsync(name, cancellationToken);

        /// <inheritdoc />
        public StreamingProfileResult DeleteStreamingProfile(string name) =>
            cloudinaryAdmin.DeleteStreamingProfile(name);

        /// <inheritdoc />
        public Task<StreamingProfileResult> GetStreamingProfileAsync(string name, CancellationToken? cancellationToken = null) =>
            cloudinaryAdmin.GetStreamingProfileAsync(name, cancellationToken);

        /// <inheritdoc />
        public StreamingProfileResult GetStreamingProfile(string name) =>
            cloudinaryAdmin.GetStreamingProfile(name);

        /// <inheritdoc />
        public Task<StreamingProfileListResult> ListStreamingProfilesAsync(CancellationToken? cancellationToken = null) =>
            cloudinaryAdmin.ListStreamingProfilesAsync(cancellationToken);

        /// <inheritdoc />
        public StreamingProfileListResult ListStreamingProfiles() =>
            cloudinaryAdmin.ListStreamingProfiles();
    }
}
