namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base class for multi-assets requests operations.
    /// </summary>
    public class MultiAssetsParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiAssetsParams"/> class.
        /// </summary>
        /// <param name="tag">The multi-assets entity is created from all images with this tag.</param>
        public MultiAssetsParams(string tag)
        {
            Tag = tag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiAssetsParams"/> class.
        /// </summary>
        /// <param name="urls">The multi-assets entity is created from all images with the urls specified.</param>
        public MultiAssetsParams(List<string> urls)
        {
            Urls = urls;
        }

        /// <summary>
        /// Gets or sets the The multi-assets entity that is created from all images with this tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets a list of urls for all images used in the multi-assets entity.
        /// </summary>
        public List<string> Urls { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets transformation to run on all the individual images before creating the multi-assets entity. Optional.
        /// </summary>
        public Transformation Transformation { get; set; }

        /// <summary>
        /// Gets or sets an HTTP or HTTPS URL to notify your application (a webhook) when the process has completed.
        /// </summary>
        public string NotificationUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to perform the multi-assets entity generation in the background
        /// (asynchronously). Default: false.
        /// </summary>
        public bool Async { get; set; }

        /// <summary>
        /// Gets or sets a value that can be set to 'zip' to generate a zip file containing the images instead of an multi-assets file.
        /// Default: gif (deprecated - use the new CreateArchive method to create zip files).
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets a value that defines whether to return the generated file ('download').
        /// </summary>
        public ArchiveCallMode? Mode { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            var isUrlsEmpty = Urls == null || !Urls.Any();
            if (string.IsNullOrEmpty(Tag) && isUrlsEmpty)
            {
                throw new ArgumentException("Either Tag or Urls must be specified");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();

            AddParam(dict, "tag", Tag);
            AddParam(dict, "notification_url", NotificationUrl);
            AddParam(dict, "format", Format);
            AddParam(dict, "async", Async);

            if (Urls != null && Urls.Any())
            {
                AddParam(dict, "urls", Urls);
            }

            if (Transformation != null)
            {
                AddParam(dict, "transformation", Transformation.Generate());
            }

            if (Mode.HasValue)
            {
                AddParam(dict, "mode", Api.GetCloudinaryParam(Mode.Value));
            }

            return dict;
        }
    }
}
