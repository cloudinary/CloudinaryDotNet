namespace CloudinaryDotNet
{
    using System;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Allows to generate images for your responsive website in various width dimensions,
    /// and to define the minimum file size step (performance budget).
    /// </summary>
    public class ResponsiveBreakpoint : JObject
    {
        private const string CREATEDERIVED = "create_derived";
        private const string TRANSFORMATION = "transformation";
        private const string MAXWIDTH = "max_width";
        private const string MINWIDTH = "min_width";
        private const string BYTESSTEP = "bytes_step";
        private const string MAXIMAGES = "max_images";
        private const string FORMAT = "format";

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponsiveBreakpoint"/> class.
        /// </summary>
        public ResponsiveBreakpoint()
        {
            Add(CREATEDERIVED, true);
        }

        /// <summary>
        /// Get value of the create_derived flag.
        /// </summary>
        /// <returns>True if derived images are to be created; otherwise, false.</returns>
        public bool IsCreateDerived()
        {
            return GetValue(CREATEDERIVED, StringComparison.Ordinal).Value<bool>();
        }

        /// <summary>
        /// Set create_derived flag.
        /// If true, create and keep the derived images of the selected breakpoints during the API call.
        /// If false, images generated during the analysis process are thrown away.
        /// </summary>
        /// <param name="createDerived">Flag that determines whether derived images are created.</param>
        /// <returns>The responsive breakpoint with flag set.</returns>
        public ResponsiveBreakpoint CreateDerived(bool createDerived)
        {
            this[CREATEDERIVED] = createDerived;
            return this;
        }

        /// <summary>
        /// The base transformation to first apply to the image before finding the best breakpoints.
        /// </summary>
        /// <param name="transformation">Transformation to base on.</param>
        /// <returns>The responsive breakpoint with the transformation applied.</returns>
        public ResponsiveBreakpoint Transformation(Transformation transformation)
        {
            this[TRANSFORMATION] = transformation.ToString();
            return this;
        }

        /// <summary>
        /// Get maximal width in pixels.
        /// </summary>
        /// <returns>Integer value that represents maximum width.</returns>
        public int MaxWidth()
        {
            return Value<int>(MAXWIDTH);
        }

        /// <summary>
        /// The maximum width needed for this image. If specifying a width bigger than the original image, the width
        /// of the original image is used instead. Default: 1000.
        /// </summary>
        /// <param name="maxWidth">Maximum width in pixels.</param>
        /// <returns>The responsive breakpoint with maximum width defined.</returns>
        public ResponsiveBreakpoint MaxWidth(int maxWidth)
        {
            this[MAXWIDTH] = maxWidth;
            return this;
        }

        /// <summary>
        /// Get minimum width in pixels.
        /// </summary>
        /// <returns>Integer value that represents minimum width.</returns>
        public int MinWidth()
        {
            return Value<int>(MINWIDTH);
        }

        /// <summary>
        /// Set the minimum width needed for this image. Default: 50.
        /// </summary>
        /// <param name="minWidth">Minimum width in pixels.</param>
        /// <returns>The responsive breakpoint with minimum width defined.</returns>
        public ResponsiveBreakpoint MinWidth(int minWidth)
        {
            this[MINWIDTH] = minWidth;
            return this;
        }

        /// <summary>
        /// Get minimum number of bytes between two consecutive breakpoints (images).
        /// </summary>
        /// <returns>Integer value that represents bytes step.</returns>
        public int BytesStep()
        {
            return Value<int>(BYTESSTEP);
        }

        /// <summary>
        /// Set the minimum number of bytes between two consecutive breakpoints (images). Default: 20000.
        /// </summary>
        /// <param name="bytesStep">File size step in bytes.</param>
        /// <returns>Breakpoint with bytes step defined.</returns>
        public ResponsiveBreakpoint BytesStep(int bytesStep)
        {
            this[BYTESSTEP] = bytesStep;
            return this;
        }

        /// <summary>
        /// Get maximum number of breakpoints(images) to find.
        /// </summary>
        /// <returns>Integer value that represents maximum number of images.</returns>
        public int MaxImages()
        {
            return Value<int>(MAXIMAGES);
        }

        /// <summary>
        /// Set the maximum number of breakpoints to find, between 3 and 200. This means that there might be size
        /// differences bigger than the given bytes_step value between consecutive images. Default: 20.
        /// </summary>
        /// <param name="maxImages">Maximum number of breakpoints to find.</param>
        /// <returns>Breakpoint with maximum number of images defined.</returns>
        public ResponsiveBreakpoint MaxImages(int maxImages)
        {
            this[MAXIMAGES] = maxImages;
            return this;
        }

        /// <summary>
        /// Get the file extension of the derived resources to the format indicated.
        /// </summary>
        /// <returns>A string that represents file extension.</returns>
        public string Format()
        {
            return Value<string>(FORMAT);
        }

        /// <summary>
        /// Sets the file extension of the derived resources to the format indicated.
        /// </summary>
        /// <param name="format">File extension of the derived resources.</param>
        /// <returns>Breakpoint with file extension defined.</returns>
        public ResponsiveBreakpoint Format(string format)
        {
            this[FORMAT] = format;
            return this;
        }
    }
}
