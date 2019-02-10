using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Search response with information about the assets matching the search criteria.
    /// </summary>
    [DataContract]
    public class SearchResult : BaseResult
    {
        /// <summary>
        /// The total count of assets matching the search criteria.
        /// </summary>
        [DataMember(Name = "total_count")]
        public int TotalCount { get; protected set; }

        /// <summary>
        /// The time taken to process the request.
        /// </summary>
        [DataMember(Name = "time")]
        public long Time { get; protected set; }

        /// <summary>
        /// The details of each of the assets (resources) found.
        /// </summary>
        [DataMember(Name = "resources")]
        public List<SearchResource> Resources { get; protected set; }

        /// <summary>
        /// When a search request has more results to return than max_results, the next_cursor value is returned as
        /// part of the response.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; protected set; }
        
    }

    /// <summary>
    /// The details of the asset (resource) found.
    /// </summary>
    [DataContract]
    public class SearchResource
    {
        /// <summary>
        /// The public id of the asset.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        /// <summary>
        /// Date when asset was created.
        /// </summary>
        [DataMember(Name = "created_at")]
        public string Created { get; protected set; }

        /// <summary>
        /// The format of the asset (png, mp4, etc...).
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        /// <summary>
        /// Width of the media asset.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Width of the media asset.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// The size of the asset.
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Length { get; protected set; }
    }
}
