namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

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