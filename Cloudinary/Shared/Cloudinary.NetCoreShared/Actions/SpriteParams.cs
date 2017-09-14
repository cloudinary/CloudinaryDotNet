using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of tag management
    /// </summary>
    public class SpriteParams : BaseParams
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="tag">The tag name assigned to images that we should merge into the sprite.</param>
        public SpriteParams(string tag)
        {
            Tag = tag;
        }

        /// <summary>
        /// The tag name assigned to images that we should merge into the sprite.
        /// </summary>
        public string Tag { get; set; }

        public Transformation Transformation { get; set; }

        public string NotificationUrl { get; set; }

        public bool Async { get; set; }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            if (String.IsNullOrEmpty(Tag))
                throw new ArgumentException("Tag must be set!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
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
