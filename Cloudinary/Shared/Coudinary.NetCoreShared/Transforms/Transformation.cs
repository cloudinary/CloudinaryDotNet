using CloudinaryDotNet.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CloudinaryDotNet
{
    public partial class Transformation : Core.ICloneable
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
            "ki", "keyframe_interval",
            "z", "zoom",
            "ac", "audio_codec",
            "br", "bit_rate",
            "af", "audio_frequency",
            "ar", "aspect_ratio",
            "vs", "video_sampling",
            "sp", "streaming_profile"
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
        public static Transformation ResponsiveWidthTransform {
            get {
                if (m_responsiveWidthTransform == null)
                    return DEFAULT_RESPONSIVE_WIDTH_TRANSFORM;
                else
                    return m_responsiveWidthTransform;
            }
            set {
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
        public Transformation(List<Transformation> transforms) {
            if (transforms != null)
                m_nestedTransforms = transforms;
        }

        public Transformation(params string[] transformParams) {
            foreach (var pair in transformParams) {
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
        public Transformation(Dictionary<string, object> transformParams) {
            foreach (var key in transformParams.Keys) {
                m_transformParams.Add(key, transformParams[key]);
            }
        }

        /// <summary>
        /// Creates transformation object from results of <seealso cref="Actions.GetTransformResult"/>.
        /// </summary>
        /// <param name="dictionary">One can use <seealso cref="Actions.GetTransformResult.Info"/> array.</param>
        public Transformation(Dictionary<string, object>[] dictionary) {
            for (int i = 0; i < dictionary.Length; i++) {
                if (i == dictionary.Length - 1) {
                    m_transformParams = dictionary[i];
                }
                else {
                    m_nestedTransforms.Add(new Transformation(dictionary[i]));
                }
            }
        }

        public Dictionary<string, object> Params {
            get { return m_transformParams; }
        }

        public List<Transformation> NestedTransforms {
            get { return m_nestedTransforms; }
        }

        public bool HiDpi { get; private set; }
        public bool IsResponsive { get; private set; }

        public Transformation Chain() {
            Transformation nested = this.Clone();
            nested.m_nestedTransforms = null;
            m_nestedTransforms.Add(nested);
            m_transformParams = new Dictionary<string, object>();
            Transformation transform = new Transformation(m_nestedTransforms);
            return transform;
        }

        public Transformation Add(string key, object value) {
            if (m_transformParams.ContainsKey(key))
                m_transformParams[key] = value;
            else
                m_transformParams.Add(key, value);

            return this;
        }

        public virtual string Generate()
        {
            List<string> parts = new List<string>(m_nestedTransforms.Select(t => t.GenerateThis()).ToList());

            var thisTransform = GenerateThis();
            if (!string.IsNullOrEmpty(thisTransform))
                parts.Add(thisTransform);

            return string.Join("/", parts.ToArray());
        }

        public string GenerateThis() {
            string size = GetString(m_transformParams, "size");
            if (size != null) {
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

            bool hasLayer = !string.IsNullOrEmpty(GetString(m_transformParams, "overlay")) ||
                !string.IsNullOrEmpty(GetString(m_transformParams, "underlay"));

            var crop = GetString(m_transformParams, "crop");
            var angle = string.Join(".", GetStringArray(m_transformParams, "angle"));

            bool isResponsive = false;
            if (!bool.TryParse(GetString(m_transformParams, "responsive_width"), out isResponsive))
                isResponsive = DefaultIsResponsive;

            bool no_html_sizes = hasLayer || !String.IsNullOrEmpty(angle) || crop == "fit" || crop == "limit";
            if (width != null && (width.IndexOf("auto") != -1 || Single.Parse(width, CultureInfo.InvariantCulture) < 1 || no_html_sizes || isResponsive))
                m_htmlWidth = null;
            if (height != null && (Single.Parse(height, CultureInfo.InvariantCulture) < 1 || no_html_sizes || isResponsive))
                m_htmlHeight = null;

            string background = GetString(m_transformParams, "background");
            if (background != null) {
                background = background.Replace("^#", "rgb:");
            }

            string color = GetString(m_transformParams, "color");
            if (color != null) {
                color = color.Replace("^#", "rgb:");
            }

            List<string> transformations = GetStringArray(m_transformParams, "transformation").ToList();

            string namedTransformation = string.Join(".", transformations.ToArray());

            transformations = new List<string>();

            string flags = string.Join(".", GetStringArray(m_transformParams, "flags"));

            object obj = null;
            string startOffset = null;
            string endOffset = null;

            if (m_transformParams.TryGetValue("start_offset", out obj))
                startOffset = NormRangeValue(obj);

            if (m_transformParams.TryGetValue("end_offset", out obj))
                endOffset = NormRangeValue(obj);

            if (m_transformParams.TryGetValue("offset", out obj)) {
                var offset = SplitRange(m_transformParams["offset"]);
                if (offset != null && offset.Length == 2) {
                    startOffset = NormRangeValue(offset[0]);
                    endOffset = NormRangeValue(offset[1]);
                }
            }

            var parameters = new SortedDictionary<string, string>();
            parameters.Add("w", width);
            parameters.Add("h", height);
            parameters.Add("t", namedTransformation);
            parameters.Add("c", crop);
            parameters.Add("b", background);
            parameters.Add("co", color);
            parameters.Add("a", angle);
            parameters.Add("fl", flags);
            parameters.Add("so", startOffset);
            parameters.Add("eo", endOffset);

            if (m_transformParams.TryGetValue("duration", out obj))
                parameters.Add("du", NormRangeValue(obj));

            ProcessVideoCodec(parameters, m_transformParams);

            for (int i = 0; i < SimpleParams.Length; i += 2) {
                if (m_transformParams.TryGetValue(SimpleParams[i + 1], out obj))
                    parameters.Add(SimpleParams[i], ToString(obj));
            }

            object dpr = null;
            if (!m_transformParams.TryGetValue("dpr", out dpr))
                dpr = DefaultDpr;

            var dprStr = ToString(dpr);

            if (dprStr != null) {
                if (dprStr.ToLower() == "auto")
                    HiDpi = true;

                parameters.Add("dpr", dprStr);
            }

            if (width == "auto" || isResponsive)
                IsResponsive = true;

            List<string> components = new List<string>();
            foreach (var param in parameters) {
                if (!string.IsNullOrEmpty(param.Value))
                    components.Add(string.Format("{0}_{1}", param.Key, param.Value));
            }

            string rawTransformation = GetString(m_transformParams, "raw_transformation");
            if (rawTransformation != null) {
                components.Add(rawTransformation);
            }

            string ifValue = GetString(m_transformParams, "if");
            if (!string.IsNullOrEmpty(ifValue)) {
                components.Insert(0, string.Format("if_{0}", new Condition(ifValue).ToString()));
            }

            if (components.Count > 0) {
                transformations.Add(string.Join(",", components.ToArray()));
            }

            if (isResponsive)
                transformations.Add(ResponsiveWidthTransform.Generate());

            return string.Join("/", transformations.ToArray());
        }

        public string HtmlWidth {
            get { return m_htmlWidth; }
        }

        public string HtmlHeight {
            get { return m_htmlHeight; }
        }

        private string[] GetStringArray(Dictionary<string, object> options, string key) {
            if (!options.ContainsKey(key)) return new string[0];

            object value = options[key];

            if (value is string[]) {
                return (string[])value;
            }
            else {
                List<string> list = new List<string>();
                list.Add(ToString(value));
                return list.ToArray();
            }
        }

        private string GetString(Dictionary<string, object> options, string key) {
            if (options.ContainsKey(key))
                return ToString(options[key]);
            else
                return null;
        }

        private static string ToString(object obj) {
            if (obj == null) return null;

            if (obj is String) return obj.ToString();

            if (obj is Single || obj is Double)
                return String.Format(CultureInfo.InvariantCulture, "{0:0.0#}", obj);

            return String.Format(CultureInfo.InvariantCulture, "{0}", obj);
        }

        public override string ToString() {
            return Generate();
        }

        #region ICloneable

        public Transformation Clone() {
            Transformation t = (Transformation)this.MemberwiseClone();

            t.m_transformParams = new Dictionary<string, object>();

            foreach (var key in m_transformParams.Keys) {
                var value = m_transformParams[key];

                if (value is Array) {
                    t.Add(key, ((Array)value).Clone());
                }
                else if (value is String || value is ValueType) {
                    t.Add(key, value);
                }
                else if (value is Dictionary<string, string>) {
                    t.Add(key, new Dictionary<string, string>((Dictionary<string, string>)value));
                }
                else {
                    throw new Exception(String.Format("Couldn't clone parameter '{0}'!", key));
                }
            }

            if (m_nestedTransforms != null) {
                t.m_nestedTransforms = new List<Transformation>();
                foreach (var nestedTransform in m_nestedTransforms) {
                    t.m_nestedTransforms.Add(nestedTransform.Clone());
                }
            }

            return t;
        }

        object Core.ICloneable.Clone() {
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
