namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed result of folders listing.
    /// </summary>
    [DataContract]
    public class GetFoldersResult : BaseResult
    {
        /// <summary>
        /// Gets or sets list of folders.
        /// </summary>
        [DataMember(Name = "folders")]
        public List<Folder> Folders { get; set; }

        /// <summary>
        /// Gets or sets the next cursor.
        ///
        /// When a listing request has more results to return than <see cref="GetFoldersParams.MaxResults"/>,
        /// the <see cref="NextCursor"/> value is returned as part of the response. You can then specify this value as
        /// the <see cref="NextCursor"/> parameter of the following listing request.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; set; }

        /// <summary>
        /// Gets or sets the total count of folders.
        /// </summary>
        [DataMember(Name = "total_count")]
        public int TotalCount { get; set; }
    }
}
