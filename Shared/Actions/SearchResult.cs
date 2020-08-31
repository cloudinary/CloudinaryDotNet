namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Search response with information about the assets matching the search criteria.
    /// </summary>
    [DataContract]
    public class SearchResult : BaseResult
    {
        /// <summary>
        /// Gets or sets the total count of assets matching the search criteria.
        /// </summary>
        [DataMember(Name = "total_count")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the time taken to process the request.
        /// </summary>
        [DataMember(Name = "time")]
        public long Time { get; set; }

        /// <summary>
        /// Gets or sets the details of each of the assets (resources) found.
        /// </summary>
        [DataMember(Name = "resources")]
        public List<SearchResource> Resources { get; set; }

        /// <summary>
        /// Gets or sets when a search request has more results to return than max_results, the next_cursor value is returned as
        /// part of the response.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; set; }

        /// <summary>
        /// Gets or sets counts of assets, grouped by specified parameters.
        /// </summary>
        [DataMember(Name = "aggregations")]
        public Dictionary<string, Dictionary<string, int>> Aggregations { get; set; }
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
        /// Gets or sets the public id of the asset.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets folder name where the resource is stored.
        /// </summary>
        [DataMember(Name = "folder")]
        public string Folder { get; set; }

        /// <summary>
        /// Gets or sets the name of the resource file.
        /// </summary>
        [DataMember(Name = "filename")]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the format of the asset (png, mp4, etc...).
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets current version of the resource.
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets the type of resource. Possible values: image, raw, video.
        /// </summary>
        public ResourceType ResourceType => Api.ParseCloudinaryParam<ResourceType>(m_resourceType);

        /// <summary>
        /// Gets or sets the storage type.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets date when asset was created.
        /// </summary>
        [Obsolete("Property Created is deprecated, please use CreatedAt instead")]
        public string Created
        {
            get { return CreatedAt; }
            set { CreatedAt = value; }
        }

        /// <summary>
        /// Gets or sets date when the resource was created.
        /// </summary>
        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets date when asset was uploaded (overwritten).
        /// </summary>
        [Obsolete("Property Uploaded is deprecated, please use UploadedAt instead")]
        public string Uploaded
        {
            get { return UploadedAt; }
            set { UploadedAt = value; }
        }

        /// <summary>
        /// Gets or sets date when asset was uploaded (overwritten).
        /// </summary>
        [DataMember(Name = "uploaded_at")]
        public string UploadedAt { get; set; }

        /// <summary>
        /// Gets or sets the size of the asset.
        /// </summary>
        [Obsolete("Property Length is deprecated, please use Bytes instead")]
        public long Length
        {
            get { return Bytes; }
            set { Bytes = value; }
        }

        /// <summary>
        /// Gets or sets the size of the asset.
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Bytes { get; set; }

        /// <summary>
        /// Gets or sets if the resource is backed up, indicates the space taken in the backup system by all backed up versions.
        /// </summary>
        [DataMember(Name = "backup_bytes")]
        public long BackupBytes { get; set; }

        /// <summary>
        /// Gets or sets width of the media asset.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets width of the media asset.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets aspect ratio of the media asset.
        /// </summary>
        [DataMember(Name = "aspect_ratio")]
        public double AspectRatio { get; set; }

        /// <summary>
        /// Gets or sets number of total pixels of the media asset.
        /// </summary>
        [DataMember(Name = "pixels")]
        public long Pixels { get; set; }

        /// <summary>
        /// Gets or sets the number of pages in the image, if the image has multiple pages.
        /// </summary>
        [DataMember(Name = "pages")]
        public int Pages { get; set; }

        /// <summary>
        /// Gets or sets the HTTP URL for accessing the media asset.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the HTTPS URL for securely accessing the media asset.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; set; }

        /// <summary>
        /// Gets or sets the status of the resource. Possible values: active, deleted.
        /// By default, a search response includes only resources with active status.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the authentication level currently set for the resource.
        /// Possible values: public, authenticated.
        /// </summary>
        [DataMember(Name = "access_mode")]
        public string AccessMode { get; set; }

        /// <summary>
        /// Gets or sets parameters of the asset access management.
        /// </summary>
        [DataMember(Name = "access_control")]
        public List<AccessControlRule> AccessControl { get; set; }

        /// <summary>
        /// Gets or sets a value to determine whether two versions of an asset are identical.
        /// </summary>
        [DataMember(Name = "etag")]
        public string Etag { get; set; }

        /// <summary>
        /// Gets or sets a list of tag names assigned to resource.
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Gets or sets IPTC, XMP, and detailed Exif metadata. Supported for images, video, and audio.
        /// </summary>
        [DataMember(Name = "image_metadata")]
        public Dictionary<string, string> ImageMetadata { get; set; }

        /// <summary>
        /// Gets or sets the key-value pairs of general textual context metadata attached to the media asset.
        /// </summary>
        [DataMember(Name = "context")]
        public Dictionary<string, string> Context { get; set; }

        /// <summary>
        /// Gets or sets image analysis results.
        /// </summary>
        [DataMember(Name = "image_analysis")]
        public ImageAnalysis ImageAnalysis { get; set; }

        /// <summary>
        /// Gets or sets identity data of asset creator.
        /// </summary>
        [DataMember(Name = "created_by")]
        public IdentityInfo CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets identity data about resource was uploaded.
        /// </summary>
        [DataMember(Name = "uploaded_by")]
        public IdentityInfo UploadedBy { get; set; }
    }

    /// <summary>
    /// The results of the advanced image analysis.
    /// </summary>
    [DataContract]
    public class ImageAnalysis
    {
        /// <summary>
        /// Gets or sets amount of faces the image contains.
        /// </summary>
        [DataMember(Name = "face_count")]
        public int FaceCount { get; set; }

        /// <summary>
        /// Gets or sets a list of coordinates of detected faces.
        /// </summary>
        [DataMember(Name = "faces")]
        public int[][] Faces { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the image only contains a single grayscale channel.
        /// </summary>
        [DataMember(Name = "grayscale")]
        public bool GrayScale { get; set; }

        /// <summary>
        /// Gets or sets the likelihood that the image is an illustration as opposed to a photograph.
        /// A value between 0 (photo) and 1.0 (illustration).
        /// </summary>
        [DataMember(Name = "illustration_score")]
        public double IllustrationScore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the image contains one or more colors with an alpha channel.
        /// </summary>
        [DataMember(Name = "transparent")]
        public bool Transparent { get; set; }

        /// <summary>
        /// Gets or sets a values to determine whether two versions of an analysis are identical.
        /// </summary>
        [DataMember(Name = "etag")]
        public string Etag { get; set; }

        /// <summary>
        /// Gets or sets the predominant colors uploaded image.
        /// </summary>
        [DataMember(Name = "colors")]
        public Dictionary<string, double> Colors { get; set; }
    }

    /// <summary>
    /// Represents indetity data.
    /// </summary>
    [DataContract]
    public class IdentityInfo
    {
        /// <summary>
        /// Gets or sets identity access key.
        /// </summary>
        [DataMember(Name = "access_key")]
        public string AccessKey { get; set; }
    }
}
