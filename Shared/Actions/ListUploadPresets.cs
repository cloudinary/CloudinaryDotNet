﻿namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;

    /// <summary>
    ///  Parameters of list upload presets request.
    /// </summary>
    public class ListUploadPresetsParams : BaseParams
    {
        /// <summary>
        /// Gets or sets max number of resources to return. Default=10. Maximum=500. Optional.
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Gets or sets next cursor value.
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
            {
                AddParam(dict, "max_results", MaxResults.ToString(CultureInfo.InvariantCulture));
            }

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
        /// Gets or sets presets.
        /// </summary>
        [DataMember(Name = "presets")]
        public List<GetUploadPresetResult> Presets { get; set; }

        /// <summary>
        /// Gets or sets the cursor value if there are more presets than <see cref="ListUploadPresetsParams.MaxResults"/>.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; set; }
    }
}
