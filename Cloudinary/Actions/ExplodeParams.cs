using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of tag management
    /// </summary>
    public class ExplodeParams : BaseParams
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="tag">The tag name assigned to images that we should merge into the sprite.</param>
        public ExplodeParams(string publicId, Transformation transformation)
        {
            PublicId = publicId;
            Transformation = transformation;
        }

        /// <summary>
        /// The tag name assigned to images that we should merge into the sprite.
        /// </summary>
        public string PublicId { get; set; }

        public Transformation Transformation { get; set; }

        public string NotificationUrl { get; set; }

        public string Format { get; set; }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            if (String.IsNullOrEmpty(PublicId))
                throw new ArgumentException("PublicId must be set!");

            if (Transformation == null)
                throw new ArgumentException("Transformation must be set!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = new SortedDictionary<string, object>();

            AddParam(dict, "public_id", PublicId);
            AddParam(dict, "notification_url", NotificationUrl);
            AddParam(dict, "format", Format);

            if (Transformation != null)
                AddParam(dict, "transformation", Transformation.Generate());

            return dict;
        }
    }
}
