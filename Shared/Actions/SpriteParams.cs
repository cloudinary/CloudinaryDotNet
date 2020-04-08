﻿namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

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
        /// Gets or sets a value for which the sprite is created from all images with this tag.
        /// </summary>
        public string Tag { get; set; }

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

            AddParam(dict, "async", Async);

            if (Transformation != null)
            {
                AddParam(dict, "transformation", Transformation.Generate());
            }

            if (!string.IsNullOrEmpty(Format))
            {
                AddParam(dict, "format", Format);
            }

            return dict;
        }
    }
}
