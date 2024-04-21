namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters for analysing assets.
    /// </summary>
    public class AnalyzeParams : BaseParams
    {
        /// <summary>
        /// Gets or sets the type of analysis to run ('google_tagging', 'captioning', 'fashion').
        /// </summary>
        public string AnalysisType { get; set; }

        /// <summary>
        /// Gets or sets the type of input for the asset to analyze ('uri').
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets additional parameters.
        /// </summary>
        public AnalyzeUriRequestParameters Parameters { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(AnalysisType))
            {
                throw new ArgumentException("AnalysisType must be specified!");
            }

            if (string.IsNullOrEmpty(Uri))
            {
                throw new ArgumentException("Uri must be specified!");
            }

            Parameters?.Check();
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();

            AddParam(dict, "analysis_type", AnalysisType);
            AddParam(dict, "uri", Uri);
            AddParam(dict, "parameters", Parameters);

            return dict;
        }
    }
}
