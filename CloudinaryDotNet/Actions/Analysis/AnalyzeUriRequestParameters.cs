namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Analyze Uri Request Parameters.
    /// </summary>
    public class AnalyzeUriRequestParameters : BaseParams
    {
        /// <summary>
        /// Gets or sets custom parameters.
        /// </summary>
        public CustomParameters Custom { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
        }

        /// <inheritdoc />
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = new SortedDictionary<string, object>();

            AddParam(dict, "custom", Custom);

            return dict;
        }
    }
}
