namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters for managing folders list.
    /// </summary>
    public class RenameFolderParams : BaseParams
    {
        /// <summary>
        /// Gets or sets the full path of the existing folder.
        /// </summary>
        public string FromPath { get; set; }

        /// <summary>
        /// Gets or sets the full path of the new folder.
        /// </summary>
        public string ToPath { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(FromPath))
            {
                throw new ArgumentException("FromPath must be specified!");
            }

            if (string.IsNullOrEmpty(ToPath))
            {
                throw new ArgumentException("ToPath must be specified!");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();

            AddParam(dict, "to_folder", ToPath);

            return dict;
        }
    }
}
