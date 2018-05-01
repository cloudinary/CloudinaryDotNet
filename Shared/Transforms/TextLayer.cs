using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Represents property of the overlay parameter (l_text: in URLs)
    /// for placing text as an overlay.
    /// </summary>
    public class TextLayer : BaseLayer<TextLayer>
    {
        protected string m_text;
        protected string m_fontFamily;
        protected int m_fontSize;

        protected string m_fontWeight;
        protected string m_fontStyle;
        protected string m_textDecoration;
        protected string m_textAlign;
        protected string m_stroke;
        protected string m_letterSpacing;
        protected string m_lineSpacing;

        public TextLayer()
        {
            m_resourceType = "text";
            FontSize(12);
        }

        /// <param name="text">The text to generate an image for.</param>
        public TextLayer(string text) : this()
        {
            Text(text);
        }

        public new TextLayer ResourceType(string resourceType)
        {
            throw new InvalidOperationException("Cannot modify resourceType for text layers");
        }

        public new TextLayer Type(string type)
        {
            throw new InvalidOperationException("Cannot modify type for text layers");
        }

        public new TextLayer Format(string format)
        {
            throw new InvalidOperationException("Cannot modify format for text layers");
        }

        /// <summary>
        /// The text to generate an image for.
        /// </summary>
        public TextLayer Text(string text)
        {
            this.m_text = OverlayTextEncode(text);
            return this;
        }

        private string Encode(string text)
        {
            return Utils.Encode(text)
                .Replace("%2f", "%252f").Replace("/", "%252f")
                .Replace("%3a", ":").Replace("+", "%20")
                .Replace("%2c", "%252c").Replace(",", "%252c")
                .Replace("(", "%28").Replace(")", "%29")
                .Replace("$", "%24");
        }

        /// <summary>
        /// Prepare text for Overlay
        /// </summary>
        private string OverlayTextEncode(string text)
        {
            string part;
            StringBuilder result = new StringBuilder();

            // Don't encode interpolation expressions e.g. $(variable)
            var match = Regex.Matches(text, "\\$\\([a-zA-Z]\\w+\\)");
            int start = 0;
            foreach (Match m in match)
            {
                part = text.Substring(start, m.Index-start);
                part = Encode(part);
                result.Append(part);
                result.Append(m.Value);
                start = m.Index + m.Length;
            }
            result.Append(Encode(text.Substring(start)));

            return result.ToString();
        }

        /// <summary>
        /// Required name of the font family. e.g. "arial"
        /// </summary>
        public TextLayer FontFamily(string fontFamily)
        {
            this.m_fontFamily = fontFamily;
            return this;
        }

        /// <summary>
        /// Font size in pixels. Default: 12.
        /// </summary>
        public TextLayer FontSize(int fontSize)
        {
            this.m_fontSize = fontSize;
            return this;
        }

        /// <summary>
        /// Whether to use a "normal" or a "bold" font. Default: "normal".
        /// </summary>
        public TextLayer FontWeight(string fontWeight)
        {
            this.m_fontWeight = fontWeight;
            return this;
        }

        /// <summary>
        /// Whether to use a "normal" or an "italic" font style. Default: "normal".
        /// </summary>
        public TextLayer FontStyle(string fontStyle)
        {
            this.m_fontStyle = fontStyle;
            return this;
        }

        /// <summary>
        /// Text decoration: underline or strikethrough. Default: "none".
        /// </summary>
        public TextLayer TextDecoration(string textDecoration)
        {
            this.m_textDecoration = textDecoration;
            return this;
        }

        /// <summary>
        /// Text alignment: left, center, right, end, start or justify. Default: "left".
        /// </summary>
        public TextLayer TextAlign(string textAlign)
        {
            this.m_textAlign = textAlign;
            return this;
        }

        /// <summary>
        /// Font Stroke(border) for the text. Possible values: "none" or "stroke". Default: "none".
        /// Set the color and weight of the stroke with the border parameter.
        /// </summary>
        public TextLayer Stroke(string stroke)
        {
            this.m_stroke = stroke;
            return this;
        }

        /// <summary>
        /// Spacing between the letters in pixels. 
        /// Can be a positive or negative, integer or decimal value.
        /// </summary>
        public TextLayer LetterSpacing(string letterSpacing)
        {
            this.m_letterSpacing = letterSpacing;
            return this;
        }

        /// <summary>
        /// Spacing between the lines in pixels (only relevant for multi-line text). 
        /// Can be a positive or negative, integer or decimal value.
        /// </summary>
        public TextLayer LineSpacing(string lineSpacing)
        {
            this.m_lineSpacing = lineSpacing;
            return this;
        }

        public override string AdditionalParams()
        {
            List<string> components = new List<string>();

            var styleIdentifier = TextStyleIdentifier();
            if (!string.IsNullOrEmpty(styleIdentifier))
            {
                components.Add(styleIdentifier);
            }

            if (!string.IsNullOrEmpty(m_text))
            {
                components.Add(m_text);
            }
            return string.Join(":", components);
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(m_publicId) && string.IsNullOrEmpty(m_text))
            {
                throw new ArgumentException("Must supply either text or publicId.");
            }
            return base.ToString();
        }

        private string TextStyleIdentifier()
        {
            List<string> components = new List<string>();

            if (!string.IsNullOrEmpty(m_fontWeight) && !m_fontWeight.Equals("normal"))
                components.Add(m_fontWeight);
            if (!string.IsNullOrEmpty(m_fontStyle) && !m_fontStyle.Equals("normal"))
                components.Add(m_fontStyle);
            if (!string.IsNullOrEmpty(m_textDecoration) && !m_textDecoration.Equals("none"))
                components.Add(m_textDecoration);
            if (!string.IsNullOrEmpty(m_textAlign))
                components.Add(m_textAlign);
            if (!string.IsNullOrEmpty(m_stroke) && !m_stroke.Equals("none"))
                components.Add(m_stroke);
            if (!string.IsNullOrEmpty(m_letterSpacing))
                components.Add(string.Format("letter_spacing_{0}", m_letterSpacing));
            if (!string.IsNullOrEmpty(m_lineSpacing))
                components.Add(string.Format("line_spacing_{0}", m_lineSpacing));

            if (string.IsNullOrEmpty(m_fontFamily) && components.Count == 0)
            {
                return null;
            }

            if (string.IsNullOrEmpty(m_fontFamily))
            {
                throw new ArgumentException("Must supply fontFamily.");
            }

            components.Insert(0, m_fontSize.ToString());
            components.Insert(0, m_fontFamily);

            return string.Join("_", components);
        }
    }
}
