using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Linq;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Response of upload mappings manipulation.
    /// </summary>
    [DataContract]
    public class UploadMappingResults : BaseResult
    {
        /// <summary>
        /// Result of CRUD operations.
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// Returned upload mappings.
        /// </summary>
        public Dictionary<string, string> Mappings { get; protected set; }

        /// <summary>
        /// Holds the cursor value if there are more upload mappings than <see cref="UploadMappingParams.MaxResults"/>.
        /// </summary>
        public string NextCursor { get; protected set; }
        
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            if (Mappings == null)
                Mappings = new Dictionary<string, string>();

            if (source != null)
            {
                // parsing message
                var message = source.Value<string>("message") ?? string.Empty;
                Message = message;

                // parsing mappings
                var mappingsJToken = source["mappings"];
                if (mappingsJToken != null)
                {
                    var mappings = mappingsJToken.Children();
                    foreach (var mapping in mappings)
                    {
                        Mappings.Add(mapping["folder"].ToString(), mapping["template"].ToString());
                    }
                }

                // parsing single mapping
                var folder = source.Value<string>("folder") ?? string.Empty;
                var template = source.Value<string>("template") ?? string.Empty;
                if (!string.IsNullOrEmpty(folder))
                    Mappings.Add(folder, template);

                //parsing NextCursor
                NextCursor = source.Value<string>("next_cursor") ?? string.Empty;
            }
        }
    }
}
