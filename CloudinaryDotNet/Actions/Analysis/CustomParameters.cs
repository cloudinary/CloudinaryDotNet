namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Custom Parameters.
    /// </summary>
    public class CustomParameters : BaseParams
    {
        /// <summary>
        /// Gets or sets the model name.
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets the model version.
        /// </summary>
        public int ModelVersion { get; set; }

        /// <inheritdoc />
        public override void Check()
        {
        }

        /// <inheritdoc />
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = new SortedDictionary<string, object>();

            AddParam(dict, "model_name", ModelName);
            AddParam(dict, "model_version", ModelVersion);

            return dict;
        }
    }
}
