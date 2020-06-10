namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of video file uploading.
    /// </summary>
    public class VideoUploadParams : ImageUploadParams
    {
        /// <summary>
        /// Gets get the type of video asset for upload.
        /// </summary>
        public override ResourceType ResourceType
        {
            get { return Actions.ResourceType.Video; }
        }
    }
}
