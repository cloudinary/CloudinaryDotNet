﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    ///  Allows to list upload presets.
    /// </summary>
    public class ListUploadPresetsParams : BaseParams
    {
        /// <summary>
        /// Optional. Max number of resources to return. Default=10. Maximum=500.
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Optional.
        /// </summary>
        public string NextCursor { get; set; }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            // ok
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = new SortedDictionary<string, object>();

            if (MaxResults > 0)
                AddParam(dict, "max_results", MaxResults.ToString());

            AddParam(dict, "next_cursor", NextCursor);

            return dict;
        }
    }

    /// <summary>
    /// Response to <see cref="ListUploadPresetsParams"/>.
    /// </summary>
    //[DataContract]
    public class ListUploadPresetsResult : BaseResult
    {
        /// <summary>
        /// Gets presets.
        /// </summary>
        [JsonProperty(PropertyName = "presets")]
        public List<GetUploadPresetResult> Presets { get; protected set; }

        /// <summary>
        /// Holds the cursor value if there are more presets than <see cref="ListUploadPresetsParams.MaxResults"/>.
        /// </summary>
        [JsonProperty(PropertyName = "next_cursor")]
        public string NextCursor { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static ListUploadPresetsResult Parse(HttpWebResponse response)
        {
            return Parse<ListUploadPresetsResult>(response);
        }
    }
}
