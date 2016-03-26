using System;
using System.Collections.Generic;
using System.Web;

namespace CloudinaryDotNet
{
    public class TextLayer : BaseLayer<TextLayer>
    {
        protected string text;
        protected string fontFamily;
        protected int fontSize;

        protected string fontWeight;
        protected string fontStyle;
        protected string textDecoration;
        protected string textAlign;
        protected string stroke;
        protected string letterSpacing;
        protected string lineSpacing;

        public TextLayer()
        {
            fontSize = 12;
            resourceType = "text";
        }

        /// <param name="text">The text to generate an image for.</param>
        public TextLayer(string text) : this()
        {
            this.text = OverlayTextEncode(text);
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
            this.text = OverlayTextEncode(text);
            return this;
        }

        /// <summary>
        /// Prepare text for Overlay
        /// </summary>
        private string OverlayTextEncode(string text)
        {
            return HttpUtility.UrlEncodeUnicode(text)
                .Replace("%2f", "/").Replace("%3a", ":").Replace("+", "%20")
                .Replace("%2c", "%e2%80%9a").Replace("/", "%e2%81%84");
        }

        /// <summary>
        /// Required name of the font family. e.g. "arial"
        /// </summary>
        public TextLayer FontFamily(string fontFamily)
        {
            this.fontFamily = fontFamily;
            return this;
        }

        /// <summary>
        /// Font size in pixels. Default: 12.
        /// </summary>
        public TextLayer FontSize(int fontSize)
        {
            this.fontSize = fontSize;
            return this;
        }

        /// <summary>
        /// Whether to use a "normal" or a "bold" font. Default: "normal".
        /// </summary>
        public TextLayer FontWeight(string fontWeight)
        {
            this.fontWeight = fontWeight;
            return this;
        }

        /// <summary>
        /// Whether to use a "normal" or an "italic" font style. Default: "normal".
        /// </summary>
        public TextLayer FontStyle(string fontStyle)
        {
            this.fontStyle = fontStyle;
            return this;
        }

        /// <summary>
        /// Text decoration: underline or strikethrough. Default: "none".
        /// </summary>
        public TextLayer TextDecoration(string textDecoration)
        {
            this.textDecoration = textDecoration;
            return this;
        }

        /// <summary>
        /// Text alignment: left, center, right, end, start or justify. Default: "left".
        /// </summary>
        public TextLayer TextAlign(string textAlign)
        {
            this.textAlign = textAlign;
            return this;
        }

        /// <summary>
        /// Font Stroke(border) for the text. Possible values: "none" or "stroke". Default: "none".
        /// Set the color and weight of the stroke with the border parameter.
        /// </summary>
        public TextLayer Stroke(string stroke)
        {
            this.stroke = stroke;
            return this;
        }

        /// <summary>
        /// Spacing between the letters in pixels. 
        /// Can be a positive or negative, integer or decimal value.
        /// </summary>
        public TextLayer LetterSpacing(string letterSpacing)
        {
            this.letterSpacing = letterSpacing;
            return this;
        }

        /// <summary>
        /// Spacing between the lines in pixels (only relevant for multi-line text). 
        /// Can be a positive or negative, integer or decimal value.
        /// </summary>
        public TextLayer LineSpacing(string lineSpacing)
        {
            this.lineSpacing = lineSpacing;
            return this;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(publicId) && string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Must supply either text or publicId.");
            }

            List<string> components = new List<string>();
            components.Add(resourceType);

            var styleIdentifier = TextStyleIdentifier();
            if (!string.IsNullOrEmpty(styleIdentifier))
            {
                components.Add(styleIdentifier);
            }

            if (!string.IsNullOrEmpty(publicId))
            {
                components.Add(FormattedPublicId());
            }

            if (!string.IsNullOrEmpty(text))
            {
                components.Add(text);
            }

            return string.Join(":", components);
        }

        private string TextStyleIdentifier()
        {
            List<string> components = new List<string>();

            if (!string.IsNullOrEmpty(fontWeight) && !fontWeight.Equals("normal"))
                components.Add(fontWeight);
            if (!string.IsNullOrEmpty(fontStyle) && !fontStyle.Equals("normal"))
                components.Add(fontStyle);
            if (!string.IsNullOrEmpty(textDecoration) && !textDecoration.Equals("none"))
                components.Add(textDecoration);
            if (!string.IsNullOrEmpty(textAlign))
                components.Add(textAlign);
            if (!string.IsNullOrEmpty(stroke) && !stroke.Equals("none"))
                components.Add(stroke);
            if (!string.IsNullOrEmpty(letterSpacing))
                components.Add(string.Format("letter_spacing_{0}", letterSpacing));
            if (!string.IsNullOrEmpty(lineSpacing))
                components.Add(string.Format("line_spacing_{0}", lineSpacing));

            if (string.IsNullOrEmpty(fontFamily) && components.Count == 0)
            {
                return null;
            }

            if (string.IsNullOrEmpty(fontFamily))
            {
                throw new ArgumentException("Must supply fontFamily.");
            }

            components.Insert(0, fontSize.ToString());
            components.Insert(0, fontFamily);

            return string.Join("_", components);
        }
    }
}
