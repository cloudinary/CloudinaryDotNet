using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of tags management
    /// </summary>
    [DataContract]
    public class SpriteResult : BaseResult
    {
        [DataMember(Name = "css_url")]
        public Uri CssUri { get; protected set; }

        [DataMember(Name = "secure_css_url")]
        public Uri SecureCssUri { get; protected set; }

        [DataMember(Name = "image_url")]
        public Uri ImageUri { get; protected set; }

        [DataMember(Name = "json_url")]
        public Uri JsonUri { get; protected set; }

        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        [DataMember(Name = "version")]
        public string Version { get; protected set; }

        [DataMember(Name = "image_infos")]
        public Dictionary<string, ImageInfo> ImageInfos { get; protected set; }
        
    }

    [DataContract]
    public class ImageInfo
    {
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        [DataMember(Name = "x")]
        public int X { get; protected set; }

        [DataMember(Name = "y")]
        public int Y { get; protected set; }
    }
}
