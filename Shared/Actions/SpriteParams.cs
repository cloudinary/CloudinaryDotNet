using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of create sprite request.
    /// </summary>
    public class SpriteParams : BaseParams
    {
        /// <summary>
        /// Instantiates the <see cref="SpriteParams"/> object.
        /// </summary>
        /// <param name="tag">The tag name assigned to images that we should merge into the sprite.</param>
        public SpriteParams(string tag)
        {
            Tag = tag;
        }

        /// <summary>
        /// The sprite is created from all images with this tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// (Optional) A transformation to run on all the individual images before creating the sprite.
        /// </summary>
        public Transformation Transformation { get; set; }

        /// <summary>
        /// (Optional) An HTTP or HTTPS URL to notify your application (a webhook) when the process has completed.
        /// </summary>
        public string NotificationUrl { get; set; }

        /// <summary>
        /// Tells Cloudinary whether to perform the sprite generation in the background (asynchronously). 
        /// Default: false.
        /// </summary>
        public bool Async { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (String.IsNullOrEmpty(Tag))
                throw new ArgumentException("Tag must be set!");
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
                AddParam(dict, "transformation", Transformation.Generate());

            return dict;
        }
    }
}
