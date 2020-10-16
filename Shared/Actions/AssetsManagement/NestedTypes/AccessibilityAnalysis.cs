namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Details of the accessibility analysis.
    /// </summary>
    [DataContract]
    public class AccessibilityAnalysis
    {
        /// <summary>
        /// Gets or sets details of colorblind accessibility analysis.
        /// </summary>
        [DataMember(Name = "colorblind_accessibility_analysis")]
        public ColorblindAccessibilityAnalysis ColorblindAccessibilityAnalysis { get; set; }

        /// <summary>
        /// Gets or sets value between 0-1.
        /// </summary>
        [DataMember(Name = "colorblind_accessibility_score")]
        public double ColorblindAccessibilityScore { get; set; }
    }
}