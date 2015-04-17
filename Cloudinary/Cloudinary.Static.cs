using System;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Provides quick syntax for ASP.NET views.
    /// User should call <see cref="Initialize"/> before start using in views.
    /// </summary>
    public static class CL
    {
        static Cloudinary m_cloudinary;

        /// <summary>
        /// Initializes using the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public static void Initialize(Account account)
        {
            m_cloudinary = new Cloudinary(account);
        }

        /// <summary>
        /// Creates am empty transformation.
        /// </summary>
        public static Transformation T { get { return new Transformation(); } }

        /// <summary>
        /// Gets the URL builder for uploaded image.
        /// </summary>
        public static Url UrlImgUp { get { return m_cloudinary.Api.UrlImgUp; } }

        /// <summary>
        /// Gets the URL builder for uploaded video.
        /// </summary>
        public static Url UrlVideoUp { get { return m_cloudinary.Api.UrlVideoUp; } }

        /// <summary>
        /// Gets the instance of <see cref="Cloudinary"/> object.
        /// </summary>
        public static Cloudinary C { get { return m_cloudinary; } }
    }
}
