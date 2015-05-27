using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
//using System.Runtime.Serialization.Json;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of generating an image of a given textual string
    /// </summary>
    //[DataContract]
    public class TextResult : BaseResult
    {
        /// <summary>
        /// Image width
        /// </summary>
        [JsonProperty(PropertyName = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Image height
        /// </summary>
        [JsonProperty(PropertyName = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static TextResult Parse(HttpWebResponse response)
        {
            return Parse<TextResult>(response);
        }
    }
}
