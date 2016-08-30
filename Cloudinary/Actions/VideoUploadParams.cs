
namespace CloudinaryDotNet.Actions
{
    public class VideoUploadParams : ImageUploadParams
    {
        public new ResourceType ResourceType
        {
            get { return Actions.ResourceType.Video; }
        }
    }
}
