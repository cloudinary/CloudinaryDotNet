namespace CloudinaryDotNet
{
    public class CustomFunction
    {
        private readonly string _params;

        private CustomFunction(params string[] components)
        {
            _params = string.Join(":", components);
        }

        /// <summary>
        /// Generate a web-assembly custom function param to send to CustomFunction(customFunction) transformation 
        /// </summary>
        /// <param name="publicId">The public id of the web-assembly file</param>
        /// <returns>A new instance of custom function param</returns>
        public static CustomFunction Wasm(string publicId)
        {
            return new CustomFunction("wasm", publicId);
        }

        /// <summary>
        /// Generate a remote lambda custom action function to send to CustomFunction(customFunction) transformation 
        /// </summary>
        /// <param name="url">The public url of the aws lambda function</param>
        /// <returns>A new instance of custom function param</returns>
        public static CustomFunction Remote(string url)
        {
            return new CustomFunction("remote", Utils.EncodeUrlSafe(url));
        }

        public override string ToString()
        {
            return _params;
        }
    }
}
