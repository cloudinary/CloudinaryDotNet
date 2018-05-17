using System;
using System.Collections.Generic;
using System.Text;

namespace CloudinaryDotNet.HtmlTags
{
    class ImageTag : HtmlTag
    {
        protected const string CL_BLANK = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";

        public ImageTag(string src) : base("img")
        {
            Attributes.Add("src", src);
        }

        public ImageTag(Url url) : this(url.BuildUrl())
        {
            AdditionalOptions.Add("url", url);
        }

        public override string ToString()
        {
            if (!AdditionalOptions.ContainsKey("url"))
            {
                return base.ToString();
            }

            // Here we use information from Transformation of the Url
            var url = AdditionalOptions["url"] as Url;
            var transformation = url.Transformation;

            if (!string.IsNullOrEmpty(transformation.HtmlWidth))
            {
                Attributes.Add("width", transformation.HtmlWidth);
            }

            if (!string.IsNullOrEmpty(transformation.HtmlHeight))
            {
                Attributes.Add("height", transformation.HtmlHeight);
            }

            if (transformation.HiDpi || transformation.IsResponsive)
            {
                Class(transformation.IsResponsive ? "cld-responsive" : "cld-hidpi");

                Attr("data-src", Attributes.Remove("src"));

                string responsivePlaceholder = null;

                if (AdditionalOptions.ContainsKey("responsive_placeholder"))
                {
                    responsivePlaceholder = AdditionalOptions["responsive_placeholder"] as string;

                    if (responsivePlaceholder == "blank")
                    {
                        responsivePlaceholder = CL_BLANK;
                    }
                }

                if (!String.IsNullOrEmpty(responsivePlaceholder))
                {
                    Attributes["src"] = responsivePlaceholder;
                }

            }

            // Always pop "src" attribute to the top
            if (Attributes.ContainsKey("src"))
            {
                Attributes.Insert(0, "src", Attributes.Remove("src"));
            }

            return base.ToString();
        }
    }
}
