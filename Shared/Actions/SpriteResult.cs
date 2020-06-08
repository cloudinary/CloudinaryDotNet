namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed response with detailed information about the created sprite.
    /// </summary>
    [DataContract]
    public class SpriteResult : BaseResult
    {
        /// <summary>
        /// Gets or sets URL to css file for the sprite.
        /// </summary>
        [Obsolete("Property CssUri is deprecated, please use CssUrl instead")]
        public Uri CssUri
        {
            get { return CssUrl; }
            set { CssUrl = value; }
        }

        /// <summary>
        /// URL to css file for the sprite.
        /// </summary>
        [DataMember(Name = "css_url")]
        public Uri CssUrl { get; protected set; }

        /// <summary>
        /// Gets or sets secure URL to css file for the sprite.
        /// </summary>
        [Obsolete("Property SecureCssUri is deprecated, please use SecureCssUrl instead")]
        public Uri SecureCssUri
        {
            get { return SecureCssUrl; }
            set { SecureCssUrl = value; }
        }

        /// <summary>
        /// Gets or sets secure URL to css file for the sprite.
        /// </summary>
        [DataMember(Name = "secure_css_url")]
        public Uri SecureCssUrl { get; protected set; }

        /// <summary>
        /// URL to access the created sprite.
        /// </summary>
        [Obsolete("Property ImageUri is deprecated, please use ImageUrl instead")]
        public Uri ImageUri
        {
            get { return ImageUrl; }
            set { ImageUrl = value; }
        }

        /// <summary>
        /// Gets or sets URL to access the created sprite.
        /// </summary>
        [DataMember(Name = "image_url")]
        public Uri ImageUrl { get; protected set; }

        /// <summary>
        /// HTTPS URL to access the created sprite.
        /// </summary>
        [DataMember(Name = "secure_image_url")]
        public Uri SecureImageUrl { get; protected set; }

        /// <summary>
        /// URL to json file with detailed parameters of the created sprite.
        /// </summary>
        [Obsolete("Property JsonUri is deprecated, please use JsonUrl instead")]
        public Uri JsonUri
        {
            get { return JsonUrl; }
            set { JsonUrl = value; }
        }

        /// <summary>
        /// Gets or sets URL to json file with detailed parameters of the created sprite.
        /// </summary>
        [DataMember(Name = "json_url")]
        public Uri JsonUrl { get; protected set; }

        /// <summary>
        /// HTTPS URL to json file with detailed parameters of the created sprite.
        /// </summary>
        [DataMember(Name = "secure_json_url")]
        public Uri SecureJsonUrl { get; protected set; }

        /// <summary>
        /// Gets or sets public ID assigned to the sprite.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        /// <summary>
        /// Gets or sets version of the created sprite.
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; protected set; }

        /// <summary>
        /// Gets or sets details of the images included in the sprite.
        /// </summary>
        [DataMember(Name = "image_infos")]
        public Dictionary<string, ImageInfo> ImageInfos { get; protected set; }
    }

    /// <summary>
    /// Details of an image in the sprite.
    /// </summary>
    [DataContract]
    public class ImageInfo
    {
        /// <summary>
        /// Gets or sets width of the image.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Gets or sets width of the image.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// Gets or sets x-coordinate of the image in sprite.
        /// </summary>
        [DataMember(Name = "x")]
        public int X { get; protected set; }

        /// <summary>
        /// Gets or sets y-coordinate of the image in sprite.
        /// </summary>
        [DataMember(Name = "y")]
        public int Y { get; protected set; }
    }
}
