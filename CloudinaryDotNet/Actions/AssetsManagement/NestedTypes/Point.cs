namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Point, represented by X and Y coordinates.
    /// </summary>
    [DataContract]
    public class Point
    {
        /// <summary>
        /// Gets or sets x - coordinate.
        /// </summary>
        [DataMember(Name = "x")]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets y - coordinate.
        /// </summary>
        public double Y { get; set; }
    }
}