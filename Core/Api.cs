using System;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Technological layer to work with cloudinary API.
    /// </summary>
    public class Api : ApiShared
    {
        /// <summary>
        /// Default parameterless constructor. Assumes that environment variable CLOUDINARY_URL is set.
        /// </summary>
        public Api() : base() { }

        /// <summary>
        /// Instantiates the cloudinary <see cref="Api"/> object with cloudinary URL.
        /// </summary>
        /// <param name="cloudinaryUrl">Cloudinary URL.</param>
        public Api(string cloudinaryUrl) : base(cloudinaryUrl)
        {
        }

        /// <summary>
        /// Instantiates the cloudinary <see cref="Api"/> object with initial parameters.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        /// <param name="usePrivateCdn">Whether to use private Content Delivery Network.</param>
        /// <param name="privateCdn">Private Content Delivery Network.</param>
        /// <param name="shortenUrl">Whether to use shorten url when possible.</param>
        /// <param name="cSubDomain">Whether to use sub domain.</param>
        public Api(Account account, bool usePrivateCdn, string privateCdn, bool shortenUrl, bool cSubDomain)
            : base(account, usePrivateCdn, privateCdn, shortenUrl, cSubDomain)
        {
        }

        /// <summary>
        /// Instantiates the cloudinary <see cref="Api"/> object with account.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        public Api(Account account) : base(account) { }

        /// <summary>
        /// Check file path for callback url.
        /// </summary>
        /// <param name="path">File path to check.</param>
        /// <returns>Provided path if it matches the callback url format.</returns>
        public override string BuildCallbackUrl(string path = "")
        {
            if (!Regex.IsMatch(path.ToLower(), "^https?:/.*"))
            {
                throw new Exception("Provide an absolute path to file!");
            }
            return path;
        }
    }
}
