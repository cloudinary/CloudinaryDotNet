namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of video file uploading.
    /// </summary>
    public class AutoUploadParams : RawUploadParams
    {
        /// <summary>
        /// Gets get the type of auto asset for upload.
        /// </summary>
        public override ResourceType ResourceType => ResourceType.Auto;
    }
}
