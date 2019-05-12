﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Represents property of the overlay parameter (l_text: in URLs) for placing text as an overlay.
    /// </summary>
    public class TextLayer : BaseLayer<TextLayer>
    {
        /// <summary>
        /// The text to generate an image for.
        /// </summary>
        protected string m_text;

        /// <summary>
        /// Required name of the font family. e.g. "arial".
        /// </summary>
        protected string m_fontFamily;

        /// <summary>
        /// Font size in pixels. Default: 12.
        /// </summary>
        protected int m_fontSize;

        /// <summary>
        /// Whether to use a "normal" or a "bold" font. Default: "normal".
        /// </summary>
        protected string m_fontWeight;

        /// <summary>
        /// Whether to use a "normal" or an "italic" font style. Default: "normal".
        /// </summary>
        protected string m_fontStyle;
        
        /// <summary>
        /// Type of font antialiasing to use.
        /// </summary>
        protected string m_fontAntialiasing;

        /// <summary>
        /// Type of font hinting to use.
        /// </summary>
        protected string m_fontHinting;

        /// <summary>
        /// Text decoration: underline or strikethrough. Default: "none".
        /// </summary>
        protected string m_textDecoration;

        /// <summary>
        /// Text alignment: left, center, right, end, start or justify. Default: "left".
        /// </summary>
        protected string m_textAlign;

        /// <summary>
        /// Font Stroke(border) for the text. Possible values: "none" or "stroke". Default: "none".
        /// Set the color and weight of the stroke with the border parameter.
        /// </summary>
        protected string m_stroke;

        /// <summary>
        /// Spacing between the letters in pixels. Can be a positive or negative, integer or decimal value.
        /// </summary>
        protected string m_letterSpacing;

        /// <summary>
        /// Spacing between the lines in pixels (only relevant for multi-line text). 
        /// Can be a positive or negative, integer or decimal value.
        /// </summary>
        protected string m_lineSpacing;

        /// <summary>
        /// Default parameterless constructor. Instantiates the <see cref="TextLayer"/> object.
        /// </summary>
        public TextLayer()
        {
            m_resourceType = "text";
            FontSize(12);
        }

        /// <summary>
        /// Instantiates the <see cref="TextLayer"/> object with text.
        /// </summary>
        /// <param name="text">The text to generate an image for.</param>
        public TextLayer(string text) : this()
        {
            Text(text);
        }

        /// <summary>
        /// Overridden method. Restricted to use for text layers.
        /// </summary>
        public new TextLayer ResourceType(string resourceType)
        {
            throw new InvalidOperationException("Cannot modify resourceType for text layers");
        }

        /// <summary>
        /// Overridden method. Restricted to use for text layers.
        /// </summary>
        public new TextLayer Type(string type)
        {
            throw new InvalidOperationException("Cannot modify type for text layers");
        }

        /// <summary>
        /// Overridden method. Restricted to use for text layers.
        /// </summary>
        public new TextLayer Format(string format)
        {
            throw new InvalidOperationException("Cannot modify format for text layers");
        }

        /// <summary>
        /// Set the text to generate an image for.
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
        /// Prepare text for Overlay.
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
        /// Type of font antialiasing to use.
        /// </summary>
        public TextLayer FontAntialiasing(FontAntialiasing value)
        {
            m_fontAntialiasing = ApiShared.GetCloudinaryParam(value);
            return this;
        }

        /// <summary>
        /// Type of font hinting to use.
        /// </summary>
        public TextLayer FontHinting(FontHinting value)
        {
            m_fontHinting = ApiShared.GetCloudinaryParam(value);
            return this;
        }

        /// <summary>
        /// Required name of the font family. e.g. "arial".
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
        /// Spacing between the letters in pixels. Can be a positive or negative, integer or decimal value.
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

        /// <summary>
        /// Get an additional parameters for the text layer.
        /// </summary>
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

        /// <summary>
        /// Get this text layer represented as string.
        /// </summary>
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
            if (!string.IsNullOrEmpty(m_fontAntialiasing))
                components.Add($"antialias_{m_fontAntialiasing}");
            if (!string.IsNullOrEmpty(m_fontHinting))
                components.Add($"hinting_{m_fontHinting}");
            if (!string.IsNullOrEmpty(m_textDecoration) && !m_textDecoration.Equals("none"))
                components.Add(m_textDecoration);
            if (!string.IsNullOrEmpty(m_textAlign))
                components.Add(m_textAlign);
            if (!string.IsNullOrEmpty(m_stroke) && !m_stroke.Equals("none"))
                components.Add(m_stroke);
            if (!string.IsNullOrEmpty(m_letterSpacing))
                components.Add($"letter_spacing_{m_letterSpacing}");
            if (!string.IsNullOrEmpty(m_lineSpacing))
                components.Add($"line_spacing_{m_lineSpacing}");

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

    /// <summary>
    /// Type of font antialiasing.
    /// </summary>
    public enum FontAntialiasing
    {
        /// <summary>
        /// Use a bi-level alpha mask.
        /// </summary>
        [EnumMember(Value = "none")]
        None,
        /// <summary>
        /// Perform single-color antialiasing. For example, using shades of gray for black text on a white background.
        /// </summary>
        [EnumMember(Value = "gray")]
        Gray,
        /// <summary>
        /// Perform antialiasing by taking advantage of the order of subpixel elements on devices such as LCD panels.
        /// </summary>
        [EnumMember(Value = "subpixel")]
        Subpixel,
        /// <summary>
        /// Some antialiasing is performed, but speed is prioritized over quality.
        /// </summary>
        [EnumMember(Value = "fast")]
        Fast,
        /// <summary>
        /// Antialiasing that balances quality and performance.
        /// </summary>
        [EnumMember(Value = "good")]
        Good,
        /// <summary>
        /// Renders at the highest quality, sacrificing speed if necessary.
        /// </summary>
        [EnumMember(Value = "best")]
        Best
    }

    /// <summary>
    /// Type of font hinting.
    /// </summary>
    public enum FontHinting
    {
        /// <summary>
        /// Do not hint outlines.
        /// </summary>
        [EnumMember(Value = "none")]
        None,
        /// <summary>
        /// Hint outlines slightly to improve contrast while retaining good fidelity to the original shapes.
        /// </summary>
        [EnumMember(Value = "slight")]
        Slight,
        /// <summary>
        /// Hint outlines with medium strength, providing a compromise between fidelity to the original shapes and 
        /// contrast.
        /// </summary>
        [EnumMember(Value = "medium")]
        Medium,
        /// <summary>
        /// Hint outlines to the maximize contrast.
        /// </summary>
        [EnumMember(Value = "full")]
        Full
    }
}
