namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Response of upload mappings manipulation.
    /// </summary>
    [DataContract]
    public class UploadMappingResults : BaseResult
    {
        /// <summary>
        /// Gets or sets result of CRUD operations.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets returned upload mappings.
        /// </summary>
        public Dictionary<string, string> Mappings { get; set; }

        /// <summary>
        /// Gets or sets the cursor value if there are more upload mappings than <see cref="UploadMappingParams.MaxResults"/>.
        /// </summary>
        public string NextCursor { get; set; }

        /// <summary>
        /// Overrides corresponding method of <see cref="BaseResult"/> class.
        /// Populates additional token fields.
        /// </summary>
        /// <param name="source">JSON token received from the server.</param>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            if (Mappings == null)
            {
                Mappings = new Dictionary<string, string>();
            }

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
                {
                    Mappings.Add(folder, template);
                }

                // parsing NextCursor
                NextCursor = source.Value<string>("next_cursor") ?? string.Empty;
            }
        }
    }
}
