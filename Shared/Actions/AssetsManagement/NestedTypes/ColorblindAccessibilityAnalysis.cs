namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Details of colorblind accessibility analysis.
    /// </summary>
    [DataContract]
    public class ColorblindAccessibilityAnalysis
    {
        /// <summary>
        /// Gets or sets distinct edges value between 0-1.
        /// </summary>
        [DataMember(Name = "distinct_edges")]
        public double DistinctEdges { get; set; }

        /// <summary>
        /// Gets or sets distinct colors value between 0-1.
        /// </summary>
        [DataMember(Name = "distinct_colors")]
        public double DistinctColors { get; set; }

        /// <summary>
        /// Gets or sets most indistinct pair of colors.
        /// </summary>
        [DataMember(Name = "most_indistinct_pair")]
        public string[] MostIndistinctPair { get; set; }
    }
}