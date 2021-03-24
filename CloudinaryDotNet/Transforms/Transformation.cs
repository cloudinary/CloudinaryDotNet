namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The building blocks for assets transformation.
    /// </summary>
    public partial class Transformation : Core.ICloneable
    {
        /// <summary>
        /// A dictionary of transformation parameters.
        /// </summary>
        protected Dictionary<string, object> m_transformParams = new Dictionary<string, object>();

        /// <summary>
        /// A list of nested transformations.
        /// </summary>
        protected List<Transformation> m_nestedTransforms = new List<Transformation>();

        /// <summary>
        /// HTML width attribute.
        /// </summary>
        protected string m_htmlWidth;

        /// <summary>
        /// HTML height attribute.
        /// </summary>
        protected string m_htmlHeight;

        private const string VARIABLESPARAMKEY = "variables";

        private static readonly string[] SimpleParams = new string[]
        {
            "ac", "audio_codec",
            "af", "audio_frequency",
            "bo", "border",
            "br", "bit_rate",
            "cs", "color_space",
            "d", "default_image",
            "dl", "delay",
            "dn", "density",
            "f", "fetch_format",
            "fps", "fps",
            "g", "gravity",
            "ki", "keyframe_interval",
            "l", "overlay",
            "p", "prefix",
            "pg", "page",
            "u", "underlay",
            "vs", "video_sampling",
            "sp", "streaming_profile",
        };

        private static readonly Transformation DefaultResponsiveWidthTransform
            = new Transformation().Width("auto").Crop("limit");

        private static Transformation m_responsiveWidthTransform;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transformation"/> class.
        /// Creates empty transformation object.
        /// </summary>
        public Transformation()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transformation"/> class.
        /// Creates transformation object chained with other transformations.
        /// </summary>
        /// <param name="transforms">List of transformations to chain with.</param>
        public Transformation(List<Transformation> transforms)
        {
            if (transforms != null)
            {
                m_nestedTransforms = transforms;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transformation"/> class.
        /// Creates transformation object initialized with array of transformation parameters.
        /// </summary>
        /// <param name="transformParams">List of transformation parameters represented as pairs 'name=value'.</param>
        public Transformation(params string[] transformParams)
        {
            foreach (var pair in transformParams)
            {
                string[] splittedPair = pair.Split('=');
                if (splittedPair.Length != 2)
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Couldn't parse '{0}'!", pair));
                }

                Add(splittedPair[0], splittedPair[1]);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transformation"/> class.
        /// Creates transformation object from single result of  <seealso cref="Actions.GetTransformResult"/>.
        /// </summary>
        /// <param name="transformParams">
        /// One can use an element of <seealso cref="Actions.GetTransformResult.Info"/> array.
        /// </param>
        public Transformation(Dictionary<string, object> transformParams)
        {
            foreach (var key in transformParams.Keys)
            {
                m_transformParams.Add(key, transformParams[key]);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transformation"/> class.
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

        /// <summary>
        /// Gets or sets default Device Pixel Ratio (float, integer and "auto" values are allowed").
        /// </summary>
        public static object DefaultDpr { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable automatic adaptation of website images by default.
        /// See http://cloudinary.com/blog/how_to_automatically_adapt_website_images_to_retina_and_hidpi_devices for
        /// further info.
        /// </summary>
        public static bool DefaultIsResponsive { get; set; }

        /// <summary>
        /// Gets or sets common responsive width transformation.
        /// </summary>
        public static Transformation ResponsiveWidthTransform
        {
            get
            {
                if (m_responsiveWidthTransform == null)
                {
                    return DefaultResponsiveWidthTransform;
                }
                else
                {
                    return m_responsiveWidthTransform;
                }
            }

            set
            {
                m_responsiveWidthTransform = value;
            }
        }

        /// <summary>
        /// Gets the transformation parameters dictionary.
        /// </summary>
        public Dictionary<string, object> Params
        {
            get { return m_transformParams; }
        }

        /// <summary>
        /// Gets list of nested transformations.
        /// </summary>
        public List<Transformation> NestedTransforms
        {
            get { return m_nestedTransforms; }
        }

        /// <summary>
        /// Gets a value indicating whether to support a HiDPI devices.
        /// </summary>
        public bool HiDpi { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to support a responsive layout.
        /// </summary>
        public bool IsResponsive { get; private set; }

        /// <summary>
        /// Gets the HTML width parameter.
        /// </summary>
        public string HtmlWidth
        {
            get { return m_htmlWidth; }
        }

        /// <summary>
        /// Gets the HTML height parameter.
        /// </summary>
        public string HtmlHeight
        {
            get { return m_htmlHeight; }
        }

        /// <summary>
        /// Chain transformation.
        /// </summary>
        /// <returns>A transformation for chaining.</returns>
        public Transformation Chain()
        {
            Transformation nested = this.Clone();
            nested.m_nestedTransforms = null;
            m_nestedTransforms.Add(nested);
            m_transformParams = new Dictionary<string, object>();
            Transformation transform = new Transformation(m_nestedTransforms);
            return transform;
        }

        /// <summary>
        /// Define a user defined variable.
        /// </summary>
        /// <param name="name">The name of variable.</param>
        /// <param name="value">The value.</param>
        /// <returns>The transformation with variable added.</returns>
        public Transformation Variable(string name, object value)
        {
            Expression.CheckVariableName(name);
            Add(name, value);
            return this;
        }

        /// <summary>
        /// Define a user defined string variable.
        /// </summary>
        /// <param name="name">The name of variable.</param>
        /// <param name="values">A list of values.</param>
        /// <returns>The transformation with variable added.</returns>
        public Transformation Variable(string name, string[] values)
        {
            return Variable(name, $"!{(values != null ? string.Join(":", values) : string.Empty)}!");
        }

        /// <summary>
        /// Add user defined variables to the transformation.
        /// </summary>
        /// <param name="variables">A list of variables.</param>
        /// <returns>The transformation with variables added.</returns>
        public Transformation Variables(params Expression[] variables)
        {
            Add(VARIABLESPARAMKEY, variables);
            return this;
        }

        /// <summary>
        /// Add custom function to the transformation.
        /// </summary>
        /// <param name="function">The custom function.</param>
        /// <returns>The transformation with custom function added.</returns>
        public Transformation CustomFunction(CustomFunction function)
        {
            Add("custom_function", function);
            return this;
        }

        /// <summary>
        /// Add custom pre-function to the transformation.
        /// </summary>
        /// <param name="function">The custom pre-function.</param>
        /// <returns>The transformation with custom pre-function added.</returns>
        public Transformation CustomPreFunction(CustomFunction function)
        {
            string serialized = ToString(function);
            if (!string.IsNullOrEmpty(serialized))
            {
                Add("custom_pre_function", $"pre:{function}");
            }

            return this;
        }

        /// <summary>
        /// Add transformation parameter.
        /// </summary>
        /// <param name="key">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>The transformation with parameter added.</returns>
        public Transformation Add(string key, object value)
        {
            if (m_transformParams.ContainsKey(key))
            {
                m_transformParams[key] = value;
            }
            else
            {
                m_transformParams.Add(key, value);
            }

            return this;
        }

        /// <summary>
        /// Get this transformation represented as string.
        /// </summary>
        /// <returns>The transformation represented as string.</returns>
        public virtual string Generate()
        {
            List<string> parts = new List<string>(m_nestedTransforms.Select(t => t.GenerateThis()).ToList());

            var thisTransform = GenerateThis();
            if (!string.IsNullOrEmpty(thisTransform))
            {
                parts.Add(thisTransform);
            }

            return string.Join("/", parts.ToArray());
        }

        /// <summary>
        /// Generate transformation string.
        /// </summary>
        /// <returns>Generated transformation.</returns>
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
            {
                m_htmlWidth = width;
            }

            if (m_htmlHeight == null)
            {
                m_htmlHeight = height;
            }

            bool hasLayer = !string.IsNullOrEmpty(GetString(m_transformParams, "overlay")) ||
                !string.IsNullOrEmpty(GetString(m_transformParams, "underlay"));

            var crop = GetString(m_transformParams, "crop");
            var angle = string.Join(".", GetStringArray(m_transformParams, "angle"));

            bool isResponsive;
            if (!bool.TryParse(GetString(m_transformParams, "responsive_width"), out isResponsive))
            {
                isResponsive = DefaultIsResponsive;
            }

            bool noHtmlSizes = hasLayer || !string.IsNullOrEmpty(angle) || crop == "fit" || crop == "limit";

            if (!string.IsNullOrEmpty(width) && (Expression.ValueContainsVariable(width) ||
                                                 width.IndexOf("auto", StringComparison.OrdinalIgnoreCase) != -1 ||
                                                 (float.TryParse(width, out var wResult) && wResult < 1) ||
                                                 noHtmlSizes ||
                                                 isResponsive))
            {
                m_htmlWidth = null;
            }

            if (!string.IsNullOrEmpty(height) && (Expression.ValueContainsVariable(height) ||
                                                  (float.TryParse(height, out var hResult) && hResult < 1) ||
                                                  noHtmlSizes ||
                                                  isResponsive))
            {
                m_htmlHeight = null;
            }

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

            string namedTransformation = string.Join(".", transformations.ToArray());

            transformations = new List<string>();

            string flags = string.Join(".", GetStringArray(m_transformParams, "flags"));

            object obj = null;

            string startOffset = null;
            if (m_transformParams.TryGetValue("start_offset", out obj))
            {
                startOffset = NormAutoRangeValue(obj);
            }

            string endOffset = null;
            if (m_transformParams.TryGetValue("end_offset", out obj))
            {
                endOffset = NormRangeValue(obj);
            }

            if (m_transformParams.TryGetValue("offset", out obj))
            {
                var offset = SplitRange(m_transformParams["offset"]);
                if (offset != null && offset.Length == 2)
                {
                    startOffset = NormAutoRangeValue(offset[0]);
                    endOffset = NormRangeValue(offset[1]);
                }
            }

            string duration = null;
            if (m_transformParams.TryGetValue("duration", out obj))
            {
                duration = NormRangeValue(obj);
            }

            string videoCodec = m_transformParams.TryGetValue("video_codec", out obj) ? ProcessVideoCodec(obj) : null;

            if (!m_transformParams.TryGetValue("dpr", out object dpr))
            {
                dpr = DefaultDpr;
            }

            var dprStr = ToString(dpr);
            if (!string.IsNullOrEmpty(dprStr))
            {
                if (dprStr.ToLowerInvariant() == "auto")
                {
                    HiDpi = true;
                }
            }

            var parameters = new SortedList<string, string>();

            parameters.Add("a", Expression.Normalize(angle));
            parameters.Add("ar", Expression.Normalize(GetString(m_transformParams, "aspect_ratio")));
            parameters.Add("b", background);
            parameters.Add("c", crop);
            parameters.Add("co", color);
            parameters.Add("dpr", dprStr);
            parameters.Add("du", duration);
            parameters.Add("e", Expression.Normalize(GetString(m_transformParams, "effect")));
            parameters.Add("eo", endOffset);
            parameters.Add("fl", flags);
            var fnValue = GetString(m_transformParams, "custom_function") ??
                        GetString(m_transformParams, "custom_pre_function");
            parameters.Add("fn", fnValue);
            parameters.Add("h", Expression.Normalize(height));
            parameters.Add("o", Expression.Normalize(GetString(m_transformParams, "opacity")));
            parameters.Add("q", Expression.Normalize(GetString(m_transformParams, "quality")));
            parameters.Add("r", Expression.Normalize(GetString(m_transformParams, "radius")));
            parameters.Add("so", startOffset);
            parameters.Add("t", namedTransformation);
            parameters.Add("vc", videoCodec);
            parameters.Add("w", Expression.Normalize(width));
            parameters.Add("x", Expression.Normalize(GetString(m_transformParams, "x")));
            parameters.Add("y", Expression.Normalize(GetString(m_transformParams, "y")));
            parameters.Add("z", Expression.Normalize(GetString(m_transformParams, "zoom")));

            for (int i = 0; i < SimpleParams.Length; i += 2)
            {
                if (m_transformParams.TryGetValue(SimpleParams[i + 1], out obj))
                {
                    parameters.Add(SimpleParams[i], ToString(obj));
                }
            }

            List<string> components = new List<string>();

            string ifValue = GetString(m_transformParams, "if");
            if (!string.IsNullOrEmpty(ifValue))
            {
                components.Insert(0, string.Format(CultureInfo.InvariantCulture, "if_{0}", new Condition(ifValue).ToString()));
            }

            SortedSet<string> varParams = new SortedSet<string>();
            foreach (var key in m_transformParams.Keys)
            {
                if (Regex.IsMatch(key, Expression.VARIABLE_NAME_REGEX))
                {
                    varParams.Add($"{key}_{GetString(m_transformParams, key)}");
                }
            }

            if (varParams.Count > 0)
            {
                components.Add(string.Join(",", varParams));
            }

            string vars = m_transformParams.TryGetValue(VARIABLESPARAMKEY, out obj) && obj is Expression[] expressions
                ? ProcessVariables(expressions)
                : null;

            if (!string.IsNullOrEmpty(vars))
            {
                components.Add(string.Join(",", vars));
            }

            foreach (var param in parameters)
            {
                if (!string.IsNullOrEmpty(param.Value))
                {
                    components.Add(string.Format(CultureInfo.InvariantCulture, "{0}_{1}", param.Key, param.Value));
                }
            }

            string rawTransformation = GetString(m_transformParams, "raw_transformation");
            if (rawTransformation != null)
            {
                components.Add(rawTransformation);
            }

            if (components.Count > 0)
            {
                transformations.Add(string.Join(",", components.ToArray()));
            }

            if (isResponsive)
            {
                transformations.Add(ResponsiveWidthTransform.Generate());
            }

            if (width == "auto" || isResponsive)
            {
                IsResponsive = true;
            }

            return string.Join("/", transformations.ToArray());
        }

        /// <summary>
        /// Get this transformation represented as string.
        /// </summary>
        /// <returns>The transformation represented as string.</returns>
        public override string ToString()
        {
            return Generate();
        }

        /// <summary>
        /// Get a deep cloned copy of this transformation.
        /// </summary>
        /// <returns>A deep cloned copy of this transformation.</returns>
        public Transformation Clone()
        {
            Transformation t = (Transformation)this.MemberwiseClone();

            t.m_transformParams = new Dictionary<string, object>();

            foreach (var key in m_transformParams.Keys)
            {
                var value = m_transformParams[key];

                if (value is Array)
                {
                    t.Add(key, ((Array)value).Clone());
                }
                else if (value is string || value is ValueType || value is BaseExpression)
                {
                    t.Add(key, value);
                }
                else if (value is Core.ICloneable)
                {
                    t.Add(key, ((Core.ICloneable)value).Clone());
                }
                else if (value is Dictionary<string, string>)
                {
                    t.Add(key, new Dictionary<string, string>((Dictionary<string, string>)value));
                }
                else
                {
                    throw new Exception(string.Format(CultureInfo.InvariantCulture, "Couldn't clone parameter '{0}'!", key));
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

        /// <summary>
        /// Creates a new object that is a deep copy of the current instance.
        /// </summary>
        /// <returns> A new object that is a deep copy of this instance. </returns>
        object Core.ICloneable.Clone()
        {
            return Clone();
        }

        private static string ToString(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is string)
            {
                return obj.ToString();
            }

            if (obj is float || obj is double)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0:0.0#}", obj);
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}", obj);
        }

        private static string ProcessVariables(Expression[] variables)
        {
            if (variables == null || variables.Length == 0)
            {
                return null;
            }

            return string.Join(",", variables.Select(v => v.ToString()).ToArray());
        }

        private static string[] GetStringArray(Dictionary<string, object> options, string key)
        {
            if (!options.ContainsKey(key))
            {
                return new string[0];
            }

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

        private static string GetString(Dictionary<string, object> options, string key)
        {
            if (options.ContainsKey(key))
            {
                return ToString(options[key]);
            }
            else
            {
                return null;
            }
        }
    }
}
