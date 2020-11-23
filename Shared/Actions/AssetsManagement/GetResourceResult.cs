namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed response with the detailed resource information.
    /// </summary>
    [DataContract]
    public class GetResourceResult : BaseResult
    {
        /// <summary>
        /// The type of file. Possible values: image, raw, video.
        /// </summary>
        [DataMember(Name = "resource_type")]
        protected string m_resourceType;

        /// <summary>
        /// Gets or sets public ID assigned to the resource.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets the format this resource is delivered in.
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
        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        /// <summary>
        /// Gets or sets the storage type: upload, private, authenticated, facebook, twitter, gplus, instagram_name, gravatar,
        /// youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets date when the resource was created.
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
        /// Gets or sets size of the resource in bytes.
        /// </summary>
        [Obsolete("Property Length is deprecated, please use Bytes instead")]
        public long Length
        {
            get { return Bytes; }
            set { Bytes = value; }
        }

        /// <summary>
        /// Gets or sets size of the resource in bytes.
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Bytes { get; set; }

        /// <summary>
        /// Gets or sets parameter "width" of the resource. Not relevant for raw files.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets parameter "height" of the resource. Not relevant for raw files.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets URL to the resource.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the HTTPS URL for securely accessing the media asset.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; set; }

        /// <summary>
        /// Gets or sets when a listing request has more results to return than <see cref="GetResourceParams.MaxResults"/>,
        /// the <see cref="NextCursor"/> value is returned as part of the response. You can then specify this value as
        /// the <see cref="NextCursor"/> parameter of the following listing request.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; set; }

        /// <summary>
        /// Gets or sets if there are more derived images than <see cref="GetResourceParams.MaxResults"/>,
        /// the <see cref="DerivedNextCursor"/> value is returned as part of the response. You can then specify this value as
        /// the <see cref="DerivedNextCursor"/> parameter of the following listing request.
        /// </summary>
        [DataMember(Name = "derived_next_cursor")]
        public string DerivedNextCursor { get; set; }

        /// <summary>
        /// Gets or sets exif metadata of the resource.
        /// </summary>
        [DataMember(Name = "exif")]
        public Dictionary<string, string> Exif { get; set; }

        /// <summary>
        /// Gets or sets iPTC, XMP, and detailed Exif metadata. Supported for images, video, and audio.
        /// </summary>
        [Obsolete("Property Metadata is deprecated, please use ImageMetadata instead")]
        public Dictionary<string, string> Metadata
        {
            get { return ImageMetadata; }
            set { ImageMetadata = value; }
        }

        /// <summary>
        /// Gets or sets iPTC, XMP, and detailed Exif metadata. Supported for images, video, and audio.
        /// </summary>
        [DataMember(Name = "image_metadata")]
        public Dictionary<string, string> ImageMetadata { get; set; }

        /// <summary>
        /// Gets or sets a list of coordinates of detected faces.
        /// </summary>
        [DataMember(Name = "faces")]
        public int[][] Faces { get; set; }

        /// <summary>
        /// Gets or sets a quality analysis value for the image.
        /// </summary>
        [DataMember(Name = "quality_analysis")]
        public QualityAnalysis QualityAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the quality score.
        /// </summary>
        [DataMember(Name = "quality_score")]
        public double QualityScore { get; set; }

        /// <summary>
        /// Gets or sets color information: predominant colors and histogram of 32 leading colors.
        /// </summary>
        [DataMember(Name = "colors")]
        public string[][] Colors { get; set; }

        /// <summary>
        /// Gets or sets a list of derived resources.
        /// </summary>
        [DataMember(Name = "derived")]
        public Derived[] Derived { get; set; }

        /// <summary>
        /// Gets or sets a list of tag names assigned to resource.
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Gets or sets image moderation status of the resource.
        /// </summary>
        [DataMember(Name = "moderation")]
        public List<Moderation> Moderation { get; set; }

        /// <summary>
        /// Gets or sets a key-value pairs of context associated with the resource.
        /// </summary>
        [DataMember(Name = "context")]
        public JToken Context { get; set; }

        /// <summary>
        /// Gets or sets a key-value pairs of custom metadata fields associated with the resource.
        /// </summary>
        [DataMember(Name = "metadata")]
        public JToken MetadataFields { get; set; }

        /// <summary>
        /// Gets or sets a perceptual hash (pHash) of the uploaded resource for image similarity detection.
        /// </summary>
        [DataMember(Name = "phash")]
        public string Phash { get; set; }

        /// <summary>
        /// Gets or sets the predominant colors in the image according to both a Google palette and a Cloudinary palette.
        /// </summary>
        [DataMember(Name = "predominant")]
        public Predominant Predominant { get; set; }

        /// <summary>
        /// Gets or sets the coordinates of a single region contained in an image that is subsequently used for cropping the image using
        /// the custom gravity mode.
        /// </summary>
        [DataMember(Name = "coordinates")]
        public Coordinates Coordinates { get; set; }

        /// <summary>
        /// Gets or sets any requested information from executing one of the Cloudinary Add-ons on the media asset.
        /// </summary>
        [DataMember(Name = "info")]
        public Info Info { get; set; }

        /// <summary>
        /// Gets or sets parameters of the asset access management.
        /// </summary>
        [DataMember(Name = "access_control")]
        public List<AccessControlRule> AccessControl { get; set; }

        /// <summary>
        /// Gets or sets the number of pages in the asset: included if the asset has multiple pages (e.g., PDF or animated GIF).
        /// </summary>
        [DataMember(Name = "pages")]
        public int Pages { get; set; }

        /// <summary>
        /// Gets or sets the accessibility mode of the media asset: public, or authenticated.
        /// </summary>
        [DataMember(Name = "access_mode")]
        public string AccessMode { get; set; }

        /// <summary>
        /// Gets or sets details of cinemagraph analysis for the resource.
        /// </summary>
        [DataMember(Name = "cinemagraph_analysis")]
        public CinemagraphAnalysis CinemagraphAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the color ambiguity score that indicate how good\bad an image is for colorblind people.
        /// Will they be able to differentiate between different elements in the image.
        /// </summary>
        [DataMember(Name = "accessibility_analysis")]
        public AccessibilityAnalysis AccessibilityAnalysis { get; set; }

        /// <summary>
        /// Gets or sets asset identifier.
        /// </summary>
        [DataMember(Name = "asset_id")]
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets list of asset versions.
        /// </summary>
        [DataMember(Name = "versions")]
        public List<AssetVersion> Versions { get; set; }
    }
}
