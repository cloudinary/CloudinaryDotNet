namespace CloudinaryDotNet
{
    /// <summary>
    /// Source definition of the URL.
    /// </summary>
    public class CSource
    {
        /// <summary>
        /// Source part of the URL.
        /// </summary>
        public string Source;

        /// <summary>
        /// Source part of the URL to be signed.
        /// </summary>
        public string SourceToSign;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSource"/> class with a source.
        /// </summary>
        /// <param name="source">Source part of the URL.</param>
        public CSource(string source)
        {
            SourceToSign = Source = source;
        }

        /// <summary>
        /// Add source value to the source part of the URL.
        /// </summary>
        /// <param name="src">Source definition of the URL.</param>
        /// <param name="value">Source value to add.</param>
        /// <returns>Updated source definition.</returns>
        public static CSource operator +(CSource src, string value)
        {
            return OpAddition(src, value);
        }

        /// <summary>
        /// Add source value to the source part of the URL.
        /// </summary>
        /// <param name="src">Source definition of the URL.</param>
        /// <param name="value">Source value to add.</param>
        /// <returns>Updated source definition.</returns>
        public static CSource Add(CSource src, string value)
        {
            return OpAddition(src, value);
        }

        private static CSource OpAddition(CSource src, string value)
        {
            src.Source += value;
            src.SourceToSign += value;

            return src;
        }
    }
}
