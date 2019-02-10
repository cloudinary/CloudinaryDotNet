namespace CloudinaryDotNet
{
    /// <summary>
    /// Source definition of the Url.
    /// </summary>
    public class CSource
    {
        /// <summary>
        /// Instantiates the <see cref="CSource"/> object with a source.
        /// </summary>
        /// <param name="source">Source part of the Url.</param>
        public CSource(string source)
        {
            SourceToSign = Source = source;
        }

        /// <summary>
        /// Add source value to the source part of the Url.
        /// </summary>
        /// <param name="src">Source definition of the Url.</param>
        /// <param name="value">Source value to add.</param>
        /// <returns>Updated source definition.</returns>
        public static CSource operator +(CSource src, string value)
        {
            src.Source += value;
            src.SourceToSign += value;

            return src;
        }

        /// <summary>
        /// Source part of the Url.
        /// </summary>
        public string Source;

        /// <summary>
        /// Source part of the Url to be signed.
        /// </summary>
        public string SourceToSign;
    }
}
