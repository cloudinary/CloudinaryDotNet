namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows to filter resources by moderation kind/status.
    /// </summary>
    public class ListResourcesByModerationParams : ListResourcesParams
    {
        /// <summary>
        /// Gets or sets the kind of the moderation (manual, etc.).
        /// </summary>
        public string ModerationKind { get; set; }

        /// <summary>
        /// Gets or sets the moderation status.
        /// </summary>
        public ModerationStatus ModerationStatus { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            base.Check();

            if (string.IsNullOrEmpty(ModerationKind))
            {
                throw new ArgumentException("ModerationKind must be set to filter resources by moderation kind/status!");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (dict.ContainsKey("type"))
            {
                dict.Remove("type");
            }

            return dict;
        }
    }
}