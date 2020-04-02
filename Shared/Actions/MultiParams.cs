namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters of request to create a single animated GIF file from a group of images.
    /// </summary>
    public class MultiParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiParams"/> class.
        /// </summary>
        /// <param name="tag">The animated GIF is created from all images with this tag.</param>
        public MultiParams(string tag)
        {
            Tag = tag;
        }

        /// <summary>
        /// Gets or sets the animated GIF that is created from all images with this tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets transformation to run on all the individual images before creating the animated GIF. Optional.
        /// </summary>
        public Transformation Transformation { get; set; }

        /// <summary>
        /// Gets or sets an HTTP or HTTPS URL to notify your application (a webhook) when the process has completed.
        /// </summary>
        public string NotificationUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to perform the GIF generation in the background (asynchronously). Default: false.
        /// </summary>
        public bool Async { get; set; }

        /// <summary>
        /// Gets or sets a value that can be set to 'zip' to generate a zip file containing the images instead of an animated GIF file.
        /// Default: gif (deprecated - use the new CreateArchive method to create zip files).
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(Tag))
            {
                throw new ArgumentException("Tag must be set!");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "tag", Tag);
            AddParam(dict, "notification_url", NotificationUrl);
            AddParam(dict, "format", Format);
            AddParam(dict, "async", Async);

            if (Transformation != null)
            {
                AddParam(dict, "transformation", Transformation.Generate());
            }

            return dict;
        }
    }
}
