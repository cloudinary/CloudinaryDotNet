namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Details of executing an OCR add-on.
    /// </summary>
    [DataContract]
    public class Ocr
    {
        /// <summary>
        /// Gets or sets details of executing an ADV_OCR engine.
        /// </summary>
        [DataMember(Name = "adv_ocr")]
        public AdvOcr AdvOcr { get; set; }
    }
}