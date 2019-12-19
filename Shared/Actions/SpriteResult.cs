﻿namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed response with detailed information about the created sprite.
    /// </summary>
    [DataContract]
    public class SpriteResult : BaseResult
    {
        /// <summary>
        /// URL to css file for the sprite.
        /// </summary>
        [DataMember(Name = "css_url")]
        public Uri CssUri { get; protected set; }

        /// <summary>
        /// Secure URL to css file for the sprite.
        /// </summary>
        [DataMember(Name = "secure_css_url")]
        public Uri SecureCssUri { get; protected set; }

        /// <summary>
        /// URL to access the created sprite.
        /// </summary>
        [DataMember(Name = "image_url")]
        public Uri ImageUri { get; protected set; }

        /// <summary>
        /// URL to json file with detailed parameters of the created sprite.
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

        /// <inheritdoc/>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            CssUri = source.ReadValueAsSnakeCase<Uri>(nameof(CssUri).FixUri());
            SecureCssUri = source.ReadValueAsSnakeCase<Uri>(nameof(SecureCssUri).FixUri());
            ImageUri = source.ReadValueAsSnakeCase<Uri>(nameof(ImageUri).FixUri());
            JsonUri = source.ReadValueAsSnakeCase<Uri>(nameof(JsonUri).FixUri());
            PublicId = source.ReadValueAsSnakeCase<string>(nameof(PublicId));
            Version = source.ReadValueAsSnakeCase<string>(nameof(Version));

            ImageInfos = source
                .ReadValueAsSnakeCase<Dictionary<string, JObject>>(nameof(ImageInfos))
                .ToDictionary(kvp => kvp.Key, kvp => new ImageInfo(kvp.Value));
        }
    }

    /// <summary>
    /// Details of an image in the sprite.
    /// </summary>
    [DataContract]
    public class ImageInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageInfo"/> class.
        /// </summary>
        public ImageInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageInfo"/> class.
        /// </summary>
        /// <param name="source">JSON Token.</param>
        internal ImageInfo(JToken source)
        {
            Width = source.ReadValueAsSnakeCase<int>(nameof(Width));
            Height = source.ReadValueAsSnakeCase<int>(nameof(Height));
            X = source.ReadValueAsSnakeCase<int>(nameof(X));
            Y = source.ReadValueAsSnakeCase<int>(nameof(Y));
        }

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
