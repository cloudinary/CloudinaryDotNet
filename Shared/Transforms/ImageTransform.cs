namespace CloudinaryDotNet
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The building blocks for image assets transformation.
    /// </summary>
    public partial class Transformation : Core.ICloneable
    {
        /// <summary>
        /// The required width of a transformed image or an overlay. Can be specified separately or together with the
        /// height value.
        /// </summary>
        /// <param name="value">
        /// Pixels, percents or string. Can also be a decimal value (e.g., 0.2) for percentage based resizing.
        /// </param>
        /// <returns>The transformation with width applied.</returns>
        public Transformation Width(object value)
        {
            return Add("width", value);
        }

        /// <summary>
        /// Set required height of a transformed image or an overlay. Can be specified separately or together with the
        /// width value.
        /// </summary>
        /// <param name="value">
        /// Can also be a decimal value (e.g., 0.2) for percentage based resizing.
        /// </param>
        /// <returns>The transformation with height applied.</returns>
        public Transformation Height(object value)
        {
            return Add("height", value);
        }

        /// <summary>
        /// Set HTML width to transform an uploaded image to a certain dimension and display it within your page in a
        /// different dimension.
        /// </summary>
        /// <param name="value">The HTML width value.</param>
        /// <returns>The transformation with HTML width applied.</returns>
        public Transformation SetHtmlWidth(object value)
        {
            m_htmlWidth = value.ToString();
            return this;
        }

        /// <summary>
        /// Set HTML height to transform an uploaded image to a certain dimension and display it within your page in a
        /// different dimension.
        /// </summary>
        /// <param name="value">The HTML height value.</param>
        /// <returns>The transformation with HTML height applied.</returns>
        public Transformation SetHtmlHeight(object value)
        {
            m_htmlHeight = value.ToString();
            return this;
        }

        /// <summary>
        /// Add named transformation.
        /// </summary>
        /// <param name="value">An array of transformations names.</param>
        /// <returns>The transformation with name applied.</returns>
        public Transformation Named(params string[] value)
        {
            return Add("transformation", value);
        }

        /// <summary>
        /// Add the aspect_ratio parameter to resize or crop the image to a new aspect ratio.
        /// Decimal format (e.g., 1.33 or 2.5).
        /// </summary>
        /// <param name="value">A decimal value representing the ratio of the width divided by the height.</param>
        /// <returns>The transformation with aspect ratio applied.</returns>
        public Transformation AspectRatio(double value)
        {
            return AspectRatio(value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Add the aspect_ratio parameter to resize or crop the image to a new aspect ratio.
        /// Format - nom:denom (e.g., 4:3 or 16:9).
        /// </summary>
        /// <param name="nom">Signifies the relative width.</param>
        /// <param name="denom">Signifies the relative height.</param>
        /// <returns>The transformation with aspect ratio applied.</returns>
        public Transformation AspectRatio(int nom, int denom)
        {
            return AspectRatio(string.Format(CultureInfo.InvariantCulture, "{0}:{1}", nom, denom));
        }

        /// <summary>
        /// Resize or crop the image to a new aspect ratio. This parameter is used together with a specified crop mode
        /// that determines how the image is adjusted to the new dimensions.
        /// </summary>
        /// <param name="value">A string value representing the aspect ratio.</param>
        /// <returns>The transformation with aspect ratio applied.</returns>
        public Transformation AspectRatio(string value)
        {
            return Add("aspect_ratio", value);
        }

        /// <summary>
        /// A crop mode that determines how to transform the image for fitting into the desired width and height dimensions.
        /// </summary>
        /// <param name="value">A string representing the crop value, e.g.: 'scale'.</param>
        /// <returns>The transformation with crop applied.</returns>
        public Transformation Crop(string value)
        {
            return Add("crop", value);
        }

        /// <summary>
        /// Set the background color of the image. An opaque color can be set as an RGB hex
        /// triplet (e.g., '3e2222'), a 3-digit RGB hex (e.g., '777') or a named color (e.g., 'green').
        /// </summary>
        /// <param name="value"><see langword="abstract"/>A value of background color.</param>
        /// <returns>The transformation with background applied.</returns>
        public Transformation Background(string value)
        {
            return Add("background", Regex.Replace(value, "^#", "rgb:"));
        }

        /// <summary>
        /// Defines the color to use for various effects.
        /// </summary>
        /// <param name="value">A string representing the color value.</param>
        /// <returns>The transformation with color applied.</returns>
        public Transformation Color(string value)
        {
            return Add("color", Regex.Replace(value, "^#", "rgb:"));
        }

        /// <summary>
        /// Apply a filter or an effect on an image. The value includes the name of the effect and an additional parameter that controls the behavior of the specific effect.
        /// </summary>
        /// <param name="value">A string representing name of the effect and its additional parameter.</param>
        /// <returns>The transformation with added parameter.</returns>
        public Transformation Effect(string value)
        {
            return Add("effect", value);
        }

        /// <summary>
        /// Apply a filter or an effect on an image.
        /// </summary>
        /// <param name="effect">The name of the effect.</param>
        /// <param name="param">An additional parameter that controls the behavior of the specific effect.</param>
        /// <returns>The transformation with effect applied.</returns>
        public Transformation Effect(string effect, object param)
        {
            return Add("effect", effect + ":" + param);
        }

        /// <summary>
        /// Set angle to rotate the image by.
        /// </summary>
        /// <param name="value">The value of angle.</param>
        /// <returns>The transformation with angle applied.</returns>
        public Transformation Angle(int value)
        {
            return Add("angle", value);
        }

        /// <summary>
        /// Apply the multiple rotation values to the image.
        /// </summary>
        /// <param name="value">An array of rotation values.</param>
        /// <returns>The transformation with angles applied.</returns>
        public Transformation Angle(params string[] value)
        {
            return Add("angle", value);
        }

        /// <summary>
        /// Add a solid border around the image. The value has a CSS-like format: width_style_color.
        /// </summary>
        /// <param name="value">A string representing border in CSS-like format.</param>
        /// <returns>The transformation with border added.</returns>
        public Transformation Border(string value)
        {
            return Add("border", value);
        }

        /// <summary>
        /// Add a solid border around the image.
        /// </summary>
        /// <param name="width">Border width.</param>
        /// <param name="color">Border color.</param>
        /// <returns>The transformation with border added.</returns>
        public Transformation Border(int width, string color)
        {
            return Add("border", string.Empty + width + "px_solid_" + Regex.Replace(color, "^#", "rgb:"));
        }

        /// <summary>
        /// Horizontal position for custom-coordinates based cropping and overlay placement.
        /// </summary>
        /// <param name="value">Value of horizontal position.</param>
        /// <returns>The transformation with horizontal position applied.</returns>
        public Transformation X(object value)
        {
            return Add("x", value);
        }

        /// <summary>
        /// Vertical position for custom-coordinates based cropping and overlay placement.
        /// </summary>
        /// <param name="value">Value of vertical position.</param>
        /// <returns>The transformation with vertical position applied.</returns>
        public Transformation Y(object value)
        {
            return Add("y", value);
        }

        /// <summary>
        /// Defines radius value for corners rounding.
        /// </summary>
        /// <param name="value">
        /// Can be string, number or collection with 1..4 values.
        /// </param>
        /// <returns>The transformation with radius defined.</returns>
        public Transformation Radius(object value) => Add("radius", new Radius(value));

        /// <summary>
        /// Defines radius value for corners rounding.
        /// </summary>
        /// <param name="radius">Object for the advanced definition of the radius.</param>
        /// <returns>The transformation with radius defined.</returns>
        public Transformation Radius(Radius radius) => Add("radius", radius);

        /// <summary>
        /// Control the JPG compression quality. 1 is the lowest quality and 100 is the highest. The default is the
        /// original image's quality or 90% if not available. Reducing quality generates JPG images much smaller in
        /// file size.
        /// </summary>
        /// <param name="value">Quality level.</param>
        /// <returns>The transformation with quality level defined.</returns>
        public Transformation Quality(object value)
        {
            return Add("quality", value);
        }

        /// <summary>
        /// Define the default image placeholder. Can be used in the case that a requested image does not exist.
        /// </summary>
        /// <param name="value">The default image name.</param>
        /// <returns>The transformation with default image defined.</returns>
        public Transformation DefaultImage(string value)
        {
            return Add("default_image", value);
        }

        /// <summary>
        /// Decides which part of the image to keep while 'crop', 'pad' and 'fill' crop modes are used. For overlays,
        /// this decides where to place the overlay.
        /// </summary>
        /// <param name="value">
        /// The gravity value. Use static class <see cref="CloudinaryDotNet.Gravity"/> for values.
        /// </param>
        /// <returns>The transformation with gravity applied.</returns>
        public Transformation Gravity(string value)
        {
            return Add("gravity", value);
        }

        /// <summary>
        /// Decides which part of the image to keep while 'crop', 'pad' and 'fill' crop modes are used. For overlays,
        /// this decides where to place the overlay.
        /// </summary>
        /// <param name="value">The gravity value (e.g. 'faces').</param>
        /// <param name="param">The gravity parameter (e.g. 'center').</param>
        /// <returns>The transformation with gravity applied.</returns>
        public Transformation Gravity(string value, string param)
        {
            return Gravity($"{value}:{param}");
        }

        /// <summary>
        /// Control the color space used for the delivered image.
        /// </summary>
        /// <param name="value">The value of color space. Possible values: 'srgb', 'tinysrgb','cmyk', 'no_cmyk', 'cs_icc:[public_id]'. </param>
        /// <returns>The transformation with color space applied.</returns>
        public Transformation ColorSpace(string value)
        {
            return Add("color_space", value);
        }

        /// <summary>
        /// Add prefix to class names while creating sprites.
        /// </summary>
        /// <param name="value">The name of prefix.</param>
        /// <returns>The transformation with prefix added.</returns>
        public Transformation Prefix(string value)
        {
            return Add("prefix", value);
        }

        /// <summary>
        /// Manipulate image opacity in order to make the image semi-transparent.
        /// </summary>
        /// <param name="value">Opacity level: 100 means opaque, while 0 is completely transparent.</param>
        /// <returns>The transformation with opacity level defined.</returns>
        public Transformation Opacity(int value)
        {
            return Add("opacity", value);
        }

        /// <summary>
        /// Add an overlay over the base image.
        /// </summary>
        /// <param name="value">A value of the overlay (e.g. 'text:hello').</param>
        /// <returns>The transformation with overlay added.</returns>
        public Transformation Overlay(string value)
        {
            return Add("overlay", value);
        }

        /// <summary>
        /// Add an overlay over the base image.
        /// </summary>
        /// <param name="value">An object representing the overlay.</param>
        /// <returns>The transformation with overlay added.</returns>
        public Transformation Overlay(BaseLayer value)
        {
            return Add("overlay", value);
        }

        /// <summary>
        /// Add an underlay image below a base partially-transparent image.
        /// </summary>
        /// <param name="value">A value of the underlay (e.g. 'text:hello').</param>
        /// <returns>The transformation with underlay added.</returns>
        public Transformation Underlay(string value)
        {
            return Add("underlay", value);
        }

        /// <summary>
        /// Add an underlay image below a base partially-transparent image.
        /// </summary>
        /// <param name="value">An object representing the underlay.</param>
        /// <returns>The transformation with underlay added.</returns>
        public Transformation Underlay(BaseLayer value)
        {
            return Add("underlay", value);
        }

        /// <summary>
        /// Force format conversion to the given image format for remote 'fetch' URLs and auto uploaded images that
        /// already have a different format as part of their URLs.
        /// </summary>
        /// <param name="value">A format the image to convert to while fetching.</param>
        /// <returns>The transformation with format conversion defined.</returns>
        public Transformation FetchFormat(string value)
        {
            return Add("fetch_format", value);
        }

        /// <summary>
        /// Control the density to use while converting a PDF document to images. (range: 50-300, default: 150).
        /// </summary>
        /// <param name="value">Dpi value.</param>
        /// <returns>The transformation with density defined.</returns>
        public Transformation Density(object value)
        {
            return Add("density", value);
        }

        /// <summary>
        /// Given a multi-page PDF document, generate an image of a single page using the given index.
        /// </summary>
        /// <param name="value">Page number or layer name.</param>
        /// <returns>The transformation with paging defined.</returns>
        public Transformation Page(object value)
        {
            return Add("page", value);
        }

        /// <summary>
        /// Controls the time delay between the frames of an animated image, in milliseconds.
        /// </summary>
        /// <param name="value">The time delay in milliseconds.</param>
        /// <returns>The transformation with time delay defined.</returns>
        public Transformation Delay(object value)
        {
            return Add("delay", value);
        }

        /// <summary>
        /// Pass an entire transformation string (including chained transformations) in URL format from an SDK helper
        /// method that builds a URL, image tag, or video tag. The URL string is appended as is (with no processing or
        /// validation) to the **end** of any other transformation parameters passed in the same method.
        /// </summary>
        /// <param name="value">A raw transformation string.</param>
        /// <returns>The transformation with raw transformation string applied.</returns>
        public Transformation RawTransformation(string value)
        {
            return Add("raw_transformation", value);
        }

        /// <summary>
        /// Set one or more flags that alter the default transformation behavior.
        /// </summary>
        /// <param name="value">An array with transformation flags.</param>
        /// <returns>The transformation with array of transformation flags applied.</returns>
        public Transformation Flags(params string[] value)
        {
            return Add("flags", value);
        }

        /// <summary>
        /// How much zoom should be applying when detecting faces for crop, thumb or for overlays. (e.g. 0.5 will cause zoom out of x2 on both axes).
        /// </summary>
        /// <param name="value">Zoom percent value represented by integer number.</param>
        /// <returns>The transformation with zoom percent applied.</returns>
        public Transformation Zoom(int value)
        {
            return Add("zoom", value);
        }

        /// <summary>
        /// How much zoom should be applying when detecting faces for crop, thumb or for overlays. (e.g. 0.5 will cause zoom out of x2 on both axes).
        /// </summary>
        /// <param name="value">Zoom percent value represented by string.</param>
        /// <returns>The transformation with zoom percent applied.</returns>
        public Transformation Zoom(string value)
        {
            return Add("zoom", value);
        }

        /// <summary>
        /// How much zoom should be applying when detecting faces for crop, thumb or for overlays. (e.g. 0.5 will cause zoom out of x2 on both axes).
        /// </summary>
        /// <param name="value">Zoom percent value represented by single-precision number.</param>
        /// <returns>The transformation with zoom percent applied.</returns>
        public Transformation Zoom(float value)
        {
            return Add("zoom", value);
        }

        /// <summary>
        /// How much zoom should be applying when detecting faces for crop, thumb or for overlays. (e.g. 0.5 will cause zoom out of x2 on both axes).
        /// </summary>
        /// <param name="value">Zoom percent value represented by double-precision number.</param>
        /// <returns>The transformation with zoom percent applied.</returns>
        public Transformation Zoom(double value)
        {
            return Add("zoom", value);
        }

        /// <summary>
        /// Sets Device Pixel Ratio  (float, integer and "auto" values are allowed").
        /// See http://cloudinary.com/blog/how_to_automatically_adapt_website_images_to_retina_and_hidpi_devices for further info.
        /// </summary>
        /// <param name="value">Device pixel ratio.</param>
        /// <returns>The transformation with DPR defined.</returns>
        public Transformation Dpr(object value)
        {
            return Add("dpr", value);
        }

        /// <summary>
        /// Whether to enable automatic adaptation of website images.
        /// See http://cloudinary.com/blog/how_to_automatically_adapt_website_images_to_retina_and_hidpi_devices for further info.
        /// </summary>
        /// <param name="value">Flag that determines automatic adaptation usage.</param>
        /// <returns>The transformation with responsive width defined.</returns>
        public Transformation ResponsiveWidth(bool value)
        {
            return Add("responsive_width", value);
        }

        /// <summary>
        /// Start defining a condition, which will be completed with a call. <seealso cref="Condition.Then()"/>
        /// </summary>
        /// <returns>Condition.</returns>
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

        /// <summary>
        /// Define a conditional transformation.
        /// </summary>
        /// <param name="expression">An expression.</param>
        /// <returns>The transformation for chaining.</returns>
        public Transformation IfCondition(BaseExpression expression)
        {
            return IfCondition(expression.ToString());
        }

        /// <summary>
        /// Specify a transformation that is applied in the case that the initial condition is evaluated as negative.
        /// </summary>
        /// <returns>The transformation for chaining.</returns>
        public Transformation IfElse()
        {
            Chain();
            return Add("if", "else");
        }

        /// <summary>
        /// Set a condition for applying multiple transformations (in the form of chained transformation components).
        /// Apply EndIf to the last transformation component in the chain.
        /// </summary>
        /// <returns>The transformation for chaining.</returns>
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
                    if (ifValue.Equals("end", StringComparison.Ordinal))
                    {
                        break;
                    }

                    if (segment.Params.Count > 1)
                    {
                        segment.Params.Remove("if"); // {c: fill, w: 500}
                        m_nestedTransforms[i] = segment; // [..., {c: fill, w: 500}, ...]
                        m_nestedTransforms.Insert(i, new Transformation(string.Format(CultureInfo.InvariantCulture, "if={0}", value.ToString()))); // [..., "if_w_gt_1000", {c: fill, w: 500}, ...]
                    }

                    // otherwise keep looking for if_condition
                    if (!string.Equals("else", ifValue, StringComparison.Ordinal))
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
