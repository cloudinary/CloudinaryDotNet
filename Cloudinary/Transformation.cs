using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    public class Transformation : ICloneable
    {
        static readonly string[] SimpleParams = new string[] {
            "x", "x",
            "y", "y",
            "r", "radius",
            "d", "default_image", 
            "g", "gravity",
            "cs", "color_space",
            "p", "prefix",
            "l", "overlay",
            "u", "underlay",
            "f", "fetch_format",
            "dn", "density",
            "pg", "page",
            "dl", "delay",
            "e", "effect",
            "bo", "border",
            "q", "quality",
            "o", "opacity",
            "z", "zoom"
        };

        /// <summary>
        /// Default Device Pixel Ratio (float, integer and "auto" values are allowed").
        /// </summary>
        public static object DefaultDpr { get; set; }

        /// <summary>
        /// Whether to enable automatic adaptation of website images by default.
        /// See http://cloudinary.com/blog/how_to_automatically_adapt_website_images_to_retina_and_hidpi_devices for further info.
        /// </summary>
        public static bool DefaultIsResponsive { get; set; }

        /// <summary>
        /// Common responsive width transformation.
        /// </summary>
        public static Transformation ResponsiveWidthTransform
        {
            get
            {
                if (m_responsiveWidthTransform == null)
                    return DEFAULT_RESPONSIVE_WIDTH_TRANSFORM;
                else
                    return m_responsiveWidthTransform;
            }
            set
            {
                m_responsiveWidthTransform = value;
            }
        }
        private static readonly Transformation DEFAULT_RESPONSIVE_WIDTH_TRANSFORM = new Transformation().Width("auto").Crop("limit");
        private static Transformation m_responsiveWidthTransform = null;

        protected Dictionary<string, object> m_transformParams = new Dictionary<string, object>();
        protected List<Transformation> m_nestedTransforms = new List<Transformation>();

        protected string m_htmlWidth = null;
        protected string m_htmlHeight = null;

        /// <summary>
        /// Creates empty transformation object.
        /// </summary>
        public Transformation() { }

        /// <summary>
        /// Creates transformation object chained with other transformations.
        /// </summary>
        public Transformation(List<Transformation> transforms)
        {
            if (transforms != null)
                m_nestedTransforms = transforms;
        }

        public Transformation(params string[] transformParams)
        {
            foreach (var pair in transformParams)
            {
                string[] splittedPair = pair.Split('=');
                if (splittedPair.Length != 2)
                    throw new ArgumentException(String.Format("Couldn't parse '{0}'!", pair));

                Add(splittedPair[0], splittedPair[1]);
            }
        }

        /// <summary>
        /// Creates transformation object from single result of  <seealso cref="Actions.GetTransformResult"/>.
        /// </summary>
        /// <param name="transformParams">One can use an element of <seealso cref="Actions.GetTransformResult.Info"/> array.</param>
        public Transformation(Dictionary<string, object> transformParams)
        {
            foreach (var key in transformParams.Keys)
            {
                m_transformParams.Add(key, transformParams[key]);
            }
        }

        /// <summary>
        /// Creates transformation object from results of <seealso cref="Actions.GetTransformResult"/>.
        /// </summary>
        /// <param name="dictionary">One can use <seealso cref="Actions.GetTransformResult.Info"/> array.</param>
        public Transformation(Dictionary<string, object>[] dictionary)
        {
            for (int i = 0; i < dictionary.Length; i++)
            {
                if (i == dictionary.Length - 1)
                {
                    m_transformParams = dictionary[i];
                }
                else
                {
                    m_nestedTransforms.Add(new Transformation(dictionary[i]));
                }
            }
        }

        public Dictionary<string, object> Params
        {
            get { return m_transformParams; }
        }

        public List<Transformation> NestedTransforms
        {
            get { return m_nestedTransforms; }
        }

        public bool HiDpi { get; private set; }
        public bool IsResponsive { get; private set; }

        public Transformation Chain()
        {
            Transformation nested = this.Clone();
            nested.m_nestedTransforms = null;
            m_nestedTransforms.Add(nested);
            Transformation transform = new Transformation(m_nestedTransforms);
            return transform;
        }

        /// <summary>
        /// The required width of a transformed image or an overlay. Can be specified separately or together with the height value. Can also be a decimal value (e.g., 0.2) for percentage based resizing.
        /// </summary>
        public Transformation Width(object value) { return Add("width", value); }
        
        /// <summary>
        /// The required height of a transformed image or an overlay. Can be specified separately or together with the width value. Can also be a decimal value (e.g., 0.2) for percentage based resizing.
        /// </summary>
        public Transformation Height(object value) { return Add("height", value); }
        public Transformation SetHtmlWidth(object value) { m_htmlWidth = value.ToString(); return this; }
        public Transformation SetHtmlHeight(object value) { m_htmlHeight = value.ToString(); return this; }
        public Transformation Named(params string[] value) { return Add("transformation", value); }
        
        /// <summary>
        /// A crop mode that determines how to transform the image for fitting into the desired width & height dimensions.
        /// </summary>
        public Transformation Crop(string value) { return Add("crop", value); }
        public Transformation Background(string value) { return Add("background", Regex.Replace(value, "^#", "rgb:")); }
        
        /// <summary>
        /// Defines the color to use for various effects.
        /// </summary>
        public Transformation Color(string value) { return Add("color", Regex.Replace(value, "^#", "rgb:")); }
        
        /// <summary>
        /// Apply a filter or an effect on an image. The value includes the name of the effect and an additional parameter that controls the behavior of the specific effect.
        /// </summary>
        public Transformation Effect(string value) { return Add("effect", value); }

        /// <summary>
        /// Apply a filter or an effect on an image.
        /// </summary>
        /// <param name="effect">The name of the effect.</param>
        /// <param name="param">An additional parameter that controls the behavior of the specific effect.</param>
        /// <returns></returns>
        public Transformation Effect(string effect, Object param) { return Add("effect", effect + ":" + param); }
        public Transformation Angle(int value) { return Add("angle", value); }
        public Transformation Angle(params string[] value) { return Add("angle", value); }
        
        /// <summary>
        /// Add a solid border around the image. The value has a CSS-like format: width_style_color.
        /// </summary>
        public Transformation Border(string value) { return Add("border", value); }
        
        /// <summary>
        /// Add a solid border around the image.
        /// </summary>
        public Transformation Border(int width, string color) { return Add("border", "" + width + "px_solid_" + Regex.Replace(color, "^#", "rgb:")); }
        
        /// <summary>
        /// Horizontal position for custom-coordinates based cropping and overlay placement.
        /// </summary>
        public Transformation X(object value) { return Add("x", value); }
        
        /// <summary>
        /// Vertical position for custom-coordinates based cropping and overlay placement.
        /// </summary>
        public Transformation Y(object value) { return Add("y", value); }
        public Transformation Radius(object value) { return Add("radius", value); }
        
        /// <summary>
        /// Control the JPG compression quality. 1 is the lowest quality and 100 is the highest. The default is the original image's quality or 90% if not available. Reducing quality generates JPG images much smaller in file size.
        /// </summary>
        public Transformation Quality(object value) { return Add("quality", value); }
        public Transformation DefaultImage(string value) { return Add("default_image", value); }
        
        /// <summary>
        /// Decides which part of the image to keep while 'crop', 'pad' and 'fill' crop modes are used. For overlays, this decides where to place the overlay.
        /// </summary>
        public Transformation Gravity(string value) { return Add("gravity", value); }
        public Transformation ColorSpace(string value) { return Add("color_space", value); }
        public Transformation Prefix(string value) { return Add("prefix", value); }
        
        /// <summary>
        /// Manipulate image opacity in order to make the image semi-transparent.
        /// </summary>
        public Transformation Opacity(int value) { return Add("opacity", value); }
        public Transformation Overlay(string value) { return Add("overlay", value); }
        public Transformation Underlay(string value) { return Add("underlay", value); }
        public Transformation FetchFormat(string value) { return Add("fetch_format", value); }
        
        /// <summary>
        /// Control the density to use while converting a PDF document to images. (range: 50-300, default: 150).
        /// </summary>
        public Transformation Density(object value) { return Add("density", value); }

        /// <summary>
        /// Given a multi-page PDF document, generate an image of a single page using the given index.
        /// </summary>
        public Transformation Page(object value) { return Add("page", value); }
        public Transformation Delay(object value) { return Add("delay", value); }
        public Transformation RawTransformation(string value) { return Add("raw_transformation", value); }
        public Transformation Flags(params string[] value) { return Add("flags", value); }

        /// <summary>
        /// How much zoom should be applying when detecting faces for crop, thumb or for overlays. (e.g. 0.5 will cause zoom out of x2 on both axes).
        /// </summary>
        public Transformation Zoom(object value) { return Add("zoom", value); }

        /// <summary>
        /// Sets Device Pixel Ratio  (float, integer and "auto" values are allowed").
        /// See http://cloudinary.com/blog/how_to_automatically_adapt_website_images_to_retina_and_hidpi_devices for further info.
        /// </summary>
        public Transformation Dpr(object value) { return Add("dpr", value); }

        /// <summary>
        /// Whether to enable automatic adaptation of website images.
        /// See http://cloudinary.com/blog/how_to_automatically_adapt_website_images_to_retina_and_hidpi_devices for further info.
        /// </summary>
        public Transformation ResponsiveWidth(bool value) { return Add("responsive_width", value); }

        public Transformation Add(string key, object value)
        {
            if (m_transformParams.ContainsKey(key))
                m_transformParams[key] = value;
            else
                m_transformParams.Add(key, value);

            return this;
        }

        public virtual string Generate()
        {
            List<string> parts =
                m_nestedTransforms.Select(t => t.GenerateThis()).ToList();

            parts.Add(GenerateThis());

            return String.Join("/", parts.ToArray());
        }

        public string GenerateThis()
        {
            string size = GetString(m_transformParams, "size");
            if (size != null)
            {
                string[] sizeComponents = size.Split("x".ToArray());
                m_transformParams.Add("width", sizeComponents[0]);
                m_transformParams.Add("height", sizeComponents[1]);
            }

            string width = GetString(m_transformParams, "width");
            string height = GetString(m_transformParams, "height");

            if (m_htmlWidth == null)
                m_htmlWidth = width;

            if (m_htmlHeight == null)
                m_htmlHeight = height;

            bool hasLayer = !String.IsNullOrEmpty(GetString(m_transformParams, "overlay")) ||
                !String.IsNullOrEmpty(GetString(m_transformParams, "underlay"));

            var crop = GetString(m_transformParams, "crop");
            var angle = String.Join(".", GetStringArray(m_transformParams, "angle"));

            bool isResponsive = false;
            if (!Boolean.TryParse(GetString(m_transformParams, "responsive_width"), out isResponsive))
                isResponsive = DefaultIsResponsive;

            bool no_html_sizes = hasLayer || !String.IsNullOrEmpty(angle) || crop == "fit" || crop == "limit";
            if (width != null && (width == "auto" || Single.Parse(width, CultureInfo.InvariantCulture) < 1 || no_html_sizes || isResponsive)) m_htmlWidth = null;
            if (height != null && (Single.Parse(height, CultureInfo.InvariantCulture) < 1 || no_html_sizes || isResponsive))
                m_htmlHeight = null;

            string background = GetString(m_transformParams, "background");
            if (background != null)
            {
                background = background.Replace("^#", "rgb:");
            }

            string color = GetString(m_transformParams, "color");
            if (color != null)
            {
                color = color.Replace("^#", "rgb:");
            }

            List<string> transformations = GetStringArray(m_transformParams, "transformation").ToList();

            string namedTransformation = String.Join(".", transformations.ToArray());

            transformations = new List<string>();

            string flags = String.Join(".", GetStringArray(m_transformParams, "flags"));

            SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();
            parameters.Add("w", width);
            parameters.Add("h", height);
            parameters.Add("t", namedTransformation);
            parameters.Add("c", crop);
            parameters.Add("b", background);
            parameters.Add("co", color);
            parameters.Add("a", angle);
            parameters.Add("fl", flags);

            for (int i = 0; i < SimpleParams.Length; i += 2)
            {
                if (m_transformParams.ContainsKey(SimpleParams[i + 1]))
                    parameters.Add(SimpleParams[i], GetString(m_transformParams, SimpleParams[i + 1]));
            }

            var dpr = GetString(m_transformParams, "dpr") ?? ToString(DefaultDpr);
            if (dpr != null)
            {
                if (dpr.ToLower() == "auto")
                    HiDpi = true;

                parameters.Add("dpr", dpr);
            }

            if (width == "auto" || isResponsive)
                IsResponsive = true;

            List<string> components = new List<string>();
            foreach (var param in parameters)
            {
                if (!String.IsNullOrEmpty(param.Value))
                    components.Add(String.Format("{0}_{1}", param.Key, param.Value));
            }

            string rawTransformation = GetString(m_transformParams, "raw_transformation");
            if (rawTransformation != null)
            {
                components.Add(rawTransformation);
            }

            if (components.Count > 0)
            {
                transformations.Add(String.Join(",", components.ToArray()));
            }

            if (isResponsive)
                transformations.Add(ResponsiveWidthTransform.Generate());

            return String.Join("/", transformations.ToArray());
        }

        public string HtmlWidth
        {
            get { return m_htmlWidth; }
        }

        public string HtmlHeight
        {
            get { return m_htmlHeight; }
        }

        private string[] GetStringArray(Dictionary<string, object> options, string key)
        {
            if (!options.ContainsKey(key)) return new string[0];

            object value = options[key];

            if (value is string[])
            {
                return (string[])value;
            }
            else
            {
                List<string> list = new List<string>();
                list.Add(ToString(value));
                return list.ToArray();
            }
        }

        private string GetString(Dictionary<string, object> options, string key)
        {
            if (options.ContainsKey(key))
                return ToString(options[key]);
            else
                return null;
        }

        private static string ToString(object obj)
        {
            if (obj == null) return null;

            if (obj is String) return obj.ToString();

            if (obj is Single || obj is Double)
                return String.Format(CultureInfo.InvariantCulture, "{0:0.0#}", obj);

            return String.Format(CultureInfo.InvariantCulture, "{0}", obj);
        }

        #region ICloneable

        public Transformation Clone()
        {
            Transformation t = (Transformation)this.MemberwiseClone();

            t.m_transformParams = new Dictionary<string, object>();

            foreach (var key in m_transformParams.Keys)
            {
                if (m_transformParams[key] is Array)
                {
                    t.Add(key, ((Array)m_transformParams[key]).Clone());
                }
                else if (m_transformParams[key] is String || m_transformParams[key] is ValueType)
                {
                    t.Add(key, m_transformParams[key]);
                }
                else
                {
                    throw new ApplicationException(String.Format("Couldn't clone parameter '{0}'!", key));
                }
            }

            if (m_nestedTransforms != null)
            {
                t.m_nestedTransforms = new List<Transformation>();
                foreach (var nestedTransform in m_nestedTransforms)
                {
                    t.m_nestedTransforms.Add(nestedTransform.Clone());
                }
            }

            return t;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }

    public class EagerTransformation : Transformation
    {
        public EagerTransformation(params Transformation[] transforms)
            : base(transforms.ToList()) { }

        public EagerTransformation(List<Transformation> transforms)
            : base(transforms) { }

        public EagerTransformation() : base() { }

        public EagerTransformation SetFormat(string format)
        {
            Format = format;
            return this;
        }

        public string Format { get; set; }

        public override string Generate()
        {
            string s = base.Generate();

            if (!String.IsNullOrEmpty(Format))
                s += "/" + Format;

            return s;
        }
    }
}
