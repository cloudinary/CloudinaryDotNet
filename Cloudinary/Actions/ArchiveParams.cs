using System;
using System.Collections.Generic;
using System.ComponentModel;

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

        private string resourceType = null;
        private string type = null;
        private List<Transformation> transformations = null;
        private ArchiveCallMode mode = ArchiveCallMode.Create;
        private ArchiveFormat targetFormat = ArchiveFormat.Zip;
        private bool flattenFolders = false;
        private bool flattenTransformations = false;
        private int expiresAt = 0;
        private bool useOriginalFilename = false;
        private string notificationUrl = null;
        private bool keepDerived = false;

        private string targetPublicId = null;
        private bool async = false;
        private List<string> targetTags = null;


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
            return mode;
        }

        /// <summary>
        /// Determines whether to return the generated archive file (download) 
        /// or to store it as a raw resource in your Cloudinary account and return a JSON with the URLs for accessing the archive file (create)
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public ArchiveParams Mode(ArchiveCallMode mode)
        {
            this.mode = mode;
            return this;
        }

        /// <summary>
        /// Get the resource type (image, video or raw) of files to include in the archive.
        /// </summary>
        public string ResourceType()
        {
            return resourceType;
        }

        /// <summary>
        /// Set the resource type (image, video or raw) of files to include in the archive: Default: image.
        /// </summary>
        public ArchiveParams ResourceType(string resourceType)
        {
            this.resourceType = resourceType;
            return this;
        }

        /// <summary>
        /// Get the specific file type of resources to include in the archive (upload/private/authenticated).
        /// </summary>
        public string Type()
        {
            return type;
        }

        /// <summary>
        /// Set the specific file type of resources to include in the archive (upload/private/authenticated). Default: upload.
        /// </summary>
        public ArchiveParams Type(string type)
        {
            this.type = type;
            return this;
        }

        /// <summary>
        /// Get a list of transformations applied to the images before they are included in the generated archive.
        /// </summary>
        /// <returns></returns>
        public List<Transformation> Transformations()
        {
            return transformations;
        }

        /// <summary>
        /// Set a list of transformations to apply to the images before they are included in the generated archive.
        /// </summary>
        public ArchiveParams Transformations(List<Transformation> transformations)
        {
            this.transformations = transformations;
            return this;
        }

        /// <summary>
        /// Get the format for the generated archive.
        /// </summary>
        public ArchiveFormat TargetFormat()
        {
            return targetFormat;
        }

        /// <summary>
        /// Set the format for the generated archive. Currently only 'zip' is supported.
        /// </summary>
        public ArchiveParams TargetFormat(ArchiveFormat targetFormat)
        {
            this.targetFormat = targetFormat;
            return this;
        }

        /// <summary>
        /// Get the Public ID to assign to the generated archive.
        /// </summary>
        public string TargetPublicId()
        {
            return targetPublicId;
        }

        /// <summary>
        /// Set the Public ID to assign to the generated archive.
        /// relevant only for create call.
        /// </summary>
        public ArchiveParams TargetPublicId(string targetPublicId)
        {
            this.targetPublicId = targetPublicId;
            return this;
        }

        /// <summary>
        /// Get whether to flatten all files to be in the root of the archive file (no sub-folders).
        /// </summary>
        public bool IsFlattenFolders()
        {
            return flattenFolders;
        }

        /// <summary>
        /// Determines whether to flatten all files to be in the root of the archive file (no sub-folders).
        /// </summary>
        public ArchiveParams FlattenFolders(bool flattenFolders)
        {
            this.flattenFolders = flattenFolders;
            return this;
        }

        /// <summary>
        /// Get whether to flatten the folder structure of the derived images and store the transformation details on the file name instead.
        /// </summary>
        /// <returns></returns>
        public bool IsFlattenTransformations()
        {
            return flattenTransformations;
        }

        /// <summary>
        /// Determines whether to flatten the folder structure of the derived images and store the transformation details on the file name instead.
        /// </summary>
        public ArchiveParams FlattenTransformations(bool flattenTransformations)
        {
            this.flattenTransformations = flattenTransformations;
            return this;
        }

        /// <summary>
        /// Get the Unix time in seconds when the generated download URL expires.
        /// </summary>
        public int ExpiresAt()
        {
            return expiresAt;
        }

        /// <summary>
        /// Set the Unix time in seconds when the generated download URL expires (e.g., 1415060076). 
        /// If this parameter is omitted then the generated download URL expires after 1 hour.
        /// Note: Relevant only for download call.
        /// </summary>
        public ArchiveParams ExpiresAt(int expiresAt)
        {
            this.expiresAt = expiresAt;
            return this;
        }

        /// <summary>
        /// Get whether to use the original file name of the included images (if available) instead of the Public ID.
        /// </summary>
        public bool IsUseOriginalFilename()
        {
            return useOriginalFilename;
        }

        /// <summary>
        /// Set whether to use the original file name of the included images (if available) instead of the Public ID.
        /// </summary>
        /// <param name="useOriginalFilename"></param>
        /// <returns></returns>
        public ArchiveParams UseOriginalFilename(bool useOriginalFilename)
        {
            this.useOriginalFilename = useOriginalFilename;
            return this;
        }

        /// <summary>
        /// Get whether to perform the archive generation in the background (asynchronously).
        /// </summary>
        public bool IsAsync()
        {
            return async;
        }

        /// <summary>
        /// Set whether to perform the archive generation in the background (asynchronously). Default: false.
        /// Relevant only for create call.
        /// </summary>
        public ArchiveParams Async(bool async)
        {
            this.async = async;
            return this;
        }

        /// <summary>
        /// Get an HTTP or HTTPS URL to notify your application (a webhook) when the process has completed.
        /// </summary>
        public string NotificationUrl()
        {
            return notificationUrl;
        }

        /// <summary>
        /// Set an HTTP or HTTPS URL to notify your application (a webhook) when the process has completed.
        /// </summary>
        public ArchiveParams NotificationUrl(string notificationUrl)
        {
            this.notificationUrl = notificationUrl;
            return this;
        }

        /// <summary>
        /// Get a list of tag names to assign to the generated archive.
        /// </summary>
        public List<string> TargetTags()
        {
            return targetTags;
        }

        /// <summary>
        /// Set a list of tag names to assign to the generated archive.
        /// Relevant only for create call.
        /// </summary>
        public ArchiveParams TargetTags(List<string> targetTags)
        {
            this.targetTags = targetTags;
            return this;
        }

        /// <summary>
        /// Get whether to keep the derived images used for generating the archive or not.
        /// </summary>
        public bool IsKeepDerived()
        {
            return keepDerived;
        }

        /// <summary>
        /// Set whether to keep the derived images used for generating the archive or not.
        /// </summary>
        public ArchiveParams KeepDerived(bool keepDerived)
        {
            this.keepDerived = keepDerived;
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
                AddParam(dict, "tags", string.Join(",", m_tags));

            if (m_publicIds != null && m_publicIds.Count > 0)
                AddParam(dict, "public_ids", string.Join(",", m_publicIds));

            if (m_prefixes != null && m_prefixes.Count > 0)
                AddParam(dict, "prefixes", string.Join(",", m_prefixes));

            if (!string.IsNullOrEmpty(resourceType))
                AddParam(dict, "resource_type", resourceType);

            if (!string.IsNullOrEmpty(type))
                AddParam(dict, "type", type);

            if (transformations != null && transformations.Count > 0)
                AddParam(dict, "transformations", string.Join("/", transformations));

            if (targetFormat != ArchiveFormat.Zip)
                AddParam(dict, "target_format", Api.GetCloudinaryParam(targetFormat));

            if (flattenFolders)
                AddParam(dict, "flatten_folders", flattenFolders);

            if (flattenTransformations)
                AddParam(dict, "flatten_transformations", flattenTransformations);

            if (useOriginalFilename)
                AddParam(dict, "use_original_filename", useOriginalFilename);

            if (!string.IsNullOrEmpty(notificationUrl))
                AddParam(dict, "notification_url", notificationUrl);

            if (keepDerived)
                AddParam(dict, "keep_derived", keepDerived);

            if (mode == ArchiveCallMode.Create)
            {
                if (async)
                    AddParam(dict, "async", async);

                if (!string.IsNullOrEmpty(targetPublicId))
                    AddParam(dict, "target_public_id", targetPublicId);

                if (targetTags != null && targetTags.Count > 0)
                    AddParam(dict, "target_tags", string.Join(",", targetTags));
            }

            if (expiresAt > 0 && mode == ArchiveCallMode.Download)
                AddParam(dict, "expires_at", expiresAt);

            return dict;
        }
    }

    /// <summary>
    /// The format for the generated archive
    /// </summary>
    public enum ArchiveFormat
    {
        /// <summary>
        /// Specifies ZIP format for an archive
        /// </summary>
        [Description("zip")]
        Zip
    }

    public enum ArchiveCallMode
    {
        /// <summary>
        ///  Indicates to return the generated archive file
        /// </summary>
        [Description("download")]
        Download,
        /// <summary>
        /// Indicates to store the generated archive file as a raw resource in your Cloudinary account and return a JSON with the URLs for accessing it
        /// </summary>
        [Description("create")]
        Create
    }
}
