using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class UploadMappingResults : BaseResult
    {
        /// <summary>
        /// Result of CRUD operations
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// Returned upload mappings
        /// </summary>
        public Dictionary<string, string> Mappings { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static UploadMappingResults Parse(HttpWebResponse response)
        {
            UploadMappingResults result = Parse<UploadMappingResults>(response);
            if (result.Mappings == null)
                result.Mappings = new Dictionary<string, string>();

            if (result.JsonObj != null)
            {
                // parsing message
                var message = result.JsonObj.Value<string>("message") ?? string.Empty;
                result.Message = message;

                // parsing mappings
                var mappingsJToken = result.JsonObj["mappings"];
                if (mappingsJToken != null)
                {
                    var mappings = mappingsJToken.Children();
                    foreach(var mapping in mappings)
                    {
                        result.Mappings.Add(mapping["folder"].ToString(), mapping["template"].ToString());
                    }
                }

                // parsing single mapping
                var folder = result.JsonObj.Value<string>("folder") ?? string.Empty;
                var template = result.JsonObj.Value<string>("template") ?? string.Empty;
                if (!string.IsNullOrEmpty(folder))
                    result.Mappings.Add(folder, template);
            }
            
            return result;
        }
    }
}
