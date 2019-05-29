
namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of video file uploading.
    /// </summary>
    public class VideoUploadParams : ImageUploadParams
    {
        /// <summary>
        /// Get the type of video asset for upload.
        /// </summary>
        public override ResourceType ResourceType
        {
            get { return Actions.ResourceType.Video; }
        }
    }
}
