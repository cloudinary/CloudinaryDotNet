namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of auto file uploading.
    /// </summary>
    public class AutoUploadParams : ImageUploadParams
    {
        /// <summary>
        /// Gets get the type of auto asset for upload.
        /// </summary>
        public override ResourceType ResourceType => ResourceType.Auto;
    }
}
