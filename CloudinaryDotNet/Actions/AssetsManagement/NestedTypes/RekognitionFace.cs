namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Details of each face found in the image.
    /// </summary>
    [DataContract]
    public class RekognitionFace
    {
        /// <summary>
        /// Gets or sets status of the recognition process.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets properties of each detected face.
        /// </summary>
        [DataMember(Name = "data")]
        public List<Face> Faces { get; set; }
    }
}