namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// For multi-page files (e.g. PDFs), a node indicating the containing page.
    /// </summary>
    [DataContract]
    public class FullTextAnnotation
    {
        /// <summary>
        /// Gets or sets a list of detected pages.
        /// </summary>
        [DataMember(Name = "pages")]
        public List<Page> Pages { get; set; }

        /// <summary>
        /// Gets or sets recognized text.
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}