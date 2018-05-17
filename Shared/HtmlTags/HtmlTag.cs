using System.Text;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CloudinaryDotNet.HtmlTags
{
    class HtmlTag
    {
        /// <summary>
        /// Name of the tag
        /// </summary>
        protected readonly string TagName;

        /// <summary>
        /// Keeps attributes of the tag
        /// </summary>
        protected readonly StringDictionary Attributes = new StringDictionary();

        /// <summary>
        /// Keeps additional options that can be used for building tag
        /// </summary>
        protected readonly Dictionary<string, object> AdditionalOptions = new Dictionary<string, object>();

        /// <summary>
        /// Keeps classes of the class attribute
        /// </summary>
        protected readonly HashSet<string> Classes = new HashSet<string>();
        /// <summary>
        /// Indicates whether current tag is a void tag
        /// </summary>
        protected bool isVoid = false;

        /// <summary>
        /// List of void elements that only have a start tag
        /// </summary>
        /// <see href="http://w3c.github.io/html/syntax.html#void-elements">Void Element</see>
        private static readonly HashSet<string> voidTags = new HashSet<string>
        {
            "area", "base", "br", "col", "embed", "hr", "img", "input", "link", "meta", "param", "source", "track", "wbr"
        };

        public HtmlTag(string tagName)
        {
            if (!tagName.All(char.IsLetterOrDigit))
            {
                throw new ArgumentException("Tag name must be alphanumeric!");
            }

            TagName = tagName;

            if (voidTags.Contains(tagName))
            {
                isVoid = true;
            }
        }

        /// <summary>
        /// Adds new attribute to the tag
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public HtmlTag Attr(string name, string value)
        {
            // In case someone wanted to bypass Class method, catch it here
            if (name == "class")
            {
                Classes.UnionWith(value.Split(' '));
                return this;
            }

            Attributes.Add(name, value);

            return this;
        }

        /// <summary>
        /// Adds new attribute to the tag
        /// </summary>
        /// <param name="attr">Attribute containing name and value</param>
        public HtmlTag Attr(KeyValuePair<string, string> attr)
        {
            Attr(attr.Key, attr.Value);

            return this;
        }

        /// <summary>
        /// Adds new attribute from IDictionary<string, object>
        /// </summary>
        /// <param name="attr">Attribute containing name and value</param>
        public HtmlTag Attrs(IDictionary<string, string> attributes)
        {
            foreach (var attr in attributes)
            {
                Attr(attr.Key, attr.Value);
            }

            return this;
        }

        /// <summary>
        /// Adds class to the "class" attribute
        /// </summary>
        /// <param name="className">Name of the class</param>
        public HtmlTag Class(string className)
        {
            Classes.UnionWith(className.Split(' '));

            // Since Attributes StringDictionary preserves insertion order,
            // we need to keep some a placeholder for "class" attribute
            Attributes["class"] = null;

            return this;
        }

        /// <summary>
        /// Adds new option to the tag
        /// </summary>
        /// <param name="name">Option name</param>
        /// <param name="value">Option value</param>
        public HtmlTag Option(string name, object value)
        {
            AdditionalOptions.Add(name, value);

            return this;
        }

        /// <summary>
        /// Adds option from KeyValuePair
        /// </summary>
        /// <param name="attr">Attribute containing name and value</param>
        public HtmlTag Option(KeyValuePair<string, string> option)
        {
            Option(option.Key, option.Value);

            return this;
        }
        /// <summary>
        /// Adds options from IDictionary
        /// </summary>
        /// <param name="name">Option name</param>
        /// <param name="value">Option value</param>
        public HtmlTag Options(IDictionary<string, string> options)
        {
            foreach(var option in options)
            {
                Option(option.Key, option.Value);
            }

            return this;
        }

        public override string ToString()
        {
            var modifiedAttributes = new StringDictionary(Attributes);

            var tagBuilder = new StringBuilder($"<{TagName}");

            if(Classes.Count > 0)
            {
                modifiedAttributes["class"] = string.Join(" ", Classes);
            }

            foreach (var item in modifiedAttributes)
            {
                tagBuilder.Append(" ").Append(item.Key);

                if (item.Value != null)
                {
                    tagBuilder.Append("=\"").Append(WebUtility.HtmlEncode(item.Value)).Append("\"");
                }
            }

            tagBuilder.Append(">");

            if (!isVoid)
            {
                tagBuilder.Append($"</{TagName}>");
            }

            return tagBuilder.ToString();
        }
    }
}
