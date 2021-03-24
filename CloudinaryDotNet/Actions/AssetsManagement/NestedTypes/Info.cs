namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Any requested information from executing one of the Cloudinary Add-ons on the media asset.
    /// </summary>
    [DataContract]
    public class Info
    {
        /// <summary>
        /// Gets or sets requested information from executing a Rekognition face add-ons.
        /// </summary>
        [DataMember(Name = "detection")]
        public Detection Detection { get; set; }

        /// <summary>
        /// Gets or sets requested information from executing an OCR add-ons.
        /// </summary>
        [DataMember(Name = "ocr")]
        public Ocr Ocr { get; set; }
    }
}