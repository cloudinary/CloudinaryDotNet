using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of image uploading
    /// </summary>
    [DataContract]
    public class ImageUploadResult : RawUploadResult
    {
        /// <summary>
        /// Image width
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Image height
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        [DataMember(Name = "exif")]
        public Dictionary<string, string> Exif { get; protected set; }

        [DataMember(Name = "image_metadata")]
        public Dictionary<string, string> Metadata { get; protected set; }

        [DataMember(Name = "faces")]
        public int[][] Faces { get; protected set; }

        [DataMember(Name = "colors")]
        public string[][] Colors { get; protected set; }

        [DataMember(Name = "phash")]
        public string Phash { get; protected set; }

        [DataMember(Name = "delete_token")]
        public string DeleteToken { get; protected set; }

        [DataMember(Name = "info")]
        public Info Info { get; protected set; }

        /// <summary>
        /// List of responsive image breakpoints
        /// </summary>
        public List<ResponsiveBreakpointList> ResponsiveBreakpoints { get; set; }
        
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            var responsiveBreakpoints = source["responsive_breakpoints"];
            if (responsiveBreakpoints != null)
            {
                ResponsiveBreakpoints = responsiveBreakpoints.ToObject<List<ResponsiveBreakpointList>>();
            }
        }
    }
}
