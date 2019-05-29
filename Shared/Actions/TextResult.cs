using System;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of generating an image of a given textual string.
    /// </summary>
    [DataContract]
    public class TextResult : BaseResult
    {
        /// <summary>
        /// Parameter "width" of the asset.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Parameter "height" of the asset.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }
        
    }
}
