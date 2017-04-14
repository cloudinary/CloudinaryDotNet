using System.Net;
using System.Runtime.Serialization;

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
        /// Holds the cursor value if there are more upload mappings than <see cref="UploadMappingParams.MaxResults"/>.
        /// </summary>
        public string NextCursor { get; protected set; }

        protected override void OnParse()
        {
            if (Mappings == null)
                Mappings = new Dictionary<string, string>();

            if (JsonObj != null)
            {
                // parsing message
                var message = JsonObj.Value<string>("message") ?? string.Empty;
                Message = message;

                // parsing mappings
                var mappingsJToken = JsonObj["mappings"];
                if (mappingsJToken != null)
                {
                    var mappings = mappingsJToken.Children();
                    foreach(var mapping in mappings)
                    {
                        Mappings.Add(mapping["folder"].ToString(), mapping["template"].ToString());
                    }
                }

                // parsing single mapping
                var folder = JsonObj.Value<string>("folder") ?? string.Empty;
                var template = JsonObj.Value<string>("template") ?? string.Empty;
                if (!string.IsNullOrEmpty(folder))
                    Mappings.Add(folder, template);

                //parsing NextCursor
                NextCursor = JsonObj.Value<string>("next_cursor") ?? string.Empty;
            }
        }
    }
}
