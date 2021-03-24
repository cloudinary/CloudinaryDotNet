namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Parameters for working with archives, common for both of the methods (create and download).
    /// </summary>
    public class ArchiveParams : BaseParams
    {
        private List<string> m_publicIds;
        private List<string> m_fullyQualifiedPublicIds;
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
        private bool m_allow_missing;

        private string m_targetPublicId;
        private bool m_async;
        private List<string> m_targetTags;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveParams"/> class.
        /// </summary>
        public ArchiveParams()
        {
            m_resourceType = Constants.RESOURCE_TYPE_IMAGE;
        }

        /// <summary>
        /// Get a list of Public IDs for the specific assets to be included in the archive.
        /// </summary>
        /// <returns>A list of strings where each element represents Public ID.</returns>
        public List<string> PublicIds()
        {
            return m_publicIds;
        }

        /// <summary>
        /// Set a list of Public IDs for the specific assets to be included in the archive.
        /// Up to 1000 public IDs are supported.
        /// </summary>
        /// <param name="publicIds">List of Public IDs of the assets.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams PublicIds(List<string> publicIds)
        {
            m_publicIds = publicIds;
            return this;
        }

        /// <summary>
        /// Get a list of Fully Qualified Public IDs for the specific assets to be included in the archive.
        /// </summary>
        /// <returns>A list of strings where each element represents Fully Qualified Public ID.</returns>
        public List<string> FullyQualifiedPublicIds()
        {
            return m_fullyQualifiedPublicIds;
        }

        /// <summary>
        /// Set a list of Fully Qualified Public IDs for the specific assets to be included in the archive.
        /// </summary>
        /// <param name="fullyQualifiedPublicIds">List of fully qualified Public IDs.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams FullyQualifiedPublicIds(List<string> fullyQualifiedPublicIds)
        {
            m_fullyQualifiedPublicIds = fullyQualifiedPublicIds;
            return this;
        }

        /// <summary>
        /// Get a list of tag names. All assets with the specified tags are included in the archive.
        /// </summary>
        /// <returns>A list of strings where each element represents a tag name.</returns>
        public List<string> Tags()
        {
            return m_tags;
        }

        /// <summary>
        /// Set a list of tag names. All assets with the specified tags are included in the archive.
        /// Up to 20 tags are supported.
        /// </summary>
        /// <param name="tags">List of tags to be included.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams Tags(List<string> tags)
        {
            m_tags = tags;
            return this;
        }

        /// <summary>
        /// Get a list of prefixes of Public IDs (e.g., folders).
        /// </summary>
        /// <returns>A list of strings where each element represents a Public ID prefix.</returns>
        public List<string> Prefixes()
        {
            return m_prefixes;
        }

        /// <summary>
        /// Set a list of prefixes of Public IDs (e.g., folders). Setting this parameter to a slash (/) is a shortcut
        /// for including all assets in the account for the given ResourceType and Type (up to the max files limit).
        /// Up to 20 prefixes are supported.
        /// </summary>
        /// <param name="prefixes">List of prefixes to be included.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams Prefixes(List<string> prefixes)
        {
            m_prefixes = prefixes;
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (m_publicIds?.Any() != true &&
                m_fullyQualifiedPublicIds?.Any() != true &&
                m_prefixes?.Any() != true &&
                m_tags?.Any() != true)
            {
                throw new ArgumentException("At least one of the following \"filtering\" parameters needs " +
                                            "to be specified: PublicIds, FullyQualifiedPublicIds, Tags or Prefixes.");
            }

            if (m_resourceType == "auto" ^ (m_fullyQualifiedPublicIds?.Any() ?? false))
            {
                throw new ArgumentException(
                    "To create an archive with multiple types of assets, you must set ResourceType to \"auto\" " +
                    "and provide FullyQualifiedPublicIds (For example, 'video/upload/my_video.mp4')");
            }
        }

        /// <summary>
        /// Get Mode whether to return the generated archive file ('download') or to store it as a raw resource
        /// ('create').
        /// </summary>
        /// <returns>A member of <see cref="ArchiveCallMode"/> enum.</returns>
        public virtual ArchiveCallMode Mode()
        {
            return m_mode;
        }

        /// <summary>
        /// Determines whether to return a URL to dynamically generate and download the archive file ('download'), or to
        /// create and store it as a raw asset in your Cloudinary account and return JSON with the URLs to access
        /// the archive file ('create').
        /// </summary>
        /// <param name="mode">One of the values of <see cref="ArchiveCallMode"/> enum.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams Mode(ArchiveCallMode mode)
        {
            this.m_mode = mode;
            return this;
        }

        /// <summary>
        /// Get the resource type (image, video or raw) of files to include in the archive.
        /// </summary>
        /// <returns>A string that represents type of the resource.</returns>
        public string ResourceType()
        {
            return m_resourceType;
        }

        /// <summary>
        /// Set the resource type (image, video or raw) of files to include in the archive: Default: image.
        /// </summary>
        /// <param name="resourceType">Resource type to be included.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams ResourceType(string resourceType)
        {
            this.m_resourceType = resourceType;
            return this;
        }

        /// <summary>
        /// Get the specific file type of assets to include in the archive (upload/private/authenticated).
        /// </summary>
        /// <returns>A string that represents file type of the asset.</returns>
        public string Type()
        {
            return m_type;
        }

        /// <summary>
        /// Set the specific file type of assets to include in the archive (upload/private/authenticated). If tags are
        /// specified as a filter then all types are included. Default: upload.
        /// </summary>
        /// <param name="type">File type of assets to be included.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams Type(string type)
        {
            this.m_type = type;
            return this;
        }

        /// <summary>
        /// Get a list of transformations to run on all the derived assets before storing them in your Cloudinary
        /// account.
        /// </summary>
        /// <returns>A list of transformations.</returns>
        public List<Transformation> Transformations()
        {
            return m_transformations;
        }

        /// <summary>
        /// Set a list of transformations to run on all the derived assets before storing them in your Cloudinary
        /// account.
        /// </summary>
        /// <param name="transformations">List of transformations to run on.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams Transformations(List<Transformation> transformations)
        {
            this.m_transformations = transformations;
            return this;
        }

        /// <summary>
        /// Get the format for the generated archive.
        /// </summary>
        /// <returns>A member of <see cref="ArchiveFormat"/> enum.</returns>
        public ArchiveFormat TargetFormat()
        {
            return m_targetFormat;
        }

        /// <summary>
        /// Set the format for the generated archive: zip or tgz.
        /// Currently only 'zip' is supported.
        /// Default: zip.
        /// </summary>
        /// <param name="targetFormat">Generated archive format.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams TargetFormat(ArchiveFormat targetFormat)
        {
            this.m_targetFormat = targetFormat;
            return this;
        }

        /// <summary>
        /// Get the Public ID to assign to the generated archive.
        /// </summary>
        /// <returns>A string that represents archive Public ID.</returns>
        public string TargetPublicId()
        {
            return m_targetPublicId;
        }

        /// <summary>
        /// Set the Public ID to assign to the generated archive. If not specified, a random Public ID is generated.
        /// Only relevant when using the 'create' method.
        /// </summary>
        /// <param name="targetPublicId">Public ID of the generated archive.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams TargetPublicId(string targetPublicId)
        {
            this.m_targetPublicId = targetPublicId;
            return this;
        }

        /// <summary>
        /// Get whether to flatten all files to be in the root of the archive file (no sub-folders).
        /// </summary>
        /// <returns>True if files are to be flattened; otherwise, false.</returns>
        public bool IsFlattenFolders()
        {
            return m_flattenFolders;
        }

        /// <summary>
        /// Determines whether to flatten all files to be in the root of the archive file (no sub-folders). Any folder
        /// information included in the Public ID is stripped and a numeric counter is added to the file name in the
        /// case of a name conflict. Default: false.
        /// </summary>
        /// <param name="flattenFolders">Flag that determines files flattening.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams FlattenFolders(bool flattenFolders)
        {
            this.m_flattenFolders = flattenFolders;
            return this;
        }

        /// <summary>
        /// Get whether to flatten the folder structure of the derived assets and store the transformation details on
        /// the file name instead.
        /// </summary>
        /// <returns>True if the folder structure should be flattened; otherwise, false.</returns>
        public bool IsFlattenTransformations()
        {
            return m_flattenTransformations;
        }

        /// <summary>
        /// If multiple transformations are also applied, determines whether to flatten the folder structure of the
        /// derived assets and store the transformation details on the file name instead. Default: false.
        /// </summary>
        /// <param name="flattenTransformations">Flag that determines folder structure flattening.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams FlattenTransformations(bool flattenTransformations)
        {
            this.m_flattenTransformations = flattenTransformations;
            return this;
        }

        /// <summary>
        /// Get the date (UNIX time in seconds) of the URL expiration.
        /// </summary>
        /// <returns>UNIX time in seconds represented by an integer value.</returns>
        public int ExpiresAt()
        {
            return m_expiresAt;
        }

        /// <summary>
        /// Set the date (UNIX time in seconds) of the URL expiration (e.g., 1415060076). Only relevant when using the
        /// 'download' SDK methods. Default: 1 hour from the time that the URL is generated.
        /// </summary>
        /// <param name="expiresAt">UNIX time in seconds of the URL expiration.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams ExpiresAt(int expiresAt)
        {
            this.m_expiresAt = expiresAt;
            return this;
        }

        /// <summary>
        /// Get whether to use the original file name of the included assets (if available) instead of the public ID.
        /// </summary>
        /// <returns>True if original file name is to be used; otherwise, false.</returns>
        public bool IsUseOriginalFilename()
        {
            return m_useOriginalFilename;
        }

        /// <summary>
        /// Whether to use the original file name of the included assets (if available) instead of the public ID.
        /// Default: false.
        /// </summary>
        /// <param name="useOriginalFilename">Flag that determines original file usage.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams UseOriginalFilename(bool useOriginalFilename)
        {
            this.m_useOriginalFilename = useOriginalFilename;
            return this;
        }

        /// <summary>
        /// Get whether to perform the archive generation in the background (asynchronously).
        /// </summary>
        /// <returns>True for background archive generation; otherwise, false.</returns>
        public bool IsAsync()
        {
            return m_async;
        }

        /// <summary>
        /// Set whether to perform the archive generation in the background (asynchronously).
        /// Only relevant when using the 'create' SDK methods. Default: false.
        /// </summary>
        /// <param name="async">Flag that determines background archive generation.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams Async(bool async)
        {
            this.m_async = async;
            return this;
        }

        /// <summary>
        /// Get an HTTP or HTTPS URL to notify your application (a webhook) when the archive creation process has
        /// completed.
        /// </summary>
        /// <returns>A string that represents URL.</returns>
        public string NotificationUrl()
        {
            return m_notificationUrl;
        }

        /// <summary>
        /// Set an HTTP or HTTPS URL to notify your application (a webhook) when the archive creation process has
        /// completed. Only relevant when using the 'create' methods.
        /// </summary>
        /// <param name="notificationUrl">Notification URL.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams NotificationUrl(string notificationUrl)
        {
            this.m_notificationUrl = notificationUrl;
            return this;
        }

        /// <summary>
        /// Get a list of tag names to assign to the generated archive.
        /// </summary>
        /// <returns>A list of strings where each element represents a tag name.</returns>
        public List<string> TargetTags()
        {
            return m_targetTags;
        }

        /// <summary>
        /// Set a list of tag names to assign to the generated archive.
        /// Only relevant when using the 'create' SDK methods.
        /// </summary>
        /// <param name="targetTags">List of tag names to assign.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams TargetTags(List<string> targetTags)
        {
            this.m_targetTags = targetTags;
            return this;
        }

        /// <summary>
        /// Get whether to keep the derived assets used for generating the archive.
        /// </summary>
        /// <returns>True if derived assets are to be used; otherwise, false.</returns>
        public bool IsKeepDerived()
        {
            return m_keepDerived;
        }

        /// <summary>
        /// Set whether to keep the derived assets used for generating the archive.
        /// </summary>
        /// <param name="keepDerived">Flag that determines derived assets usage.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams KeepDerived(bool keepDerived)
        {
            this.m_keepDerived = keepDerived;
            return this;
        }

        /// <summary>
        /// Get whether to strip all transformation details from file names and add a numeric counter to a file name
        /// in the case of a name conflict.
        /// </summary>
        /// <returns>True if all transformation details are to be skipped; otherwise, false.</returns>
        public bool IsSkipTransformationName()
        {
            return m_skipTransformationName;
        }

        /// <summary>
        /// Determine whether to strip all transformation details from file names and add a numeric counter to a file
        /// name in the case of a name conflict. Default: false.
        /// </summary>
        /// <param name="skipTransformationName">Flag that determines whether to strip all transformation details.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams SkipTransformationName(bool skipTransformationName)
        {
            this.m_skipTransformationName = skipTransformationName;
            return this;
        }

        /// <summary>
        /// Allows generation of the archive if any of the supplied Public IDs are not found,
        /// instead of returning an error.
        /// </summary>
        /// <returns>True if any of the supplied Public IDs are not found; otherwise, false.</returns>
        public bool AllowMissing()
        {
            return m_allow_missing;
        }

        /// <summary>
        /// Optional. Allows generation of the archive if any of the supplied Public IDs are not found,
        /// instead of returning an error. Default: false.
        /// </summary>
        /// <param name="allowMissing">Flag that allows generation of the archive if any of the supplied Public IDs are not found,
        /// instead of returning an error. Default: false.</param>
        /// <returns>The instance of Archive parameters with set parameter.</returns>
        public ArchiveParams AllowMissing(bool allowMissing)
        {
            this.m_allow_missing = allowMissing;
            return this;
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            Check();

            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "mode", Api.GetCloudinaryParam(Mode()));

            if (m_tags != null && m_tags.Count > 0)
            {
                AddParam(dict, "tags", m_tags);
            }

            if (m_publicIds != null && m_publicIds.Count > 0)
            {
                AddParam(dict, "public_ids", m_publicIds);
            }

            if (m_fullyQualifiedPublicIds != null && m_fullyQualifiedPublicIds.Count > 0)
            {
                AddParam(dict, "fully_qualified_public_ids", m_fullyQualifiedPublicIds);
            }

            if (m_prefixes != null && m_prefixes.Count > 0)
            {
                AddParam(dict, "prefixes", m_prefixes);
            }

            if (!string.IsNullOrEmpty(m_type))
            {
                AddParam(dict, "type", m_type);
            }

            if (m_transformations != null && m_transformations.Count > 0)
            {
                AddParam(dict, "transformations", string.Join("/", m_transformations));
            }

            if (m_targetFormat != ArchiveFormat.Zip)
            {
                AddParam(dict, "target_format", Api.GetCloudinaryParam(m_targetFormat));
            }

            if (m_flattenFolders)
            {
                AddParam(dict, "flatten_folders", m_flattenFolders);
            }

            if (m_flattenTransformations)
            {
                AddParam(dict, "flatten_transformations", m_flattenTransformations);
            }

            if (m_useOriginalFilename)
            {
                AddParam(dict, "use_original_filename", m_useOriginalFilename);
            }

            if (!string.IsNullOrEmpty(m_notificationUrl))
            {
                AddParam(dict, "notification_url", m_notificationUrl);
            }

            if (m_keepDerived)
            {
                AddParam(dict, "keep_derived", m_keepDerived);
            }

            if (m_skipTransformationName)
            {
                AddParam(dict, "skip_transformation_name", m_skipTransformationName);
            }

            if (!string.IsNullOrEmpty(m_targetPublicId))
            {
                AddParam(dict, "target_public_id", m_targetPublicId);
            }

            if (m_mode == ArchiveCallMode.Create)
            {
                if (m_async)
                {
                    AddParam(dict, "async", m_async);
                }

                if (m_targetTags != null && m_targetTags.Count > 0)
                {
                    AddParam(dict, "target_tags", m_targetTags);
                }
            }

            if (m_expiresAt > 0 && m_mode == ArchiveCallMode.Download)
            {
                AddParam(dict, "expires_at", m_expiresAt);
            }

            if (m_allow_missing)
            {
                AddParam(dict, "allow_missing", m_allow_missing);
            }

            return dict;
        }
    }
}
