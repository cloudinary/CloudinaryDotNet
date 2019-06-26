using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest
{
    public class JsConfigTest : IntegrationTestBase
    {
        [Test]
        public void TestJsConfig()
        {
            var config = m_cloudinary.GetCloudinaryJsConfig().ToString();
            var expected = String.Join(
                Environment.NewLine,
                new List<string>
                {
                    "<script src=\"/Scripts/jquery.ui.widget.js\"></script>",
                    "<script src=\"/Scripts/jquery.iframe-transport.js\"></script>",
                    "<script src=\"/Scripts/jquery.fileupload.js\"></script>",
                    "<script src=\"/Scripts/jquery.cloudinary.js\"></script>",
                    "<script type='text/javascript'>",
                    "$.cloudinary.config({",
                    "  \"cloud_name\": \"" + m_account.Cloud + "\",",
                    "  \"api_key\": \"" + m_account.ApiKey + "\",",
                    "  \"private_cdn\": false,",
                    "  \"cdn_subdomain\": false",
                    "});",
                    "</script>",
                    ""
                }
            );

            Assert.AreEqual(expected, config);
        }

        [Test]
        public void TestJsConfigFull()
        {
            var config = m_cloudinary.GetCloudinaryJsConfig(true, @"https://raw.github.com/cloudinary/cloudinary_js/master/js").ToString();
            var expected = String.Join(
                Environment.NewLine,
                new List<string>
                {
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.ui.widget.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.iframe-transport.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.fileupload.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.cloudinary.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/canvas-to-blob.min.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.fileupload-image.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.fileupload-process.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.fileupload-validate.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/load-image.min.js\"></script>",
                    "<script type='text/javascript'>",
                    "$.cloudinary.config({",
                    "  \"cloud_name\": \"" + m_account.Cloud + "\",",
                    "  \"api_key\": \"" + m_account.ApiKey + "\",",
                    "  \"private_cdn\": false,",
                    "  \"cdn_subdomain\": false",
                    "});",
                    "</script>",
                    ""
                }
            );

            Assert.AreEqual(expected, config);
        }
    }
}
