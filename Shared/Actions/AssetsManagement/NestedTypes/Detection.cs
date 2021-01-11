namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Requested information from executing a Rekognition face add-on.
    /// </summary>
    [DataContract]
    public class Detection
    {
        /// <summary>
        /// Gets or sets details of the result of recognition.
        /// </summary>
        [DataMember(Name = "rekognition_face")]
        public RekognitionFace RekognitionFace { get; set; }
    }
}