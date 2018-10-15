using System;
using System.Collections.Generic;
using System.Text;

namespace CloudinaryDotNet
{
    public class CustomFunction
    {
        private readonly string m_params;

        private CustomFunction(params string[] components)
        {
            m_params = string.Join(":", components);
        }

        /// <summary>
        /// Generate a web-assembly custom function param to send to CustomAction(customFunction) transformation 
        /// </summary>
        /// <param name="publicId">The public id of the web-assembly file</param>
        /// <returns>A new instance of custom function param</returns>
        public static CustomFunction WASM(string publicId)
        {
            return new CustomFunction("wasm", publicId);
        }

        /// <summary>
        /// Generate a remote lambda custom action function to send to CustomAction(customFunction) transformation 
        /// </summary>
        /// <param name="url">The public url of the aws lambda function</param>
        /// <returns>A new instance of custom function param</returns>
        public static CustomFunction Remote(string url)
        {
            return new CustomFunction("remote", Utils.EncodeUrlSafe(url));
        }

        public override string ToString()
        {
            return m_params.ToString();
        }
    }
}
