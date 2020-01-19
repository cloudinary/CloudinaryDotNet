namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Search response with information about the assets matching the search criteria.
    /// </summary>
    [DataContract]
    public class SearchResult : BaseResult
    {
        /// <summary>
        /// The total count of assets matching the search criteria.
        /// </summary>
        [DataMember(Name = "total_count")]
        public int TotalCount { get; protected set; }

        /// <summary>
        /// The time taken to process the request.
        /// </summary>
        [DataMember(Name = "time")]
        public long Time { get; protected set; }

        /// <summary>
        /// The details of each of the assets (resources) found.
        /// </summary>
        [DataMember(Name = "resources")]
        public List<SearchResource> Resources { get; protected set; }

        /// <summary>
        /// When a search request has more results to return than max_results, the next_cursor value is returned as
        /// part of the response.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; protected set; }

        /// <summary>
        /// Counts of assets, grouped by specified parameters.
        /// </summary>
        [DataMember(Name = "aggregations")]
        public Dictionary<string, Dictionary<string, int>> Aggregations { get; protected set; }
    }

    /// <summary>
    /// The details of the asset (resource) found.
    /// </summary>
    [DataContract]
    public class SearchResource
    {
        /// <summary>
        /// The type of file. Possible values: image, raw, video.
        /// </summary>
        [DataMember(Name = "resource_type")]
        protected string m_resourceType;

        /// <summary>
        /// The public id of the asset.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        /// <summary>
        /// Folder name where the resource is stored.
        /// </summary>
        [DataMember(Name = "folder")]
        public string Folder { get; protected set; }

        /// <summary>
        /// The name of the resource file.
        /// </summary>
        [DataMember(Name = "filename")]
        public string FileName { get; protected set; }

        /// <summary>
        /// The format of the asset (png, mp4, etc...).
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        /// <summary>
        /// Current version of the resource.
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; protected set; }

        /// <summary>
        /// The type of resource. Possible values: image, raw, video.
        /// </summary>
        public ResourceType ResourceType => Api.ParseCloudinaryParam<ResourceType>(m_resourceType);

        /// <summary>
        /// The storage type.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; protected set; }

        /// <summary>
        /// Date when asset was created.
        /// </summary>
        [DataMember(Name = "created_at")]
        public string Created { get; protected set; }

        /// <summary>
        /// Date when asset was uploaded (overwritten).
        /// </summary>
        [DataMember(Name = "uploaded_at")]
        public string Uploaded { get; protected set; }

        /// <summary>
        /// The size of the asset.
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Length { get; protected set; }

        /// <summary>
        /// If the resource is backed up, indicates the space taken in the backup system by all backed up versions.
        /// </summary>
        [DataMember(Name = "backup_bytes")]
        public long BackupBytes { get; protected set; }

        /// <summary>
        /// Width of the media asset.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Width of the media asset.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// Aspect ratio of the media asset.
        /// </summary>
        [DataMember(Name = "aspect_ratio")]
        public double AspectRatio { get; protected set; }

        /// <summary>
        /// Number of total pixels of the media asset.
        /// </summary>
        [DataMember(Name = "pixels")]
        public long Pixels { get; protected set; }

        /// <summary>
        /// The number of pages in the image, if the image has multiple pages.
        /// </summary>
        [DataMember(Name = "pages")]
        public int Pages { get; protected set; }

        /// <summary>
        /// The HTTP URL for accessing the media asset.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; protected set; }

        /// <summary>
        /// The HTTPS URL for securely accessing the media asset.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; protected set; }

        /// <summary>
        /// The status of the resource. Possible values: active, deleted.
        /// By default, a search response includes only resources with active status.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; protected set; }

        /// <summary>
        /// The authentication level currently set for the resource.
        /// Possible values: public, authenticated.
        /// </summary>
        [DataMember(Name = "access_mode")]
        public string AccessMode { get; protected set; }

        /// <summary>
        /// Parameters of the asset access management.
        /// </summary>
        [DataMember(Name = "access_control")]
        public List<AccessControlRule> AccessControl { get; protected set; }

        /// <summary>
        /// Used to determine whether two versions of an asset are identical.
        /// </summary>
        [DataMember(Name = "etag")]
        public string Etag { get; protected set; }

        /// <summary>
        /// A list of tag names assigned to resource.
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; protected set; }

        /// <summary>
        /// IPTC, XMP, and detailed Exif metadata. Supported for images, video, and audio.
        /// </summary>
        [DataMember(Name = "image_metadata")]
        public Dictionary<string, string> ImageMetadata { get; protected set; }

        /// <summary>
        /// The key-value pairs of general textual context metadata attached to the media asset.
        /// </summary>
        [DataMember(Name = "context")]
        public Dictionary<string, string> Context { get; protected set; }

        /// <summary>
        /// Image analysis results.
        /// </summary>
        [DataMember(Name = "image_analysis")]
        public ImageAnalysis ImageAnalysis { get; protected set; }
    }

    /// <summary>
    /// The results of the advanced image analysis.
    /// </summary>
    [DataContract]
    public class ImageAnalysis
    {
        /// <summary>
        /// Represents amount of faces the image contains.
        /// </summary>
        [DataMember(Name = "face_count")]
        public int FaceCount { get; protected set; }

        /// <summary>
        /// A list of coordinates of detected faces.
        /// </summary>
        [DataMember(Name = "faces")]
        public int[][] Faces { get; protected set; }

        /// <summary>
        /// If the image only contains a single grayscale channel.
        /// </summary>
        [DataMember(Name = "grayscale")]
        public bool GrayScale { get; protected set; }

        /// <summary>
        /// The likelihood that the image is an illustration as opposed to a photograph.
        /// A value between 0 (photo) and 1.0 (illustration).
        /// </summary>
        [DataMember(Name = "illustration_score")]
        public double IllustrationScore { get; protected set; }

        /// <summary>
        /// If the image contains one or more colors with an alpha channel.
        /// </summary>
        [DataMember(Name = "transparent")]
        public bool Transparent { get; protected set; }

        /// <summary>
        /// Used to determine whether two versions of an analysis are identical.
        /// </summary>
        [DataMember(Name = "etag")]
        public string Etag { get; protected set; }

        /// <summary>
        /// The predominant colors uploaded image.
        /// </summary>
        [DataMember(Name = "colors")]
        public Dictionary<string, double> Colors { get; protected set; }
    }
}
