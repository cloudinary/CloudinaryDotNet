namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters of restore a deleted resources request.
    /// </summary>
    public class RestoreParams : BaseParams
    {
        private List<string> m_publicIds = new List<string>();
        private ResourceType m_resourceType = ResourceType.Image;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreParams"/> class.
        /// </summary>
        public RestoreParams()
        {
        }

        /// <summary>
        /// Gets or sets the public IDs of (deleted or existing) backed up resources to restore. Reverts to the latest backed up
        /// version of the resource.
        /// </summary>
        public List<string> PublicIds
        {
            get { return m_publicIds; }
            set { m_publicIds = value; }
        }

        /// <summary>
        /// Gets or sets restore resources with the given resource type. Default: image.
        /// </summary>
        public ResourceType ResourceType
        {
            get { return m_resourceType; }
            set { m_resourceType = value; }
        }

        private bool PublicIdsExist
        {
            get { return PublicIds != null && PublicIds.Count > 0; }
        }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (!PublicIdsExist)
            {
                throw new ArgumentException("At least one PublicId must be specified!");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (PublicIdsExist)
            {
                dict.Add("public_ids", PublicIds);
            }

            return dict;
        }
    }
}
