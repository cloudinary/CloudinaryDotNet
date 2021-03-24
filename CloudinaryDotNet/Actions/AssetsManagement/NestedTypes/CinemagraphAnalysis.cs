namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Details of the cinemagraph analysis.
    /// </summary>
    [DataContract]
    public class CinemagraphAnalysis
    {
        /// <summary>
        /// Gets or sets value between 0-1, where 0 means definitely not a cinemagraph
        /// and 1 means definitely a cinemagraph).
        /// </summary>
        [DataMember(Name = "cinemagraph_score")]
        public double CinemagraphScore { get; set; }
    }
}