namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters for concatenating a list of remote video segments into a single MP4 asset
    /// (the server remuxes them without re-encoding).
    /// Mirrors POST /v1_1/{cloud}/video/concat.
    /// The concat runs asynchronously on the server; the final upload result is delivered via
    /// <see cref="RawUploadParams.NotificationUrl"/> (or the global notification URL configured on the account).
    /// The inherited <see cref="BasicRawUploadParams.File"/> property is ignored for this endpoint;
    /// supply <see cref="Urls"/> instead.
    /// </summary>
    public class ConcatVideoParams : VideoUploadParams
    {
        /// <summary>
        /// Maximum number of URLs accepted per request.
        /// </summary>
        public const int MaxUrls = 256;

        /// <summary>
        /// Gets or sets the ordered list of HTTP(S) URLs of the video segments to concatenate, in playback order.
        /// </summary>
        public List<string> Urls { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (Urls == null || Urls.Count == 0)
            {
                throw new ArgumentException("Urls must contain at least one URL.");
            }

            if (Urls.Count > MaxUrls)
            {
                throw new ArgumentException($"Urls must contain at most {MaxUrls} entries.");
            }

            foreach (var url in Urls)
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    throw new ArgumentException("Urls must not contain blank entries.");
                }
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();
            dict.Remove("file");
            AddParam(dict, "urls", Urls);
            return dict;
        }
    }
}
