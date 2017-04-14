﻿using System.IO;
using System.Net;
using System.Runtime.Serialization;


namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of generating an image of a given textual string
    /// </summary>
    [DataContract]
    public class TextResult : BaseResult
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

    }
}
