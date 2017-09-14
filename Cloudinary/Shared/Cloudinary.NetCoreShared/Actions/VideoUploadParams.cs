
namespace CloudinaryDotNet.Actions
{
    public class VideoUploadParams : ImageUploadParams
    {
        public override ResourceType ResourceType
        {
            get { return Actions.ResourceType.Video; }
        }
    }
}
