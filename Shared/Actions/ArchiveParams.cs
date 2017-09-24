using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters for working with archives, common for both of the methods (create and download)
    /// </summary>
    public class ArchiveParams : BaseParams
    {
        private List<string> m_publicIds;
        private List<string> m_tags;
        private List<string> m_prefixes;

        private string m_resourceType;
        private string m_type;
        private List<Transformation> m_transformations;
        private ArchiveCallMode m_mode = ArchiveCallMode.Create;
        private ArchiveFormat m_targetFormat = ArchiveFormat.Zip;
        private bool m_flattenFolders;
        private bool m_flattenTransformations;
        private int m_expiresAt;
        private bool m_useOriginalFilename;
        private string m_notificationUrl;
        private bool m_keepDerived;
        private bool m_skipTransformationName;

        private string m_targetPublicId;
        private bool m_async;
        private List<string> m_targetTags;


        /// <summary>
        /// Get a list of Public IDs for the specific images to be included in the archive
        /// </summary>
        public List<string> PublicIds()
        {
            return m_publicIds;
        }

        /// <summary>
        /// Set a list of Public IDs for the specific images to be included in the archive
        /// </summary>
        public ArchiveParams PublicIds(List<string> publicIds)
        {
            m_publicIds = publicIds;
            return this;
        }

        /// <summary>
        /// Get a list of tag names. All images with this tag(s) will be included in the archive
        /// </summary>
        public List<string> Tags()
        {
            return m_tags;
        }

        /// <summary>
        /// Set a list of tag names. All images with this tag(s) will be included in the archive
        /// </summary>
        public ArchiveParams Tags(List<string> tags)
        {
            m_tags = tags;
            return this;
        }

        /// <summary>
        /// Get a list of prefixes of Public IDs (e.g., folders).
        /// </summary>
        public List<string> Prefixes()
        {
            return m_prefixes;
        }

        /// <summary>
        /// Set a list of prefixes of Public IDs (e.g., folders). All images with this tag(s) will be included in the archive.
        /// Setting this parameter to a slash (/) is a shortcut for including all images in the account for the given resource type and type (up to the max files limit).
        /// </summary>
        public ArchiveParams Prefixes(List<string> prefixes)
        {
            m_prefixes = prefixes;
            return this;
        }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            if ((m_publicIds == null || m_publicIds.Count == 0) &&
                (m_prefixes == null || m_prefixes.Count == 0) &&
                (m_tags == null || m_tags.Count == 0))
                throw new ArgumentException("At least one of the following \"filtering\" parameters needs to be specified: PublicIds, Tags or Prefixes.");
        }

        /// <summary>
        /// Get Mode whether to return the generated archive file (download) 
        /// or to store it as a raw resource (create)
        /// </summary>
        public virtual ArchiveCallMode Mode()
        {
            return m_mode;
        }

        /// <summary>
        /// Determines whether to return the generated archive file (download) 
        /// or to store it as a raw resource in your Cloudinary account and return a JSON with the URLs for accessing the archive file (create)
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public ArchiveParams Mode(ArchiveCallMode mode)
        {
            this.m_mode = mode;
            return this;
        }

        /// <summary>
        /// Get the resource type (image, video or raw) of files to include in the archive.
        /// </summary>
        public string ResourceType()
        {
            return m_resourceType;
        }

        /// <summary>
        /// Set the resource type (image, video or raw) of files to include in the archive: Default: image.
        /// </summary>
        public ArchiveParams ResourceType(string resourceType)
        {
            this.m_resourceType = resourceType;
            return this;
        }

        /// <summary>
        /// Get the specific file type of resources to include in the archive (upload/private/authenticated).
        /// </summary>
        public string Type()
        {
            return m_type;
        }

        /// <summary>
        /// Set the specific file type of resources to include in the archive (upload/private/authenticated). Default: upload.
        /// </summary>
        public ArchiveParams Type(string type)
        {
            this.m_type = type;
            return this;
        }

        /// <summary>
        /// Get a list of transformations applied to the images before they are included in the generated archive.
        /// </summary>
        /// <returns></returns>
        public List<Transformation> Transformations()
        {
            return m_transformations;
        }

        /// <summary>
        /// Set a list of transformations to apply to the images before they are included in the generated archive.
        /// </summary>
        public ArchiveParams Transformations(List<Transformation> transformations)
        {
            this.m_transformations = transformations;
            return this;
        }

        /// <summary>
        /// Get the format for the generated archive.
        /// </summary>
        public ArchiveFormat TargetFormat()
        {
            return m_targetFormat;
        }

        /// <summary>
        /// Set the format for the generated archive. Currently only 'zip' is supported.
        /// </summary>
        public ArchiveParams TargetFormat(ArchiveFormat targetFormat)
        {
            this.m_targetFormat = targetFormat;
            return this;
        }

        /// <summary>
        /// Get the Public ID to assign to the generated archive.
        /// </summary>
        public string TargetPublicId()
        {
            return m_targetPublicId;
        }

        /// <summary>
        /// Set the Public ID to assign to the generated archive.
        /// relevant only for create call.
        /// </summary>
        public ArchiveParams TargetPublicId(string targetPublicId)
        {
            this.m_targetPublicId = targetPublicId;
            return this;
        }

        /// <summary>
        /// Get whether to flatten all files to be in the root of the archive file (no sub-folders).
        /// </summary>
        public bool IsFlattenFolders()
        {
            return m_flattenFolders;
        }

        /// <summary>
        /// Determines whether to flatten all files to be in the root of the archive file (no sub-folders).
        /// </summary>
        public ArchiveParams FlattenFolders(bool flattenFolders)
        {
            this.m_flattenFolders = flattenFolders;
            return this;
        }

        /// <summary>
        /// Get whether to flatten the folder structure of the derived images and store the transformation details on the file name instead.
        /// </summary>
        /// <returns></returns>
        public bool IsFlattenTransformations()
        {
            return m_flattenTransformations;
        }

        /// <summary>
        /// Determines whether to flatten the folder structure of the derived images and store the transformation details on the file name instead.
        /// </summary>
        public ArchiveParams FlattenTransformations(bool flattenTransformations)
        {
            this.m_flattenTransformations = flattenTransformations;
            return this;
        }

        /// <summary>
        /// Get the Unix time in seconds when the generated download URL expires.
        /// </summary>
        public int ExpiresAt()
        {
            return m_expiresAt;
        }

        /// <summary>
        /// Set the Unix time in seconds when the generated download URL expires (e.g., 1415060076). 
        /// If this parameter is omitted then the generated download URL expires after 1 hour.
        /// Note: Relevant only for download call.
        /// </summary>
        public ArchiveParams ExpiresAt(int expiresAt)
        {
            this.m_expiresAt = expiresAt;
            return this;
        }

        /// <summary>
        /// Get whether to use the original file name of the included images (if available) instead of the Public ID.
        /// </summary>
        public bool IsUseOriginalFilename()
        {
            return m_useOriginalFilename;
        }

        /// <summary>
        /// Set whether to use the original file name of the included images (if available) instead of the Public ID.
        /// </summary>
        /// <param name="useOriginalFilename"></param>
        /// <returns></returns>
        public ArchiveParams UseOriginalFilename(bool useOriginalFilename)
        {
            this.m_useOriginalFilename = useOriginalFilename;
            return this;
        }

        /// <summary>
        /// Get whether to perform the archive generation in the background (asynchronously).
        /// </summary>
        public bool IsAsync()
        {
            return m_async;
        }

        /// <summary>
        /// Set whether to perform the archive generation in the background (asynchronously). Default: false.
        /// Relevant only for create call.
        /// </summary>
        public ArchiveParams Async(bool async)
        {
            this.m_async = async;
            return this;
        }

        /// <summary>
        /// Get an HTTP or HTTPS URL to notify your application (a webhook) when the process has completed.
        /// </summary>
        public string NotificationUrl()
        {
            return m_notificationUrl;
        }

        /// <summary>
        /// Set an HTTP or HTTPS URL to notify your application (a webhook) when the process has completed.
        /// </summary>
        public ArchiveParams NotificationUrl(string notificationUrl)
        {
            this.m_notificationUrl = notificationUrl;
            return this;
        }

        /// <summary>
        /// Get a list of tag names to assign to the generated archive.
        /// </summary>
        public List<string> TargetTags()
        {
            return m_targetTags;
        }

        /// <summary>
        /// Set a list of tag names to assign to the generated archive.
        /// Relevant only for create call.
        /// </summary>
        public ArchiveParams TargetTags(List<string> targetTags)
        {
            this.m_targetTags = targetTags;
            return this;
        }

        /// <summary>
        /// Get whether to keep the derived images used for generating the archive or not.
        /// </summary>
        public bool IsKeepDerived()
        {
            return m_keepDerived;
        }

        /// <summary>
        /// Set whether to keep the derived images used for generating the archive or not.
        /// </summary>
        public ArchiveParams KeepDerived(bool keepDerived)
        {
            this.m_keepDerived = keepDerived;
            return this;
        }

        /// <summary>
        /// Get whether to strip all transformation details from file names and add a numeric counter to a file name in the case of a name conflict. Default: false.
        /// </summary>
        public bool IsSkipTransformationName()
        {
            return m_skipTransformationName;
        }

        /// <summary>
        /// Set whether to strip all transformation details from file names and add a numeric counter to a file name in the case of a name conflict. Default: false.
        /// </summary>
        /// <param name="useOriginalFilename"></param>
        /// <returns></returns>
        public ArchiveParams SkipTransformationName(bool skipTransformationName)
        {
            this.m_skipTransformationName = skipTransformationName;
            return this;
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            Check();

            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "mode", Api.GetCloudinaryParam(Mode()));

            if (m_tags != null && m_tags.Count > 0)
                AddParam(dict, "tags", m_tags);

            if (m_publicIds != null && m_publicIds.Count > 0)
                AddParam(dict, "public_ids", m_publicIds);

            if (m_prefixes != null && m_prefixes.Count > 0)
                AddParam(dict, "prefixes", m_prefixes);

            if (!string.IsNullOrEmpty(m_type))
                AddParam(dict, "type", m_type);

            if (m_transformations != null && m_transformations.Count > 0)
                AddParam(dict, "transformations", string.Join("/", m_transformations));

            if (m_targetFormat != ArchiveFormat.Zip)
                AddParam(dict, "target_format", Api.GetCloudinaryParam(m_targetFormat));

            if (m_flattenFolders)
                AddParam(dict, "flatten_folders", m_flattenFolders);

            if (m_flattenTransformations)
                AddParam(dict, "flatten_transformations", m_flattenTransformations);

            if (m_useOriginalFilename)
                AddParam(dict, "use_original_filename", m_useOriginalFilename);

            if (!string.IsNullOrEmpty(m_notificationUrl))
                AddParam(dict, "notification_url", m_notificationUrl);

            if (m_keepDerived)
                AddParam(dict, "keep_derived", m_keepDerived);

            if (m_skipTransformationName)
                AddParam(dict, "skip_transformation_name", m_skipTransformationName);

            if (m_mode == ArchiveCallMode.Create)
            {
                if (m_async)
                    AddParam(dict, "async", m_async);

                if (!string.IsNullOrEmpty(m_targetPublicId))
                    AddParam(dict, "target_public_id", m_targetPublicId);

                if (m_targetTags != null && m_targetTags.Count > 0)
                    AddParam(dict, "target_tags", m_targetTags);
            }

            if (m_expiresAt > 0 && m_mode == ArchiveCallMode.Download)
                AddParam(dict, "expires_at", m_expiresAt);

            return dict;
        }
    }
}
