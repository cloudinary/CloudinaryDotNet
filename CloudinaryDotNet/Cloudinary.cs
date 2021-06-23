namespace CloudinaryDotNet
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Main class of Cloudinary .NET API.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    public partial class Cloudinary
    {
        /// <summary>
        /// Cloudinary <see cref="Api"/> object.
        /// </summary>
        protected Api m_api;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cloudinary"/> class.
        /// Default parameterless constructor. Assumes that environment variable CLOUDINARY_URL is set.
        /// </summary>
        public Cloudinary()
        {
            m_api = new Api();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cloudinary"/> class with Cloudinary URL.
        /// </summary>
        /// <param name="cloudinaryUrl">Cloudinary URL.</param>
        public Cloudinary(string cloudinaryUrl)
        {
            m_api = new Api(cloudinaryUrl);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cloudinary"/> class with Cloudinary account.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        public Cloudinary(Account account)
        {
            m_api = new Api(account);
        }

        /// <summary>
        /// Gets API object that used by this instance.
        /// </summary>
        public Api Api
        {
            get { return m_api; }
        }

        /// <summary>
        /// Gets java script that configures Cloudinary JS.
        /// </summary>
        /// <param name="directUpload">Whether to reference additional scripts that are necessary for uploading files directly from browser.</param>
        /// <param name="dir">Override location of js files (default: ~/Scripts).</param>
        /// <returns>HTML script tag with Cloudinary JS configuration.</returns>
        public string GetCloudinaryJsConfig(bool directUpload = false, string dir = "")
        {
            if (string.IsNullOrEmpty(dir))
            {
                dir = "/Scripts";
            }

            StringBuilder sb = new StringBuilder(1000);

            AppendScriptLine(sb, dir, "jquery.ui.widget.js");
            AppendScriptLine(sb, dir, "jquery.iframe-transport.js");
            AppendScriptLine(sb, dir, "jquery.fileupload.js");
            AppendScriptLine(sb, dir, "jquery.cloudinary.js");

            if (directUpload)
            {
                AppendScriptLine(sb, dir, "canvas-to-blob.min.js");
                AppendScriptLine(sb, dir, "jquery.fileupload-image.js");
                AppendScriptLine(sb, dir, "jquery.fileupload-process.js");
                AppendScriptLine(sb, dir, "jquery.fileupload-validate.js");
                AppendScriptLine(sb, dir, "load-image.min.js");
            }

            var cloudinaryParams = new JObject(
                new JProperty[]
                {
                    new JProperty("cloud_name", m_api.Account.Cloud),
                    new JProperty("api_key", m_api.Account.ApiKey),
                    new JProperty("private_cdn", m_api.UsePrivateCdn),
                    new JProperty("cdn_subdomain", m_api.CSubDomain),
                });

            if (!string.IsNullOrEmpty(m_api.PrivateCdn))
            {
                cloudinaryParams.Add("secure_distribution", m_api.PrivateCdn);
            }

            sb.AppendLine("<script type='text/javascript'>");
            sb.Append("$.cloudinary.config(");
            sb.Append(cloudinaryParams.ToString());
            sb.AppendLine(");");
            sb.AppendLine("</script>");

            return sb.ToString();
        }

        private static void AppendScriptLine(StringBuilder sb, string dir, string script)
        {
            sb.Append("<script src=\"");
            sb.Append(dir);

            if (!dir.EndsWith("/", StringComparison.Ordinal) && !dir.EndsWith("\\", StringComparison.Ordinal))
            {
                sb.Append('/');
            }

            sb.Append(script);

            sb.AppendLine("\"></script>");
        }

        /// <summary>
        /// Get default API URL with version.
        /// </summary>
        /// <returns>URL of the API.</returns>
        private Url GetApiUrlV()
        {
            return m_api.ApiUrlV;
        }
    }
}
