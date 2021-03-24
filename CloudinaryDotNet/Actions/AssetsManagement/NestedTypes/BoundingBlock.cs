namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The outer bounding polygon for the detected image annotation.
    /// </summary>
    [DataContract]
    public class BoundingBlock
    {
        /// <summary>
        /// Gets or sets the bounding polygon vertices.
        /// </summary>
        [DataMember(Name = "vertices")]
        public List<Point> Vertices { get; set; }
    }
}