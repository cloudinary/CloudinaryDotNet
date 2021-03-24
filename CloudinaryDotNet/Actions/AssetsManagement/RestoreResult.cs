namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed results of resources restore.
    /// </summary>
    [DataContract]
    public class RestoreResult : BaseResult
    {
        /// <summary>
        /// The type of file. Possible values: image, raw, video. Default: image.
        /// </summary>
        [DataMember(Name = "resource_type")]
        protected string m_resourceType;

        /// <summary>
        /// Gets the cloudinary resource type.
        /// </summary>
        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        /// <summary>
        /// Gets or sets collection of restored resources.
        /// </summary>
        public Dictionary<string, RestoredResource> RestoredResources { get; set; }

        /// <summary>
        /// Overrides corresponding method of <see cref="BaseResult"/> class.
        /// Populates additional token fields.
        /// </summary>
        /// <param name="source">JSON token received from the server.</param>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            if (RestoredResources == null)
            {
                RestoredResources = new Dictionary<string, RestoredResource>();
            }

            if (source != null)
            {
                // parsing message
                foreach (var resource in source.Children())
                {
                    var tagName = resource.ToObject<JProperty>().Name;
                    var restoredResourceAsObject = resource.ToObject<JProperty>().Value.ToObject<RestoredResource>();

                    RestoredResources.Add(tagName, restoredResourceAsObject);
                }
            }
        }
    }
}
