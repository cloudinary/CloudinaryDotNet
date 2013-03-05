using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    public class Transformation
    {
        protected Dictionary<string, object> m_transformParams = new Dictionary<string, object>();
        protected List<Transformation> m_nestedTransforms = new List<Transformation>();

        protected string m_htmlWidth;
        protected string m_htmlHeight;

        public Transformation() { }

        public Transformation(List<Transformation> transforms)
        {
            if (transforms != null)
                m_nestedTransforms = transforms;
        }

        public Transformation(Dictionary<string, string> transformParams)
        {
            foreach (var key in transformParams.Keys)
            {
                m_transformParams.Add(key, transformParams[key]);
            }
        }

        public Dictionary<string, object> Params
        {
            get { return m_transformParams; }
            private set { m_transformParams = value; }
        }

        public Transformation Chain()
        {
            m_nestedTransforms.Add(this);
            Transformation transform = new Transformation(m_nestedTransforms);
            return transform;
        }

        public Transformation Width(object value) { return Add("width", value); }
        public Transformation Height(object value) { return Add("height", value); }
        public Transformation Named(params string[] value) { return Add("transformation", value); }
        public Transformation Crop(string value) { return Add("crop", value); }
        public Transformation Background(string value) { return Add("background", Regex.Replace(value, "^#", "rgb:")); }
        public Transformation Effect(string value) { return Add("effect", value); }
        public Transformation Effect(string effect, Object param) { return Add("effect", effect + ":" + param); }
        public Transformation Angle(int value) { return Add("angle", value); }
        public Transformation Angle(params string[] value) { return Add("angle", value); }
        public Transformation Border(string value) { return Add("border", value); }
        public Transformation Border(int width, string color) { return Add("border", "" + width + "px_solid_" + Regex.Replace(color, "^#", "rgb:")); }
        public Transformation X(object value) { return Add("x", value); }
        public Transformation Y(object value) { return Add("y", value); }
        public Transformation Radius(object value) { return Add("radius", value); }
        public Transformation Quality(object value) { return Add("quality", value); }
        public Transformation DefaultImage(string value) { return Add("default_image", value); }
        public Transformation Gravity(string value) { return Add("gravity", value); }
        public Transformation ColorSpace(string value) { return Add("color_space", value); }
        public Transformation Prefix(string value) { return Add("prefix", value); }
        public Transformation Overlay(string value) { return Add("overlay", value); }
        public Transformation Underlay(string value) { return Add("underlay", value); }
        public Transformation FetchFormat(string value) { return Add("fetch_format", value); }
        public Transformation Density(object value) { return Add("density", value); }
        public Transformation Page(object value) { return Add("page", value); }
        public Transformation Delay(object value) { return Add("delay", value); }
        public Transformation RawTransformation(string value) { return Add("raw_transformation", value); }
        public Transformation Flags(params string[] value) { return Add("flags", value); }

        public Transformation Add(string key, object value)
        {
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

            string width = m_htmlWidth = GetString(m_transformParams, "width");
            string height = m_htmlHeight = GetString(m_transformParams, "height");

            bool hasLayer = !String.IsNullOrEmpty(GetString(m_transformParams, "overlay")) ||
                !String.IsNullOrEmpty(GetString(m_transformParams, "underlay"));

            string crop = GetString(m_transformParams, "crop");
            String angle = String.Join(".", GetStringArray(m_transformParams, "angle"));

            bool no_html_sizes = hasLayer || !String.IsNullOrEmpty(angle) || crop == "fit" || crop == "limit";
            if (width != null && (Single.Parse(width, CultureInfo.InvariantCulture) < 1 || no_html_sizes))
            {
                this.m_htmlWidth = null;
            }
            if (height != null && (Single.Parse(height, CultureInfo.InvariantCulture) < 1 || no_html_sizes))
            {
                this.m_htmlHeight = null;
            }

            string background = GetString(m_transformParams, "background");
            if (background != null)
            {
                background = background.Replace("^#", "rgb:");
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
            parameters.Add("a", angle);
            parameters.Add("fl", flags);

            string[] simpleParams = new string[]{
                "x", "x", "y", "y", "r", "radius", "d", "default_image", "g", "gravity", "cs", "color_space",
                "p", "prefix", "l", "overlay", "u", "underlay", "f", "fetch_format", "dn", "density",
                "pg", "page", "dl", "delay", "e", "effect", "bo", "border", "q", "quality"
            };

            for (int i = 0; i < simpleParams.Length; i += 2)
            {
                if (m_transformParams.ContainsKey(simpleParams[i + 1]))
                    parameters.Add(simpleParams[i], GetString(m_transformParams, simpleParams[i + 1]));
            }

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
                list.Add(value.ToString());
                return list.ToArray();
            }
        }

        private string GetString(Dictionary<string, object> options, string key)
        {
            if (options.ContainsKey(key))
            {
                if (options[key] is Single || options[key] is Double)
                {
                    return String.Format(CultureInfo.InvariantCulture, "{0:0.##}", options[key]);
                }
                else
                {
                    return String.Format(CultureInfo.InvariantCulture, "{0}", options[key]);
                }
            }
            else
                return null;
        }
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
