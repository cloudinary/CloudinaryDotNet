namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Detected language for a structural component.
    /// </summary>
    [DataContract]
    public class DetectedLanguage
    {
        /// <summary>
        /// Gets or sets the BCP-47 language code, such as "en-US" or "sr-Latn".
        /// For more information, see http://www.unicode.org/reports/tr35/#Unicode_locale_identifier.
        /// </summary>
        [DataMember(Name = "languageCode")]
        public string LanguageCode { get; set; }
    }
}