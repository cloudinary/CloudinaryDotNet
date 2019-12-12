namespace CloudinaryDotNet
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// Technological layer to work with cloudinary API.
    /// </summary>
    public class Api : ApiShared
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Api"/> class.
        /// Default parameterless constructor. Assumes that environment variable CLOUDINARY_URL is set.
        /// </summary>
        public Api()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Api"/> class with cloudinary URL.
        /// </summary>
        /// <param name="cloudinaryUrl">Cloudinary URL.</param>
        public Api(string cloudinaryUrl)
            : base(cloudinaryUrl)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Api"/> class with initial parameters.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        /// <param name="usePrivateCdn">Whether to use private Content Delivery Network.</param>
        /// <param name="privateCdn">Private Content Delivery Network.</param>
        /// <param name="shortenUrl">Whether to use shortened URL when possible.</param>
        /// <param name="cSubDomain">Whether to use sub domain.</param>
        public Api(Account account, bool usePrivateCdn, string privateCdn, bool shortenUrl, bool cSubDomain)
            : base(account, usePrivateCdn, privateCdn, shortenUrl, cSubDomain)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Api"/> class with account.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        public Api(Account account)
            : base(account)
        {
        }

        /// <summary>
        /// Check file path for callback url.
        /// </summary>
        /// <param name="path">File path to check.</param>
        /// <returns>Provided path if it matches the callback url format.</returns>
        public override string BuildCallbackUrl(string path = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                path = "/Content/cloudinary_cors.html";
            }

            if (Regex.IsMatch(path.ToLower(), "^https?:/.*"))
            {
                return path;
            }
            else
            {
                if (HttpContext.Current != null)
                {
                    return new Uri(HttpContext.Current.Request.Url, path).ToString();
                }
                else
                {
                    throw new ArgumentException("Http context is not set. Either use this method in the right context or provide an absolute path to file!");
                }
            }
        }
    }
}
