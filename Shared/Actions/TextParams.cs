namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Parameters of generating an image of a given textual string.
    /// </summary>
    public class TextParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextParams"/> class.
        /// </summary>
        public TextParams()
        {
            FontSize = 12;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextParams"/> class.
        /// Parameterized constructor.
        /// </summary>
        /// <param name="text">The text string to generate an image for.</param>
        public TextParams(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Gets or sets the text string to generate an image for.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the identifier that is used for accessing the generated image. If not specified, a unique identifier is
        /// generated, persistently mapped to the given text and style settings. This way, you can keep using
        /// Cloudinary’s API for generating texts. Cloudinary will make sure not to generate multiple images for the
        /// same text and style.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets the name of the font family.
        /// Supported font families: Andale Mono, Arial, Arial Black, AvantGarde, Bookman, Century Schoolbook,
        /// Comic Sans MS, Courier, Courier New, DejaVu Sans, DejaVu Sans Mono, DejaVu Serif, Dingbats, Georgia,
        /// Helvetica, Helvetica Narrow, Impact, Liberation Mono, Liberation Sans, Liberation Sans Narrow,
        /// Liberation Serif, NewCenturySchlbk, Nimbus Mono, Nimbus Roman No9, Nimbus Sans, Palatino, Standard Symbols,
        /// Symbol, Times, Times New Roman, Trebuchet MS, URW Bookman, URW Chancery, URW Gothic, URW Palladio, Verdana,
        /// Webdings.
        /// </summary>
        public string FontFamily { get; set; }

        /// <summary>
        /// Gets or sets font size in points. Default: 12.
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// Gets or sets name or RGB representation of the font"s color. For example: "red", "#ff0000". Default: "black".
        /// </summary>
        public string FontColor { get; set; }

        /// <summary>
        /// Gets or sets whether to use a "normal" or a "bold" font. Default: "normal".
        /// </summary>
        [Obsolete("Property FontWeitgh is deprecated, please use FontWeight instead")]
        public string FontWeitgh
        {
            get { return FontWeight; }
            set { FontWeight = value; }
        }

        /// <summary>
        /// Gets or sets whether to use a "normal" or a "bold" font. Default: "normal".
        /// </summary>
        public string FontWeight { get; set; }

        /// <summary>
        /// Gets or sets whether to use a "normal" or an "italic" font. Default: "normal".
        /// </summary>
        public string FontStyle { get; set; }

        /// <summary>
        /// Gets or sets name or RGB representation of the background color of the generated image.
        /// For example: "red", "#ff0000". Default: "transparent".
        /// </summary>
        public string Background { get; set; }

        /// <summary>
        /// Gets or sets text opacity value between 0 (invisible) and 100. Default: 100.
        /// </summary>
        public string Opacity { get; set; }

        /// <summary>
        /// Gets or sets an "underline" to the text. Default: "none".
        /// </summary>
        public string TextDecoration { get; set; }

        /// <summary>
        /// Gets or sets text alignment for the text. Default: "left".
        /// </summary>
        public string TextAlign { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(Text))
            {
                throw new ArgumentException("Text must be specified in TextParams!");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "text", Text);
            AddParam(dict, "public_id", PublicId);
            AddParam(dict, "font_family", FontFamily);
            AddParam(dict, "font_size", FontSize.ToString(CultureInfo.InvariantCulture));
            AddParam(dict, "font_color", FontColor);
            AddParam(dict, "font_weight", FontWeight);
            AddParam(dict, "font_style", FontStyle);
            AddParam(dict, "background", Background);
            AddParam(dict, "opacity", Opacity);
            AddParam(dict, "text_decoration", TextDecoration);
            AddParam(dict, "text_align", TextAlign);

            return dict;
        }
    }
}
