using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    ///  Parameters of list upload presets request.
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
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            // ok
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (MaxResults > 0)
                AddParam(dict, "max_results", MaxResults.ToString());

            AddParam(dict, "next_cursor", NextCursor);

            return dict;
        }
    }

    /// <summary>
    /// Parsed result of upload presets listing.
    /// </summary>
    [DataContract]
    public class ListUploadPresetsResult : BaseResult
    {
        /// <summary>
        /// Gets presets.
        /// </summary>
        [DataMember(Name = "presets")]
        public List<GetUploadPresetResult> Presets { get; protected set; }

        /// <summary>
        /// Holds the cursor value if there are more presets than <see cref="ListUploadPresetsParams.MaxResults"/>.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class.
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <returns>New instance of this class.</returns>
        internal static ListUploadPresetsResult Parse(Object response)
        {
            return Api.Parse<ListUploadPresetsResult>(response);
        }
    }
}
