namespace CloudinaryDotNet
{
    /// <summary>
    /// Parameters of custom function.
    /// </summary>
    public class CustomFunction : Core.ICloneable
    {
        private readonly string parameters;

        private CustomFunction(params string[] components)
        {
            parameters = string.Join(":", components);
        }

        /// <summary>
        /// Generate a web-assembly custom function param to send to CustomFunction(customFunction) transformation.
        /// </summary>
        /// <param name="publicId">The public id of the web-assembly file.</param>
        /// <returns>A new instance of custom function param.</returns>
        public static CustomFunction Wasm(string publicId)
        {
            return new CustomFunction("wasm", publicId);
        }

        /// <summary>
        /// Generate a remote lambda custom action function to send to CustomFunction(customFunction) transformation.
        /// </summary>
        /// <param name="url">The public URL of the aws lambda function.</param>
        /// <returns>A new instance of custom function param.</returns>
        public static CustomFunction Remote(string url)
        {
            return new CustomFunction("remote", Utils.EncodeUrlSafe(url));
        }

        /// <summary>
        /// Get string representation of custom function parameters.
        /// </summary>
        /// <returns>String with parameters joined with ':'.</returns>
        public override string ToString()
        {
            return parameters;
        }

        /// <summary>
        /// Creates a new object that is a deep copy of the current instance.
        /// </summary>
        /// <returns> A new object that is a deep copy of this instance.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
