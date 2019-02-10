using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parsed response with detailed information about the created sprite.
    /// </summary>
    [DataContract]
    public class SpriteResult : BaseResult
    {
        /// <summary>
        /// Url to css file for the sprite.
        /// </summary>
        [DataMember(Name = "css_url")]
        public Uri CssUri { get; protected set; }

        /// <summary>
        /// Secure Url to css file for the sprite.
        /// </summary>
        [DataMember(Name = "secure_css_url")]
        public Uri SecureCssUri { get; protected set; }

        /// <summary>
        /// Url to access the created sprite.
        /// </summary>
        [DataMember(Name = "image_url")]
        public Uri ImageUri { get; protected set; }

        /// <summary>
        /// Url to json file with detailed parameters of the created sprite.
        /// </summary>
        [DataMember(Name = "json_url")]
        public Uri JsonUri { get; protected set; }

        /// <summary>
        /// Public ID assigned to the sprite. 
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        /// <summary>
        /// Version of the created sprite.
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; protected set; }

        /// <summary>
        /// Details of the images included in the sprite.
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
        /// Width of the image.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Width of the image.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// X-coordinate of the image in sprite.
        /// </summary>
        [DataMember(Name = "x")]
        public int X { get; protected set; }

        /// <summary>
        /// Y-coordinate of the image in sprite.
        /// </summary>
        [DataMember(Name = "y")]
        public int Y { get; protected set; }
    }
}
