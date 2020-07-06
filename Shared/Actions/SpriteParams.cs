namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Parameters of create sprite request.
    /// </summary>
    public class SpriteParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteParams"/> class.
        /// </summary>
        /// <param name="tag">The tag name assigned to images that we should merge into the sprite.</param>
        public SpriteParams(string tag)
        {
            Tag = tag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteParams"/> class.
        /// </summary>
        /// <param name="urls">The sprite is created from all images with the urls specified.</param>
        public SpriteParams(List<string> urls)
        {
            Urls = urls;
        }

        /// <summary>
        /// Gets or sets a value for which the sprite is created from all images with this tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets a list of urls for all images used in the sprite.
        /// </summary>
        public List<string> Urls { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a transformation to run on all the individual images before creating the sprite. Optional.
        /// </summary>
        public Transformation Transformation { get; set; }

        /// <summary>
        /// Gets or sets a format to convert the sprite before saving it in your Cloudinary account. Default: png. Optional.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets an HTTP or HTTPS URL to notify your application (a webhook) when the process has completed. Optional.
        /// </summary>
        public string NotificationUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to perform the sprite generation in the background (asynchronously).
        /// Default: false.
        /// </summary>
        public bool Async { get; set; }

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
            AddParam(dict, "async", Async);

            if (Transformation != null)
            {
                AddParam(dict, "transformation", Transformation.Generate());
            }

            if (!string.IsNullOrEmpty(Format))
            {
                AddParam(dict, "format", Format);
            }

            if (Urls != null && Urls.Any())
            {
                AddParam(dict, "urls", Urls);
            }

            if (Mode.HasValue)
            {
                AddParam(dict, "mode", Api.GetCloudinaryParam(Mode.Value));
            }

            return dict;
        }
    }
}
