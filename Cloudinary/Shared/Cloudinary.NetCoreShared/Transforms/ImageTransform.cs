using CloudinaryDotNet.Core;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    public partial class Transformation : Core.ICloneable
    {
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
        /// Add the aspect_ratio parameter to resize or crop the image to a new aspect ratio.
        /// Decimal format (e.g., 1.33 or 2.5)
        /// </summary>
        /// <param name="value">A decimal value representing the ratio of the width divided by the height</param>
        public Transformation AspectRatio(double value) { return AspectRatio(value.ToString(CultureInfo.InvariantCulture)); }

        /// <summary>
        /// Add the aspect_ratio parameter to resize or crop the image to a new aspect ratio.
        /// Format - nom:denom (e.g., 4:3 or 16:9).
        /// </summary>
        /// <param name="nom">Signifies the relative width</param>
        /// <param name="denom">Signifies the relative height</param>
        public Transformation AspectRatio(int nom, int denom) { return AspectRatio(string.Format("{0}:{1}", nom, denom)); }

        public Transformation AspectRatio(string value) { return Add("aspect_ratio", value); }

        /// <summary>
        /// A crop mode that determines how to transform the image for fitting into the desired width and height dimensions.
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
        public Transformation Overlay(BaseLayer value) { return Add("overlay", value); }
        public Transformation Underlay(string value) { return Add("underlay", value); }
        public Transformation Underlay(BaseLayer value) { return Add("underlay", value); }
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
        public Transformation Zoom(int value) { return Add("zoom", value); }

        /// <summary>
        /// How much zoom should be applying when detecting faces for crop, thumb or for overlays. (e.g. 0.5 will cause zoom out of x2 on both axes).
        /// </summary>
        public Transformation Zoom(string value) { return Add("zoom", value); }

        /// <summary>
        /// How much zoom should be applying when detecting faces for crop, thumb or for overlays. (e.g. 0.5 will cause zoom out of x2 on both axes).
        /// </summary>
        public Transformation Zoom(float value) { return Add("zoom", value); }

        /// <summary>
        /// How much zoom should be applying when detecting faces for crop, thumb or for overlays. (e.g. 0.5 will cause zoom out of x2 on both axes).
        /// </summary>
        public Transformation Zoom(double value) { return Add("zoom", value); }

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

        /// <summary>
        /// Start defining a condition, which will be completed with a call <seealso cref="Condition.Then()"/>
        /// </summary>
        /// <returns>Condition</returns>
        public Condition IfCondition()
        {
            return new Condition().SetParent(this);
        }

        /// <summary>
        /// Define a conditional transformation defined by the condition string.
        /// </summary>
        /// <param name="condition">A condition string.</param>
        /// <returns>The transformation for chaining.</returns>
        public Transformation IfCondition(string condition)
        {
            return Add("if", condition);
        }

        public Transformation IfElse()
        {
            Chain();
            return Add("if", "else");
        }

        public Transformation EndIf()
        {
            Chain();
            int transSize = m_nestedTransforms.Count;
            for (int i = transSize - 1; i >= 0; i--)
            {
                Transformation segment = m_nestedTransforms[i]; // [..., {if: "w_gt_1000",c: "fill", w: 500}, ...]
                if (segment.Params.ContainsKey("if"))
                { // if: "w_gt_1000"
                    var value = segment.Params["if"];
                    string ifValue = value.ToString();
                    if (ifValue.Equals("end")) break;
                    if (segment.Params.Count > 1)
                    {
                        segment.Params.Remove("if"); // {c: fill, w: 500}
                        m_nestedTransforms[i] = segment; // [..., {c: fill, w: 500}, ...]
                        m_nestedTransforms.Insert(i, new Transformation(string.Format("if={0}", value.ToString()))); // [..., "if_w_gt_1000", {c: fill, w: 500}, ...]
                    }
                    // otherwise keep looking for if_condition
                    if (!string.Equals("else", ifValue))
                    {
                        break;
                    }
                }
            }
            Add("if", "end");
            return Chain();
        }

    }
}
